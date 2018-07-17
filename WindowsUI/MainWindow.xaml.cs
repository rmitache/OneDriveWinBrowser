using ServiceLayer.CloudStorageProviders;
using ServiceLayer.Services;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WindowsUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Fields
        private FileStorageService fileStorageService;

        // Constructor
        public MainWindow()
        {
            this.fileStorageService = new FileStorageService();
            InitializeComponent();
        }


        // Event handlers
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            bool loginSuccessful = false;

            await this.fileStorageService.GetFileStorageRootFolder();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Log in failed. Please try again " + "(" + ex.Message + ")");
            //}

            if (loginSuccessful)
            {
                MainWindow main = new MainWindow();
                App.Current.MainWindow = main;
                this.Close();
                main.Show();
            }
        }
    }
}
