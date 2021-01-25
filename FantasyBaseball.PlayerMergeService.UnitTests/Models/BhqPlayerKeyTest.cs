using FantasyBaseball.CommonModels.Enums;
using FantasyBaseball.PlayerMergeService.Models;
using Xunit;

namespace FantasyBaseball.PlayerMergeService.UnitTests.Models
{
    public class BhqPlayerKeyTest
    {
        private static readonly BhqPlayerKey BaseTestObj = new BhqPlayerKey(10, PlayerType.B);

        [Fact] public void EqualsDifferentIdTest() => Assert.False(BaseTestObj.Equals(new BhqPlayerKey(100, PlayerType.B)));

        [Fact] public void EqualsDifferentTypeTest() => Assert.False(BaseTestObj.Equals(new BhqPlayerKey(10, PlayerType.P)));

        [Fact] public void EqualsNullTest() => Assert.False(BaseTestObj.Equals(null));

        [Fact] public void EqualsOtherClassTest() => Assert.False(BaseTestObj.Equals(""));

        [Fact] public void EqualsSameTest() => Assert.True(BaseTestObj.Equals(new BhqPlayerKey(10, PlayerType.B)));

        [Fact] public void GetHashCodeEqualsTest() => 
            Assert.Equal(BaseTestObj.GetHashCode(), new BhqPlayerKey(10, PlayerType.B).GetHashCode());

        [Fact] public void GetHashCodeNotEqualsTest() => 
            Assert.NotEqual(BaseTestObj.GetHashCode(), new BhqPlayerKey(1, PlayerType.P).GetHashCode());
    }
}