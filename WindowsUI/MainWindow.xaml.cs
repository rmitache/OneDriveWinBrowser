using BusinessLogicLayer.BusinessEntities;
using BusinessLogicLayer.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
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


            // Some dummy data
            var Folder1 = new Folder("Test");
            var ChildFile1 = new File("Child 1", 4250);
            var ChildFile2 = new File("Child 2", 250);
            var ChildFolder1 = new Folder("Child Folder 1");
            Folder1.Files.Add(ChildFile1);
            Folder1.Files.Add(ChildFile2);
            Folder1.Folders.Add(ChildFolder1);
            var Folder2 = new Folder("Hey");
            var File1 = new File("Hey", 250);
            Treeview.ItemsSource = new object[] { Folder1, Folder2, File1 };
        }

        // Event handlers
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Disable and clear controls
            LoginButton.IsEnabled = false;

            // Get data from cloud and load into TreeView
            try
            {
                var rootFolder = await this.fileStorageService.GetRootFolderWithDescendants();
                Treeview.ItemsSource = new object[] { rootFolder };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login failed - " + ex.Message);
            }
        }
        private void Treeview_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue != null)
            {
                var selectedEntity = e.NewValue as IFileSystemEntity;
                lblTaskNameTitle.Content = selectedEntity.Name;


                // Enable/disable action buttons
                this.UploadButton.IsEnabled = true; // always enabled when something is selected
                switch (selectedEntity.EntityType)
                {
                    case IFileSystemEntityType.Folder:
                        this.DownloadButton.IsEnabled = false;

                        break;
                    case IFileSystemEntityType.File:
                        this.DownloadButton.IsEnabled = true;
                        break;
                }
            }
        }
        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
