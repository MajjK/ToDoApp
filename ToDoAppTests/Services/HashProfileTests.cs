using Xunit;
using ToDoApp.Services;

namespace ToDoAppTests.Services
{
    public class HashProfileTests
    {
        [Fact]
        private void TestGenerateSalt()
        {
            string firstSalt = HashProfile.GenerateSalt();
            string secondSalt = HashProfile.GenerateSalt();

            Assert.NotEqual(firstSalt, secondSalt);
        }

        [Fact]
        private void TestGetSaltedHashDataDifferentSalts()
        {
            string password = "ultra_safe_P455w0rD";
            string firstSalt = HashProfile.GenerateSalt();
            string secondSalt = HashProfile.GenerateSalt();

            string firstSaltedHashPassword = HashProfile.GetSaltedHashData(password, firstSalt);
            string secondSaltedHashPassword = HashProfile.GetSaltedHashData(password, secondSalt);

            Assert.NotEqual(firstSaltedHashPassword, secondSaltedHashPassword);

        }

        [Fact]
        private void TestGetSaltedHashDataDifferentPasswords()
        {
            string firstPassword = "ultra_safe_P455w0rD";
            string secondPassword = "second_safe_P455w0rD";
            string salt = HashProfile.GenerateSalt();

            string firstSaltedHashPassword = HashProfile.GetSaltedHashData(firstPassword, salt);
            string secondSaltedHashPassword = HashProfile.GetSaltedHashData(secondPassword, salt);

            Assert.NotEqual(firstSaltedHashPassword, secondSaltedHashPassword);
        }

        [Fact]
        private void TestGetSaltedHashData()
        {
            string password = "ultra_safe_P455w0rD";
            string salt = HashProfile.GenerateSalt();

            string firstSaltedHashPassword = HashProfile.GetSaltedHashData(password, salt);
            string secondSaltedHashPassword = HashProfile.GetSaltedHashData(password, salt);

            Assert.Equal(firstSaltedHashPassword, secondSaltedHashPassword);
        }

        [Fact]
        private void TestValidatePasswordsDifferentSalts()
        {
            string password = "ultra_safe_P455w0rD";
            string correctSalt = HashProfile.GenerateSalt();
            string incorrectSalt = HashProfile.GenerateSalt();
            string hashedPassword = HashProfile.GetSaltedHashData(password, correctSalt);

            Assert.False(HashProfile.ValidatePasswords(password, hashedPassword, incorrectSalt));
        }

        [Fact]
        private void TestValidatePasswordsDifferentPasswords()
        {
            string password = "ultra_safe_P455w0rD";
            string secondPassword = "second_safe_P455w0rD";
            string salt = HashProfile.GenerateSalt();
            string hashedPassword = HashProfile.GetSaltedHashData(password, salt);

            Assert.False(HashProfile.ValidatePasswords(secondPassword, hashedPassword, salt));
        }

        [Fact]
        private void TestValidatePasswordsTrue()
        {
            string password = "ultra_safe_P455w0rD";
            string salt = HashProfile.GenerateSalt();
            string hashedPassword = HashProfile.GetSaltedHashData(password, salt);

            Assert.True(HashProfile.ValidatePasswords(password, hashedPassword, salt));
        }
    }
}