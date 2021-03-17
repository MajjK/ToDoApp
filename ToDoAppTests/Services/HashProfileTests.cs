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
        private void TestGetSaltedHashPasswordDifferentSalts()
        {
            string password = "ultra_safe_P455w0rD";
            string firstSalt = HashProfile.GenerateSalt();
            string secondSalt = HashProfile.GenerateSalt();

            string firstSaltedHashPassword = HashProfile.GetSaltedHashPassword(password, firstSalt);
            string secondSaltedHashPassword = HashProfile.GetSaltedHashPassword(password, secondSalt);

            Assert.NotEqual(firstSaltedHashPassword, secondSaltedHashPassword);

        }

        [Fact]
        private void TestGetSaltedHashPasswordDifferentPasswords()
        {
            string firstPassword = "ultra_safe_P455w0rD";
            string secondPassword = "second_safe_P455w0rD";
            string salt = HashProfile.GenerateSalt();

            string firstSaltedHashPassword = HashProfile.GetSaltedHashPassword(firstPassword, salt);
            string secondSaltedHashPassword = HashProfile.GetSaltedHashPassword(secondPassword, salt);

            Assert.NotEqual(firstSaltedHashPassword, secondSaltedHashPassword);
        }

        [Fact]
        private void TestGetSaltedHashPassword()
        {
            string password = "ultra_safe_P455w0rD";
            string salt = HashProfile.GenerateSalt();

            string firstSaltedHashPassword = HashProfile.GetSaltedHashPassword(password, salt);
            string secondSaltedHashPassword = HashProfile.GetSaltedHashPassword(password, salt);

            Assert.Equal(firstSaltedHashPassword, secondSaltedHashPassword);
        }

        [Fact]
        private void TestValidatePasswordsDifferentSalts()
        {
            string password = "ultra_safe_P455w0rD";
            string correctSalt = HashProfile.GenerateSalt();
            string incorrectSalt = HashProfile.GenerateSalt();
            string hashedPassword = HashProfile.GetSaltedHashPassword(password, correctSalt);

            Assert.False(HashProfile.ValidatePasswords(password, hashedPassword, incorrectSalt));
        }

        [Fact]
        private void TestValidatePasswordsDifferentPasswords()
        {
            string password = "ultra_safe_P455w0rD";
            string secondPassword = "second_safe_P455w0rD";
            string salt = HashProfile.GenerateSalt();
            string hashedPassword = HashProfile.GetSaltedHashPassword(password, salt);

            Assert.False(HashProfile.ValidatePasswords(secondPassword, hashedPassword, salt));
        }

        [Fact]
        private void TestValidatePasswordsTrue()
        {
            string password = "ultra_safe_P455w0rD";
            string salt = HashProfile.GenerateSalt();
            string hashedPassword = HashProfile.GetSaltedHashPassword(password, salt);

            Assert.True(HashProfile.ValidatePasswords(password, hashedPassword, salt));
        }
    }
}