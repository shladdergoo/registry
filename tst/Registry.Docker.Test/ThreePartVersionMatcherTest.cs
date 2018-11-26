namespace Registry.Docker.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Xunit;

    public class ThreePartVersionMatcherTest
    {
        private ThreePartVersionMatcher sut;

        [Fact]
        public void GetVersions_NullCandidates_ThrowsException()
        {
            this.sut = new ThreePartVersionMatcher();

            Assert.Throws<ArgumentNullException>(() => this.sut.GetVersions(null));
        }

        [Theory]
        [InlineData("master", "sprint10", "1", 0)]
        [InlineData("1.0.0", "1.2.3", "2.3.4", 3)]
        [InlineData("master", "sprint10", "1.1.1", 1)]
        [InlineData("1.0.002", "sprint10", "1.1.1", 2)]
        [InlineData("1.1.002", "1.1.2", "1.1.1", 2)]
        public void GetVersions_HasCandidates_GetsVersions(
            string cand1,
            string cand2,
            string cand3,
            int expectedVersions)
        {
            this.sut = new ThreePartVersionMatcher();

            IEnumerable<string> candidates = new[] { cand1, cand2, cand3 };

            IDictionary<Version, string> result = this.sut.GetVersions(candidates);

            Assert.Equal(expectedVersions, result.Count);
        }

        [Fact]
        public void GetVersions_GetVersion_HasOriginalTag()
        {
            this.sut = new ThreePartVersionMatcher();

            IEnumerable<string> candidates = new[] { "1.0.0008" };

            IDictionary<Version, string> result = this.sut.GetVersions(candidates);

            Assert.Equal(1, result.Count);
            Assert.Equal("1.0.0008", result.First().Value);
        }
    }
}
