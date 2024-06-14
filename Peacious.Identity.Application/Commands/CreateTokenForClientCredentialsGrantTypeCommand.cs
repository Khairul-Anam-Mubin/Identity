using Peacious.Framework.CQRS;
using Peacious.Identity.Contracts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacious.Identity.Application.Commands;

public record CreateTokenForClientCredentialsGrantTypeCommand(
    [Required] string ClientId,
    [Required] string ClientSecret) : ICommand;