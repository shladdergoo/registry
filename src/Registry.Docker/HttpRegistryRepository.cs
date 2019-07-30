namespace Registry.Docker
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;

    using Newtonsoft.Json;

    public class HttpRegistryRepository : IRegistryRepository
    {
        private readonly IHttpClient httpClient;

        public HttpRegistryRepository(IHttpClient httpClient)
        {
            if (httpClient == null) { throw new ArgumentNullException(nameof(httpClient)); }

            this.httpClient = httpClient;
        }

        public IEnumerable<string> GetTags(
            string registry,
            string repository,
            string registryUsername,
            string registryPassword)
        {
            if (string.IsNullOrWhiteSpace(registry)) { throw new ArgumentNullException(nameof(registry)); }
            if (string.IsNullOrWhiteSpace(repository)) { throw new ArgumentNullException(nameof(repository)); }
            if (string.IsNullOrWhiteSpace(registryUsername)) { throw new ArgumentNullException(nameof(registryUsername)); }
            if (string.IsNullOrWhiteSpace(registryPassword)) { throw new ArgumentNullException(nameof(registryPassword)); }

            HttpWebResponse response = this.httpClient
                .Execute(this.GetTagsRequest(registry, repository, registryUsername, registryPassword));

            if (response.StatusCode != HttpStatusCode.OK) { return null; }

            IEnumerable<string> tags = this.GetTagsFromResponse(response);

            if (!tags.Any()) { return new List<string>(); }

            return tags;
        }

        private HttpWebRequest GetTagsRequest(
            string registry,
            string repository,
            string registryUsername,
            string registryPassword)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest
                .Create(new Uri(new Uri($"https://{registry}"), $"/v2/{repository}/tags/list"));

            request.Method = "GET";

            request.Credentials = new NetworkCredential(registryUsername, registryPassword);

            return request;
        }

        private IEnumerable<string> GetTagsFromResponse(HttpWebResponse response)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());

            Repository repository;
            using (JsonTextReader textReader = new JsonTextReader(reader))
            {
                JsonSerializer serializer = new JsonSerializer();
                repository = serializer.Deserialize<Repository>(textReader);
            }

            return repository.Tags;
        }
    }
}
