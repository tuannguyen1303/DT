using DigitalTwin.Models.Requests.Entity;
using DigitalTwin.Models.Validators.Entity;
using FluentValidation.TestHelper;

namespace DigitaiTwin.UnitTest.Validators.Entity
{
    public class GetAllEntitiesRequestValidatorTest
    {
        private readonly GetAllEntitiesRequestValidator _validationRules;

        public GetAllEntitiesRequestValidatorTest()
        {
            _validationRules = new GetAllEntitiesRequestValidator();
        }

        [Fact]
        public void GetEntityRequestDateValidator_Success()
        {
            var request = new GetAllEntitiesRequest
            {
                ValueChainType = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                FromDate = DateTime.Parse("2021-06-30"),
                ToDate = DateTime.Parse("2021-05-30"),
                IsDaily = false,
                IsMonthly = true,
                IsMonthToDate = false,
                IsQuarterly = false,
                IsYearToDaily = false,
                IsYearToMonthly = false,
                IsYearEndProjection = false
            };
            var validationResult = _validationRules.TestValidate(request);
            validationResult.ShouldHaveValidationErrorFor(valuechainrequest => valuechainrequest.FromDate)
                .WithErrorMessage("FromDate value must be smaller than ToDate!");
        }
 
    }
}
