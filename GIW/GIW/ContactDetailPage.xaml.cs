using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIW.Classes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using SQLitePCL;

namespace GIW
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactDetailPage : ContentPage       
    {
        Contact selectedContact;

        public ContactDetailPage(Contact selectedContact)
        {
            InitializeComponent();

            //Creates items to add to product, sales territory, and US region pickers
            List<string> productPickerStrings = new List<string>();

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

            //Pulls the form info for the selected contact and displays to be edited
            this.selectedContact = selectedContact;

            firstNameEntry.Text = selectedContact.FirstName;
            lastNameEntry.Text = selectedContact.LastName;
            companyNameEntry.Text = selectedContact.CompanyName;
            emailEntry.Text = selectedContact.Email;
            phoneNumberEntry.Text = selectedContact.PhoneNumber;
            commentsEntry.Text = selectedContact.Comments;
            QRCodeEntry.Text = selectedContact.QRCode;
            ProductPicker.SelectedItem = selectedContact.Product;
            SalesTerritoryPicker.SelectedItem = selectedContact.SalesTerritory;
            UsPicker.SelectedItem = selectedContact.USRegion;
            blogRegisterChkbx.IsChecked = selectedContact.BlogChkBox;
            quarterlyNewsletterChkbx.IsChecked = selectedContact.QrtlyNewsltrChkBox;
        }

        //Click this button to update contact with any edited information in the contact details page
        private void updateButton_Clicked(object sender, EventArgs e)
        {
            selectedContact.FirstName = firstNameEntry.Text;
            selectedContact.LastName = lastNameEntry.Text;
            selectedContact.CompanyName = companyNameEntry.Text;
            selectedContact.Email = emailEntry.Text;
            selectedContact.PhoneNumber = phoneNumberEntry.Text;
            selectedContact.QRCode = QRCodeEntry.Text;
            selectedContact.Product = ProductPicker.SelectedItem.ToString();
            selectedContact.SalesTerritory = SalesTerritoryPicker.SelectedItem.ToString();
            selectedContact.USRegion = UsPicker.SelectedItem.ToString();
            selectedContact.Comments = commentsEntry.Text;
            selectedContact.BlogChkBox = blogRegisterChkbx.IsChecked;
            selectedContact.QrtlyNewsltrChkBox = quarterlyNewsletterChkbx.IsChecked;
            

            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Contact>();

                int rows = conn.Update(selectedContact);
                if (rows > 0)
                {
                    DisplayAlert("Success", "Contact Updated", "Ok");
                    Navigation.PushAsync(new ContactsPage());
                }
                else
                {
                    DisplayAlert("Failure", "Contact failed to be updated", "Ok");
                }
            }

        }

        //Click this button to delete contact
        private async void deleteButton_Clicked(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Contact>();

                var input = await Application.Current.MainPage.DisplayAlert("Delete Contact", "Are you sure you want to delete this contact?", "Yes", "No");
                if (input)
                {
                    int rows = conn.Delete(selectedContact);

                    if (rows > 0)
                    {
                        await DisplayAlert("Success", "Contact Deleted", "Ok");
                        await Navigation.PushAsync(new ContactsPage());
                    }
                    else
                    {
                        await DisplayAlert("Failure", "Contact failed to be deleted", "Ok");
                    }
                }
                else
                {
                    return;
                }
                
            }
        }

        //This will take user back to Contacts listview
        private void ContactsLink_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ContactsPage());
        }

        //Sets value for product picker
        private void ProductPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProductPicker.SelectedIndex != -1)
            {
                var name = ProductPicker.Items[ProductPicker.SelectedIndex];
            }

        }

        //Sets value for sales territory picker
        private void SalesTerritoryPicker_SelectedIndexChanged(object sender, EventArgs e)
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

        //Sets value for US picker
        private void UsPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UsPicker.SelectedIndex != -1)
            {
                var usName = UsPicker.Items[UsPicker.SelectedIndex];
            }
        }
    }
}