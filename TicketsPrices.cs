
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace ticket_purchase_system
{
    public class TicketsPrices
    {
        public decimal singleTrip;
        public decimal allDay;
        public decimal allWeek;

      
        //reads the prices of the different types of tickets from the configuration file and sets the fields in the class
        public TicketsPrices()
        {
            string[] configFileLine = File.ReadAllLines(TickettingConfigurations.tickettingConfigFilePath);

            string ticketType1 = "Single trip";
            string ticketType2 = "All day";
            string ticketType3 = "All week";

            //specifies the index position of the value of the ticket prices in the array after splitting.
            int positionOfPrices = 1;

            char xterToSplitBy = ':';

            //reads the different prices of tickets from the configuration file and sets the fields in the class
            foreach (string line in configFileLine)
            {
                try
                {
                    if (line.Contains(ticketType1))
                    {
                        string[] lineInfo = line.Split(xterToSplitBy);
                        this.singleTrip = decimal.Parse(lineInfo[positionOfPrices]);

                    }
                    if (line.Contains(ticketType2))
                    {
                        string[] lineInfo = line.Split(xterToSplitBy);
                        this.allDay = decimal.Parse(lineInfo[positionOfPrices]);
                    }
                    if (line.Contains(ticketType3))
                    {
                        string[] lineInfo = line.Split(xterToSplitBy);
                        this.allWeek = decimal.Parse(lineInfo[positionOfPrices]);
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
