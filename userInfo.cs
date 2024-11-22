using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace ticket_purchase_system
{
    public class UserInfo
    {
        public static string UserRecordsFilePath = "Customersdatabase.txt";
        public static string email;

        private string username;
        private string password;
        public string category;
        private string chosenPassword;
        private string passwordConfirmation;

        public static bool IsEmailValid(string emailAdress)
        {
            // checks if email address is valid
            string validEmailPattern = @"^([\w\.-]+)@([\w-]+)\.([a-z]{2,5})(\.[a-z]{2,5})*";
            Regex rx = new Regex(validEmailPattern, RegexOptions.IgnoreCase);
            return rx.IsMatch(emailAdress);

        }

        // reads the database and check if the account( email address) is already registered
        public static bool AccountExists(string filepath)
        {
            string[] line = File.ReadAllLines(filepath);
            int emailPosition = 0;

            foreach (string i in line)
            {
                string[] columns = i.Split(',');
                if (IsFoundUser(columns, email, emailPosition))
                {
                    Console.WriteLine("Account already exists.");

                    return true;
                }
            }
            return false;
        }

        // searches each line of the database using the email address to get a match.
        public static bool IsFoundUser(string[] uniqueLine, string accountIdentifier, int accountIdPosition)
        {
            
            if (uniqueLine[accountIdPosition].Equals(accountIdentifier))
            {
                return true;
            }
            return false;
        }

        // sets the username field of the class after it must have passed the required checks.
        public void SetUsername()
        {

            while (!IsEmailValid(email))
            {
                Console.Write("Enter a valid Email address: ");
                email = Console.ReadLine();
            }
            // Ensures that a username field is only set if the account did not previously exist in the database
            if (!AccountExists(UserRecordsFilePath))
            {
                this.username = email;
            }
        }
        
        public void PasswordInstruction()
        {
            Console.Write("Create password: ");
            chosenPassword = Console.ReadLine();
            Console.Write("Confirm password: ");
            passwordConfirmation = Console.ReadLine();
        }
        // sets the password field of the class.
        public void SetPassword()
        {
            PasswordInstruction();
            while (chosenPassword != passwordConfirmation)
            {
                Console.WriteLine("Passwords do not match. Try again");
                PasswordInstruction();
            }
            Console.WriteLine("Password created successfully");

            this.password = chosenPassword;
        }

        //sets the category field of the user based on the group they fall into.
        public void SetCategory(string emailAddress)
        {
            //specifies the keywords in the email address entered by the user that identifies whether the user is a student or a staff
            string studentIdentifier = "@my.ntu.ac.uk";
            string staffIdentifier = "@ntu.ac.uk";

            string category1 = "student";
            string category2 = "staff";
            string category3 = "guest";

            if (emailAddress.Contains(studentIdentifier))
            {
                this.category = category1;
            }
            else if (emailAddress.Contains(staffIdentifier))
            {
                this.category = category2;
            }
            else
            {
                this.category = category3;
            }
        }

        // adds the users details to the database. 
        public void AddUser()
        {

            try
            {
                StreamWriter writeToDatabase = new StreamWriter(UserRecordsFilePath, append: true);
                writeToDatabase.WriteLine(username + "," + password + "," + category);
                writeToDatabase.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("Error. Arguments must be in string format and in the right order.");
            }
        }

        // procedure created to carry out a series of steps in a single line of code.
        public void AccountSetupProcedure()
        {
            SetUsername();
            SetPassword();
            SetCategory(UserInfo.email);
            AddUser();
        }
    }
}
