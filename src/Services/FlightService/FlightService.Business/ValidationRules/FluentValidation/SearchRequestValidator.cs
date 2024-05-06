using FluentValidation;
using Shared.Models.WCFServiceModels;

namespace FlightService.Business.ValidationRules.FluentValidation
{
    public class SearchRequestValidator : AbstractValidator<SearchRequestModel>
    {
        public SearchRequestValidator()
        {
            RuleFor(x => x.Origin)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Destination)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.DepartureDate)
                .NotEmpty()
                .NotNull();
        }
    }
}