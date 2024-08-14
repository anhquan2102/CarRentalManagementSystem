using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace CarRentalManagementSystem
{
    /// <summary>
    /// Interaction logic for ManageCarRental.xaml
    /// </summary>
    public partial class ManageCarRental : Window
    {
        CarRentalManagementSystemContext con;
        public ManageCarRental()
        {
            InitializeComponent();
            con = new CarRentalManagementSystemContext();
            loadCarRental();
            loadInfoCar();
            loadInfoCustomer();
            loadTime();
        }
        public void loadCarRental()
        {
            lvCarRental.ItemsSource = con.CarRentals.Include(x => x.Customer).Include(r => r.LicensePlatesNavigation).Include(s => s.Staff).Where(x => x.IsDeleted == false).ToList();
        }
        public void loadInfoCar()
        {
            var usedLicensePlates = con.CarRentals.Where(o => o.IsDeleted == false).Select(o => o.LicensePlates).ToList();
            cbLicenPlates.ItemsSource = con.Cars.Where(x => !usedLicensePlates.Contains(x.LicensePlates)).ToList();

        }
        public void loadInfoCustomer()
        {
            cbCustomerName.ItemsSource = con.Customers.ToList();
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure want to cancel process?", "Alert", MessageBoxButton.YesNo, MessageBoxImage.Error);

            // Check if the "Yes" button was pressed
            if (result == MessageBoxResult.Yes)
            {
                // Close the current window
                Clear();
            }
        }
        private void Clear()
        {
            txtNameCar.Text = "";
            txtNumberOfSeats.Text = "";
            txtBrand.Text = "";
            txtColor.Text = "";
            txtPrice.Text = "";
            txtRentalPrice.Text = "";
            cbLicenPlates = null;
            txtPhoneNumber.Text = "";
            txtAddress.Text = "";
            cbCustomerName = null;
            cbStartTimeHour.SelectedItem = null;
            cbEndTimeHour.SelectedItem = null;
            cbStartTimeMinute.SelectedItem = null;
            cbEndTimeMinute.SelectedItem = null;
            dpEndTime.SelectedDate = null;
            dpStartTime.SelectedDate = null;
            txtDiscount.Text = "";
            txtRankLevel.Text = "";
            txtTotalPrice.Text = "";
        }
        private void cbLicenPlates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Car carSelected = (sender as ComboBox).SelectedItem as Car;
            if (carSelected != null)
            {
                Car car = con.Cars.FirstOrDefault(x => x.LicensePlates == carSelected.LicensePlates);
                txtNameCar.Text = car.CarName;
                txtNumberOfSeats.Text = car.NumberOfSeats.ToString();
                txtBrand.Text = car.Brand;
                txtColor.Text = car.Color;
                txtPrice.Text = car.Price.ToString();
                txtRentalPrice.Text = car.RentalPrice.ToString();

            }
        }
        public void loadTime()
        {
            // Tạo danh sách các giá trị giờ
            var hours = Enumerable.Range(0, 24).Select(h => new ComboBoxItem { Content = h.ToString() });
            cbStartTimeHour.ItemsSource = hours;
            cbStartTimeHour.SelectedIndex = 0;
            cbEndTimeHour.ItemsSource = hours;
            cbEndTimeHour.SelectedIndex = 0;
            // Tạo danh sách các giá trị phút
            var minutes = Enumerable.Range(0, 60).Select(m => new ComboBoxItem { Content = m.ToString() });
            cbStartTimeMinute.ItemsSource = minutes;
            cbStartTimeMinute.SelectedIndex = 0;
            cbEndTimeMinute.ItemsSource = minutes;
            cbEndTimeMinute.SelectedIndex = 0;
        }
        private void cbCustomerName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Customer customerSelected = (sender as ComboBox).SelectedItem as Customer;
            if (customerSelected != null)
            {
                Customer customer = con.Customers.Include(x => x.RankLevelNavigation).FirstOrDefault(x => x.CustomerId == customerSelected.CustomerId);
                txtPhoneNumber.Text = customer.PhoneNumber;
                txtAddress.Text = customer.Address;
                txtRankLevel.Text = customer.RankLevelNavigation.RankLevelName;
                txtDiscount.Text = customer.RankLevelNavigation.Discount.ToString();
            }
        }

        private void btnTotal_Click(object sender, RoutedEventArgs e)
        {
            if (dpEndTime.SelectedDate != null && dpStartTime.SelectedDate != null && cbStartTimeHour.SelectedItem != null && cbEndTimeHour.SelectedItem != null && cbStartTimeMinute.SelectedItem != null && cbEndTimeMinute.SelectedItem != null)
            {
                if (txtRentalPrice.Text.Length > 0)
                {
                    if (dpEndTime.SelectedDate > dpStartTime.SelectedDate)
                    {
                        DateTime dtStartTime = DateTime.Parse(dpStartTime.SelectedDate.ToString());
                        DateTime dtEndTime = DateTime.Parse(dpEndTime.SelectedDate.ToString());
                        int startHour = Int32.Parse(((ComboBoxItem)cbStartTimeHour.SelectedItem).Content.ToString());
                        int startMinute = Int32.Parse(((ComboBoxItem)cbStartTimeMinute.SelectedItem).Content.ToString());
                        int endHour = Int32.Parse(((ComboBoxItem)cbEndTimeHour.SelectedItem).Content.ToString());
                        int endMinute = Int32.Parse(((ComboBoxItem)cbEndTimeMinute.SelectedItem).Content.ToString());

                        int minuteStartTime = startHour * 60 + startMinute;
                        int minuteEndTime = endHour * 60 + endMinute;

                        int totalMinute = minuteEndTime - minuteStartTime;

                        TimeSpan timeSpan = dtEndTime - dtStartTime;
                        Double result = (Double)timeSpan.TotalMinutes + totalMinute;
                        Double rentalPrice = Double.Parse(txtRentalPrice.Text);
                        Double discount = Double.Parse(txtDiscount.Text);
                        txtTotalPrice.Text = Math.Ceiling((rentalPrice * result - (rentalPrice * result * (discount / 100)))).ToString();
                    }
                    else
                    {
                        MessageBox.Show("End Time can't be after Start Time", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Rental Price can't empty", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("StartTime or EndTime can't empty", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            CarRental carRental = new CarRental();
            Car carSelected = (cbLicenPlates.SelectedItem as Car);
            carRental.LicensePlates = carSelected.LicensePlates;
            Customer customerSelected = (cbCustomerName.SelectedItem as Customer);
            carRental.CustomerId = customerSelected.CustomerId;
            int startHour = int.Parse((cbStartTimeHour.SelectedItem as ComboBoxItem).Content.ToString());
            int startMinute = int.Parse((cbStartTimeMinute.SelectedItem as ComboBoxItem).Content.ToString());
            int endHour = int.Parse((cbEndTimeHour.SelectedItem as ComboBoxItem).Content.ToString());
            int endMinute = int.Parse((cbEndTimeMinute.SelectedItem as ComboBoxItem).Content.ToString());
            DateTime startDate = dpStartTime.SelectedDate.Value;
            DateTime endDate = dpEndTime.SelectedDate.Value;
            Staff loggedInUser = Application.Current.Properties["LoggedInUser"] as Staff;
            carRental.StaffId = loggedInUser.StaffId;
            carRental.IsDeleted = false;
            // Kết hợp để tạo DateTime
            DateTime startTime = new DateTime(
                startDate.Year,
                startDate.Month,
                startDate.Day,
                startHour,
                startMinute,
                0
            );
            // Kết hợp để tạo DateTime
            DateTime endTime = new DateTime(
                endDate.Year,
                endDate.Month,
                endDate.Day,
                endHour,
                endMinute,
                0
            );
            // Định dạng DateTime thành chuỗi theo định dạng "M/d/yyyy h:mm:ss tt"
            string formattedStartTime = startTime.ToString("M/d/yyyy h:mm:ss tt");
            string formattedEndTime = endTime.ToString("M/d/yyyy h:mm:ss tt");
            carRental.StartDate = startTime;
            carRental.EndDate = endTime;
            if (txtTotalPrice.Text.Length > 0)
            {
                string totalPrice = txtTotalPrice.Text.Replace(".", "");
                if (decimal.TryParse(totalPrice, out decimal parseTotalPrice))
                {
                    carRental.Total = parseTotalPrice;
                }
            }
            else
            {
                MessageBox.Show("Total empty!!!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            con.Add(carRental);
            if (con.SaveChanges() > 0)
            {
                MessageBox.Show("Add successfully", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                loadCarRental();
                loadInfoCustomer();
                loadInfoCar();
                Clear();

            }
            else
            {
                MessageBox.Show("Add unsuccessfully", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            HistoryCarRental historyCarRental = new HistoryCarRental();
            CarRental? carRental = (sender as FrameworkElement).DataContext as CarRental;
            Car car = con.Cars.FirstOrDefault(x => x.LicensePlates.Equals(carRental.LicensePlates)) as Car;
            if (carRental != null && car != null)
            {
                Staff staff = carRental.Staff;
                historyCarRental.RentalId = carRental.RentalId;
                historyCarRental.ActualReturnTime = DateTime.Now;
                historyCarRental.Rental.Staff = staff;
                historyCarRental.StartDate = carRental.StartDate;
                historyCarRental.EndDate = carRental.EndDate;
                carRental.IsDeleted = true;

                decimal rentalPrice = car.RentalPrice;
                decimal totalPrice = carRental.Total;

                if (historyCarRental.ActualReturnTime < historyCarRental.EndDate)
                {
                    TimeSpan timeSpan = historyCarRental.ActualReturnTime - (DateTime)historyCarRental.StartDate;
                    double timeRemaining = Math.Ceiling(timeSpan.TotalMinutes);
                    totalPrice -= (decimal)(timeRemaining * (double)rentalPrice);
                }
                else if (historyCarRental.ActualReturnTime > historyCarRental.EndDate)
                {
                    TimeSpan timeSpan = historyCarRental.ActualReturnTime - (DateTime)historyCarRental.EndDate;
                    double timeRemaining = Math.Ceiling(timeSpan.TotalMinutes);
                    totalPrice += (decimal)(timeRemaining * (double)rentalPrice);
                }

                historyCarRental.TotalPrice = totalPrice;
                con.HistoryCarRentals.Add(historyCarRental);
                con.CarRentals.Update(carRental);

                if (con.SaveChanges() > 0)
                {
                    MessageBox.Show("Order completed", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                    loadCarRental();
                }
                else
                {
                    MessageBox.Show("Process error!!!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
