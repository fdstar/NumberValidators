using NumberValidators.Invoices;
using NumberValidators.Invoices.Validators;
using System;
using Xunit;

namespace NumberValidators.Tests
{
    public class VATCode10ValidatorTests
    {
        private readonly VATCode10Validator validator = new VATCode10Validator();

        [Fact]
        public void GenerateRandomNumber_Length_Should_Equals_10()
        {
            var number = validator.GenerateRandomNumber();
            Assert.Equal(10, number.Length);
        }

        [Fact]
        public void GenerateVATCode_Length_Should_Equals_10()
        {
            var number = validator.GenerateVATCode(3100, 2020, 1, VATKind.Special);
            Assert.Equal(10, number.Length);
        }
        [Fact]
        public void GenerateVATCode_Exception()
        {
            Assert.Throws<ArgumentException>(() => validator.GenerateVATCode(5, 2020, 1, VATKind.Special));
            Assert.Throws<NotSupportedException>(() => validator.GenerateVATCode(3100, 2020, 1, VATKind.Blockchain));
        }

        [Theory]
        [InlineData("3100203130")]
        [InlineData("3120203130")]
        [InlineData("1100172320")]
        public void Validate_Error(string no)
        {
            var result = validator.Validate(no, VATKind.Plain, 2018);
            Assert.False(result.IsValid);
            Assert.Equal(no, result.Number);
            Assert.NotEmpty(result.Errors);
        }

        [Theory]
        [InlineData("3100153130")]
        public void Validate_Correct(string no)
        {
            var result = validator.Validate(no);
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
            Assert.Equal(VATLength.Ten, result.VATLength);
            Assert.Equal(3100, result.AreaNumber);
            Assert.Equal("上海市", result.AreaName);
            Assert.Equal(VATKind.Special, result.Category);
            Assert.Equal(2015, result.Year);
            Assert.Equal(1, result.Batch);
            Assert.Equal(3, result.DuplicateNumber);
            Assert.Equal(AmountVersion.Computer, result.AmountVersion);
        }
    }
}
