using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacious.Identity.Contracts.Constants;

public class GrantType
{
    public const string Password = "password";
    public const string RefreshToken = "refresh_token";
    public const string ClientCredentials = "client_credentials";
    public const string AuthorizationCode = "authorization_code";
}
