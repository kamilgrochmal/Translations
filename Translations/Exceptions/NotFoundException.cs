using Translations.Exceptions.Abstractions;

namespace Translations.Exceptions;

//This should be generic exception but for now i place everything in this project
public class NotFoundException : TranslationException
{
    public NotFoundException(string message) : base(message)
    {
    }
}