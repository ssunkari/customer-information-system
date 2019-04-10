using Dao.Repositories;
using FluentAssertions;
using NUnit.Framework;

namespace SmokeTests
{
    public class ConsistentHashingTests
    {
        [TestCase("123","123")]
        [TestCase("test","test")]
        public void TwoSimilarStringHashesShouldAlwaysMatch(string str1, string str2)
        {
            str1.GetStableHashCode().Should().Be(str2.GetStableHashCode());
        }
    }
}
