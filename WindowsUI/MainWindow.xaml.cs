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

        // Private methods
        //private List<TreeViewItem> GenerateTreeViewItems(List<IFileSystemEntity> list)
        //{
        //    //// This needs to be recursive !!!!
        //    //var itemsList = new List<TreeViewItem>();
        //    //foreach (IFileSystemEntity fileSystemEntity in list)
        //    //{
        //    //    TreeViewItem treeViewItem = new TreeViewItem();
        //    //    treeViewItem.Header = fileSystemEntity.Name + " (" + ByteSize.FromBytes((double)fileSystemEntity.Size) + ")";
        //    //    itemsList.Add(treeViewItem);
        //    //}

        //    //return itemsList;

        //    return null;
        //}
        
        //private TreeViewItem GenerateTreeViewItemRecursive(Folder folder)
        //{
        //    //// Create self
        //    //var treeViewItem = new TreeViewItem();
        //    //treeViewItem.Header = folder.Name;

        //    //// Add items for child Folders
        //    //foreach (Folder childFolder in folder.Folders)
        //    //{
        //    //    var folderTreeViewItem = GenerateTreeViewItemRecursive(childFolder); // recursive call to self
        //    //    treeViewItem.Items.Add(folderTreeViewItem);
        //    //}

        //    //// Add items for child Files
        //    //foreach (File file in folder.Files)
        //    //{
        //    //    var fileTreeViewItem = new TreeViewItem();
        //    //    fileTreeViewItem.Header = GetFormattedTreeViewHeader(file.Name, file.Size);
        //    //    treeViewItem.Items.Add(fileTreeViewItem);
        //    //}

        //    //return treeViewItem;
        //}

        // Constructor
        public MainWindow()
        {
            this.fileStorageService = new FileStorageService();
            InitializeComponent();

            var Folder1 = new Folder("Test");
            var Folder2 = new Folder("Hey");
            var File1 = new File("Hey", 250);
            Treeview.ItemsSource = new object[] { Folder1, Folder2, File1 };
        }


        // Event handlers
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Disable and clear controls
            LoginButton.IsEnabled = false;
            Treeview.Items.Clear();

            // Get data from cloud and load into TreeView
            var rootFolder = await this.fileStorageService.GetRootFolderWithDescendants();
            //var rootTreeViewItem = this.GenerateTreeViewItemRecursive(rootFolder);
            //Treeview.Items.Add(rootTreeViewItem);
            

        }

        private void Treeview_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedEntity = e.NewValue as IFileSystemEntity;
            lblTaskNameTitle.Content = selectedEntity.Name;
        }

        
    }
}
