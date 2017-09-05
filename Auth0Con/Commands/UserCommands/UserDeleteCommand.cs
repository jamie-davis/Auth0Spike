using System;
using Auth0Con.Services;
using AutoMapper;
using ConsoleToolkit.CommandLineInterpretation.ConfigurationAttributes;
using ConsoleToolkit.ConsoleIO;

namespace Auth0Con.Commands.UserCommands
{
    [Keyword("user")]
    [Command("del")]
    [Description("Delete a user from the system.")]
    public class UserDeleteCommand
    {
        [Positional]
        [Description("Your client ID for this application")]
        public string ClientId { get; set; }

        [Positional]
        [Description("Your secret for this application")]
        public string Secret { get; set; }

        [Positional]
        [Description("The user's Auth0 ID")]
        public string UserId { get; set; }

        [CommandHandler]
        public void Handle(IConsoleAdapter console, IErrorAdapter error, IMapper mapper)
        {
            try
            {
                var ops = new UserOperations("https://senlabltd.eu.auth0.com/api/v2/", ClientId, Secret, mapper);
                ops.DeleteUser(UserId);
                console.WrapLine($"{UserId} deleted.".Cyan());
            }
            catch (AggregateException e)
            {
                error.WrapLine($"Unable to delete user {UserId} due to error:".Yellow());

                error.WrapLine(e.Message.Red());

                foreach (var exception in e.InnerExceptions)
                {
                    error.WrapLine(exception.Message.Red());
                }

                Environment.ExitCode = -100;
            }
            catch (Exception e)
            {
                error.WrapLine($"Unable to delete user {UserId}s due to error:".Yellow());

                error.WrapLine(e.Message.Red());
                if (e.InnerException != null)
                    error.WrapLine(e.InnerException.Message.Red());

                Environment.ExitCode = -100;
            }
        }
    }
}