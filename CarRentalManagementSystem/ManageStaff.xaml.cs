using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace CarRentalManagementSystem
{
    /// <summary>
    /// Interaction logic for ManageStaff.xaml
    /// </summary>
    public partial class ManageStaff : Window
    {
        CarRentalManagementSystemContext con;
        public ManageStaff()
        {
            InitializeComponent();
            con = new CarRentalManagementSystemContext();
            LoadStaff();
            LoadData(DateTime.Now.Month);
            cmbMonth.SelectedIndex = DateTime.Now.Month - 1;
        }

        public void LoadStaff()
        {
            lvStaff.ItemsSource = con.Staff.Where(s => s.IsDeleted == false).ToList();
        }

        private void btnAddStaff_Click(object sender, RoutedEventArgs e)
        {
            Clear();
            popupCreateStaff.IsOpen = true;
        }

        private void CancelStaffButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            if (popupCreateStaff.IsOpen)
            {
                popupCreateStaff.IsOpen = false;
            }
        }
        private void btnEditStaff_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is Staff staff)
            {
                txtStaffId.Text = staff.StaffId.ToString();
                txtStaffName.Text = staff.StaffName;
                txtPhoneNumber.Text = staff.PhoneNumber;
                txtEmail.Text = staff.Email;
                txtAddress.Text = staff.Address;
                txtSalary.Text = staff.Salary.ToString();
                txtPassword.Text = staff.Password;
                popupEditStaff.IsOpen = true;
            }
            else
            {
                MessageBox.Show("Cannot find Staff to edit", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SaveStaffButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateStaffInput(isCreate: false))
            {
                try
                {
                    Staff staff = con.Staff.FirstOrDefault(s => s.StaffId == Convert.ToInt32(txtStaffId.Text));
                    if (staff != null)
                    {
                        if (IsEmailDuplicate(staff.Email, staff.StaffId))
                        {
                            MessageBox.Show("Email already exists.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        staff.StaffName = txtStaffName.Text;
                        staff.PhoneNumber = txtPhoneNumber.Text;
                        staff.Email = txtEmail.Text;
                        staff.Address = txtAddress.Text;
                        staff.Salary = decimal.Parse(txtSalary.Text);
                        staff.Password = txtPassword.Text;
                        con.Staff.Update(staff);
                        con.SaveChanges();
                        popupEditStaff.IsOpen = false;
                        MessageBox.Show("Staff updated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadStaff();
                    }
                    else
                    {
                        MessageBox.Show("Staff not found", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Update failed: " + ex.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CancelStaffButton_Click(object sender, RoutedEventArgs e)
        {
            if (popupEditStaff.IsOpen)
            {
                popupEditStaff.IsOpen = false;
            }
        }

        private void StaffButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateStaffInput(isCreate: true))
            {
                try
                {
                    string email = txtEmailCreate.Text;

                    if (IsEmailDuplicate(email))
                    {
                        MessageBox.Show("Email already exists.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    Staff newStaff = new Staff
                    {
                        StaffName = txtStaffNameCreate.Text,
                        PhoneNumber = txtPhoneNumberCreate.Text,
                        Email = email,
                        Address = txtAddressCreate.Text,
                        Salary = decimal.Parse(txtSalaryCreate.Text),
                        Password = txtPasswordCreate.Text,
                        IsDeleted = false,
                    };

                    con.Staff.Add(newStaff);
                    con.SaveChanges();
                    popupCreateStaff.IsOpen = false;
                    MessageBox.Show("Staff created successfully", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadStaff();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Creation failed: " + ex.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool ValidateStaffInput(bool isCreate)
        {
            string staffName, phoneNumber, email, address, salary, password;

            if (isCreate)
            {
                staffName = txtStaffNameCreate.Text;
                phoneNumber = txtPhoneNumberCreate.Text;
                email = txtEmailCreate.Text;
                address = txtAddressCreate.Text;
                salary = txtSalaryCreate.Text;
                password = txtPasswordCreate.Text;
            }
            else
            {
                staffName = txtStaffName.Text;
                phoneNumber = txtPhoneNumber.Text;
                email = txtEmail.Text;
                address = txtAddress.Text;
                salary = txtSalary.Text;
                password = txtPassword.Text;
            }

            if (string.IsNullOrWhiteSpace(staffName))
            {
                MessageBox.Show("Staff Name is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Email is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Email is not in a valid format", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Address is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(salary) || !decimal.TryParse(salary, out _))
            {
                MessageBox.Show("Salary is required and must be a valid number", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (isCreate && string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Password is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }


        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();
            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new MailAddress(trimmedEmail);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        private bool IsEmailDuplicate(string email, int? staffId = null)
        {
            if (staffId.HasValue)
            {
                return con.Staff.Any(s => s.Email == email && s.StaffId != staffId.Value && s.IsDeleted == false);
            }
            return con.Staff.Any(s => s.Email == email && s.IsDeleted == false);
        }

        private void btnDeleteStaff_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is Staff staff)
            {
                if (staff != null)
                {
                    try
                    {
                        staff.IsDeleted = true;
                        con.Staff.Update(staff);
                        con.SaveChanges();
                        LoadStaff();
                        MessageBox.Show("Staff deleted successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Deletion failed: " + ex.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Cannot find Staff to delete", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Clear()
        {
            txtStaffNameCreate.Text = "";
            txtPhoneNumberCreate.Text = "";
            txtEmailCreate.Text = "";
            txtAddressCreate.Text = "";
            txtSalaryCreate.Text = "";
        }
        private void btnClearInput_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnSearchStaff_Click(object sender, RoutedEventArgs e)
        {
            ApplySearchFilters();
        }

        private void ApplySearchFilters()
        {
            string searchNameEmail = txtSearchNameEmail.Text.Trim().ToLower();
            var staffList = con.Staff.AsQueryable();

            if (!string.IsNullOrEmpty(searchNameEmail))
            {
                staffList = staffList.Where(s => s.StaffName.ToLower().Contains(searchNameEmail) && s.IsDeleted == false || s.Email.ToLower().Contains(searchNameEmail) && s.IsDeleted == false);
            }

            lvStaff.ItemsSource = staffList.ToList();
        }
        private void btnShowSalary_Click(object sender, RoutedEventArgs e)
        {
            if (cmbMonth.SelectedItem is ComboBoxItem selectedItem && int.TryParse(selectedItem.Tag.ToString(), out int selectedMonth))
            {
                LoadData(selectedMonth);
            }
            else
            {
                MessageBox.Show("Please select a valid month.", "Invalid Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LoadData(int month)
        {
            var staffList = con.Staff.Where(s => s.IsDeleted == false).ToList();
            var rentalsInMonth = con.HistoryCarRentals
                .Include(h => h.Rental)
                .Where(h => h.ActualReturnTime.Month == month)
                .ToList();

            var staffProfit = staffList.Select(staff =>
            {
                var staffRentals = rentalsInMonth.Where(rental => rental.Rental.Staff.StaffId == staff.StaffId).ToList();
                var commission = staffRentals.Sum(rental => rental.TotalPrice) * 0.03m;
                var totalSalary = staff.Salary + commission;

                return new StaffSalary
                {
                    StaffId = staff.StaffId,
                    StaffName = staff.StaffName,
                    Salary = (decimal)staff.Salary,
                    RentalCount = staffRentals.Count,
                    Commission = commission,
                    TotalSalary = totalSalary
                };
            }).ToList();

            dgSalary.ItemsSource = staffProfit;
        }

    }
}
