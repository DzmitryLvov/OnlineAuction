using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web.Security;
using OnlineAuction.Buisness.Data;

namespace OnlineAuction.Buisness.Security
{
    // ReSharper disable ClassNeverInstantiated.Global
    public sealed class AuctionRoleProvider : RoleProvider
    {
        private const string EVENT_SOURCE = "AuctionRoleProvider";

        private const string EVENT_LOG = "Application";

        private const string EXCEPTION_MESSAGE = "An exception occurred. Please check the Event Log.";

        private MainDataBase _dataBase;

        private bool WriteExceptionsToEventLog { get; set; }

        private ParallelQuery<Role> DataBaseRoles
        {
            get { return _dataBase.Roles.Where(r => !r.IsDeleted).AsParallel(); }
        }

        private ParallelQuery<User> DataBaseUsers
        {
            get { return _dataBase.Users.Where(u => !u.IsDeleted).AsParallel(); }
        }

        public override string ApplicationName { get; set; }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (name.Length == 0)
            {
                name = "AuctionRoleProvider";
            }

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Sample SQLite Role provider");
            }

            base.Initialize(name, config);

            if (config["applicationName"] == null || config["applicationName"].Trim() == "")
            {
                ApplicationName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
            }
            else
            {
                ApplicationName = config["applicationName"];
            }


            if (config["writeExceptionsToEventLog"] != null)
            {
                if (config["writeExceptionsToEventLog"].ToUpper() == "TRUE")
                {
                    WriteExceptionsToEventLog = true;
                }
            }

