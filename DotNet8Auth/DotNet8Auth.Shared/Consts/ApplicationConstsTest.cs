using NUnit.Framework;

namespace DotNet8Auth.Shared.Consts.Tests
{
    [TestFixture]
    public class ApplicationConstsTests
    {
        [Test]
        public void AccessToken_ShouldHaveCorrectValue()
        {
            // Arrange
            string expectedValue = "accessToken";

            // Act
            string actualValue = ApplicationConsts.AccessToken;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void RefreshToken_ShouldHaveCorrectValue()
        {
            // Arrange
            string expectedValue = "refreshToken";

            // Act
            string actualValue = ApplicationConsts.RefreshToken;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}