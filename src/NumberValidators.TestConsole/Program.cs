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
            //var vat10Validator = new VATCode10Validator();
            //Console.WriteLine("随机的增值税发票：" + vat10Validator.GenerateRandomNumber());
            //Console.WriteLine("生成指定的增值税专用发票：" + vat10Validator.GenerateVATCode(3700, 2017, 1, Invoices.VATKind.Special));
            //Console.WriteLine("生成指定的增值税普通发票：" + vat10Validator.GenerateVATCode(1100, 2017, 2, Invoices.VATKind.Plain));

            //var id18Validator = new ID18Validator();
            //Console.WriteLine("随机的新身份证：" + id18Validator.GenerateRandomNumber());
            //var id15Validator = new ID15Validator();
            //Console.WriteLine("随机的旧身份证：" + id15Validator.GenerateRandomNumber());

            //var idValid = IDValidatorHelper.Validate("32021919900101003X");
            //var eInvoiceValid = VATCodeValidatorHelper.Validate("031001600311");
            //var specialInvoiceValid = VATCodeValidatorHelper.Validate("3100153130");

            var registrationNo10Validator = new RegistrationNo15Validator();
            var registrationNoValid = registrationNo10Validator.Validate("110108000000016");
            
            
            Console.ReadLine();
        }
    }
}
