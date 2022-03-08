using NumberValidators.BusinessRegistrationNos;
using NumberValidators.BusinessRegistrationNos.Validators;
using System;
using Xunit;

namespace NumberValidators.Tests.BusinessRegistrationNos
{
    public class RegistrationNo18ValidatorTests
    {
        [Fact]
        public void Default_Prop()
        {
            var validator = new RegistrationNo18Validator();
            Assert.Equal(RegistrationNoLength.Eighteen, validator.RegistrationNoLength);
        }

        [Fact]
        public void GenerateRandomNumber_Length_Should_Equals_18()
        {
            var validator = new RegistrationNo18Validator();
            var number = validator.GenerateRandomNumber();
            Assert.Equal(18, number.Length);
        }

        [Fact]
        public void GenerateRegistrationNo_Length_Should_Equals_18()
        {
            var validator = new RegistrationNo18Validator();
            var number = validator.GenerateRegistrationNo(310104, ManagementCode.Business, ManagementKindCode.Foundation);
            Assert.Equal(18, number.Length);
        }

        [Fact]
        public void Validate_Error_With_CheckBit()
        {
            var number = "91320621MA1MRHG207";
            IValidator<RegistrationNo18ValidationResult> validator = new RegistrationNo18Validator();
            var result = validator.Validate(number);
            Assert.False(result.IsValid);
            Assert.Equal(number, result.Number);
            Assert.NotEmpty(result.Errors);
            Assert.True(result.Errors.Contains("´íÎóµÄÐ£ÑéÂë"));
        }

        [Fact]
        public void Validate_Correct()
        {
            var number = "91320621MA1MRHG205";
            var validator = new RegistrationNo18Validator();
            var result = validator.Validate(number);
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
            Assert.NotNull(result.RecognizableArea);
            Assert.Equal(ManagementCode.Business, result.ManagementCode);
            Assert.Equal(ManagementKindCode.Enterprise, result.ManagementKindCode);
            Assert.Equal(320621, result.AreaNumber);
            Assert.Equal(result.AreaNumber, result.RecognizableArea.Number);
            Assert.Equal("MA1MRHG20", result.OrganizationCode);
            Assert.Equal('5', result.CheckBit);
            Assert.Equal(RegistrationNoLength.Eighteen, result.RegistrationNoLength);
        }
    }
}
