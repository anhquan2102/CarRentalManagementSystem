using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;

namespace CarRentalManagementSystem
{
    /// <summary>
    /// Interaction logic for ReportandStatistics.xaml
    /// </summary>
    public partial class ReportandStatistics : Window
    {
        readonly CarRentalManagementSystemContext con;
        public ReportandStatistics()
        {
            InitializeComponent();
            con = new CarRentalManagementSystemContext();
            loadReport();
            cmbReportType.SelectedIndex = -1;

        }
        private void loadReport()
        {
            var rentals = con.HistoryCarRentals.Include(h => h.Rental).ThenInclude(r => r.Customer).Include(h => h.Rental).ThenInclude(r => r.Staff).Include(h => h.Rental).ThenInclude(r => r.LicensePlatesNavigation).ToList();
            decimal totalRevenue = rentals.Sum(h => h.TotalPrice);
            txtRevenue.Text = $"{totalRevenue:C}";
            txtRevenuebyTime.Text = $"{totalRevenue:C}";
            dgReportData.ItemsSource = rentals;
            dgReportByTimeData.ItemsSource = rentals;
        }

        private void ViewReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (cmbReportType.SelectedItem is ComboBoxItem selectedItem)
            {
                string reportType = selectedItem.Tag.ToString();

                switch (reportType)
                {
                    case "YearlyReport":
                        LoadYearlyReport();
                        break;
                    case "MonthlyReport":
                        LoadMonthlyReport();
                        break;
                    case "WeeklyReport":
                        LoadWeeklyReport();
                        break;
                    case "DailyReport":
                        LoadDailyReport();
                        break;
                    default:
                        MessageBox.Show("Please select a valid report type.", "Invalid Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                }
            }
            else
            {
                MessageBox.Show("Please select a report type.", "Invalid Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void LoadDailyReport()
        {
            var rentals = con.HistoryCarRentals.Where(h => h.ActualReturnTime.Date == DateTime.Now.Date).Include(h => h.Rental).Include(h => h.Rental.Customer).Include(h
                 => h.Rental.Staff).ToList();
            decimal totalRevenue = rentals.Sum(h => h.TotalPrice);
            txtRevenue.Text = $"{totalRevenue:C}";
            dgReportData.ItemsSource = rentals;
        }
        private void LoadWeeklyReport()
        {
            DateTime startOfWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime endOfWeek = startOfWeek.AddDays(6);
            var rentals = con.HistoryCarRentals.Where(h => h.ActualReturnTime.Date >= startOfWeek && h.ActualReturnTime.Date <= endOfWeek).Include(h => h.Rental).Include(h => h.Rental.Customer).Include(h
                => h.Rental.Staff).ToList();
            decimal totalRevenue = rentals.Sum(h => h.TotalPrice);
            txtRevenue.Text = $"{totalRevenue:C}";
            dgReportData.ItemsSource = rentals;
        }
        private void LoadMonthlyReport()
        {
            var rentals = con.HistoryCarRentals.Where(h => h.ActualReturnTime.Month == DateTime.Now.Month).Include(h => h.Rental).Include(h => h.Rental.Customer).Include(h
               => h.Rental.Staff).ToList();
            decimal totalRevenue = rentals.Sum(h => h.TotalPrice);
            txtRevenue.Text = $"{totalRevenue:C}";
            dgReportData.ItemsSource = rentals;
        }
        private void LoadYearlyReport()
        {
            var rentals = con.HistoryCarRentals.Where(h => h.ActualReturnTime.Year == DateTime.Now.Year).Include(h => h.Rental.Customer).Include(h
               => h.Rental.Staff).ToList();
            decimal totalRevenue = rentals.Sum(h => h.TotalPrice);
            txtRevenue.Text = $"{totalRevenue:C}";
            dgReportData.ItemsSource = rentals;
        }

        private void ViewReportByTimeButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = dpStartDate.SelectedDate;
            DateTime? endDate = dpEndDate.SelectedDate;

            if (startDate == null && endDate == null)
            {
                MessageBox.Show("Please select at least one date.", "Invalid Date Range", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (startDate != null && endDate != null)
            {
                if (startDate > endDate)
                {
                    MessageBox.Show("Start date cannot be after end date.", "Invalid Date Range", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            var rentals = con.HistoryCarRentals
                .Include(h => h.Rental.Customer)
                .Include(h => h.Rental.Staff)
                .ToList();

            if (startDate != null)
            {
                rentals = rentals.Where(h => h.StartDate >= startDate).ToList();
            }

            if (endDate != null)
            {
                rentals = rentals.Where(h => h.ActualReturnTime <= endDate).ToList();
            }

            decimal totalRevenue = rentals.Sum(h => h.TotalPrice);
            txtRevenuebyTime.Text = $"{totalRevenue:C}";
            dgReportByTimeData.ItemsSource = rentals;
        }
    }
}
