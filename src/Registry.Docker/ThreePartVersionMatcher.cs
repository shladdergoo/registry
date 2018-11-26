namespace Registry.Docker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ThreePartVersionMatcher : IVersionMatcher
    {
        public IDictionary<Version, string> GetVersions(IEnumerable<string> candidates)
        {
            if (candidates == null) { throw new ArgumentNullException(nameof(candidates)); }

            IEnumerable<Tuple<Version, string>> candidateVersions =
                candidates.Where(this.IsValidVersion).Select(v => new Tuple<Version, string>(new Version(v), v));

            IDictionary<Version, string> versions = new Dictionary<Version, string>();
            foreach (Tuple<Version, string> candidateVersion in candidateVersions)
            {
                if (!versions.ContainsKey(candidateVersion.Item1))
                {
                    versions.Add(candidateVersion.Item1, candidateVersion.Item2);
                }
            }

            return versions;
        }

        private bool IsValidVersion(string candidate)
        {
            if (string.IsNullOrWhiteSpace(candidate)) { return false; }

            string[] parts = candidate.Split('.');

            if (parts.Length != 3) { return false; }

            return this.AllPartsNumeric(parts);
        }

        private bool AllPartsNumeric(IEnumerable<string> parts)
        {
            foreach (string part in parts)
            {
                int result;
                if (!int.TryParse(part, out result))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
