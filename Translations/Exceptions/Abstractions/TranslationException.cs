namespace Translations.Exceptions.Abstractions;

public abstract class TranslationException : Exception
{
    protected TranslationException(string message) : base(message)
    {
        
    }
}