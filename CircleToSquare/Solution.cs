using System;
using System.Collections.Generic;

namespace CircleToSquare
{
    class Solution
    {
        public static Dictionary<string, string> countries = new Dictionary<string, string>
        {
            ["UA"] = "Ukraine",
            ["RU"] = "Russia",
            ["CA"] = "Canada"
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Input country code");
            string countryCode = Console.ReadLine();
            Console.WriteLine("Input company");
            string company = Console.ReadLine();
            Console.WriteLine("Input contact first name");
            string firstName = Console.ReadLine();
            Console.WriteLine("Input contact last name");
            string lastName = Console.ReadLine();
            Console.WriteLine("Input country phone code");
            int countryPhoneCode = Convert.ToInt16(Console.ReadLine());
            Console.WriteLine("Input phone number (up to 10 digits)");
            ulong phoneNumber = Convert.ToUInt64(Console.ReadLine());

            IIncomingData incomingData = new SimpleIncomingData(countryCode, company, firstName, lastName, countryPhoneCode, phoneNumber);
            Console.WriteLine("\nIncoming data:");
            Console.WriteLine("Country code is " + incomingData.getCountryCode());
            Console.WriteLine("Company is " + incomingData.getCompany());
            Console.WriteLine("First name is " + incomingData.getContactFirstName());
            Console.WriteLine("last name is " + incomingData.getContactLastName());
            Console.WriteLine("Country phone code is " + incomingData.getCountryPhoneCode());
            Console.WriteLine("Phone number is " + incomingData.getPhoneNumber());

            IncomingDataToCustomerAdapter toCustomer = new IncomingDataToCustomerAdapter(incomingData);
            Console.WriteLine("\nTo customer:");
            Console.WriteLine("--Company name is " + toCustomer.getCompanyName());
            Console.WriteLine("--Country name is " + toCustomer.getCountryName());

            IncomingDataToContactAdapter toContact = new IncomingDataToContactAdapter(incomingData);
            Console.WriteLine("\nTo contact:");
            Console.WriteLine("--Person name is " + toContact.getName());
            Console.WriteLine("--Phone number is " + toContact.getPhoneNumber());

            Console.ReadKey();
        }

        public interface IIncomingData
        {
            string getCountryCode(); //For example: UA
            string getCompany(); //For example: JavaRush Ltd.
            string getContactFirstName(); //For example: Ivan
            string getContactLastName(); //For example: Ivanov
            int getCountryPhoneCode(); //For example: 38
            ulong getPhoneNumber(); //For example: 501234567
        }

        public interface ICustomer
        {
            string getCompanyName(); //For example: JavaRush Ltd.
            string getCountryName(); //For example: Ukraine
        }

        public interface IContact
        {
            string getName(); //For example: Ivanov, Ivan
            string getPhoneNumber(); //For example: +38(050)123-45-67
        }

        public class SimpleIncomingData : IIncomingData
        {
            private string countryCode;
            private string company;
            private string firstName;
            private string lastName;
            private int countryPhoneCode;
            private ulong phoneNumber; // 0 to 18,446,744,073,709,551,615

            public SimpleIncomingData(string countryCode, string company, string firstName, string lastName, int countryPhoneCode, ulong phoneNumber)
            {
                this.countryCode = countryCode;
                this.company = company;
                this.firstName = firstName;
                this.lastName = lastName;
                this.countryPhoneCode = countryPhoneCode;
                this.phoneNumber = phoneNumber;
            }

            public string getCountryCode()
            {
                return countryCode;
            } //For example: UA

            public string getCompany()
            {
                return company;
            } //For example: JavaRush Ltd.

            public string getContactFirstName()
            {
                return firstName;
            } //For example: Ivan
            public string getContactLastName()
            {
                return lastName;
            } //For example: Ivanov

            public int getCountryPhoneCode()
            {
                return countryPhoneCode;
            } //For example: 38

            public ulong getPhoneNumber()
            {
                return phoneNumber;
            } //For example: 501234567
        }

        public class IncomingDataToCustomerAdapter : ICustomer
        {
            private IIncomingData incomingData;

            public IncomingDataToCustomerAdapter(IIncomingData incomingData)
            {
                this.incomingData = incomingData;
            }

            public string getCompanyName()
            {
                return incomingData.getCompany();
            }

            public string getCountryName()
            {
                return countries[incomingData.getCountryCode()];
            }
        }

        public class IncomingDataToContactAdapter : IContact
        {
            private IIncomingData incomingData;

            public IncomingDataToContactAdapter(IIncomingData incomingData)
            {
                this.incomingData = incomingData;
            }

            public string getName()
            {
                return incomingData.getContactLastName() + ", " + incomingData.getContactFirstName();
            }

            public string getPhoneNumber()
            {
                string tenDigitsNumber = incomingData.getPhoneNumber().ToString("D10");
                return '+' + incomingData.getCountryPhoneCode().ToString() + string.Format("({0}){1}-{2}-{3}",
                    tenDigitsNumber.Substring(0, 3),
                    tenDigitsNumber.Substring(3, 3),
                    tenDigitsNumber.Substring(6, 2),
                    tenDigitsNumber.Substring(8));
            }
        }
    }
}
