using FluentValidation;

namespace Shared.Core.Extensions
{
    public static class ExceptionExtensions
    {
        public static IEnumerable<string> ToErrorListEx(this Exception e)
        {
            if (e is ValidationException validationException)
                return validationException.Errors.Select(x => x.PropertyName + " " + x.ErrorMessage);

            return new List<string>();
        }
    }
}