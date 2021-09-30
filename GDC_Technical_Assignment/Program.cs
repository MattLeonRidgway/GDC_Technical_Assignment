/* GDC Technical Assignment
* Created by Matthew Leon Ridgway
* 9/30/2021
* This program will ask the user for the name of a CSV file
* Then Search the directory for the CSV file
* If file exists iterate through each row for email addresses
* Validate the email address found
* Build two lists one for valid and one for invalid email addresses
* When finished show both lists to the console
* If the file doesn't exist show an error message
*
*
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GDC_Technical_Assignment
{
    internal class Program
    { // Check for empty/null list
        public static bool IsEmpty<T>(List<T> list)
        {// Check list for NULL
            if (list == null)
            {
                return true;
            }
            // Linq
            return list.FirstOrDefault() != null;
        }

        // Email validation Code used: https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
        public static bool IsValidEmail(string email)
        {
            // check for Null or white space
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }
            try
            {
                // Regex validation of email address
                email = Regex.Replace(email, @"(@)(.+)$", DomainMap, RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Using the domain
                string DomainMap(Match match)
                {
                    // Convert to Unicode domain
                    var idnMap = new IdnMapping();

                    // Throws Argument Exception if invalid domain
                    string domain_Name = idnMap.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domain_Name;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private static void Main(string[] args)
        {// Two lists
            List<string> validEmail = new List<string>();
            List<string> invalidEmail = new List<string>();
            // Ask for file name in Console save to string fileName
            Console.WriteLine("Please enter a file name to search: ");
            string fileName = Console.ReadLine();

            //If file found
            if (File.Exists(fileName))
            {
                //StreamReader for CSV
                using (var reader = new StreamReader(@fileName))
                {
                    //While Not the end of stream
                    while (!reader.EndOfStream)
                    {
                        //read line
                        var line = reader.ReadLine();
                        // split by comma
                        var value = line.Split(',');
                        //Iterate through CSV check for a valid email if valid add to validEmail list
                        // Else add to invalidEmail list
                        // CSV file is first name0, last name1, email2

                        if (IsValidEmail(value[2]))
                        {
                            validEmail.Add(value[0] + " " + value[1] + " " + value[2]);
                        }
                        else
                        {
                            invalidEmail.Add(value[0] + " " + value[1] + " " + value[2]);
                        }
                    }
                }
            }// else Error Message
            else
            {
                Console.WriteLine("Error File not found");
            }
            bool validEmailList = IsEmpty(validEmail);// check null empty
            if (validEmailList)
            {
                // for loop through valid email list
                for (int forLoopCount = 0; forLoopCount < validEmail.Count; forLoopCount++)
                {
                    Console.WriteLine(validEmail[forLoopCount]);
                }
                Console.WriteLine("End of valid email list");
            }
            else
            {
                Console.WriteLine("Valid Email List is empty");
            }

            bool inValidEmailList = IsEmpty(invalidEmail);// check null empty
            if (inValidEmailList)
            {
                // foreach loop inValidEmail list
                foreach (var inValid in invalidEmail)
                {
                    Console.WriteLine(inValid);
                }
                Console.WriteLine("End of invalid email list");
            }
            else
            {
                Console.WriteLine("inValid Email List is empty");
            }
        }// endMain
    }
}