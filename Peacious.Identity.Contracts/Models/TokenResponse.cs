﻿using System.Text.Json.Serialization;

namespace Peacious.Identity.Contracts.Models;

public class TokenResponse
{
    [JsonPropertyName("token_type")]
    public string TokenType { get; private set; }

    [JsonPropertyName("access_token")]
    public string AccessToken { get; private set; }

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; private set; }

    [JsonPropertyName("expires_in")]
    public long ExpiresIn { get; private set; }

    [JsonPropertyName("scope")]
    public string? Scope { get; private set; }

    public TokenResponse(string tokenType, string accessToken, long expiresIn, string? refreshToken, string? scope)
    {
        TokenType = tokenType;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        ExpiresIn = expiresIn;
        Scope = scope;
    }
}