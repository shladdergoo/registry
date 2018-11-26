namespace Registry.Docker.Test
{
    using System;
    using System.Collections.Generic;

    using NSubstitute;
    using Xunit;

    public class RegistryServiceTest
    {
        private RegistryService sut;

        [Fact]
        public void Ctor_NullRepository_ThrowsException()
        {
            IVersionMatcher versionMatcher = Substitute.For<IVersionMatcher>();

            Assert.Throws<ArgumentNullException>(() => this.sut = new RegistryService(null, versionMatcher));
        }

        [Fact]
        public void Ctor_NullVersionMatcher_ThrowsException()
        {
            IRegistryRepository registryRepository = Substitute.For<IRegistryRepository>();

            Assert.Throws<ArgumentNullException>(() => this.sut = new RegistryService(registryRepository, null));
        }

        [Fact]
        public void GetLatestVersionTag_NullRegistry_ThrowsException()
        {
            IVersionMatcher versionMatcher = Substitute.For<IVersionMatcher>();
            IRegistryRepository registryRepository = Substitute.For<IRegistryRepository>();

            this.sut = new RegistryService(registryRepository, versionMatcher);

            Assert.Throws<ArgumentException>(() =>
                this.sut.GetLatestVersionTag(null, "someRepo", "username", "password"));
        }

        [Fact]
        public void GetLatestVersionTag_NullRepository_ThrowsException()
        {
            IVersionMatcher versionMatcher = Substitute.For<IVersionMatcher>();
            IRegistryRepository registryRepository = Substitute.For<IRegistryRepository>();

            this.sut = new RegistryService(registryRepository, versionMatcher);

            Assert.Throws<ArgumentException>(() =>
                this.sut.GetLatestVersionTag("someRegistry", null, "username", "password"));
        }

        [Fact]
        public void GetLatestVersionTag_NullUsername_ThrowsException()
        {
            IVersionMatcher versionMatcher = Substitute.For<IVersionMatcher>();
            IRegistryRepository registryRepository = Substitute.For<IRegistryRepository>();

            this.sut = new RegistryService(registryRepository, versionMatcher);

            Assert.Throws<ArgumentException>(() =>
                this.sut.GetLatestVersionTag("someRegistry", "someRepo", null, "password"));
        }

        [Fact]
        public void GetLatestVersionTag_NullPassword_ThrowsException()
        {
            IVersionMatcher versionMatcher = Substitute.For<IVersionMatcher>();
            IRegistryRepository registryRepository = Substitute.For<IRegistryRepository>();

            this.sut = new RegistryService(registryRepository, versionMatcher);

            Assert.Throws<ArgumentException>(() =>
                this.sut.GetLatestVersionTag("someRegistry", "someRepo", "username", null));
        }

        [Fact]
        public void GetLatestVersionTag_NoTags_ReturnsNull()
        {
            IVersionMatcher versionMatcher = Substitute.For<IVersionMatcher>();
            IRegistryRepository registryRepository = Substitute.For<IRegistryRepository>();
            registryRepository
                .GetTags(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(new List<string>());

            this.sut = new RegistryService(registryRepository, versionMatcher);

            string result =
                this.sut.GetLatestVersionTag("someRegistry", "someRepo", "username", "password");
        }

        [Fact]
        public void GetLatestVersionTag_GetsTags_GetsVersions()
        {
            IVersionMatcher versionMatcher = Substitute.For<IVersionMatcher>();
            IRegistryRepository registryRepository = Substitute.For<IRegistryRepository>();
            registryRepository
                .GetTags(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(this.GetTestTags());

            this.sut = new RegistryService(registryRepository, versionMatcher);

            this.sut.GetLatestVersionTag("someRegistry", "someRepo", "username", "password");

            versionMatcher.ReceivedWithAnyArgs().GetVersions(default(List<string>));
        }

        [Fact]
        public void GetLatestVersionTag_TagsNotVersions_ReturnsNull()
        {
            IRegistryRepository registryRepository = Substitute.For<IRegistryRepository>();
            registryRepository
                .GetTags(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(this.GetTestTags());
            IVersionMatcher versionMatcher = Substitute.For<IVersionMatcher>();
            versionMatcher.GetVersions(Arg.Any<IEnumerable<string>>()).Returns(new Dictionary<Version, string>());

            this.sut = new RegistryService(registryRepository, versionMatcher);

            Assert.Null(this.sut.GetLatestVersionTag("someRegistry", "someRepo", "username", "password"));
        }

        [Fact]
        public void GetLatestVersionTag_TagsAreVersions_ReturnsLatest()
        {
            IRegistryRepository registryRepository = Substitute.For<IRegistryRepository>();
            registryRepository
                .GetTags(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(this.GetTestTags());
            IVersionMatcher versionMatcher = Substitute.For<IVersionMatcher>();
            versionMatcher.GetVersions(Arg.Any<IEnumerable<string>>()).Returns(this.GetTestVersions());

            this.sut = new RegistryService(registryRepository, versionMatcher);

            string result =
                this.sut.GetLatestVersionTag("someRegistry", "someRepo", "username", "password");

            Assert.Equal("1.1.0", result);
        }

        private IEnumerable<string> GetTestTags()
        {
            return new[] { "1.0.1", "1.0.2", "1.1.0" };
        }

        private IDictionary<Version, string> GetTestVersions()
        {
            return new Dictionary<Version, string>
            {
                { new Version(1, 0, 1), "1.0.001" },
                { new Version(1, 1, 0), "1.1.0" },
                { new Version(1, 0, 2), "1.0.2" }
            };
        }
    }
}
