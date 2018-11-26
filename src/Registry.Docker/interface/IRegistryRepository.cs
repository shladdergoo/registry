namespace Registry.Docker
{
    using System.Collections.Generic;

    public interface IRegistryRepository
    {
        IEnumerable<string> GetTags(
            string registry,
            string repository,
            string registryUsername,
            string registryPassword);
    }
}
