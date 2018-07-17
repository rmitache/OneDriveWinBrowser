using ServiceLayer.BusinessEntities;
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

        // Private methods
        private void GenerateTreeViewItems(List<IFileSystemEntity> list)
        {
            var itemsList = new List<TreeViewItem>();
            foreach (IFileSystemEntity fileSystemEntity in list)
            {
                TreeViewItem treeViewItem = new TreeViewItem();
                treeViewItem.Header = fileSystemEntity.Name + " (" +
            }
        }

        // Constructor
        public MainWindow()
        {
            this.fileStorageService = new FileStorageService();
            InitializeComponent();
        }


        // Event handlers
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            // Disable and clear controls
            LoginButton.IsEnabled = false;
            var x = Treeview.Items[0]; // Clear();


            // Get data from cloud and load into TreeView
            await this.fileStorageService.GetFileStorageRootFolder();

        }
    }
}
