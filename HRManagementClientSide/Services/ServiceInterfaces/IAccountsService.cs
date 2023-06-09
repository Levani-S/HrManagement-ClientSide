﻿using HRManagementClientSide.Models;

namespace HRManagementClientSide.Services.ServiceInterfaces
{
    public interface IAccountsService
    {
        Task<bool> LogIn(UserLoginViewModel model);
        Task<bool> Register(UserRegistrationViewModel model);
        Task<bool> CheckTokenValidation(string token);
        Task<bool> GetNewTokens();
        void LogOut();
    }
}
