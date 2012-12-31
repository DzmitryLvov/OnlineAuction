using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Data.Objects.DataClasses;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using OnlineAuction.Buisness.Data;

namespace OnlineAuction.Buisness.Security
{
    // ReSharper disable ClassNeverInstantiated.Global
    public sealed class AuctionMembershipProvider : MembershipProvider
    {
        private const int NEW_PASSWORD_LENGTH = 8;
        private const string EVENT_SOURCE = "AuctionMembershipProvider";
        private const string EVENT_LOG = "Application";
        private const string EXCEPTION_MESSAGE = "An exception occurred. Please check the Event Log.";
        private const string ENCRYPTION_KEY = "AE09F72BA97CBBB5";
        private MainDataBase _dataBase;
        private bool _enablePasswordReset;
        private bool _enablePasswordRetrieval;
        private int _maxInvalidPasswordAttempts;
        private int _minRequiredNonAlphanumericCharacters;
        private int _minRequiredPasswordLength;
        private int _passwordAttemptWindow;
        private MembershipPasswordFormat _passwordFormat;
        private string _passwordStrengthRegularExpression;
        private bool _requiresQuestionAndAnswer;
        private bool _requiresUniqueEmail;

        #region Properties

        private IQueryable<User> DataBaseUsers
        {
            get { return _dataBase.Users.Where(u => !u.IsDeleted); }
        }

        private bool WriteExceptionsToEventLog { get; set; }

        public override string ApplicationName { get; set; }

        public override bool EnablePasswordReset
        {
            get { return _enablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return _enablePasswordRetrieval; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return _requiresQuestionAndAnswer; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return _requiresUniqueEmail; }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return _maxInvalidPasswordAttempts; }
        }

        public override int PasswordAttemptWindow
        {
            get { return _passwordAttemptWindow; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return _passwordFormat; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return _minRequiredNonAlphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return _minRequiredPasswordLength; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return _passwordStrengthRegularExpression; }
        }
        #endregion

        #region Methods
        private static string GetConfigValue(string configValue, string defaultValue)
        {
            return String.IsNullOrEmpty(configValue) ? defaultValue : configValue;
        }

        private MembershipUser ConverDataBaseUserToMemberShipUser(User user)
        {
            var msUser = new MembershipUser( // creation params here
                base.Name,
                user.Username,
                user.ID,
                user.Email,
                user.PasswordQuestion,
                user.Comment, user.IsApproved,
                user.IsLockedOut,
                user.CreationDate,
                user.LastLoginDate,
                user.LastActivityDate,
                user.LastPasswordChangedDate,
                user.LastLockedOutDate);

            return msUser;
        }
        
        private void UpdateFailureCount(string username, string failureType)
        {
            try
            {
                var user = DataBaseUsers.First(u => u.Username == username);
                var windowStart = new DateTime();
                var failureCount = 0;

                switch (failureType)
                {
                    case "password":
                        failureCount = user.FailedPasswordAttemptCount;
                        windowStart = user.FailedPasswordAttemptWindowStart;
                        break;
                    case "passwordAnswer":
                        failureCount = user.FailedPasswordAnswerAttemptCount;
                        windowStart = user.FailedPasswordAnswerAttemptWindowStart;
                        break;
                }

                var windowEnd = windowStart.AddMinutes(PasswordAttemptWindow);

                if (failureCount == 0 || DateTime.Now > windowEnd)
                {
                    // First password failure or outside of PasswordAttemptWindow. 
                    // Start a new password failure count from 1 and a new window starting now.

                    switch (failureType)
                    {
                        case "password":
                            user.FailedPasswordAttemptCount = 1;
                            user.FailedPasswordAttemptWindowStart = DateTime.Now;
                            break;
                        case "passwordAnswer":
                            user.FailedPasswordAnswerAttemptCount = 1;
                            user.FailedPasswordAnswerAttemptWindowStart = DateTime.Now;
                            break;
                    }

                    // TODO: Throw the old exception.
                    //if (cmd.ExecuteNonQuery() < 0)
                    //    throw new ProviderException("Unable to update failure count and window start.");
                }
                else
                {
                    if (failureCount++ >= MaxInvalidPasswordAttempts)
                    {
                        // Password attempts have exceeded the failure threshold. Lock out
                        // the user.

                        user.IsLockedOut = true;
                        user.LastLockedOutDate = DateTime.Now;

                        // TODO: Throw the old exception.
                        //if (cmd.ExecuteNonQuery() < 0)
                        //    throw new ProviderException("Unable to lock out user.");
                    }
                    else
                    {
                        // Password attempts have not exceeded the failure threshold. Update
                        // the failure counts. Leave the window the same.

                        switch (failureType)
                        {
                            case "password":
                                user.FailedPasswordAttemptCount = failureCount;
                                break;
                            case "passwordAnswer":
                                user.FailedPasswordAnswerAttemptCount = failureCount;
                                break;
                        }

                        // TODO: Throw the old exception.
                        //if (cmd.ExecuteNonQuery() < 0)
                        //    throw new ProviderException("Unable to update failure count.");
                    }
                }
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "UpdateFailureCount");

                    throw new ProviderException(EXCEPTION_MESSAGE);
                }

