using DigitalTwin.Models.Requests.Entity;
using DigitalTwin.Models.Requests.ValueChain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalTwin.Models.Validators.Entity
{
    public class GetAllEntitiesRequestValidator : AbstractValidator<GetAllEntitiesRequest>
    {
        public GetAllEntitiesRequestValidator()
        {
            When(er => er?.FromDate != null && er?.ToDate != null, () =>
            {
                RuleFor(er => er.FromDate)
                .Must((obj, val) => val < obj.ToDate)
                .WithMessage("FromDate value must be smaller than ToDate!");
            });
 
        }
    }
}
