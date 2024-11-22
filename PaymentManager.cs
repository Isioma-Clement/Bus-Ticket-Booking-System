using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ticket_purchase_system
{
    public class PaymentManager
    {
        private string cardNumber;
        private string cvv;
        private string cardDate;

        // checks if the card number input from the user is valid ( based on the chosen acceptable pattern)
        public bool IsValidCardNumber(string cardnumber)
        {
            string validCardNumberPattern = @"^[\d\s*]{8,20}$";
            Regex rx = new Regex(validCardNumberPattern);
            return rx.IsMatch(cardnumber);
        }

        // checks if the card number input from the user is valid.
        public bool IsCorrectCVV(string cvv)
        {
            string validCvvPattern = @"^[\d]{3}$";
            Regex rx = new Regex(validCvvPattern);
            return rx.IsMatch(cvv);
        }

        //checks if the date of the card is entered in the appropriate format and not expired.
        public bool CardDateValid(string carddate)
        {
            string acceptableDateFormat = "MM/yy";
            DateTime.TryParseExact(carddate, acceptableDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate);
            if (parsedDate > DateTime.Now)
            {
                Console.WriteLine("Payment Successful");
                return true;
            }
            Console.WriteLine("Date entry is either an invalid format or card is expired");
            return false;

        }

        // sets the cardnumber field only after checking that is valid
        public void EnterCardNumber()
        {
            do
            {
                Console.Write("Enter card number: ");
                this.cardNumber = Console.ReadLine();

            } while (!IsValidCardNumber(cardNumber));
        }

        // sets the cvv field only after checking that is valid
        public void EnterCVV()
        {
            do
            {
                Console.Write("Enter your cvv: ");
                this.cvv = Console.ReadLine();
            } while (!IsCorrectCVV(cvv));
        }

        // sets the card date field only after checking that it is valid and continues to loop while invalid
        public void EnterCardDate()
        {
            Console.Write("Enter Expiry date on card. **Date must be in MM/YY format)**: ");
            this.cardDate = Console.ReadLine();

            while (!CardDateValid(cardDate))
            {
                Console.WriteLine("Enter card date again**Date must be in MM/YY format)**: ");
                cardDate = Console.ReadLine();
            }
        }

        // procedure to invoke the different steps of the payment process in one line of code.
        public void PaymentProcedure()
        {
            EnterCardNumber();
            EnterCVV();
            EnterCardDate();
        }

    }
}