                throw;
            }
        }

        private bool CheckPassword(string password, string dbPassword)
        {
            var pass1 = password;
            var pass2 = dbPassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Encrypted:
                    pass2 = UnEncodePassword(dbPassword);
                    break;
                case MembershipPasswordFormat.Hashed:
                    pass1 = EncodePassword(password);
                    break;
            }

            return pass1 == pass2;
        }
        private string GeneratePassword(int size) 
        {
            var builder = new StringBuilder();
            var random = new Random();
            
            for (var i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
       
        private string EncodePassword(string password)
        {
            if (password == null)
            {
                password = "";
            }

            var encodedPassword = password;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    encodedPassword =
                        Convert.ToBase64String(EncryptPassword(Encoding.Unicode.GetBytes(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    var hash = new HMACSHA1
                                   {
                                       Key = HexToByte(ENCRYPTION_KEY)
                                   };
                    encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                    break;
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return encodedPassword;
        }

        private string UnEncodePassword(string encodedPassword)
        {
            var password = encodedPassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    password =
                        Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException("Cannot unencode a hashed password.");
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return password;
        }

        private static byte[] HexToByte(string hexString)
        {
            var returnBytes = new byte[hexString.Length / 2];
            for (var i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return returnBytes;
        }

        private static void WriteToEventLog(Exception e, string action)
        {
            var log = new EventLog
                          {
                              Source = EVENT_SOURCE,
                              Log = EVENT_LOG,
                          };

            var message = "An exception occurred communicating with the data source.\n\n";
            message += "Action: " + action + "\n\n";
            message += "Exception: " + e;

            log.WriteEntry(message);
        }
        #endregion

        #region Override

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (string.IsNullOrEmpty(name))
            {
                name = "AuctionMembershipProvider";
            }

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "AuctionMembership provider");
            }

            base.Initialize(name, config);

            //_applicationName = GetConfigValue(config["applicationName"], HostingEnvironment.ApplicationVirtualPath);
            _maxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            _passwordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            _minRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonAlphanumericCharacters"], "1"));
            _minRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "7"));
            _passwordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));
            _enablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            _enablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));
            _requiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
            _requiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));
            WriteExceptionsToEventLog = Convert.ToBoolean(GetConfigValue(config["writeExceptionsToEventLog"], "true"));

            _passwordFormat = MembershipPasswordFormat.Hashed;
            //var tempFormat = config["passwordFormat"] ?? "Hashed";

            /*switch (tempFormat)
            {
                case "Hashed":
                    _passwordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    _passwordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    _passwordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new ProviderException("Password format not supported.");
            }*/
            this._dataBase = new MainDataBase();
        }

        public override bool ChangePassword(string username, string oldPwd, string newPwd)
        {
            if (!ValidateUser(username, oldPwd))
            {
                return false;
            }

            var args = new ValidatePasswordEventArgs(username, newPwd, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                if (args.FailureInformation != null)
                {
                    throw args.FailureInformation;
                }

                throw new MembershipPasswordException("Change password canceled due to new password validation failure.");
            }


            int rowsAffected;

            try
            {
                var user = DataBaseUsers.First(u => u.Username == username);
                user.Password = EncodePassword(newPwd);
                user.LastPasswordChangedDate = DateTime.Now;

                rowsAffected = _dataBase.SaveChanges();
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ChangePassword");
                    throw new ProviderException(EXCEPTION_MESSAGE);
                }

                throw;
            }

            return rowsAffected > 0;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPwdQuestion,
                                                             string newPwdAnswer)
        {
            if (!ValidateUser(username, password))
            {
                return false;
            }

            int rowsAffected;

            try
            {
                var user = DataBaseUsers.FirstOrDefault(u => u.Username == username);
                if (user != default(User))
                {
                    user.PasswordQuestion = newPwdQuestion;
                    user.PasswordAnswer = newPwdAnswer;
                    rowsAffected = _dataBase.SaveChanges();
                }
                else
                {
                    rowsAffected = 0;
                }
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ChangePasswordQuestionAndAnswer");

                    throw new ProviderException(EXCEPTION_MESSAGE);
                }

                throw;
            }

            return rowsAffected > 0;
        }

        public override MembershipUser CreateUser(string username, string password, string email,
                                                  string passwordQuestion, string passwordAnswer, bool isApproved,
                                                  object providerUserKey, out MembershipCreateStatus status)
        {
            var args = new ValidatePasswordEventArgs(username, password, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }


            if (RequiresUniqueEmail && !string.IsNullOrWhiteSpace(GetUserNameByEmail(email)))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                
                return null;
            }

            var u = GetUser(username, false);

            if (u == null)
            {
                var createDate = DateTime.Now;

                if (providerUserKey != null)
                {
                    if (!(providerUserKey is Guid))
                    {
                        status = MembershipCreateStatus.InvalidProviderUserKey;
                        return null;
                    }
                }

                try
                {
                    var user = new User
                                   {
                                       Username = username,
                                       Password = this.EncodePassword(password),
                                       Email = email,
                                       PasswordQuestion = passwordQuestion,
                                       PasswordAnswer = this.EncodePassword(passwordAnswer),
                                       IsApproved = isApproved,
                                       Comment = string.Empty,
                                       CreationDate = createDate,
                                       LastPasswordChangedDate = createDate,
                                       LastActivityDate = createDate,
                                       IsLockedOut = false,
                                       LastLockedOutDate = createDate,
                                       FailedPasswordAnswerAttemptCount = 0,
                                       FailedPasswordAnswerAttemptWindowStart = createDate,
                                       FailedPasswordAttemptCount = 0,
                                       FailedPasswordAttemptWindowStart = createDate
                                   };

                    _dataBase.Users.AddObject(user);
                    var recAdded = _dataBase.SaveChanges();

                    status = recAdded > 0 ? MembershipCreateStatus.Success : MembershipCreateStatus.UserRejected;
                }
                catch (Exception e)
                {
                    if (WriteExceptionsToEventLog)
                    {
                        WriteToEventLog(e, "CreateUser");
                    }

                    status = MembershipCreateStatus.ProviderError;
                }

                return GetUser(username, false);
            }

            status = MembershipCreateStatus.DuplicateUserName;

            return null;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            try
            {
                var user = DataBaseUsers.First(u => u.Username == username);
                user.IsDeleted = true;

                if (deleteAllRelatedData)
                {
                    // TODO: !!!
                }

                var rowsAffected = _dataBase.SaveChanges();
                return rowsAffected > 0;
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "DeleteUser");

                    throw new ProviderException(EXCEPTION_MESSAGE);
                }

                throw;
            }
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            try
            {
                var allUsers = DataBaseUsers;
                var usersQuantity = allUsers.Count();
                var startIndex = Math.Min(pageIndex * pageSize, usersQuantity);
                var endIndex = Math.Min((pageIndex + 1) * pageSize - 1, usersQuantity);
                var users = allUsers.OrderBy(u => u.Username).Take(endIndex).Skip(startIndex);
                var msUsers = new MembershipUserCollection();

                foreach (var user in users)
                {
                    msUsers.Add(ConverDataBaseUserToMemberShipUser(user));
                }

                totalRecords = users.Count();

                return msUsers;
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetAllUsers");

                    throw new ProviderException(EXCEPTION_MESSAGE);
                }

                throw;
            }
        }

        public override int GetNumberOfUsersOnline()
        {
            try
            {
                var onlineSpan = new TimeSpan(0, Membership.UserIsOnlineTimeWindow, 0);
                var compareTime = DateTime.Now.Subtract(onlineSpan);
                return DataBaseUsers.Count(u => u.LastActivityDate > compareTime);
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetNumberOfUsersOnline");

                    throw new ProviderException(EXCEPTION_MESSAGE);
                }

                throw;
            }
        }

        public override string GetPassword(string username, string answer)
        {
            if (!EnablePasswordRetrieval)
            {
                throw new ProviderException("Password Retrieval Not Enabled.");
            }

            if (PasswordFormat == MembershipPasswordFormat.Hashed)
            {
                throw new ProviderException("Cannot retrieve Hashed passwords.");
            }

            var user = DataBaseUsers.FirstOrDefault(u => u.Username == username);

            string password;
            string passwordAnswer;

            try
            {
                if (user != null)
                {
                    if (user.IsLockedOut)
                    {
                        throw new MembershipPasswordException("The supplied user is locked out.");
                    }

                    password = user.Password;
                    passwordAnswer = user.PasswordAnswer;
                }
                else
                {
                    throw new MembershipPasswordException("The supplied user name is not found.");
                }
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetPassword");

                    throw new ProviderException(EXCEPTION_MESSAGE);
                }

                throw;
            }


            if (RequiresQuestionAndAnswer && !CheckPassword(answer, passwordAnswer))
            {
                UpdateFailureCount(username, "passwordAnswer");

                throw new MembershipPasswordException("Incorrect password answer.");
            }


            if (PasswordFormat == MembershipPasswordFormat.Encrypted)
            {
                password = UnEncodePassword(password);
            }

            return password;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            try
            {
                var user = DataBaseUsers.FirstOrDefault(u => u.Username == username);
                if (user == null)
                {
                    return null;
                }

                var msUser = ConverDataBaseUserToMemberShipUser(user);
                if (userIsOnline)
                {
                    user.LastActivityDate = DateTime.Now;
                    _dataBase.SaveChanges();
                }

                return msUser;
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUser(String, Boolean)");

                    throw new ProviderException(EXCEPTION_MESSAGE);
                }

                throw;
            }
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            try
            {
                int uid;
                if (!int.TryParse(providerUserKey.ToString(), out uid))
                {
                    throw new FormatException("Provider key is in invalid format.");
                }

                var user = DataBaseUsers.First(u => u.ID == uid);
                var msUser = ConverDataBaseUserToMemberShipUser(user);

                if (userIsOnline)
                {
                    user.LastActivityDate = DateTime.Now;
                    _dataBase.SaveChanges();
                }

                return msUser;
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUser(Object, Boolean)");

                    throw new ProviderException(EXCEPTION_MESSAGE);
                }

                throw;
            }
        }

        public override bool UnlockUser(string username)
        {
            try
            {
                var user = DataBaseUsers.First(u => u.Username == username);
                user.IsLockedOut = false;
                user.LastLockedOutDate = DateTime.Now;
                var rowsAffected = _dataBase.SaveChanges();
                return rowsAffected > 0;
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "UnlockUser");

                    throw new ProviderException(EXCEPTION_MESSAGE);
                }

                throw;
            }
        }

        public override string GetUserNameByEmail(string email)
        {
            try
            {
                var user = DataBaseUsers.FirstOrDefault(u => u.Email == email);

                return user == null ? null : user.Username;
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUserNameByEmail");
                    throw new ProviderException(EXCEPTION_MESSAGE);
                }

                throw;
            }
        }

        public override string ResetPassword(string username, string answer)
        {
            if (!EnablePasswordReset)
            {
                throw new NotSupportedException("Password reset is not enabled.");
            }

            if (answer == null && RequiresQuestionAndAnswer)
            {
                UpdateFailureCount(username, "passwordAnswer");

                throw new ProviderException("Password answer required for password reset.");
            }

            var newPassword = Membership.GeneratePassword(NEW_PASSWORD_LENGTH, MinRequiredNonAlphanumericCharacters);

            var args = new ValidatePasswordEventArgs(username, newPassword, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                if (args.FailureInformation != null)
                {
                    throw args.FailureInformation;
                }

                throw new MembershipPasswordException("Reset password canceled due to password validation failure.");
            }

            try
            {
                var user = DataBaseUsers.FirstOrDefault(u => u.Username == username);

                string passwordAnswer;
                if (user != null)
                {
                    if (user.IsLockedOut)
                    {
                        throw new MembershipPasswordException("The supplied user is locked out.");
                    }

                    passwordAnswer = user.PasswordAnswer;
                }
                else
                {
                    throw new MembershipPasswordException("The supplied user name is not found.");
                }

                if (RequiresQuestionAndAnswer && !CheckPassword(answer, passwordAnswer))
                {
                    UpdateFailureCount(username, "passwordAnswer");

                    throw new MembershipPasswordException("Incorrect password answer.");
                }

                user.Password = EncodePassword(newPassword);
                user.LastPasswordChangedDate = DateTime.Now;

                var rowsAffected = _dataBase.SaveChanges();

                if (rowsAffected > 0)
                {
                    return newPassword;
                }

                throw new MembershipPasswordException("User not found, or user is locked out. Password not Reset.");
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ResetPassword");
                    throw new ProviderException(EXCEPTION_MESSAGE);
                }

                throw;
            }
        }

        public override bool ValidateUser(string username, string password)
        {
            try
            {
                var isValid = false;
                var user = DataBaseUsers.FirstOrDefault(u => u.Username == username && !u.IsLockedOut);

                bool isApproved;
                string dbPassword;

                if (user != null)
                {
                    dbPassword = user.Password;
                    isApproved = user.IsApproved;
                }
                else
                {
                    return false;
                }

                if (CheckPassword(password, dbPassword))
                {
                    if (isApproved)
                    {
                        isValid = true;
                        user.LastLoginDate = DateTime.Now;
                        _dataBase.SaveChanges();
                    }
                }
                else
                {
                    UpdateFailureCount(username, "password");
                }

                return isValid;
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ValidateUser");

                    throw new ProviderException(EXCEPTION_MESSAGE);
                }

                throw;
            }
        }

        public override void UpdateUser(MembershipUser user)
        {
            try
            {
                var dbUser = DataBaseUsers.First(u => u.Username == user.UserName);
                dbUser.Email = user.Email;
                dbUser.Comment = user.Comment;
                dbUser.IsApproved = user.IsApproved;
                _dataBase.SaveChanges();
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "UpdateUser");

                    throw new ProviderException(EXCEPTION_MESSAGE);
                }

                throw;
            }
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
                                                                 out int totalRecords)
        {
            try
            {
                //var startIndex = pageIndex * pageSize;
                //var endIndex = (pageIndex + 1) * pageSize - 1;
                //var allUsers = this.DataBaseUsers.Where(u => SqlMethods.Like(u.Email, usernameToMatch));
                //var users = allUsers.Take(endIndex).Skip(startIndex);
                //var msUsers = new MembershipUserCollection();

                //foreach (var user in users)
                //{
                //    msUsers.Add(this.ConverDataBaseUserToMemberShipUser(user));
                //}

                //totalRecords = users.Count();

                //return msUsers;
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "FindUsersByName");

                    throw new ProviderException(EXCEPTION_MESSAGE);
                }

                throw;
            }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            try
            {
                //var startIndex = pageIndex * pageSize;
                //var endIndex = (pageIndex + 1) * pageSize - 1;
                //var allUsers = this.DataBaseUsers.Where(u => SqlMethods.Like(u.Email, emailToMatch));
                //var users = allUsers.Take(endIndex).Skip(startIndex);
                //var msUsers = new MembershipUserCollection();

                //foreach (var user in users)
                //{
                //    msUsers.Add(this.ConverDataBaseUserToMemberShipUser(user));
                //}

                //totalRecords = users.Count();

                //return msUsers;
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "FindUsersByEmail");

                    throw new ProviderException(EXCEPTION_MESSAGE);
                }

                throw;
            }
        }

        #endregion
    }
}