            this._dataBase = new MainDataBase();
        }

        public override void AddUsersToRoles(string[] usernames, string[] rolenames)
        {
            if (rolenames.Any(rolename => !RoleExists(rolename)))
            {
                throw new ProviderException("Role name not found.");
            }

            foreach (var username in usernames)
            {
                if (username.IndexOf(',') > 0)
                {
                    throw new ArgumentException("User names cannot contain commas.");
                }

                if (rolenames.Any(rolename => IsUserInRole(username, rolename)))
                {
                    throw new ProviderException("User is already in role.");
                }
            }

            try
            {
                var roles = this.DataBaseRoles.Where(r => rolenames.Any(rn => rn == r.Rolename));
                var users = this.DataBaseUsers.Where(u => rolenames.Any(un => un == u.Username));

                foreach (var role in roles)
                {
                    foreach (var user in users)
                    {
                        role.Users.Add(user);
                    }
                }

                this._dataBase.SaveChanges();
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "AddUsersToRoles");
                }

                throw;
            }
        }

        public override void CreateRole(string rolename)
        {
            if (rolename.IndexOf(',') > 0)
            {
                throw new ArgumentException("Role names cannot contain commas.");
            }

            if (RoleExists(rolename))
            {
                throw new ProviderException("Role name already exists.");
            }

            try
            {
                var role = new Role { Rolename = rolename };

                this._dataBase.Roles.Add(role);
                this._dataBase.SaveChanges();
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "CreateRole");
                }

                throw;
            }
        }

        public override bool DeleteRole(string rolename, bool throwOnPopulatedRole)
        {
            if (!RoleExists(rolename))
            {
                throw new ProviderException("Role does not exist.");
            }

            if (throwOnPopulatedRole && GetUsersInRole(rolename).Length > 0)
            {
                throw new ProviderException("Cannot delete a populated role.");
            }

            try
            {
                var role = this._dataBase.Roles.First(r => r.Rolename == rolename);
                role.IsDeleted = true;
                foreach (var user in role.Users)
                {
                    user.Role = null;
                }
                this._dataBase.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "DeleteRole");

                    return false;
                }

                throw;
            }
        }

        public override string[] GetAllRoles()
        {
            try
            {
                var role = this.DataBaseRoles.Select(r => r.Rolename).ToArray();
                return role.Length > 0 ? role : new string[0];
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetAllRoles");
                }

                throw;
            }
        }

        public override string[] GetRolesForUser(string username)
        {
            try
            {
                var user = this._dataBase.Users.First(u => string.Compare(u.Username, username, StringComparison.InvariantCultureIgnoreCase) == 0);
                var role = user.Role.Rolename;
                return new string[]{role};
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetRolesForUser");
                }

                throw;
            }
        }

        public override string[] GetUsersInRole(string rolename)
        {
            try
            {
                return this.DataBaseRoles.First(r => r.Rolename == rolename).Users.Select(u => u.Username).ToArray();
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUsersInRole");
                }
                throw;
            }
        }

        public override bool IsUserInRole(string username, string rolename)
        {
            try
            {
                return this.DataBaseUsers.First(u => u.Username == username).Role.Rolename == rolename;
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "IsUserInRole");
                }

                throw;
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] rolenames)
        {
            try
            {
                if (rolenames.Any(rolename => !RoleExists(rolename)))
                {
                    throw new ProviderException("Role name not found.");
                }

                if (usernames.Any(username => rolenames.Any(rolename => !IsUserInRole(username, rolename))))
                {
                    throw new ProviderException("User is not in role.");
                }

                var users = this.DataBaseUsers.Where(u => usernames.Any(un => un == u.Username));
                var roles = this.DataBaseRoles.Where(r => rolenames.Any(rn => rn == r.Rolename));

                foreach (var role in roles)
                {
                    foreach (var user in users)
                    {
                        role.Users.Remove(user);
                    }
                }

            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "RemoveUsersFromRoles");
                }

                throw;
            }
        }

        public override bool RoleExists(string rolename)
        {
            try
            {
                return this.DataBaseRoles.Any(r => r.Rolename == rolename);
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "RoleExists");
                }

                throw;
            }
        }

        public override string[] FindUsersInRole(string rolename, string usernameToMatch)
        {
            throw new NotImplementedException();

            //SQLiteConnection conn = new SQLiteConnection(_connectionString);
            //SQLiteCommand cmd = new SQLiteCommand("SELECT Username FROM `" + USERS_IN_ROLES_TABLE + "` " +
            //                                      "WHERE Username LIKE $UsernameSearch AND Rolename = $Rolename AND ApplicationName = $ApplicationName",
            //                                      conn);
            //cmd.Parameters.Add("$UsernameSearch", DbType.String, 255).Value = usernameToMatch;
            //cmd.Parameters.Add("$RoleName", DbType.String, 255).Value = rolename;
            //cmd.Parameters.Add("$ApplicationName", DbType.String, 255).Value = ApplicationName;

            //string tmpUserNames = "";
            //SQLiteDataReader reader = null;

            //try
            //{
            //    conn.Open();

            //    reader = cmd.ExecuteReader();

            //    while (reader.Read())
            //    {
            //        tmpUserNames += reader.GetString(0) + ",";
            //    }
            //}
            //catch (SqlException e)
            //{
            //    if (WriteExceptionsToEventLog)
            //    {
            //        WriteToEventLog(e, "FindUsersInRole");
            //    }
            //    else
            //    {
            //        throw e;
            //    }
            //}
            //finally
            //{
            //    if (reader != null)
            //    {
            //        reader.Close();
            //    }

            //    conn.Close();
            //}

            //if (tmpUserNames.Length > 0)
            //{
            //    // Remove trailing comma.
            //    tmpUserNames = tmpUserNames.Substring(0, tmpUserNames.Length - 1);
            //    return tmpUserNames.Split(',');
            //}

            //return new string[0];
        }

        /// <summary>A helper function that writes exception detail to the event log. Exceptions are written to the event log as a security measure to avoid private database details from being returned to the browser. If a method does not return a status or boolean indicating the action succeeded or failed, a generic exception is also  thrown by the caller. </summary>
        private static void WriteToEventLog(Exception e, string action)
        {
            var log = new EventLog
                          {
                              Source = EVENT_SOURCE,
                              Log = EVENT_LOG
                          };

            var message = EXCEPTION_MESSAGE + "\n\n";
            message += "Action: " + action + "\n\n";
            message += "Exception: " + e;

            log.WriteEntry(message);
        }
    }
}