using Common.DataModel.Domain.Models;
using Common.DataModel.DTO.Communication;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Client.Authorization
{
    public class Authorization
    {
        private readonly DataAccess.Client.Authorization.Authorization _authorizationRepository = new DataAccess.Client.Authorization.Authorization();

        public BaseResponse<Dictionary<string, string>> Signin(UserSignin user)
        {
            user.Password = new Common.Utils.Hash().GetMD5Hash(user.Password);
            int? userId = _authorizationRepository.Signin(user);
            var dict = new Dictionary<string, string>();

            string message = string.Empty;
            switch (userId.Value)
            {
                case -1:
                    message = "Username and/or password is incorrect.";
                    break;
                case -2:
                    message = "Account has not been activated.";
                    break;
                default:
                    {
                        Guid userToken = _authorizationRepository.UserToken(userId);

                        dict.Add("issued", DateTime.Now.ToString());
                        DateTime expDate = DateTime.Now.AddYears(1);
                        dict.Add("exp", expDate.ToString());
                        dict.Add("Username", user.Username);
                        dict.Add("UserId", userId.Value.ToString());
                        dict.Add("Token", userToken.ToString());
                        break;
                    }
            }
            return
                string.IsNullOrEmpty(message) ? new BaseResponse<Dictionary<string, string>>(dict) : new BaseResponse<Dictionary<string, string>>(message);
        }

        public bool ValidateUserToken(long userId, Guid userToken)
        {
            return _authorizationRepository.ValidateUserToken(userId, userToken);
        }

        public BaseResponse<long> Signup(UserSignup user)
        {
            user.Password = new Common.Utils.Hash().GetMD5Hash(user.Password);
            var userId = _authorizationRepository.Signup(user);
            string message;

            switch (userId)
            {
                case -1:
                    message = "Username already exists.\\nPlease choose a different username.";
                    break;
                case -2:
                    message = "Supplied email address has already been used.";
                    break;
                default:
                    message = "";
                    break;
            }

            return string.IsNullOrEmpty(message) ? new BaseResponse<long>(userId) : new BaseResponse<long>(message);
        }
    }
}
