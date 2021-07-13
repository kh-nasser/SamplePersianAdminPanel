using Common.DataModel.Domain.Models;
using System;
using System.Linq;

namespace DataAccess.Client.Authorization
{
    public class Authorization
    {
        public Guid UserToken(int? userId)
        {
            using (var usersEntities = new PersianAdminPanelEntities())
            {
                Guid userToken = usersEntities.UserActivations.Where(c => c.UserId == userId.Value).Select(x => x.ActivationCode).First();

                return userToken;
            }
        }

        public int? Signin(UserSignin user)
        {
            using (var usersEntities = new PersianAdminPanelEntities())
            {
                int? userId = usersEntities.ValidateUser(user.Username, user.Password).FirstOrDefault();
                return userId;

            }
        }

        public int Signup(UserSignup user)
        {
            using (var usersEntities = new PersianAdminPanelEntities())
            {
                usersEntities.Users.Add(new User()
                {
                    CreatedDate = user.CreatedDate,
                    Email = user.Email,
                    LastLoginDate = user.LastLoginDate,
                    Password = user.Password,
                    UserId = user.UserId,
                    Username = user.Username,
                });

                usersEntities.SaveChanges();

            }
            return user.UserId;
        }

        public bool ValidateUserToken(long userId, Guid userToken)
        {
            bool isAuthorized = false;
            using (var usersEntities = new PersianAdminPanelEntities())
            {
                long id = usersEntities.UserActivations.Where(c => c.ActivationCode == userToken).Select(x => x.UserId).FirstOrDefault();

                isAuthorized = id == userId;
            }
            return isAuthorized;
        }
    }
}
