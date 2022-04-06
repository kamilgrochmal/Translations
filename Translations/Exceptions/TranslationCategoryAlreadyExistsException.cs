using Translations.Exceptions.Abstractions;

namespace Translations.Exceptions;

public class TranslationCategoryAlreadyExistsException : TranslationException
{
    public TranslationCategoryAlreadyExistsException() : base("This category already exists.")
    {
    }
}