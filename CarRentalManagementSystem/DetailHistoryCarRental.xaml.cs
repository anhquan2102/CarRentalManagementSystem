using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CarRentalManagementSystem
{
    /// <summary>
    /// Interaction logic for DetailHistoryCarRental.xaml
    /// </summary>
    public partial class DetailHistoryCarRental : Window
    {
        CarRentalManagementSystemContext con;
        private HistoryCarRental _historyCarRental;

        public DetailHistoryCarRental(HistoryCarRental historyCarRental)
        {
            InitializeComponent();
            con = new CarRentalManagementSystemContext();
            _historyCarRental = historyCarRental;
            FillInfo();

        }
        public void FillInfo()
        {
            HistoryCarRental listHistoryCarRental = con.HistoryCarRentals.Include(x => x.Rental).Include(x => x.Rental.Customer.RankLevelNavigation).Include(x => x.Rental.LicensePlatesNavigation).FirstOrDefault(x => x.RentalId == _historyCarRental.RentalId);
            if(listHistoryCarRental != null)
            {
                txtCustomerId.Text = listHistoryCarRental.Rental.Customer.CustomerId.ToString();
                txtCustomerName.Text = listHistoryCarRental.Rental.Customer.CustomerName;
                txtPhoneNumber.Text = listHistoryCarRental.Rental.Customer.PhoneNumber;
                txtAddress.Text = listHistoryCarRental.Rental.Customer.Address;
                txtRankLevel.Text = listHistoryCarRental.Rental.Customer.RankLevelNavigation.RankLevelName;
                txtLicensePlate.Text = listHistoryCarRental.Rental.LicensePlatesNavigation.LicensePlates;
                txtCarName.Text = listHistoryCarRental.Rental.LicensePlatesNavigation.CarName;
                txtTypeCar.Text = listHistoryCarRental.Rental.LicensePlatesNavigation.Type;
                txtBrand.Text = listHistoryCarRental.Rental.LicensePlatesNavigation.Brand;
                txtNumberOfSeat.Text = listHistoryCarRental.Rental.LicensePlatesNavigation.NumberOfSeats.ToString();
                txtFuel.Text = listHistoryCarRental.Rental.LicensePlatesNavigation.Fuel;
                txtCarPrice.Text = listHistoryCarRental.Rental.LicensePlatesNavigation.Price.ToString();
                txtRentalPrice.Text = listHistoryCarRental.Rental.LicensePlatesNavigation.RentalPrice.ToString();
                txtHistoyCarRentalId.Text = listHistoryCarRental.HistoryCarRentalId.ToString();
                dpStartTime.SelectedDate = listHistoryCarRental.StartDate;
                dpEndTime.SelectedDate = listHistoryCarRental.EndDate;
                dpActualReturnTime.SelectedDate = listHistoryCarRental.ActualReturnTime;
                txtTotal.Text = listHistoryCarRental.TotalPrice.ToString();
            }
            
        }
    }
}
