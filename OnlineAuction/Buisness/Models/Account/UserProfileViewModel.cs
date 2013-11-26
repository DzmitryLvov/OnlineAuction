using OnlineAuction.Buisness.Data;

namespace OnlineAuction.Buisness.Models.Account
{
    public class UserProfileViewModel: User
    {
        
        public string ImageUrl{get
        {
            return new DataAccess().IndexImageExits(ID, "UserProfile")
                            ? "Content/Image/UserProfile/" + ID + "/index.jpg" // on future
                            : "Content/Image/UserProfile/Default/index.jpg";
        }}
    }
}