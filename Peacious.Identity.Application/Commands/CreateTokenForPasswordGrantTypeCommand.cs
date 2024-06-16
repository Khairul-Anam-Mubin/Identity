using Peacious.Framework.CQRS;
using System.ComponentModel.DataAnnotations;

namespace Peacious.Identity.Application.Commands;

public record CreateTokenForPasswordGrantTypeCommand(
    [Required] string ClientId,
    [Required] string UserName,
    [Required] string Password) : ICommand;