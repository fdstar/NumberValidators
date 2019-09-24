using NumberValidators.BusinessRegistrationNos.Validators;
using NumberValidators.IdentityCards.Validators;
using NumberValidators.Invoices.Validators;
using System;

namespace NumberValidators.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.WriteLine("***身份证***");
            var id18Validator = new ID18Validator();
            Console.WriteLine("随机的新身份证：" + id18Validator.GenerateRandomNumber());
            var id15Validator = new ID15Validator();
            Console.WriteLine("随机的旧身份证：" + id15Validator.GenerateRandomNumber());
            var idValid = IDValidatorHelper.Validate("32021919900101003X", ignoreCheckBit: false);
            Console.WriteLine("身份证验证结果：" + idValid.IsValid);

            Console.WriteLine();
            Console.WriteLine("***增值税发票***");
            var vat10Validator = new VATCode10Validator();
            var vat12Validator = new VATCode12Validator();
            Console.WriteLine("随机的增值税发票：" + vat10Validator.GenerateRandomNumber());
            Console.WriteLine("生成指定的增值税专用发票：" + vat10Validator.GenerateVATCode(3700, 2017, 1, Invoices.VATKind.Special));
            Console.WriteLine("生成指定的10位增值税普通发票：" + vat10Validator.GenerateVATCode(1100, 2017, 2, Invoices.VATKind.Plain));
            Console.WriteLine("生成指定的12位增值税普通发票：" + vat12Validator.GenerateVATCode(1100, 2018, 6, Invoices.VATKind.Plain));
            Console.WriteLine("随机的增值税电子/卷票/普票：" + vat12Validator.GenerateRandomNumber());
            string[] vatArr = { "034011900111", "1234567890", "144131909110", "031001600311", "3100153130", "011001800304", "035001800112", "144031909110" };
            foreach (var vat in vatArr)
            {
                var valid = VATCodeValidatorHelper.Validate(vat, minYear: 2012);
                if (valid.IsValid)
                {
                    Console.WriteLine("{0}验证结果：{1} 类型{2} 行政区划名称({3}) 验证结果类型:{4}", vat, valid.IsValid, valid.Category, valid.AreaName, valid);
                    if (valid.Category == Invoices.VATKind.Electronic)
                    {
                        Console.WriteLine("{0}对应的增值税电子发票细分类型为：{1}", vat, ((VATCode12ValidationResult)valid).ElectronicVATKind);
                    }
                    else if (valid.Category == Invoices.VATKind.Blockchain)
                    {
                        Console.WriteLine("{0}对应的区块链电子发票批次为：{1} 联次为：{2}", vat, ((VATCode12ValidationResult)valid).Batch, ((VATCode12ValidationResult)valid).DuplicateNumber);
                    }
                }
                else
                {
                    PrintError(valid);
                }
            }

            Console.WriteLine();
            Console.WriteLine("***工商注册码/统一社会信用代码***");
            string[] rnArr = { "110108000000016", "91320621MA1MRHG205" };
            foreach (var rn in rnArr)
            {
                var valid = RegistrationNoValidatorHelper.Validate(rn, validLimit: null);
                Console.WriteLine("{0}验证结果：{1} 长度{2} 行政区划名称({3}) 验证结果类型:{4}", rn, valid.IsValid, valid.RegistrationNoLength, valid.RecognizableArea.FullName, valid);
            }
            Console.WriteLine("随机的工商注册码：" + new RegistrationNo15Validator().GenerateRandomNumber());
            Console.WriteLine("随机的统一社会信用代码：" + new RegistrationNo18Validator().GenerateRandomNumber());
            Console.ReadLine();
        }

        static void PrintError(ValidationResult valid)
        {
            if (!valid.IsValid)
            {
                var prevColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("{0}验证失败：错误信息：{1}", valid.Number, string.Join(",", valid.Errors));
                Console.ForegroundColor = prevColor;
            }
        }
    }
}
