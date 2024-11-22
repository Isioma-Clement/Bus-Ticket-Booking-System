using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Runtime.CompilerServices;
using System.Net.Http;
using System.Net.Mail;
using System.Globalization;

namespace ticket_purchase_system
{
    class Program
    {
    
        public  static bool successfulLogIn = false;
        

        static void Main(string[] args)
        {
            
            // creates the reference objects of the various classes in the program 
            TickettingConfigurations.CreateConfigFile();
            Program program = new Program();
            UserInfo UI = new UserInfo();
            PaymentManager paymentManager = new PaymentManager();
            TicketsPrices ticketPrice = new TicketsPrices();
            DiscountRates discountRate = new DiscountRates();

            // create the database to store users' information if it does not already exist.
            if (!File.Exists(UserInfo.UserRecordsFilePath))
            {
                CreateDatabase(UserInfo.UserRecordsFilePath);
            }


            do
            {
                Console.WriteLine();
                Console.WriteLine("Welcome to NTU Transport Service");
                Console.WriteLine("Please note:  If you are a staff or a student and this is your first time using this service, please register with your school email to get appropriate discounts.");
                Console.WriteLine();
                Console.WriteLine("Do you have an account? ");
                Console.Write("Enter Yes if you do or Enter No if you dont: ");

                try
                {
                    // user's input of yes/no for if they already have an account.
                    string accountExists = Console.ReadLine().ToLower();
                   
                    if (accountExists == "yes" || accountExists == "y")
                    {
                        Console.Write("Please enter your email address: ");
                        UserInfo.email = Console.ReadLine().ToLower();

                        // first checks if the email address is valid
                        if (UserInfo.IsEmailValid(UserInfo.email))
                        {
                            // then checks if account already exists in the database
                            if (UserInfo.AccountExists(UserInfo.UserRecordsFilePath))
                            {
                                program.LoginProcedure();
                                UI.SetCategory(UserInfo.email);
                            }
                            else
                            {
                                Console.WriteLine("Account not found. Create password to register. ");
                                
                                UI.AccountSetupProcedure();
                                program.LoginProcedure();                              
                            }
                        }
                        else
                        {
                            do
                            {
                                Console.Write("Email address Invalid! Please enter a valid email address: ");
                                UserInfo.email = Console.ReadLine().ToLower();
                            }
                            while (!UserInfo.IsEmailValid(UserInfo.email));

                            if (UserInfo.AccountExists(UserInfo.UserRecordsFilePath))
                            {
                                program.LoginProcedure();
                            }
                            else
                            {
                                Console.WriteLine("Account not found. Create password to register ");
                               
                                UI.AccountSetupProcedure();
                                program.LoginProcedure();
                            }                         
                        }
                        
                    }
                    else if (accountExists == "no" || accountExists == "n")
                    {
                        
                        Console.Write("Please enter your email address to register: ");
                        UserInfo.email = Console.ReadLine().ToLower();

                        //still checks if the user is already registered in the system even though they have said they do not
                        if (UserInfo.AccountExists(UserInfo.UserRecordsFilePath))
                        {                           
                            program.LoginProcedure();
                            UI.SetCategory(UserInfo.email);
                        }
                        else
                        {
                            UI.AccountSetupProcedure();
                            program.LoginProcedure();
                        }

                    }
                    else
                    {
                        Console.WriteLine("Invalid Input! Enter Yes or No. First press enter to restart");
                        Console.ReadLine();
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid format. Entry must be a string");
                }

            } while (successfulLogIn == false);



            string ticketType1 = "Single";
            string ticketType2 = "All day";
            string ticketType3 = "All week";

            //dictionary to hold the keys (type of ticket) and values(amount) for the students' tickets.
            Dictionary<string, decimal> StudentTickets = new Dictionary<string, decimal>()
            {
                [ticketType1] = ticketPrice.singleTrip - (ticketPrice.singleTrip * discountRate.studentDiscount),
                [ticketType2] = ticketPrice.allDay - (ticketPrice.allDay * discountRate.studentDiscount),
                [ticketType3] = ticketPrice.allWeek - (ticketPrice.allWeek * discountRate.studentDiscount)
            };

            //dictionary to hold the keys (type of ticket) and values(amount) for the staff tickets.
            Dictionary<string, decimal> StaffTickets = new Dictionary<string, decimal>()
            {
                [ticketType1] = ticketPrice.singleTrip - (ticketPrice.singleTrip * discountRate.staffDiscount),
                [ticketType2] = ticketPrice.allDay - (ticketPrice.allDay * discountRate.staffDiscount),
                [ticketType3] = ticketPrice.allWeek - (ticketPrice.allWeek * discountRate.staffDiscount),
            };

            //dictionary to hold the keys (type of ticket) and values(amount) for the guests' tickets.
            Dictionary<string, decimal> GuestTickets = new Dictionary<string, decimal>()
            {
                [ticketType1] = ticketPrice.singleTrip - (ticketPrice.singleTrip * discountRate.guestDiscount),
                [ticketType2] = ticketPrice.allDay - (ticketPrice.allDay * discountRate.guestDiscount),
                [ticketType3] = ticketPrice.allWeek - (ticketPrice.allWeek * discountRate.guestDiscount)
            };


            Console.WriteLine();

            int userSelection;
            decimal totalPrice = 0;
            
            // assigning of these strings to variables is necessary to match the category of user to the appropriate discount type. 
            string discountType1 = "student";
            string discountType2 = "staff";
            bool validSelection = true;


            do
            {

                try
                {
                    Console.WriteLine();
                    Console.WriteLine("Available Tickets are: ");
                    Console.WriteLine();

                    string getOption = "";

                    if (UI.category == discountType1)
                    {
                        // loops through the appropriate dictionaries and prints out the list of available tickets and their prices.
                        foreach (KeyValuePair<string, decimal> kvp in StudentTickets)
                        {
                            Console.WriteLine(kvp.Key + ":  " + Math.Round(kvp.Value, 2));

                        }

                        TicketSelectionInstruction();
                        userSelection = int.Parse(Console.ReadLine());

                        //the getOption variable holds the string that the ticketSelection function returns.
                        getOption = TicketSelection(userSelection);
                        totalPrice += StudentTickets[getOption];
                    }
                    else if (UI.category == discountType2)
                    {

                        foreach (KeyValuePair<string, decimal> kvp in StaffTickets)
                        {
                            Console.WriteLine(kvp.Key + ":  " + Math.Round(kvp.Value, 2));
                        }

                        TicketSelectionInstruction();
                        userSelection = int.Parse(Console.ReadLine());

                        getOption = TicketSelection(userSelection);
                        totalPrice += StaffTickets[getOption];
                    }
                    else
                    {
                        foreach (KeyValuePair<string, decimal> kvp in GuestTickets)
                        {
                            Console.WriteLine(kvp.Key + ":  " + Math.Round(kvp.Value, 2));
                        }

                        TicketSelectionInstruction();
                        userSelection = int.Parse(Console.ReadLine());

                        getOption = TicketSelection(userSelection);

                        totalPrice += GuestTickets[getOption];

                    }

                    Console.WriteLine();
                    Console.WriteLine("Do you want to buy more tickets?");
                    Console.Write("Please press \"yes\" if you do. ** the only valid option is \"yes\" ** : ");

                    string validAdditionalTicketEntry1 = "yes";
                    string validAdditionalTicketEntry2 = "y";

                    string additionalTicketChoice = Console.ReadLine().ToLower();

                    if (additionalTicketChoice == validAdditionalTicketEntry1 || additionalTicketChoice == validAdditionalTicketEntry2)
                    {
                        validSelection = true;
                    }
                    else
                    {
                        validSelection = false;
                    }

                }
                catch (Exception)
                {

                }

            } while (validSelection);

            Console.WriteLine("Your Total Cost for Ticket: " + Math.Round(totalPrice,2));

            paymentManager.PaymentProcedure();

            Console.WriteLine();
            Console.WriteLine("Thank you for travelling with us.");
            Console.WriteLine("Please refer to the mobile ticket session to view your tickets. ");


            Console.ReadKey();
        }
 
        
        //creates the database that holds the records of the userInfo and also rights the column labels to the first line of the file
        static void CreateDatabase(string filepath)
        {
            try
            {
                StreamWriter databaseOutputHandle = new StreamWriter(filepath, append: true);
                databaseOutputHandle.WriteLine("Username, Password, Category");
                databaseOutputHandle.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("Error! Argument must be a string");
            }
        }

        //searches the database and checks if the username and password match to approve login.
        static bool IsCorrectLogin(string filepath, string email, string password)
        {
            string[] line = File.ReadAllLines(filepath);
            //specifies the position of the email address and password in the array after splitting.
            int emailPosition = 0;
            int passwordPosition = 1;

            char xterToSplitBy = ',';

            for (int i = 0; i < line.Length; i++)
            {
                string[] uniqueUserLine = line[i].Split(xterToSplitBy);
                if ((uniqueUserLine[emailPosition] == email) && (uniqueUserLine[passwordPosition] == password))
                {
                    return true;
                }
            }
            return false;
        }

        //procedure to login.
        public void LoginProcedure()
        {

            Console.Write("Enter password to log-in to account: ");
            string logIn_Password = Console.ReadLine();
            int numberOfLoginAttempts = 3;
            int loginAttemptsLeft = 2; 


            for (int i = 0; i < numberOfLoginAttempts; ++i)
            {
                if (IsCorrectLogin(UserInfo.UserRecordsFilePath, UserInfo.email, logIn_Password))
                {
                    Console.WriteLine("Log-in successful");
                    successfulLogIn = true;
                    break;

                }
                else if (i < loginAttemptsLeft)
                {
                    Console.Write("Password incorrect, try again: ");
                    logIn_Password = Console.ReadLine();

                }
                else
                {
                    Console.WriteLine("Log-in failed");

                }
            }
        }
        
        //function returns a string corresponding to the type of ticket chosen by the user.
        static string TicketSelection(int option)
        {

            Console.WriteLine();

            string ticketType1 = "Single";
            string ticketType2 = "All day";
            string ticketType3 = "All week";

            string ticketChoice = String.Empty;

            switch (option)
                {
                    case 1:
                        System.Console.WriteLine("You chose 1 for Single");
                        ticketChoice = ticketType1;
                        break;
                    case 2:
                        System.Console.WriteLine("You chose 2 for All day");
                        ticketChoice = ticketType2;
                        break;

                    case 3:
                        System.Console.WriteLine("You chose 3 for All week");
                        ticketChoice = ticketType3;
                    break;
                }
                return ticketChoice;
        }

        //displays the information for the ticket selection 
        static void TicketSelectionInstruction()
        {
            Console.WriteLine();
            Console.WriteLine("\"Which ticket would you like to buy:");
            Console.WriteLine("Please press 1 for \"Single Trip ticket\", 2 for an \"All day Ticket\" or 3 for the \"All Week ticket\"");
            Console.Write("Enter a number corresponding to the ticket you want to buy: ");
        }
        
    }
    
}
