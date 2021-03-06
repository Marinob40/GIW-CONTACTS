﻿using GIW.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GIW
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactsPage : ContentPage
    {
        public ContactsPage()
        {
            //Creates listview of contacts in local SQLite database
            InitializeComponent();
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Contact>();
                var contacts = conn.Table<Contact>().ToList();

                contactsListView.ItemsSource = contacts;
            }
        }

        //Displays listview of contacts in SQLite database
        protected override void OnAppearing()
        {
            base.OnAppearing();

            using(SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Contact>();
                var contacts = conn.Table<Contact>().ToList();

                contactsListView.ItemsSource = contacts;

                conn.Close();

            }

        }

        //Click this button to add a new contact
        private void NewContactToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }

        //Select a contact in the contact listview and open up the contact detail page
        public void contactsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedContact = contactsListView.SelectedItem as Contact;

            if (selectedContact != null)
            {
                Navigation.PushAsync(new ContactDetailPage(selectedContact));
            }
        }

        //Search contacts listview by first name, last name, company name, or date added
        void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    conn.CreateTable<Contact>();
                    var contacts = conn.Table<Contact>().ToList();

                    contactsListView.ItemsSource = contacts;

                }
            }
            else
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    conn.CreateTable<Contact>();
                    var contacts = conn.Table<Contact>().ToList();

                    List<Contact> searchedContacts = (from contact in contacts
                                                      where
                            contact.CompanyName.Contains(contactSearchBar.Text, StringComparison.OrdinalIgnoreCase)
                            || contact.FirstName.Contains(contactSearchBar.Text, StringComparison.OrdinalIgnoreCase)
                            || contact.LastName.Contains(contactSearchBar.Text, StringComparison.OrdinalIgnoreCase)
                            || contact.DateAdded.ToString().Contains(contactSearchBar.Text)
                                                      select contact).ToList<Contact>();

                    contactsListView.ItemsSource = searchedContacts;
                }
            }
        }

        //This button will transfer all contacts in the contacts listview to AWS instance of SQL Server
        private void transferContactsSQL_Clicked(object sender, EventArgs e)
        {
            try

            {

                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))

                {

                    conn.CreateTable<Contact>();

                    var contacts = conn.Table<Contact>().ToList();

                    SqlConnection sqlConn = new SqlConnection(@"Data Source=myrdstest.ccjmecrdvohs.us-east-1.rds.amazonaws.com;Initial Catalog=GIW-CONTACTS;User Id=sa;Password=password;");

                    sqlConn.Open();

                    foreach (var contact in contacts)

                    {

                        if (sqlConn != null)

                        {

                            string strOutput = "('" + contact.FirstName + "', '" + contact.LastName + "', '" + contact.CompanyName + "', '" +

                                contact.Email + "', '" + contact.PhoneNumber + "', '" + contact.Comments + "', '" + contact.DateAdded.ToString() + "', '" +

                                contact.BlogChkBox.ToString() + "', '" + contact.QrtlyNewsltrChkBox.ToString() + "', '" + contact.Product + "', '" +

                                contact.SalesTerritory + "', '" + contact.USRegion + "', '" + contact.QRCode + "')";

                            SqlCommand cmdInsert = new SqlCommand("INSERT INTO Contacts (FirstName, LastName, CompanyName, Email, PhoneNumber, Comments, " +

                                "DateAdded, BlogChkBox, QrtlyNewsltrChkBox, Product, SalesTerritory, USRegion, QRCode) VALUES " + strOutput, sqlConn);

                            cmdInsert.ExecuteNonQuery();

                        }

                    }

                    sqlConn.Close();

                    DisplayAlert("Success", "Contact(s) Transferred", "Ok");

                }

            }
            catch (Exception ex)

            {

                DisplayAlert("Error", ex.ToString(), "Ok");

            }

        }

        //Click to delete all contacts in contacts listview
        private async void DeleteAll_Clicked(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Contact>();

                var input = await Application.Current.MainPage.DisplayAlert("Delete All Contacts", "Are you sure you want to delete all contacts?", "Yes", "No");
                if (input)
                {
                    conn.DeleteAll<Contact>();
                    await Navigation.PushAsync(new MainPage());
                }
                else
                {
                    return;
                }

            }
        }
    }
}