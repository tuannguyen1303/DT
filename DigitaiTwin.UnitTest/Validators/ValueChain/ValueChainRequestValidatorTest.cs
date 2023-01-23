using DigitalTwin.Business.Services.ValueChain;
using DigitalTwin.Data.Entities;
using DigitalTwin.Models.Requests.ValueChain;
using DigitalTwin.Models.Validators.ValueChain;
using DocumentFormat.OpenXml.InkML;
using FluentValidation.TestHelper;

namespace DigitaiTwin.UnitTest.Validators.ValueChain
{
    public class ValueChainRequestValidatorTest
    {
        private readonly ValueChainRequestValidator _validationRules;

        public ValueChainRequestValidatorTest()
        {
            _validationRules = new ValueChainRequestValidator();
        }

        [Fact]
        public void GetValueChainRequestDateValidator_Success()
        {
            var request = new ValueChainRequest
            {
                ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                FromDate = DateTime.Parse("2021-06-30"),
                ToDate = DateTime.Parse("2021-05-30"),
                IsDaily = false,
                IsMonthly = true,
                IsMonthToDate = false,
                IsQuarterly = false,
                IsYearToDaily = false,
                IsYearToMonthly = false,
                IsYearEndProjection = false,
                IsFilter = false,
                Entities = new List<Guid>()
            };
            var validationResult = _validationRules.TestValidate(request);
            validationResult.ShouldHaveValidationErrorFor(valuechainrequest => valuechainrequest.FromDate)
                .WithErrorMessage("FromDate value must be smaller than ToDate!");
        }
    }
}
