using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace GIW.Classes
{
    public class Contact
    {
        [PrimaryKey, AutoIncrement]
        public int ID
        {
            get;
            set;
        }
        public string FirstName 
        { 
            get; 
            set; 
        }

        [MaxLength(10)]
        public string LastName
        {
            get;
            set;
        }

        public string CompanyName
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string PhoneNumber
        {
            get;
            set;
        }

        [MaxLength(250)]
        public string Comments
        {
            get;
            set;
        }

        public string FormattedText
        {
            get
            {
                return String.Format("{0}" + " " + "{1}" + ", " + "{2}" + ", " + "{3}" + ", " + "{4}", FirstName, LastName, CompanyName, PhoneNumber, DateAdded.ToShortDateString());

            }
        }

        public DateTime DateAdded
        {
            get;
            set;
        }

        public bool BlogChkBox
        {
            get;
            set;
        }

        public bool QrtlyNewsltrChkBox
        {
            get;
            set;
        }

        public string Product
        {
            get;
            set;
        }

        public string SalesTerritory
        {
            get;
            set;
        }

        public string USRegion
        {
            get;
            set;
        }

        public string QRCode
        {
            get;
            set;
        }
    }
}
