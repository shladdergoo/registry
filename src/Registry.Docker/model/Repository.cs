namespace Registry.Docker
{
    using System.Collections.Generic;

    public class Repository
    {
        public string Name { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}
