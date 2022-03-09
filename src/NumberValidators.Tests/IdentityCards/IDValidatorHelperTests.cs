using NumberValidators.IdentityCards;
using NumberValidators.IdentityCards.Validators;
using System;
using Xunit;

namespace NumberValidators.Tests.IdentityCards
{
    public class IDValidatorHelperTests
    {
        [Theory]
        [InlineData((string)null)]
        [InlineData("123456789")]
        public void Valid_No_Is_Empty_Or_Not_Supported_Length(string no)
        {
            var result = IDValidatorHelper.Validate(no, 9);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("411702700422319")]
        public void TryPromotion_Correct(string no)
        {
            Assert.True(IDValidatorHelper.TryPromotion(no, out string newID));
            Assert.Equal("41170219700422319X", newID);
        }

        [Theory]
        [InlineData("965432700422319")]
        public void TryPromotion_Error(string no)
        {
            Assert.False(IDValidatorHelper.TryPromotion(no, out string newID));
            Assert.Null(newID);
        }

        [Theory]
        [InlineData("411702700422319")]
        [InlineData("32021919900101003X")]
        public void Valid_Correct(string no)
        {
            var result = IDValidatorHelper.Validate(no);
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("411702700422319")]
        public void Set_Another_Validator_To_Replace_15_And_Reset(string no)
        {
            Assert.Throws<ArgumentNullException>(() => IDValidatorHelper.SetValidator((int)IDLength.Fifteen, null));
            IDValidatorHelper.SetValidator((int)IDLength.Fifteen, new NotImplementedIDValidator());
            Assert.Throws<NotImplementedException>(() => IDValidatorHelper.Validate(no));
            IDValidatorHelper.ResetDefaultValidator();
            Assert.True(IDValidatorHelper.Validate(no).IsValid);
        }
    }
    internal sealed class NotImplementedIDValidator : IIDValidator
    {
        public IValidationDictionary<int, string> Dictionary { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string GenerateID(int areaNumber, DateTime birthDay, int sequenceNumber)
        {
            throw new NotImplementedException();
        }

        public string GenerateRandomNumber()
        {
            throw new NotImplementedException();
        }

        public IDValidationResult Validate(string idNumber, ushort minYear = 0, AreaValidLimit validLimit = AreaValidLimit.Province, bool ignoreCheckBit = false)
        {
            throw new NotImplementedException();
        }

        public IDValidationResult Validate(string number)
        {
            throw new NotImplementedException();
        }
    }
}
