using System;
using Auth0Con.Services;
using AutoMapper;
using ConsoleToolkit.CommandLineInterpretation.ConfigurationAttributes;
using ConsoleToolkit.ConsoleIO;

namespace Auth0Con.Commands.UserCommands
{
    [Keyword("user")]
    [Command("add")]
    [Description("Add a new user to the system. Then user will be created in Auth0 if it does not already exist.")]
    public class UserAddCommand
    {
        [Positional]
        [Description("Your client ID for this application")]
        public string ClientId { get; set; }

        [Positional]
        [Description("Your secret for this application")]
        public string Secret { get; set; }

        [Positional]
        [Description("The user's email")]
        public string Email { get; set; }

        [Positional]
        [Description("The user's password")]
        public string Password { get; set; }

        [CommandHandler]
        public void Handle(IConsoleAdapter console, IErrorAdapter error, IMapper mapper)
        {
            try
            {
                var ops = new UserOperations("https://senlabltd.eu.auth0.com/api/v2/", ClientId, Secret, mapper);
                var user = ops.AddUser(Email, Password);
                console.WrapLine($"{user.UserId} created.".Cyan());

            }
            catch (AggregateException e)
            {
                error.WrapLine($"Unable to create user {Email} due to error:".Yellow());

                error.WrapLine(e.Message.Red());

                foreach (var exception in e.InnerExceptions)
                {
                    error.WrapLine(exception.Message.Red());
                }

                Environment.ExitCode = -100;
            }
            catch (Exception e)
            {
                error.WrapLine($"Unable to create user {Email} due to error:".Yellow());

                error.WrapLine(e.Message.Red());
                if (e.InnerException != null)
                    error.WrapLine(e.InnerException.Message.Red());

                Environment.ExitCode = -100;
            }
        }
    }
}
