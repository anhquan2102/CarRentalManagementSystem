using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.Identity.Client.NativeInterop;

namespace CarRentalManagementSystem
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        CarRentalManagementSystemContext con;
        public Login()
        {
            InitializeComponent();
            con = new CarRentalManagementSystemContext();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Staff staffAccount = con.Staff.FirstOrDefault(x => x.Email.Equals(txtEmail.Text.Trim()));
            String jsonData = File.ReadAllText("appsettings.json");
            var adminAccount = JsonSerializer.Deserialize<Staff>(jsonData);
            if (staffAccount != null && staffAccount.Password.Equals(txtPassword.Password) && staffAccount.IsDeleted == false)
            {
                Application.Current.Properties["LoggedInUser"] = staffAccount;
                this.Hide();
                ManageCar car = new ManageCar();
                car.Show();
                //HomeCustomer homeCustomer = new HomeCustomer();
                //homeCustomer.Show();
            }
            else if (adminAccount != null && adminAccount.Email.Equals(txtEmail.Text) && adminAccount.Password.Equals(txtPassword.Password))
            {
                this.Hide();
                ManageCar car = new ManageCar();
                car.Show();

            }
            else
            {
                MessageBox.Show("Email or Password is not correct", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
