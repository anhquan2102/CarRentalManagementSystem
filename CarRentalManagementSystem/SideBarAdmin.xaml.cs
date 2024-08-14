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
    /// Interaction logic for SideBarAdmin.xaml
    /// </summary>
    public partial class SideBarAdmin : UserControl
    {
        public SideBarAdmin()
        {
            InitializeComponent();
        }
        private void btnManageStaff_Click(object sender, RoutedEventArgs e)
        {
            ManageStaff manageStaff = new ManageStaff();
            Window window = Window.GetWindow(this);
            window.Close();
            manageStaff.Show();
        }

        private void ReportandStatistics(object sender, RoutedEventArgs e)
        {
            ReportandStatistics reportandStatistics = new ReportandStatistics();
            Window window = Window.GetWindow(this);
            window.Close();
            reportandStatistics.Show();
        }
    }
}
