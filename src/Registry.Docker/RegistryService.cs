namespace Registry.Docker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class RegistryService
    {
        private readonly IRegistryRepository registryRepository;
        private readonly IVersionMatcher versionMatcher;

        public RegistryService(IRegistryRepository registryRepository, IVersionMatcher versionMatcher)
        {
            if (registryRepository == null) { throw new System.ArgumentNullException(nameof(registryRepository)); }
            if (versionMatcher == null) { throw new ArgumentNullException(nameof(versionMatcher)); }

            this.registryRepository = registryRepository;
            this.versionMatcher = versionMatcher;
        }

        public string GetLatestVersionTag(
            string registry,
            string repository,
            string registryUsername,
            string registryPassword)
        {
            if (string.IsNullOrWhiteSpace(registry)) { throw new ArgumentException("message", nameof(registry)); }
            if (string.IsNullOrWhiteSpace(repository)) { throw new ArgumentException("message", nameof(repository)); }
            if (string.IsNullOrWhiteSpace(registryUsername)) { throw new ArgumentException("message", nameof(registryUsername)); }
            if (string.IsNullOrWhiteSpace(registryPassword)) { throw new ArgumentException("message", nameof(registryPassword)); }

            IEnumerable<string> tags =
                this.registryRepository.GetTags(registry, repository, registryUsername, registryPassword);

            if (!tags.Any()) { return null; }

            IDictionary<Version, string> versions = this.versionMatcher.GetVersions(tags);

            if (!versions.Any()) { return null; }

            return this.GetLatestVersionTag(versions);
        }

        private string GetLatestVersionTag(IDictionary<Version, string> versions)
        {
            SortedDictionary<Version, string> sortedVersions = new SortedDictionary<Version, string>(versions);

            return sortedVersions.Last().Value;
        }
    }
}
