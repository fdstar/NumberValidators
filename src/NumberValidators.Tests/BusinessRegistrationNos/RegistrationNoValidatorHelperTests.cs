using NumberValidators.BusinessRegistrationNos;
using NumberValidators.BusinessRegistrationNos.Validators;
using System;
using Xunit;

namespace NumberValidators.Tests.BusinessRegistrationNos
{
    public class RegistrationNoValidatorHelperTests
    {
        public RegistrationNoValidatorHelperTests()
        {
            RegistrationNoValidatorHelper.ResetDefaultValidator();
        }
        [Theory]
        [InlineData((string)null)]
        [InlineData("123456789")]
        public void Valid_No_Is_Empty_Or_Not_Supported_Length(string no)
        {
            var result = RegistrationNoValidatorHelper.Validate(no);
            Assert.IsType<RegistrationNoValidationResult>(result);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("450703583197518")]
        [InlineData("440783763666398")]
        public void Valid_Correct_With_Length_15(string no)
        {
            var result = RegistrationNoValidatorHelper.Validate(no);
            Assert.IsType<RegistrationNo15ValidationResult>(result);
            Assert.True(result.IsValid);
            var ret = (RegistrationNo15ValidationResult)result;
            Assert.True(ret.EnterpriseType > EnterpriseType.Domestic);
        }

        [Theory]
        [InlineData("91320621MA1MRHG205")]
        public void Valid_Correct_With_Length_18(string no)
        {
            var result = RegistrationNoValidatorHelper.Validate(no);
            Assert.IsType<RegistrationNo18ValidationResult>(result);
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("110108000000016")]
        public void Set_Another_Validator_To_Replace_15_And_Reset(string no)
        {
            RegistrationNoValidatorHelper.SetValidator(RegistrationNoLength.Fifteen, new NotImplementedRegistrationNo15Validator());
            Assert.Throws<NotImplementedException>(() => RegistrationNoValidatorHelper.Validate(no));
            RegistrationNoValidatorHelper.ResetDefaultValidator();
            Assert.True(RegistrationNoValidatorHelper.Validate(no).IsValid);
        }
    }

    internal sealed class NotImplementedRegistrationNo15Validator : IRegistrationNoValidator<RegistrationNoValidationResult>
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
