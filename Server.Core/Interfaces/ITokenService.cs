﻿using Server.Core.Entities;

namespace Server.Core.Interfaces;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(User user, DateTime expireTime);
    Task<bool> CheckTokenIsValid(string token);
    string GenerateRefreshToken();
}