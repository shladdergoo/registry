namespace Registry
{
    using System;

    using Microsoft.Extensions.CommandLineUtils;

    using Registry.Docker;

    internal static class GetLatestVersionTagCommand
    {
        private const string HelpOptionTemplate = "-? | --help";

        public static void Configure(CommandLineApplication command)
        {
            CommandArgument registry = command.Argument(
                "registry",
                "the regsitry in which to find the repository");

            CommandArgument repository = command.Argument(
                "repository",
                "the repository whose tags will be read");

            CommandArgument registryUsername = command.Argument(
                "registryUsername",
                "the registry username");

            CommandArgument registryPassword = command.Argument(
                "registryPassword",
                "the registry password");

            command.HelpOption(HelpOptionTemplate);

            command.OnExecute(() =>
                {
                    if (registry.Value == null || repository.Value == null || registryUsername == null || registryPassword == null)
                    {
                        command.ShowHelp();
                        return -1;
                    }

                    try
                    {
                        RegistryService registryService = new RegistryService(
                            new HttpRegistryRepository(new HttpClient()),
                            new ThreePartVersionMatcher());

                        Console.Out.WriteLine(registryService.GetLatestVersionTag(
                            registry.Value,
                            repository.Value,
                            registryUsername.Value,
                            registryPassword.Value));

                        return 0;
                    }
                    catch (Exception ex)
                    {
                        Console.Out.WriteLine(ex);

                        return -1;
                    }
                });
        }
    }
}
