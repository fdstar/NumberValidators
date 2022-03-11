using NumberValidators.Invoices;
using NumberValidators.Invoices.Validators;
using System;
using Xunit;

namespace NumberValidators.Tests
{
    public class VATCode12ValidatorTests
    {
        private readonly VATCode12Validator validator = new VATCode12Validator();

        [Fact]
        public void GenerateRandomNumber_Length_Should_Equals_12()
        {
            var number = validator.GenerateRandomNumber();
            Assert.Equal(12, number.Length);
        }

        [Fact]
        public void GenerateVATCode_Length_Should_Equals_12()
        {
            var number = validator.GenerateVATCode(3100, 2020, 1, VATKind.Electronic, ElectronicVATKind.Normal);
            Assert.Equal(12, number.Length);
        }
        [Fact]
        public void GenerateVATCode_Exception()
        {
            Assert.Throws<ArgumentException>(() => validator.GenerateVATCode(4403, 2020, 1, VATKind.Blockchain, ElectronicVATKind.Normal));
            Assert.Throws<ArgumentException>(() => validator.GenerateVATCode(5, 2020, 1, VATKind.Electronic));
            Assert.Throws<NotSupportedException>(() => validator.GenerateVATCode(3100, 2020, 1, VATKind.Transport));
        }

        [Theory]
        [InlineData("12345")]
        [InlineData("011001800304")]
        [InlineData("035001800112")]
        [InlineData("054002000314")]
        [InlineData("144052009110")]
        [InlineData("144032009111")]
        public void Validate_Error(string no)
        {
            var result = validator.Validate(no, minYear: 2020);
            Assert.False(result.IsValid);
            Assert.Equal(no, result.Number);
            Assert.NotEmpty(result.Errors);
        }

        [Theory]
        [InlineData("065122100313")]
        public void Validate_Correct_With_Electronic(string no)
        {
            var result = validator.Validate(no);
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
            Assert.Equal(VATLength.Twelve, result.VATLength);
            Assert.Equal(6512, result.AreaNumber);
            Assert.Equal("新疆维吾尔自治区", result.AreaName);
            Assert.Equal(VATKind.Electronic, result.Category);
            Assert.Equal(2021, result.Year);
            Assert.Equal(3, result.Batch);
            Assert.Equal(0, result.DuplicateNumber);
            Assert.Equal(ElectronicVATKind.Special, result.ElectronicVATKind);
        }

        [Theory]
        [InlineData("011001800604")]
        public void Validate_Correct_With_Plain(string no)
        {
            var result = validator.Validate(no);
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
            Assert.Equal(VATLength.Twelve, result.VATLength);
            Assert.Equal(VATKind.Plain, result.Category);
            Assert.Equal(2018, result.Year);
            Assert.Equal(6, result.Batch);
            Assert.Equal(2, result.DuplicateNumber);
            Assert.Null(result.ElectronicVATKind);
        }

        [Theory]
        [InlineData("144031909110")]
        public void Validate_Correct_With_Blockchain(string no)
        {
            var result = ((IValidator<VATCode12ValidationResult>)validator).Validate(no);
            Assert.True(result.IsValid);
            Assert.Equal(VATKind.Blockchain, result.Category);
        }
    }
}
