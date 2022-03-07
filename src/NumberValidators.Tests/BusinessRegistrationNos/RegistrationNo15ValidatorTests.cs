using NumberValidators.BusinessRegistrationNos;
using NumberValidators.BusinessRegistrationNos.Validators;
using System;
using Xunit;

namespace NumberValidators.Tests.BusinessRegistrationNos
{
    public class RegistrationNo15ValidatorTests
    {
        [Fact]
        public void Default_Prop()
        {
            var validator = new RegistrationNo15Validator();
            Assert.Equal(RegistrationNoLength.Fifteen, validator.RegistrationNoLength);
            Assert.NotNull(validator.AdministrationDictionary);
            Assert.True(validator.AdministrationDictionary.GetDictionary().ContainsKey(100000));
        }

        [Fact]
        public void GenerateRandomNumber_Length_Should_Equals_15()
        {
            var validator = new RegistrationNo15Validator();
            var number = validator.GenerateRandomNumber();
            Assert.Equal(15, number.Length);
        }

        [Fact]
        public void GenerateRegistrationNo_Length_Should_Equals_15()
        {
            var validator = new RegistrationNo15Validator();
            var number = validator.GenerateRegistrationNo(310104, EnterpriseType.Individual);
            Assert.Equal(15, number.Length);
        }

        [Fact]
        public void Validate_Error_With_CheckBit()
        {
            var number = "110108000000017";
            IValidator<RegistrationNo15ValidationResult> validator = new RegistrationNo15Validator();
            var result = validator.Validate(number);
            Assert.False(result.IsValid);
            Assert.Equal(number, result.Number);
            Assert.NotEmpty(result.Errors);
            Assert.True(result.Errors.Contains("´íÎóµÄÐ£ÑéÂë"));
        }

        [Fact]
        public void Validate_Correct()
        {
            var number = "110108000000016";
            var validator = new RegistrationNo15Validator();
            var result = validator.Validate(number);
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
            Assert.NotNull(result.RecognizableArea);
            Assert.Equal(110108, result.AreaNumber);
            Assert.Equal(result.AreaNumber, result.RecognizableArea.Number);
            Assert.Equal(1, result.SequenceNumber);
            Assert.Equal('6', result.CheckBit);
            Assert.Equal(EnterpriseType.Domestic, result.EnterpriseType);
            Assert.Equal(RegistrationNoLength.Fifteen, result.RegistrationNoLength);
        }
    }
}
