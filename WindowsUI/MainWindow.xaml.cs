using BusinessLogicLayer.BusinessEntities;
using BusinessLogicLayer.Services;
using ByteSizeLib;
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

        // Private methods
        private List<TreeViewItem> GenerateTreeViewItems(List<IFileSystemEntity> list)
        {
            // This needs to be recursive !!!!
            var itemsList = new List<TreeViewItem>();
            foreach (IFileSystemEntity fileSystemEntity in list)
            {
                TreeViewItem treeViewItem = new TreeViewItem();
                treeViewItem.Header = fileSystemEntity.Name + " (" + ByteSize.FromBytes((double)fileSystemEntity.Size) + ")";
                itemsList.Add(treeViewItem);
            }

            return itemsList;
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
            Treeview.Items.Clear(); 

            // Get data from cloud and load into TreeView
            var fileSystemEntities = await this.fileStorageService.GetRootFolderWithDescendants();
            //var treeViewItems = this.GenerateTreeViewItems(fileSystemEntities);
            //Treeview.ItemsSource = treeViewItems;
            //treeViewItems.ForEach(item =>
            //{
            //    Treeview.Items.Add(item);
            //});
            //treeViewItems.Reverse();

        }
    }
}
