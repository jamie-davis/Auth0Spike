using System.Collections.Generic;
using ConsoleToolkit.CommandLineInterpretation.ConfigurationAttributes;

namespace Auth0Con.Commands
{
    [Command]
    [Description("Display command help")]
    internal class HelpCommand
    {
        [Positional(DefaultValue = null)]
        [Description("The topic with which you need help.")]
        public List<string> Topic { get; set; }
    }
}