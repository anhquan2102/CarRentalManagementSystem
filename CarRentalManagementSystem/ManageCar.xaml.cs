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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CarRentalManagementSystem
{
    /// <summary>
    /// Interaction logic for ManageCar.xaml
    /// </summary>
    public partial class ManageCar : Window
    {
        CarRentalManagementSystemContext con;
        public ManageCar()
        {
            InitializeComponent();
            con = new CarRentalManagementSystemContext();
            loadCar();
            loadCarStatus();
        }
        public void loadCar()
        {
            lvCar.ItemsSource = con.Cars.Include(x => x.CarStatus).Where(x => x.IsDeleted == false).ToList();
        }
        public void loadCarStatus()
        {
            cbStatus.ItemsSource = con.CarStatuses.ToList();
            cbStatus.SelectedIndex = 0;
        }
        private void Clear()
        {
            txtLicensePlates.Text = "";
            txtName.Text = "";
            txtColor.Text = "";
            txtType.Text = "";
            txtFuel.Text = "";
            txtPrice.Text = "";
            txtRentalPrice.Text = "";
            cbStatus.SelectedItem = null;
            dpDate.SelectedDate = null;
            txtBrand.Text = "";
            txtNumberOfSeat.Text = "";
        }
        private void lvCar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Car car = (sender as ListView).SelectedItem as Car;
            if (car != null)
            {
                txtLicensePlates.Text = car.LicensePlates.ToString();
                txtName.Text = car.CarName.ToString();
                txtColor.Text = car.Color.ToString();
                txtType.Text = car.Type.ToString();
                txtFuel.Text = car.Fuel.ToString();
                txtPrice.Text = car.Price.ToString();
                txtRentalPrice.Text = car.RentalPrice.ToString();
                cbStatus.SelectedItem = car.CarStatus;
                dpDate.SelectedDate = DateTime.Parse(car.DateCar.ToString());
                txtBrand.Text = car.Brand.ToString();
                txtNumberOfSeat.Text = car.NumberOfSeats.ToString();
            }
            loadCar();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Car checkLicensePlates = con.Cars.FirstOrDefault(x => x.LicensePlates.Equals(txtLicensePlates.Text.Trim()));
                if (checkLicensePlates != null)
                {
                    MessageBox.Show("License Plates already exist!!!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Car car = new Car();
                car.LicensePlates = txtLicensePlates.Text;
                car.CarName = txtName.Text;
                car.Type = txtType.Text;
                car.Brand = txtBrand.Text;
                car.Color = txtColor.Text;
                car.Price = decimal.Parse(txtPrice.Text);
                car.RentalPrice = decimal.Parse(txtRentalPrice.Text);
                car.CarStatusId = (cbStatus.SelectedItem as CarStatus).CarStatusId;
                car.DateCar = DateOnly.Parse(dpDate.Text);
                car.NumberOfSeats = Int32.Parse(txtNumberOfSeat.Text);
                car.Fuel = txtFuel.Text;
                car.IsDeleted = false;
                con.Cars.Add(car);

                if (con.SaveChanges() > 0)
                {
                    loadCar();
                    Clear();
                    MessageBox.Show("Create Successfully", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Create Unsuccessfully", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Car car = con.Cars.FirstOrDefault(x => x.LicensePlates.Equals(txtLicensePlates.Text));
                if (car != null)
                {
                    CarRental carRental = con.CarRentals.Where(x => x.IsDeleted == false).FirstOrDefault(x => x.LicensePlates.Equals(car.LicensePlates));
                    if (carRental != null)
                    {
                        MessageBox.Show("Can't Update When The Car Already Rental", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    car.CarName = txtName.Text;
                    car.Type = txtType.Text;
                    car.Brand = txtBrand.Text;
                    car.Color = txtColor.Text;
                    car.Price = decimal.Parse(txtPrice.Text);
                    car.RentalPrice = decimal.Parse(txtRentalPrice.Text);
                    car.CarStatusId = (cbStatus.SelectedItem as CarStatus).CarStatusId;
                    car.DateCar = DateOnly.Parse(dpDate.Text);
                    car.NumberOfSeats = Int32.Parse(txtNumberOfSeat.Text);
                    car.Fuel = txtFuel.Text;
                    car.IsDeleted = false;
                    con.Cars.Update(car);
                    if (con.SaveChanges() > 0)
                    {
                        loadCar();
                        Clear();
                        MessageBox.Show("Update Successfully", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Must select to update", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Car car = con.Cars.FirstOrDefault(x => x.LicensePlates.Equals(txtLicensePlates.Text));
            if (car != null)
            {
                CarRental carRental = con.CarRentals.Where(x => x.IsDeleted == false).FirstOrDefault(x => x.LicensePlates.Equals(car.LicensePlates));
                if (carRental != null)
                {
                    MessageBox.Show("Can't Delete When The Car Already Rental", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                car.IsDeleted = true;
                con.Cars.Update(car);
                if (con.SaveChanges() > 0)
                {
                    MessageBox.Show("Delete Successfully", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                    loadCar();
                    Clear();
                }
                else
                {
                    MessageBox.Show("Delete Unsuccessfully", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            else
            {
                MessageBox.Show("Must select to delete", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void search_TextChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtValueSearch.Text != null)
                {
                    lvCar.ItemsSource = con.Cars.Include(x => x.CarStatus).Where(x => x.IsDeleted == false && x.LicensePlates.ToLower().Contains(txtValueSearch.Text.ToLower())).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Search unsuccessfully!!!\n{ex.Message}", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

