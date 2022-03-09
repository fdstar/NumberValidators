using NumberValidators.BusinessRegistrationNos;
using NumberValidators.BusinessRegistrationNos.Validators;
using System;
using Xunit;

namespace NumberValidators.Tests.BusinessRegistrationNos
{
    public class RegistrationNo15ValidatorTests
    {
        private readonly RegistrationNo15Validator validator = new RegistrationNo15Validator();

        [Fact]
        public void Default_Prop()
        {
            Assert.Equal(RegistrationNoLength.Fifteen, validator.RegistrationNoLength);
            validator.AdministrationDictionary = null;
            Assert.NotNull(validator.AdministrationDictionary);
            Assert.True(validator.AdministrationDictionary.GetDictionary().ContainsKey(100000));
        }

        [Fact]
        public void GenerateRandomNumber_Length_Should_Equals_15()
        {
            var number = validator.GenerateRandomNumber();
            Assert.Equal(15, number.Length);
        }

        [Fact]
        public void GenerateRegistrationNo_Length_Should_Equals_15()
        {
            var number = validator.GenerateRegistrationNo(310104, EnterpriseType.Foreign);
            Assert.Equal(15, number.Length);
        }

        [Theory]
        [InlineData("110108000000017")]
        [InlineData("191108000000014")]
        public void Validate_Error(string no)
        {
            var valid = (IValidator<RegistrationNo15ValidationResult>)validator;
            var result = valid.Validate(no);
            Assert.False(result.IsValid);
            Assert.Equal(no, result.Number);
            Assert.NotEmpty(result.Errors);
            result = validator.Validate(no, AreaValidLimit.City);
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
        }

        [Theory]
        [InlineData("110108000000016")]
        public void Validate_Correct(string no)
        {
            var result = validator.Validate(no);
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
