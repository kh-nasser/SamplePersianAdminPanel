using Common.DataModel.DTO.Communication;
using Common.DataModel.DTO.Dashboard.UserDTO;
using System.Collections.Generic;

namespace BissinessLogic.Client
{
    public class RegisteredUserBLL
    {
        private readonly DataAccess.Client.RegisteredUserDal _registeredUserDal = new DataAccess.Client.RegisteredUserDal();

        public List<RegisteredUserDto> GetAll() {
            return _registeredUserDal.GetAll();
        }
    }
}
