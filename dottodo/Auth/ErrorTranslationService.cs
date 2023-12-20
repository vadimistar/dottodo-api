namespace dottodo.Auth;

public class ErrorTranslationService : IErrorTranslationService
{
    public string TranslateError(string code)
    {
        return code switch
        {
            "InvalidUserName" => "Неверный логин",
            "DuplicateUserName" => "Логин уже существует",
            "PasswordMismatch" => "Пароли не совпадают",
            "PasswordTooShort" => "Пароль слишком короткий.",
            "PasswordRequiresNonAlphanumeric" => "В пароле не могут быть только буквы и цифры.",
            "PasswordRequiresDigit" => "В пароле должны быть цифры.",
            "PasswordRequiresUpper" => "В пароле должна быть буква верхнего регистра.",
            "PasswordRequiresLower" => "В пароле должна быть буква нижнего регистра.",
            _ => "Неизвестная ошибка.",
        };
    }
}