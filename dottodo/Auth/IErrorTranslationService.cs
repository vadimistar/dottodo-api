namespace dottodo.Auth;

public interface IErrorTranslationService
{
    public string TranslateError(string code);
}