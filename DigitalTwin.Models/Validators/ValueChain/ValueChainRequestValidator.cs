using DigitalTwin.Models.Requests.ValueChain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalTwin.Models.Validators.ValueChain
{
    public class ValueChainRequestValidator : AbstractValidator<ValueChainRequest>
    {
        public ValueChainRequestValidator()
        {
            When(vcr => vcr?.FromDate != null && vcr?.ToDate != null, () =>
            {
                RuleFor(vcr => vcr.FromDate)
                .Must((obj, val) => val < obj.ToDate)
                .WithMessage("FromDate value must be smaller than ToDate!");
            });
        }
    }
}
