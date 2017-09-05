using Auth0Con.Commands;
using Auth0Con.Services;
using AutoMapper;
using ConsoleToolkit;
using ConsoleToolkit.ApplicationStyles;

namespace Auth0Con
{
    internal class Program : CommandDrivenApplication
    {
        static void Main(string[] args)
        {
            Toolkit.Execute<Program>(args);
        }

        #region Overrides of ConsoleApplicationBase

        protected override void Initialise()
        {
            HelpCommand<HelpCommand>(h => h.Topic);

            var config = new MapperConfiguration(cfg => UserOperations.Map(cfg));
            RegisterInjectionInstance(config.CreateMapper());

            base.Initialise();
        }

        #endregion
    }
}
