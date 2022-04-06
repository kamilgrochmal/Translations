using Translations.Exceptions.Abstractions;

namespace Translations.Exceptions;

public class TranslationInThisLanguageAlreadyExistsException : TranslationException
{
    public TranslationInThisLanguageAlreadyExistsException() : base("Translation in this language already exists.")
    {
    }
}