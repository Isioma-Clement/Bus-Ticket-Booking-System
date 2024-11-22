using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ticket_purchase_system
{
    public class DiscountRates
    {
        public decimal studentDiscount;
        public decimal staffDiscount;
        public decimal guestDiscount;
       
        public DiscountRates()
        {
            string[] configFileLine = File.ReadAllLines(TickettingConfigurations.tickettingConfigFilePath);
            string discountType1 = "Student";
            string discountType2 = "Staff";
            string discountType3 = "Guest";

            //the index position of the value of the discount rate in the array after splitting.
            int positionOfRates = 1;
            
            char xterToSplitBy = ':';

            //reads the different rates of discounts from the configuration file and sets the fields in the class
            foreach (string line in configFileLine)
            {
                try
                {
                    if (line.Contains(discountType1))
                    {
                        string[] lineInfo = line.Split(xterToSplitBy);
                        this.studentDiscount = decimal.Parse(lineInfo[positionOfRates]);

                    }
                    if (line.Contains(discountType2))
                    {
                        string[] lineInfo = line.Split(xterToSplitBy);
                        this.staffDiscount = decimal.Parse(lineInfo[positionOfRates]);
                    }
                    if (line.Contains(discountType3))
                    {
                        string[] lineInfo = line.Split(xterToSplitBy);
                        this.guestDiscount = decimal.Parse(lineInfo[positionOfRates]);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("wrong data entry format in the config file");
                }

            }
        }
    }
}
