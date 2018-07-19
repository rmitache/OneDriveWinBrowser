using BusinessLogicLayer.BusinessEntities;
using BusinessLogicLayer.Services;
using Microsoft.Win32;
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
        private IFileStorageService fileStorageService;
        private IFileSystemEntity selectedFileSystemEntity = null;
        private object[] treeViewData;

        // Private methods
        private System.IO.Stream GetFileStreamForUpload(string targetFolderName, out string sourceFileName)
        {

            // Open a dialog and let the user choose a file
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Upload to " + targetFolderName,
                Filter = "All Files (*.*)|*.*",
                CheckFileExists = true
            };
            var response = dialog.ShowDialog();


            // User cancels
            if (response != true)
            {
                sourceFileName = null;
                return null;
            }


            // User chose a file - process it and get the fileStream
            try
            {
                sourceFileName = System.IO.Path.GetFileName(dialog.FileName);
                return new System.IO.FileStream(dialog.FileName, System.IO.FileMode.Open);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error uploading file: " + ex.Message);
                sourceFileName = null;
                return null;
            }
        }

        // Constructor
        public MainWindow()
        {
            this.fileStorageService = new FileStorageService();
            InitializeComponent();


            //// DUMMY DATA FOR TESTING PURPOSES
            //var dummyID = "NA";
            //var Folder1 = new Folder(dummyID, "Test", 0, null);
            //var ChildFile1 = new File(dummyID, "Child 1", 4250, Folder1);
            //var ChildFile2 = new File(dummyID, "Child 2", 250, Folder1);
            //var ChildFolder1 = new Folder(dummyID, "Child Folder 1", 0, Folder1);
            //Folder1.Files.Add(ChildFile1);
            //Folder1.Files.Add(ChildFile2);
            //Folder1.Folders.Add(ChildFolder1);
            //var Folder2 = new Folder(dummyID, "Hey", 0, null);
            //var File1 = new File(dummyID, "Hey", 250, null);

            //this.treeViewData = new object[] { Folder1, Folder2, File1 };
            //Treeview.ItemsSource = treeViewData;
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
                this.treeViewData = new object[] { rootFolder };
                Treeview.ItemsSource = this.treeViewData;
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
                this.selectedFileSystemEntity = e.NewValue as IFileSystemEntity;
                lblTaskNameTitle.Content = this.selectedFileSystemEntity.Name;

                // Enable/disable action buttons
                this.UploadButton.IsEnabled = true; // always enabled when something is selected in the TreeView
                switch (this.selectedFileSystemEntity.EntityType)
                {
                    case IFileSystemEntityType.Folder:
                        this.DownloadButton.IsEnabled = false;
                        this.UploadButton.IsEnabled = true;
                        break;
                    case IFileSystemEntityType.File:
                        this.DownloadButton.IsEnabled = true;
                        this.UploadButton.IsEnabled = false;
                        break;
                }
            }
        }
        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            var targetFile = this.selectedFileSystemEntity as File;
            try
            {
                // Setup a dialog prompting the user to choose the download location
                var dialog = new SaveFileDialog
                {
                    FileName = targetFile.Name,
                    Filter = "All Files (*.*)|*.*"
                };
                if (dialog.ShowDialog() != true)
                    return;

                // Download the file 
                var fileDownloadPath = dialog.FileName;
                using (var fileStream = await this.fileStorageService.DownloadFile(targetFile))
                using (var outputStream = new System.IO.FileStream(dialog.FileName, System.IO.FileMode.Create))
                {
                    await fileStream.CopyToAsync(outputStream);
                    MessageBox.Show("File " + targetFile.Name +" successfully downloaded to " + fileDownloadPath);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Download failed - " + ex.Message);
            }
        }
        private async void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            var parentUploadFolder = this.selectedFileSystemEntity as Folder;

            string filename;
            using (var stream = GetFileStreamForUpload(parentUploadFolder.Name, out filename))
            {
                if (stream != null)
                {
                    var uploadedFile = await this.fileStorageService.UploadFile(parentUploadFolder, filename, stream);
                    if(uploadedFile!=null)
                    {
                        parentUploadFolder.Files.Add(uploadedFile);
                        uploadedFile.ParentFolder = parentUploadFolder;
                        CollectionViewSource.GetDefaultView(this.treeViewData).Refresh(); // refresh the TreeView UI

                        MessageBox.Show("File with name " + filename + " was uploaded successfully !");
                    } else
                    {
                        MessageBox.Show("File with name " + filename + " couldn't be uploaded. Please try again later", "Error!");
                    }
                }
            }

        }
    }
}
