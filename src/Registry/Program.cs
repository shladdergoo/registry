namespace Registry
{
    using System;

    using Microsoft.Extensions.CommandLineUtils;

    public class Program
    {
        private const string HelpOptionTemplate = "-? | --help";

        public static int Main(string[] args)
        {
            CommandLineApplication commandLineApplication =
                new CommandLineApplication();
            commandLineApplication.HelpOption(HelpOptionTemplate);
            commandLineApplication.Command("getlatestversiontag", GetLatestVersionTagCommand.Configure);

            int retVal = -1;
            if (args.Length == 0) { commandLineApplication.ShowHelp(); }

            try
            {
                retVal = commandLineApplication.Execute(args);
            }
            catch (CommandParsingException)
            {
                commandLineApplication.ShowHelp();
            }
            finally
            {
                Console.Out.Flush();
            }

            return retVal;
        }
    }
}
