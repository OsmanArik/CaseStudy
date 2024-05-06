using FluentValidation;
using Shared.Models.WCFServiceModels;

namespace Web.Business.ValidationRules.FluentValidation
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

            RuleFor(x => x).Custom((x, content) =>
            {
                if (x.ArrivalDate!=null)
                {
                    if (x.ArrivalDate > x.DepartureDate)
                        content.AddFailure(nameof(x.ArrivalDate), "Dönüş tarihi gidiş tarihinden büyük olamaz!");
                }
            });
        }
    }
}