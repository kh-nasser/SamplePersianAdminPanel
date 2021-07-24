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