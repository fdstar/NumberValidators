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

            var idValid = IDValidatorHelper.Validate("32021919900101003X");
            var eInvoiceValid = VATCodeValidatorHelper.Validate("031001600311");
            var specialInvoiceValid = VATCodeValidatorHelper.Validate("3100153130");
            Console.ReadLine();
        }
    }
}
