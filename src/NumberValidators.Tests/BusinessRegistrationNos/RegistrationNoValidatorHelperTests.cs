using NumberValidators.BusinessRegistrationNos;
using NumberValidators.BusinessRegistrationNos.Validators;
using System;
using Xunit;

namespace NumberValidators.Tests.BusinessRegistrationNos
{
    public class RegistrationNoValidatorHelperTests
    {
        [Fact]
        public void Valid_No_Is_Empty()
        {
            string no = null;
            var result = RegistrationNoValidatorHelper.Validate(no);
            Assert.IsType<RegistrationNoValidationResult>(result);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Valid_Not_Supported_Length()
        {
            string no = "123456789";
            var result = RegistrationNoValidatorHelper.Validate(no);
            Assert.IsType<RegistrationNoValidationResult>(result);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Valid_Correct_With_Length_15()
        {
            string no = "110108000000016";
            var result = RegistrationNoValidatorHelper.Validate(no);
            Assert.IsType<RegistrationNo15ValidationResult>(result);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Valid_Correct_With_Length_18()
        {
            string no = "91320621MA1MRHG205";
            var result = RegistrationNoValidatorHelper.Validate(no);
            Assert.IsType<RegistrationNo18ValidationResult>(result);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Set_Another_Validator_To_Replace_15_And_Reset()
        {
            string no = "110108000000016";
            RegistrationNoValidatorHelper.SetValidator(RegistrationNoLength.Fifteen, new NotImplementedRegistrationNo15Validator());
            Assert.Throws<NotImplementedException>(() => RegistrationNoValidatorHelper.Validate(no));
            RegistrationNoValidatorHelper.ResetDefaultValidator();
            Assert.True(RegistrationNoValidatorHelper.Validate(no).IsValid);
        }
    }

    internal class NotImplementedRegistrationNo15Validator : IRegistrationNoValidator<RegistrationNoValidationResult>
    {
        public IValidationDictionary<int, string> Dictionary { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public RegistrationNoLength RegistrationNoLength => throw new NotImplementedException();

        public string GenerateRandomNumber()
        {
            throw new NotImplementedException();
        }

        public RegistrationNoValidationResult Validate(string code, AreaValidLimit? validLimit = null)
        {
            throw new NotImplementedException();
        }

        public RegistrationNoValidationResult Validate(string number)
        {
            throw new NotImplementedException();
        }
    }
}
