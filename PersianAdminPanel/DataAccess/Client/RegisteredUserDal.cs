using Common.DataModel.DTO.Dashboard.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Client
{
    public class RegisteredUserDal
    {
        public RegisteredUserDto Read(long userId)
        {
            RegisteredUserDto result;
            using (var usersEntities = new PersianAdminPanelEntities())
            {
                var user = usersEntities.Users.AsNoTracking().Where(x => x.UserId == userId).Single();

                result = new RegisteredUserDto()
                {
                    CreatedDate = user.CreatedDate,
                    Email = user.Email,
                    LastLoginDate = user.LastLoginDate,
                    UserId = user.UserId,
                    Username = user.Username,
                    RoleId = user.FK_User_tblRole
                };

                
            }
            return result;
        }
        public List<RegisteredUserDto> GetAll()
        {
            var userList = new List<RegisteredUserDto>();
            using (var usersEntities = new PersianAdminPanelEntities())
            {
                var users = usersEntities.Users.AsNoTracking().ToList().Select(x => new RegisteredUserDto()
                {
                    CreatedDate = x.CreatedDate,
                    Email = x.Email,
                    LastLoginDate = x.LastLoginDate,
                    UserId = x.UserId,
                    Username = x.Username,
                });

                if (users != null)
                {
                    userList.AddRange(users);
                }
            }
            return userList;
        }
    }
}