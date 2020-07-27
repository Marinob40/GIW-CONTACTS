using System;
using System.ComponentModel;
using Xamarin.Forms;
using GIW.Classes;
using SQLite;
using System.Text.RegularExpressions;
using Xamarin.Essentials;

namespace GIW
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer/Test
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();


            //Creates the items for the product, sales territory, and USregion pickers
            ProductPicker.Items.Add("LCC");
            ProductPicker.Items.Add("LCV");
            ProductPicker.Items.Add("LSA");
            ProductPicker.Items.Add("FGD");
            ProductPicker.Items.Add("HVF");
            ProductPicker.Items.Add("MDX");
            ProductPicker.Items.Add("MHD");
            ProductPicker.Items.Add("TBC");
            ProductPicker.Items.Add("WBC");
            ProductPicker.Items.Add("ZW");
            ProductPicker.Items.Add("Mega Slurry");

            SalesTerritoryPicker.Items.Add("Australia");
            SalesTerritoryPicker.Items.Add("Brazil");
            SalesTerritoryPicker.Items.Add("Canada");
            SalesTerritoryPicker.Items.Add("Central America/Mexico");
            SalesTerritoryPicker.Items.Add("Chile");
            SalesTerritoryPicker.Items.Add("Europe, Asia & Middle East");
            SalesTerritoryPicker.Items.Add("Indonesia");
            SalesTerritoryPicker.Items.Add("North America");
            SalesTerritoryPicker.Items.Add("South Africa");
            SalesTerritoryPicker.Items.Add("United States");

            UsPicker.Items.Add("Northwest");
            UsPicker.Items.Add("Northeast and Midwest");
            UsPicker.Items.Add("South");
            UsPicker.Items.Add("Florida");
        }

        //Creates a contact and inserts into local SQLIte database
        private void submitButton_Clicked(object sender, EventArgs e)
        {

            var firstNameInput = firstNameEntry.Text;
            var lastNameInput = lastNameEntry.Text;
            var companyNameInput = companyNameEntry.Text;
            var emailInput = emailEntry.Text;
            var phoneNumberInput = phoneNumberEntry.Text;
            var QRCodeInput = QRCodeEntry.Text;


            if (firstNameInput == null || firstNameInput == "")
            {
                DisplayAlert("Alert", "First name is required", "ok");
            }
            else if (lastNameInput == null || lastNameInput == "")
            {
                DisplayAlert("Alert", "Last name is required", "ok");
            }
            else if (companyNameInput == null || companyNameInput == "")
            {
                DisplayAlert("Alert", "Company name is required", "ok");
            }
            else if (emailInput == null || emailInput == "")
            {
                DisplayAlert("Alert", "Email is required", "ok");
            }
            else if (phoneNumberInput == null || phoneNumberInput == "")
            {
                DisplayAlert("Alert", "Phone number is required", "ok");
            }
            else if (ProductPicker.SelectedItem == null)
            {
                DisplayAlert("Alert", "Product selection is required", "ok");
            }
            else if (SalesTerritoryPicker.SelectedItem == null)
            {
                DisplayAlert("Alert", "Territory selection is required", "ok");
            }
            else if (SalesTerritoryPicker.SelectedIndex == 9 && UsPicker.SelectedItem == null)
            {
                DisplayAlert("Alert", "Region selection is required", "ok");
            }
            else
            {

                string email = emailEntry.Text;

                Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

                Match emailMatch = emailRegex.Match(email);

                string phoneNumber = phoneNumberEntry.Text;

                Regex phoneNumberRegex = new Regex(@"\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})");

                Match phoneNumberMatch = phoneNumberRegex.Match(phoneNumber);

                if (emailMatch.Success != true)
                {
                    DisplayAlert("Alert", "Email is not valid", "ok");
                }

                else if (phoneNumberMatch.Success != true)
                {
                    DisplayAlert("Alert", "Phone number is not valid", "ok");
                }
                else
                {
                    Contact contact = new Contact()

                    {
                        
                        FirstName = firstNameEntry.Text,
                        LastName = lastNameEntry.Text,
                        CompanyName = companyNameEntry.Text,
                        Email = emailEntry.Text,
                        PhoneNumber = phoneNumberEntry.Text,
                        Comments = commentsEntry.Text,
                        QRCode = QRCodeEntry.Text,
                        DateAdded = DateTime.Now,
                        BlogChkBox = blogRegisterChkbx.IsChecked,
                        QrtlyNewsltrChkBox = quarterlyNewsletterChkbx.IsChecked,
                        Product = ProductPicker.SelectedItem.ToString(),
                        SalesTerritory = SalesTerritoryPicker.SelectedItem.ToString(),
                        USRegion = UsPicker.SelectedItem.ToString(),
                        
                    };



                    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                    {
                        conn.CreateTable<Contact>();
                        int rows = conn.Insert(contact);

                        if (rows > 0)
                        {                          
                            DisplayAlert("Success", "Contact Added", "Ok");
                            firstNameEntry.Text = "";
                            lastNameEntry.Text = "";
                            companyNameEntry.Text = "";
                            emailEntry.Text = "";
                            phoneNumberEntry.Text = "";
                            commentsEntry.Text = "";
                            QRCodeEntry.Text = "";
                            blogRegisterChkbx.IsChecked = false;
                            quarterlyNewsletterChkbx.IsChecked = false;
                            ProductPicker.SelectedIndex = -1;
                            SalesTerritoryPicker.SelectedIndex = -1;
                            UsPicker.SelectedIndex = -1;
                            
                        }                       
                        else
                        {
                            DisplayAlert("Failure", "Contact failed to be inserted", "Ok");
                        }
                    }
                }
            }
        }

        //Takes user to contacts listview
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ContactsPage());
        }

        //Link takes user to products page on KSB's website
        public async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            string url = "https://products.ksb.com/global/";

            await Browser.OpenAsync(new Uri(url), BrowserLaunchMode.SystemPreferred);
        }

        //Link takes user to KSB's privacy policy
        private async void TapGestureRecognizer_Tapped_Privacy_Policy(object sender, EventArgs e)
        {
            string url = "https://www.ksb.com/ksb-en/legal-information/data-privacy/";

            await Browser.OpenAsync(new Uri(url), BrowserLaunchMode.SystemPreferred);
        }

        //Sets the product picker item
        private void ProductPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProductPicker.SelectedIndex != -1)
            {
                var name = ProductPicker.Items[ProductPicker.SelectedIndex];
            }
        }

        //Sets the sales territory picker item
        public void SalesTerritoryPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SalesTerritoryPicker.SelectedIndex != -1)
            {
                var name = SalesTerritoryPicker.Items[SalesTerritoryPicker.SelectedIndex];
            }

            if (SalesTerritoryPicker.SelectedIndex == 9)
            {
                
                UsPicker.IsVisible = true;
                UsPicker.Title = "Please select a region";
                
            }
            else if (SalesTerritoryPicker.SelectedIndex != 9)
            {
                UsPicker.Items.Add("");
                UsPicker.IsVisible = false;
                UsPicker.SelectedIndex = 4;
            }
        }

        //Sets the US picker item
        private void UsPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UsPicker.SelectedIndex != -1)
            {
                var usName = UsPicker.Items[UsPicker.SelectedIndex];
            }
        }

        //Will clear all contact form fields if clicked
        private void Clear_All_Clicked(object sender, EventArgs e)
        {
            firstNameEntry.Text = "";
            lastNameEntry.Text = "";
            companyNameEntry.Text = "";
            emailEntry.Text = "";
            phoneNumberEntry.Text = "";
            commentsEntry.Text = "";
            QRCodeEntry.Text = "";
            blogRegisterChkbx.IsChecked = false;
            quarterlyNewsletterChkbx.IsChecked = false;
            ProductPicker.SelectedIndex = -1;
            SalesTerritoryPicker.SelectedIndex = -1;
            UsPicker.SelectedIndex = -1;
        }
    }
}
