using Auth0Con.Services;
using AutoMapper;
using ConsoleToolkit.CommandLineInterpretation.ConfigurationAttributes;
using ConsoleToolkit.ConsoleIO;
using System;
using System.Linq;

namespace Auth0Con.Commands.UserCommands
{
    [Keyword("user", "User operations")]
    [Command("list")]
    [Description("Display a list of the users configured in Auth0")]
    public class UserListCommand
    {
        [Positional]
        [Description("Your client ID for this application")]
        public string ClientId { get; set; }

        [Positional]
        [Description("Your secret for this application")]
        public string Secret { get; set; }

        [CommandHandler]
        public void Handle(IConsoleAdapter console, IErrorAdapter error, IMapper mapper)
        {
            var ops = new UserOperations("https://senlabltd.eu.auth0.com/api/v2/", ClientId, Secret, mapper);
            try
            {
                console.FormatTable(ops.GetAllUsers().Select(c => new { c.Email, c.UserId }));
            } 
            catch (AggregateException e)
            {
                error.WrapLine($"{e.InnerException.Message.Red()} (Aggregate)");
            }
            catch (Exception e)
            {
                error.WrapLine(e.Message.Red());
            }
        }
    }
}