using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacious.Identity.Contracts.DTOs;

public class ResetPasswordRequest
{
    public string Email { get; set; }
    public string ResetCode { get; set; }
    public string NewPassword { get; set; }
}
