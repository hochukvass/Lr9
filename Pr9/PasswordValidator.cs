using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SecurityLib
{
    public class ValidationError
    {
        public string Code { get; }
        public string Message { get; }

        public ValidationError(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }

    public class PasswordValidator
    {
        private static readonly HashSet<string> BannedPasswords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "password", "qwerty", "123456", "admin", "12345678",
            "111111", "sunshine", "123123", "123456789", "1234567"
        };

        private static readonly Regex UpperCaseRegex = new Regex("[A-Z]");
        private static readonly Regex LowerCaseRegex = new Regex("[a-z]");
        private static readonly Regex DigitRegex = new Regex("[0-9]");
        private static readonly Regex SpecialCharRegex = new Regex("[!@#$%^&*()\\-_=+\\[\\]{};:'\",.<>/?]");
        private static readonly Regex RepeatRegex = new Regex("(.)\\1{3,}");

        public List<ValidationError> Validate(string password)
        {
            var errors = new List<ValidationError>();

            // Проверка на null или пустую строку
            if (string.IsNullOrEmpty(password))
            {
                errors.Add(new ValidationError("EMPTY", "Пароль не может быть пустым."));
                return errors;
            }

            // Проверка пробелов в начале/конце
            if (password != password.Trim())
            {
                errors.Add(new ValidationError("TRIM", "Пробелы в начале или конце запрещены."));
            }

            // Проверка длины
            if (password.Length < 12)
            {
                errors.Add(new ValidationError("LEN", "Пароль должен содержать минимум 12 символов."));
            }

            // Проверка заглавных букв
            if (!UpperCaseRegex.IsMatch(password))
            {
                errors.Add(new ValidationError("UPPER", "Пароль должен содержать хотя бы одну заглавную букву."));
            }

            // Проверка строчных букв
            if (!LowerCaseRegex.IsMatch(password))
            {
                errors.Add(new ValidationError("LOWER", "Пароль должен содержать хотя бы одну строчную букву."));
            }

            // Проверка цифр
            if (!DigitRegex.IsMatch(password))
            {
                errors.Add(new ValidationError("DIGIT", "Пароль должен содержать хотя бы одну цифру."));
            }

            // Проверка специальных символов
            if (!SpecialCharRegex.IsMatch(password))
            {
                errors.Add(new ValidationError("SPECIAL", "Пароль должен содержать хотя бы один специальный символ."));
            }

            // Проверка запрещенных паролей
            if (BannedPasswords.Contains(password))
            {
                errors.Add(new ValidationError("BANNED", "Этот пароль находится в списке запрещенных."));
            }

            // Проверка повторяющихся символов
            if (RepeatRegex.IsMatch(password))
            {
                errors.Add(new ValidationError("REPEAT", "Один символ не может повторяться более 3 раз подряд."));
            }

            return errors;
        }
    }
}