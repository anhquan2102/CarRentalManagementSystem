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
using System.Xml.Serialization;

namespace CarRentalManagementSystem
{
    /// <summary>
    /// Interaction logic for ManageHistoryCarRental.xaml
    /// </summary>
    public partial class ManageHistoryCarRental : Window
    {
        CarRentalManagementSystemContext con;
        public ManageHistoryCarRental()
        {
            InitializeComponent();
            con = new CarRentalManagementSystemContext();
            loadHistoryCarRental();

        }
        public void loadHistoryCarRental()
        {
            lvHistoryCarRental.ItemsSource = con.HistoryCarRentals.Include(x => x.Rental).Include(x => x.Rental.Customer).Include(x => x.Rental.LicensePlatesNavigation).ToList();
        }
        private void btnShowDetail_Click(object sender, RoutedEventArgs e)
        {
            HistoryCarRental historyCarRental = (sender as FrameworkElement).DataContext as HistoryCarRental;
            if (historyCarRental != null)
            {
                DetailHistoryCarRental detailHistoryCarRental = new DetailHistoryCarRental(historyCarRental);
                detailHistoryCarRental.ShowDialog();
            }
        }



    }
}
