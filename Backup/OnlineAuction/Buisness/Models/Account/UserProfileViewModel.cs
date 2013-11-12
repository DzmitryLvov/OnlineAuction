using OnlineAuction.Buisness.Data;

namespace OnlineAuction.Buisness.Models.Account
{
    public class UserProfileViewModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Firstname { get; set; }

        public string LastName { get; set; }

        public string Location { get; set; }

        public string Phone { get; set; }

        public string ImageUrl{get
        {
            return new DataAccess().IndexImageExits(ID, "UserProfile")
                            ? "Content/Image/UserProfile/" + ID.ToString() + "/index.jpg" // on future
                            : "Content/Image/UserProfile/Default/index.jpg";
        }}
    }
}