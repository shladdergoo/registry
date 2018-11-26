namespace Registry.Docker.Test.Integration
{
    using System;
    using System.Collections.Generic;

    using Xunit;

    public class HttpRegistryRepositoryTest
    {
        private IRegistryRepository sut;

        [Fact]
        public void GetTags_Succeeds()
        {
            this.sut = new HttpRegistryRepository(new HttpClient());

            IEnumerable<string> result =
                this.sut.GetTags(
                    "sp3registry-on.azurecr.io",
                    "managecasemsvc",
                    "sp3registry",
                    "/+UW6NK2=xt8ERBKAZ1dASA9JkpFgyIF");

            Assert.NotEmpty(result);
        }
    }
}
