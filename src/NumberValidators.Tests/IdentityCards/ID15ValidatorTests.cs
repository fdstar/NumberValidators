using NumberValidators.IdentityCards;
using NumberValidators.IdentityCards.Validators;
using System;
using Xunit;

namespace NumberValidators.Tests.IdentityCards
{
    public class ID15ValidatorTests
    {
        private readonly IDValidator validator = new ID15Validator();

        [Fact]
        public void Default_Prop()
        {
            Assert.Equal(IDLength.Fifteen, validator.IDLength);
        }

        [Fact]
        public void GenerateRandomNumber_Length_Should_Equals_15()
        {
            var number = validator.GenerateRandomNumber();
            Assert.Equal(15, number.Length);
        }

        [Fact]
        public void GenerateID_Length_Should_Equals_15()
        {
            var number = validator.GenerateID(310104, new DateTime(2000, 1, 1), 7);
            Assert.Equal(15, number.Length);
        }
        [Fact]
        public void GenerateID_ArgumentException()
        {
            Assert.Throws<ArgumentException>(()=> validator.GenerateID(5, new DateTime(2000, 1, 1), 7));
            Assert.Throws<ArgumentException>(() => validator.GenerateID(310104, new DateTime(2000, 1, 1), 9999));
        }

        [Theory]
        [InlineData("954321700422319")]
        [InlineData("100000700422319")]
        public void Validate_Error(string no)
        {
            var valid = (IValidator<IDValidationResult>)validator;
            var result = valid.Validate(no);
            Assert.False(result.IsValid);
            Assert.Equal(no, result.Number);
            Assert.NotEmpty(result.Errors);
        }

        [Theory]
        [InlineData("411702700422319")]
        public void Validate_Correct(string no)
        {
            var result = validator.Validate(no);
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
            Assert.NotNull(result.RecognizableArea);
            Assert.Equal(IDLength.Fifteen, result.IDLength);
            Assert.Equal(new DateTime(1970, 4, 22), result.Birthday);
            Assert.Equal(Gender.Male, result.Gender);
            Assert.Equal(411702, result.AreaNumber);
            Assert.Equal(result.AreaNumber, result.RecognizableArea.Number);
            Assert.Equal(319, result.Sequence);
            Assert.Equal(char.MinValue, result.CheckBit);
        }
    }
}
