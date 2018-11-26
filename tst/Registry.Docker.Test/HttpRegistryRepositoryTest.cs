namespace Registry.Docker.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;

    using NSubstitute;
    using Xunit;

    public class HttpRegistryRepositoryTest
    {
        private IRegistryRepository sut;

        [Fact]
        public void Ctor_NullHttpClient_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => this.sut = new HttpRegistryRepository(null));
        }

        [Fact]
        public void GetTags_NullRegistry_ThrowsException()
        {
            IHttpClient httpClient = Substitute.For<IHttpClient>();

            this.sut = new HttpRegistryRepository(httpClient);

            Assert.Throws<ArgumentException>(() =>
                this.sut.GetTags(null, "someRepo", "username", "password"));
        }

        [Fact]
        public void GetTags_NullRepository_ThrowsException()
        {
            IHttpClient httpClient = Substitute.For<IHttpClient>();

            this.sut = new HttpRegistryRepository(httpClient);

            Assert.Throws<ArgumentException>(() =>
                this.sut.GetTags("someRegistry", null, "username", "password"));
        }

        [Fact]
        public void GetTags_NullUsername_ThrowsException()
        {
            IHttpClient httpClient = Substitute.For<IHttpClient>();

            this.sut = new HttpRegistryRepository(httpClient);

            Assert.Throws<ArgumentException>(() =>
                this.sut.GetTags("someRegistry", "someRepo", null, "password"));
        }

        [Fact]
        public void GetTags_NullPassword_ThrowsException()
        {
            IHttpClient httpClient = Substitute.For<IHttpClient>();

            this.sut = new HttpRegistryRepository(httpClient);

            Assert.Throws<ArgumentException>(() =>
                this.sut.GetTags("someRegistry", "someRepo", "username", null));
        }

        [Fact]
        public void GetTags_ResponseHasNoTags_ReturnsEmptyList()
        {
            IHttpClient httpClient = Substitute.For<IHttpClient>();
            HttpWebResponse response = Substitute.For<HttpWebResponse>();
            response.StatusCode.Returns(HttpStatusCode.OK);
            response.GetResponseStream().Returns(this.GetTestResponse(false));
            httpClient.Execute(Arg.Any<HttpWebRequest>()).Returns(response);

            this.sut = new HttpRegistryRepository(httpClient);

            IEnumerable<string> result =
                this.sut.GetTags("someRegistry", "someRepo", "username", "password");

            Assert.Empty(result);
        }

        [Fact]
        public void GetTags_ResponseHasTags_ReturnsTags()
        {
            IHttpClient httpClient = Substitute.For<IHttpClient>();
            HttpWebResponse response = Substitute.For<HttpWebResponse>();
            response.StatusCode.Returns(HttpStatusCode.OK);
            response.GetResponseStream().Returns(this.GetTestResponse(true));
            httpClient.Execute(Arg.Any<HttpWebRequest>()).Returns(response);

            this.sut = new HttpRegistryRepository(httpClient);

            IEnumerable<string> result =
                this.sut.GetTags("someRegistry", "someRepo", "username", "password");

            Assert.Equal(19, result.Count());
        }

        [Fact]
        public void GetTags_RepoNotFound_ReturnsNull()
        {
            IHttpClient httpClient = Substitute.For<IHttpClient>();
            HttpWebResponse response = Substitute.For<HttpWebResponse>();
            response.StatusCode.Returns(HttpStatusCode.NotFound);
            httpClient.Execute(Arg.Any<HttpWebRequest>()).Returns(response);

            this.sut = new HttpRegistryRepository(httpClient);

            IEnumerable<string> result =
                this.sut.GetTags("someRegistry", "someRepo", "username", "password");

            Assert.Null(result);
        }

        private Stream GetTestResponse(bool tags)
        {
            MemoryStream responseStream = new MemoryStream();

            StreamWriter streamtWriter = new StreamWriter(responseStream);
            if (tags)
            {
                streamtWriter.Write(this.GetTestBody());
            }
            else
            {
                streamtWriter.Write(this.GetTestBodyNoTags());
            }

            streamtWriter.Flush();

            responseStream.Position = 0;

            return responseStream;
        }

        private string GetTestBody()
        {
            return "{" +
                        "\"name\": \"managecasemsvc\"," +
                        "\"tags\": [" +
                                    "\"1.2.165\"," +
                                    "\"20180709.1\"," +
                                    "\"1.2.158\"," +
                                    "\"1.2.129\"," +
                                    "\"1.2.134\"," +
                                    "\"master\"," +
                                    "\"1.2.123\"," +
                                    "\"1.2.136\"," +
                                    "\"1.2.154\"," +
                                    "\"20180706.4\"," +
                                    "\"1.2.130\"," +
                                    "\"1.2.131\"," +
                                    "\"1.2.137\"," +
                                    "\"1.2.147\"," +
                                    "\"1.2.164\"," +
                                    "\"20180530.1\"," +
                                    "\"1.2.159\"," +
                                    "\"1.2.160\"," +
                                    "\"1.2.124\"," +
                                    "]" +
                        "}";
        }

        private string GetTestBodyNoTags()
        {
            return "{" +
                        "\"name\": \"managecasemsvc\"," +
                        "\"tags\": [" +
                                    "]" +
                        "}";
        }
    }
}
