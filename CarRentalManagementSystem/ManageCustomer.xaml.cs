using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace CarRentalManagementSystem
{
    /// <summary>
    /// Interaction logic for ManageCustomer.xaml
    /// </summary>
    public partial class ManageCustomer : Window
    {
        CarRentalManagementSystemContext con;
        public ManageCustomer()
        {
            InitializeComponent();
            con = new CarRentalManagementSystemContext();
            loadCustomer();
            loadRankLevel();
        }
        public void loadCustomer()
        {
            lvCustomer.ItemsSource = con.Customers.Include(c => c.RankLevelNavigation).Where(c => c.IsDeleted == false).ToList();
        }
        public void loadRankLevel()
        {
            var rankLevels = con.RankLevelCustomers.ToList();
            rankLevels.Insert(0, new RankLevelCustomer { RankLevelName = "All" }); 
            cboRankLevel.ItemsSource = rankLevels;
            cboRankLevel.DisplayMemberPath = "RankLevelName"; 
            cboRankLevel.SelectedIndex = 0;
        }

        private void cbRankLevelCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }
        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            popupCreateCustomer.IsOpen = true;
            popupEditCustomer.IsOpen = false;
        }
        private void CancelCustomerButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            if (popupCreateCustomer.IsOpen)
            {
                popupCreateCustomer.IsOpen = false;
            }
        }
        private void btnEditCustomer_Click(object sender, RoutedEventArgs e)
        {
            popupCreateCustomer.IsOpen = false;
            if (sender is FrameworkElement element && element.DataContext is Customer customer)
            {
                popupEditCustomer.IsOpen = true;
                txtCustomerId.Text = customer.CustomerId.ToString();
                txtCustomerName.Text = customer.CustomerName;
                txtPhoneNumber.Text = customer.PhoneNumber;
                txtAddress.Text = customer.Address;


            }
            else
            {
                MessageBox.Show("Cannot find Customer to edit", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SaveCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateCustomerInput(isCreate: false))
            {
                try
                {
                    Customer customer = con.Customers.FirstOrDefault(c => c.CustomerId == Convert.ToInt32(txtCustomerId.Text));
                    if (customer != null)
                    {
                        if (IsPhoneNumberDuplicate(customer.PhoneNumber, customer.CustomerId))
                        {
                            MessageBox.Show("Phone number already exists.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        customer.CustomerName = txtCustomerName.Text;
                        customer.PhoneNumber = txtPhoneNumber.Text;
                        customer.Address = txtAddress.Text;
                        con.Customers.Update(customer);
                        con.SaveChanges();
                        popupEditCustomer.IsOpen = false;
                        MessageBox.Show("Customer updated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        loadCustomer();
                    }
                    else
                    {
                        MessageBox.Show("Customer not found", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Update failed: " + ex.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void CancelCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            if (popupEditCustomer.IsOpen)
            {
                popupEditCustomer.IsOpen = false;
            }
        }
        private void CustomerButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateCustomerInput(isCreate: true))
            {
                try
                {
                    string phoneNumber = txtPhoneNumberCreate.Text;

                    if (IsPhoneNumberDuplicate(phoneNumber))
                    {
                        MessageBox.Show("Phone number already exists.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    Customer newCustomer = new Customer
                    {
                        CustomerName = txtCustomerNameCreate.Text,
                        PhoneNumber = txtPhoneNumberCreate.Text,
                        Address = txtAddressCreate.Text,
                        Point = 0,
                        RankLevel = 0,
                        IsDeleted = false,
                    };

                    con.Customers.Add(newCustomer);
                    con.SaveChanges();
                    popupCreateCustomer.IsOpen = false;
                    MessageBox.Show("Customer created successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    loadCustomer();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Creation failed: " + ex.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool ValidateCustomerInput(bool isCreate)
        {
            string customerName, phoneNumber, address;

            if (isCreate)
            {
                customerName = txtCustomerNameCreate.Text;
                phoneNumber = txtPhoneNumberCreate.Text;
                address = txtAddressCreate.Text;
            }
            else
            {
                customerName = txtCustomerName.Text;
                phoneNumber = txtPhoneNumber.Text;
                address = txtAddress.Text;
            }

            if (string.IsNullOrWhiteSpace(customerName))
            {
                MessageBox.Show("Customer Name is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                MessageBox.Show("Phone Number is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!Regex.IsMatch(phoneNumber, @"^0\d{9}$"))
            {
                MessageBox.Show("Phone Number must be exactly 10 digits and start with '0'", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Address is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
        private bool IsPhoneNumberDuplicate(string phoneNumber, int? customerId = null)
        {
            if (customerId.HasValue)
            {
                return con.Customers.Any(c => c.PhoneNumber == phoneNumber && c.CustomerId != customerId.Value && c.IsDeleted == false);
            }
            else
            {
                return con.Customers.Any(c => c.PhoneNumber == phoneNumber && c.IsDeleted == false);
            }
        }
        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            Customer customer = con.Customers.FirstOrDefault(c => c.CustomerId == Convert.ToInt32(txtCustomerId.Text));
            if (customer != null)
            {
                customer.IsDeleted = true;
                con.Customers.Update(customer);
                con.SaveChanges();
                loadCustomer();
                MessageBox.Show("Delete Successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void Clear()
        {
            txtCustomerNameCreate.Text = "";
            txtPhoneNumberCreate.Text = "";
            txtAddressCreate.Text = "";
        }
        private void btnClearInput_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void btnSearchCustomer_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }
        private void ApplyFilters()
        {
            string searchNameMobile = txtSearchNameMobile.Text.Trim().ToLower();
            string selectedRankLevel = (cboRankLevel.SelectedItem as RankLevelCustomer)?.RankLevelName;

            var customers = con.Customers
                               .Include(c => c.RankLevelNavigation)
                               .Where(c => c.IsDeleted == false)
                               .ToList();

            if (!string.IsNullOrEmpty(searchNameMobile))
            {
                customers = customers.Where(c => c.CustomerName.ToLower().Contains(searchNameMobile) || c.PhoneNumber.Contains(searchNameMobile)).ToList();
            }
            if (selectedRankLevel != "All" && !string.IsNullOrEmpty(selectedRankLevel))
            {
                customers = customers.Where(c => c.RankLevelNavigation.RankLevelName == selectedRankLevel).ToList();
            }

            lvCustomer.ItemsSource = customers;
        }

    }
}
