﻿using Firebase.Auth;

namespace PloggingAPI.Features.UserInformation;

public interface IUserInfoRepository
{
    Task CreateUser(UserInfo user);
    //Task DeleteUser(string userId);
    //Task<UserInfo> GetUser(string userId);
}
