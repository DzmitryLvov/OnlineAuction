﻿using OnlineAuction.Buisness.Data;

namespace OnlineAuction.Buisness.Models.Lot
{
    public class CommentViewModel : CommentInfo
    {
        public string ImageUrl
        {
            get
            {
                return new DataAccess().IndexImageExits(ID, "UserProfile")
                                ? "Content/Image/UserProfile/" + ID + "/index.jpg" // on future
                                : "Content/Image/UserProfile/Default/index.jpg";
            }
        }
    }
}