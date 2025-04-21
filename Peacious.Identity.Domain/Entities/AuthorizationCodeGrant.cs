using Peacious.Framework.DDD;
using Peacious.Framework.ORM.Interfaces;
using Peacious.Framework.Security;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using Peacious.Framework.Results;
using Peacious.Identity.Domain.Errors;
using Peacious.Framework.Results.Errors;

namespace Peacious.Identity.Domain.Entities;

public class AuthorizationCodeGrant : Entity, IRepositoryItem
{
    public string UserId { get; private set; }
    public string ClientId { get; private set; }
    public string Code { get; private set; }
    public string? Scope { get; private set; }
    public string? CodeChallenge { get; private set; }
    public string? CodeChallengeMethod { get; private set; }
    public DateTime ExpireAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsUsed { get; private set; }

    private AuthorizationCodeGrant(string userId, string clientId, string code, string? scope, DateTime expireAt, string? codeChallenge, string? codeChallengeMethod) 
        : base(GetId(clientId, code))
    {
        UserId = userId;
        ClientId = clientId;
        Code = code;
        Scope = scope;
        ExpireAt = expireAt;
        CodeChallenge = codeChallenge;
        CodeChallengeMethod = codeChallengeMethod;
        CreatedAt = DateTime.UtcNow;
        IsUsed = false;
    }

    public static AuthorizationCodeGrant Create(string userId, string clientId, string? scope, DateTime expireAt, string? codeChallenge, string? codeChallengeMethod, int codeLength = 16)
    {
        var code = RandomNumberGenerator.GetHexString(codeLength);

        return new AuthorizationCodeGrant(userId, clientId, code, scope, expireAt, codeChallenge, codeChallengeMethod);
    }

    public bool HasCodeChallenge()
    {
        return !string.IsNullOrEmpty(CodeChallenge) 
            || !string.IsNullOrEmpty(CodeChallengeMethod);
    }

    public static string GenerateCodeChallenge(string codeVerifier)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
        var b64Hash = Convert.ToBase64String(hash);
        var code = Regex.Replace(b64Hash, "\\+", "-");
        code = Regex.Replace(code, "\\/", "_");
        code = Regex.Replace(code, "=+$", "");
        return code;
    }

    public IResult VerifyCodeChallenge(string? codeVerifier)
    {
        if (!HasCodeChallenge())
        {
            return Result.Success();
        }

        if (string.IsNullOrEmpty(codeVerifier))
        {
            return OAuthError.InvalidRequest("code_verifier").Result();
        }

        if (CodeChallengeMethod == "S256")
        {
            var codeChallenge = GenerateCodeChallenge(codeVerifier);
            if (codeChallenge.Equals(CodeChallenge))
            {
                return Result.Success();
            }
        }
        else if (CodeChallengeMethod == "plain")
        {
            if (codeVerifier.Equals(CodeChallenge))
            {
                return Result.Success();
            }
        } 
        else
        {
            return OAuthError.InvalidRequest("code_challenge_method").Result();
        }

        return OAuthError.InvalidRequest("code_verifier").Result();
    }

    public bool IsExpired()
    {
        return DateTime.UtcNow > ExpireAt;
    }

    public void SetAsUsed()
    {
        IsUsed = true;
    }

    public static string GetId(string clientId, string code)
    {
        return CheckSumGenerator.GetCheckSum($"{clientId}-{code}");
    }

    public string GetRepositoryName()
    {
        return nameof(AuthorizationCodeGrant);
    }
}
