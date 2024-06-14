﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Peacious.Identity.Contracts.Models;

public class Token
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

    private Token(string tokenType, string accessToken, long expiresIn, string? refreshToken, string? scope)
    {
        TokenType = tokenType;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        ExpiresIn = expiresIn;
        Scope = scope;
    }

    public static Token Create(string tokenType, string accessToken, long expiresIn, string? refreshToken, string? scope)
    {
        return new Token(tokenType, accessToken, expiresIn, refreshToken, scope);
    }
}