namespace Registry.Docker
{
    using System;
    using System.Collections.Generic;

    public interface IVersionMatcher
    {
        IDictionary<Version, string> GetVersions(IEnumerable<string> candidates);
    }
}
