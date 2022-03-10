using NumberValidators.IdentityCards;
using NumberValidators.IdentityCards.Validators;
using System;
using Xunit;

namespace NumberValidators.Tests.IdentityCards
{
    public class ID18ValidatorTests
    {
        private readonly IDValidator validator = new ID18Validator();

        [Fact]
        public void Default_Prop()
        {
            Assert.Equal(IDLength.Eighteen, validator.IDLength);
        }

        [Fact]
        public void GenerateRandomNumber_Length_Should_Equals_18()
        {
            var number = validator.GenerateRandomNumber();
            Assert.Equal(18, number.Length);
        }

        [Fact]
        public void GenerateID_Length_Should_Equals_18()
        {
            var number = validator.GenerateID(310104, new DateTime(2000, 1, 1), 7);
            Assert.Equal(18, number.Length);
        }
        [Fact]
        public void GenerateID_ArgumentException()
        {
            Assert.Throws<ArgumentException>(() => validator.GenerateID(5, new DateTime(2000, 1, 1), 7));
            Assert.Throws<ArgumentException>(() => validator.GenerateID(310104, new DateTime(2000, 1, 1), 9999));
        }

        [Theory]
        [InlineData("32021919900101003X")]
        [InlineData("320281198501010036")]
        [InlineData("320281000101010036")]
        [InlineData("32028119950101003X")]
        public void Validate_Error(string no)
        {
            var result = validator.Validate(no, 1990, AreaValidLimit.County);
            Assert.False(result.IsValid);
            Assert.Equal(no, result.Number);
            Assert.NotEmpty(result.Errors);
        }

        [Theory]
        [InlineData("32021919900101003X")]
        public void Validate_Correct(string no)
        {
            var result = validator.Validate(no);
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
            Assert.NotNull(result.RecognizableArea);
            Assert.Equal(IDLength.Eighteen, result.IDLength);
            Assert.Equal(new DateTime(1990, 1, 1), result.Birthday);
            Assert.Equal(Gender.Male, result.Gender);
            Assert.Equal(320219, result.AreaNumber);
            Assert.Equal(003, result.Sequence);
            Assert.Equal('X', result.CheckBit);
            Assert.Equal("江苏省无锡市", result.RecognizableArea.FullName);
            Assert.Equal("江苏省", result.RecognizableArea.Parent.Name);
        }
    }
}
