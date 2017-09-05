using Auth0Con.Services;
using AutoMapper;
using ConsoleToolkit.CommandLineInterpretation.ConfigurationAttributes;
using ConsoleToolkit.ConsoleIO;
using System;
using System.Linq;

namespace Auth0Con.Commands.UserCommands
{
    [Keyword("user")]
    [Command("block")]
    [Description("Block a user in Auth0")]
    public class UserBlockCommand
    {
        [Positional]
        [Description("Your client ID for this application")]
        public string ClientId { get; set; }

        [Positional]
        [Description("Your secret for this application")]
        public string Secret { get; set; }

        [Positional]
        [Description("The Auth0 ID of the user to block")]
        public string UserId { get; set; }

        [CommandHandler]
        public void Handle(IConsoleAdapter console, IErrorAdapter error, IMapper mapper)
        {
            try
            {
                var ops = new UserOperations("https://senlabltd.eu.auth0.com/api/v2/", ClientId, Secret, mapper);
                ops.BlockUser(UserId);
                console.WrapLine($"{UserId} blocked.".Cyan());
            }
            catch (AggregateException e)
            {
                error.WrapLine($"Unable to block user {UserId} due to error:".Yellow());

                error.WrapLine(e.Message.Red());

                foreach (var exception in e.InnerExceptions)
                {
                    error.WrapLine(exception.Message.Red());
                }

                Environment.ExitCode = -100;
            }
            catch (Exception e)
            {
                error.WrapLine($"Unable to block user {UserId}s due to error:".Yellow());

                error.WrapLine(e.Message.Red());
                if (e.InnerException != null)
                    error.WrapLine(e.InnerException.Message.Red());

                Environment.ExitCode = -100;
            }
        }
    }
    [Keyword("user")]
    [Command("unblock")]
    [Description("Unblock a user in Auth0")]
    public class UserUnblockCommand
    {
        [Positional]
        [Description("Your client ID for this application")]
        public string ClientId { get; set; }

        [Positional]
        [Description("Your secret for this application")]
        public string Secret { get; set; }

        [Positional]
        [Description("The Auth0 ID of the user to unblock")]
        public string UserId { get; set; }

        [CommandHandler]
        public void Handle(IConsoleAdapter console, IErrorAdapter error, IMapper mapper)
        {
            try
            {
                var ops = new UserOperations("https://senlabltd.eu.auth0.com/api/v2/", ClientId, Secret, mapper);
                ops.UnblockUser(UserId);
                console.WrapLine($"{UserId} unblocked.".Cyan());
            }
            catch (AggregateException e)
            {
                error.WrapLine($"Unable to unblock user {UserId} due to error:".Yellow());

                error.WrapLine(e.Message.Red());

                foreach (var exception in e.InnerExceptions)
                {
                    error.WrapLine(exception.Message.Red());
                }

                Environment.ExitCode = -100;
            }
            catch (Exception e)
            {
                error.WrapLine($"Unable to unblock user {UserId}s due to error:".Yellow());

                error.WrapLine(e.Message.Red());
                if (e.InnerException != null)
                    error.WrapLine(e.InnerException.Message.Red());

                Environment.ExitCode = -100;
            }
        }
    }
}