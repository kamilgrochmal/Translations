using Translations.Exceptions.Abstractions;

namespace Translations.Exceptions;

public class TranslationKeyAlreadyExistsException : TranslationException
{
    public TranslationKeyAlreadyExistsException(string key) : base($"Translation key: '{key}' already exists.")
    {
    }
}