using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecurityLib;
using System.Linq;

namespace SecurityLib.Tests
{
    [TestClass]
    public class PasswordValidatorTests
    {
        private PasswordValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new PasswordValidator();
        }

        [TestMethod]
        public void Validate_EmptyPassword_ReturnsEmptyError()
        {
            // Act
            var errors = _validator.Validate("");

            // Assert
            Assert.IsTrue(errors.Any(e => e.Code == "EMPTY"));
        }

        [TestMethod]
        public void Validate_NullPassword_ReturnsEmptyError()
        {
            // Act
            var errors = _validator.Validate(null);

            // Assert
            Assert.IsTrue(errors.Any(e => e.Code == "EMPTY"));
        }

        [TestMethod]
        public void Validate_ShortPassword_ReturnsLengthError()
        {
            // Act
            var errors = _validator.Validate("Short1!");

            // Assert
            Assert.IsTrue(errors.Any(e => e.Code == "LEN"));
        }

        [TestMethod]
        public void Validate_NoUpperCase_ReturnsUpperCaseError()
        {
            // Act
            var errors = _validator.Validate("nouppercase1!");

            // Assert
            Assert.IsTrue(errors.Any(e => e.Code == "UPPER"));
        }

        [TestMethod]
        public void Validate_NoLowerCase_ReturnsLowerCaseError()
        {
            // Act
            var errors = _validator.Validate("NOLOWERCASE1!");

            // Assert
            Assert.IsTrue(errors.Any(e => e.Code == "LOWER"));
        }

        [TestMethod]
        public void Validate_NoDigits_ReturnsDigitError()
        {
            // Act
            var errors = _validator.Validate("NoDigits!!!");

            // Assert
            Assert.IsTrue(errors.Any(e => e.Code == "DIGIT"));
        }

        [TestMethod]
        public void Validate_NoSpecialChars_ReturnsSpecialError()
        {
            // Act
            var errors = _validator.Validate("NoSpecials123");

            // Assert
            Assert.IsTrue(errors.Any(e => e.Code == "SPECIAL"));
        }

        [TestMethod]
        public void Validate_BannedPassword_ReturnsBannedError()
        {
            // Act & Assert
            Assert.IsTrue(_validator.Validate("password").Any(e => e.Code == "BANNED"));
            Assert.IsTrue(_validator.Validate("123456").Any(e => e.Code == "BANNED"));
            Assert.IsTrue(_validator.Validate("admin").Any(e => e.Code == "BANNED"));
        }

        [TestMethod]
        public void Validate_LeadingSpaces_ReturnsTrimError()
        {
            // Act
            var errors = _validator.Validate(" Abcdef1!Abcd");

            // Assert
            Assert.IsTrue(errors.Any(e => e.Code == "TRIM"));
        }

        [TestMethod]
        public void Validate_TrailingSpaces_ReturnsTrimError()
        {
            // Act
            var errors = _validator.Validate("Abcdef1!Abcd ");

            // Assert
            Assert.IsTrue(errors.Any(e => e.Code == "TRIM"));
        }

        [TestMethod]
        public void Validate_RepeatingCharacters_ReturnsRepeatError()
        {
            // Act
            var errors = _validator.Validate("AAAAbcdef1!");

            // Assert
            Assert.IsTrue(errors.Any(e => e.Code == "REPEAT"));
        }

        [TestMethod]
        public void Validate_StrongPassword_NoErrors()
        {
            // Act
            var errors = _validator.Validate("StrongPass123!");

            // Assert
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void Validate_PasswordWithInternalSpaces_Allowed()
        {
            // Act
            var errors = _validator.Validate("Good Pass 123!");

            // Assert - внутренние пробелы разрешены
            Assert.IsFalse(errors.Any(e => e.Code == "TRIM"));
        }

        [TestMethod]
        public void Validate_Exactly12Chars_NoLengthError()
        {
            // Act
            var errors = _validator.Validate("Abcdefgh1!Ab");

            // Assert
            Assert.IsFalse(errors.Any(e => e.Code == "LEN"));
        }

        [TestMethod]
        [Timeout(1000)] // Тест должен завершиться за 1 секунду
        public void Validate_LongPassword_PerformanceTest()
        {
            // Arrange
            var longPassword = new string('a', 1000) + "A1!";

            // Act
            var errors = _validator.Validate(longPassword);

            // Assert
            Assert.IsNotNull(errors);
        }
    }
}