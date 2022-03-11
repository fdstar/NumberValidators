using NumberValidators.Invoices;
using NumberValidators.Invoices.Validators;
using System;
using Xunit;

namespace NumberValidators.Tests
{
    public class VATCodeValidatorHelperTests
    {

        [Theory]
        [InlineData((string)null)]
        [InlineData("123456789")]
        public void Valid_No_Is_Empty_Or_Not_Supported_Length(string no)
        {
            var result = VATCodeValidatorHelper.Validate(no, length: 9);
            Assert.IsType<VATCodeValidationResult>(result);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("3100153130")]
        public void Valid_Correct_With_Length_10(string no)
        {
            var result = VATCodeValidatorHelper.Validate(no);
            Assert.IsType<VATCode10ValidationResult>(result);
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("011001800604")]
        public void Valid_Correct_With_Length_12(string no)
        {
            var result = VATCodeValidatorHelper.Validate(no);
            Assert.IsType<VATCode12ValidationResult>(result);
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("3100153130")]
        public void Set_Another_Validator_To_Replace_10_And_Reset(string no)
        {
            Assert.Throws<ArgumentNullException>(() => VATCodeValidatorHelper.SetValidator((int)VATLength.Ten, null));
            VATCodeValidatorHelper.SetValidator((int)VATLength.Ten, new NotImplementedVATCodeValidator());
            Assert.Throws<NotImplementedException>(() => VATCodeValidatorHelper.Validate(no));
            VATCodeValidatorHelper.ResetDefaultValidator();
            Assert.True(VATCodeValidatorHelper.Validate(no).IsValid);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "<挂起>")]
    internal sealed class NotImplementedVATCodeValidator : IVATCodeValidator<VATCodeValidationResult>
    {
        public IValidationDictionary<int, string> Dictionary { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string GenerateRandomNumber()
        {
            throw new NotImplementedException();
        }

        public string GenerateVATCode(int areaNumber, ushort year, ushort batch, VATKind kind, ElectronicVATKind? electKind = null)
        {
            throw new NotImplementedException();
        }

        public VATCodeValidationResult Validate(string vatCode, VATKind? kind = null, ushort minYear = 2012)
        {
            throw new NotImplementedException();
        }

        public VATCodeValidationResult Validate(string number)
        {
            throw new NotImplementedException();
        }
    }
}
