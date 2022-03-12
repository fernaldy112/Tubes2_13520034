using System;
using System.IO;
using System.Windows;
using Microsoft.Msagl.Drawing;
using Microsoft.WindowsAPICodePack.Dialogs;
using FolderCrawler;
using System.Threading.Tasks;

namespace FolderCrawler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private GraphContext _graphContext;

        public MainWindow()
        {
            InitializeComponent();
            _graphContext = new(ref GraphControl);
            _graphContext.ResetGraph();

        }

        private void ButtonOnClick(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new();
            dialog.IsFolderPicker = true;
            dialog.InitialDirectory = Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments
                );
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                GraphControl.Graph = new Graph();
                DirectoryPath.Text = dialog.FileName;
            }
        }


        private void OnSearch(object sender, RoutedEventArgs e)
        {
            bool useDfs = DfsModeButton.IsChecked ?? false;
            bool useBfs = BfsModeButton.IsChecked ?? false;

            if (!useDfs && !useBfs)
            {
                // Handle error here
            }

            string fileToSearch = SearchInput.Text;

            if (fileToSearch == "")
            {
                // Handle error here
            }

            string path = DirectoryPath.Text;

            if (!Directory.Exists(path))
            {
                // Handle error here
            }

            bool exhaustive = ExhaustiveCheckBox.IsChecked ?? false;

            if (useDfs)
            {
                Task.Run(() =>
                {
                    DFSClass d = new(ref _graphContext);
                    d.DFS(path, path, fileToSearch, exhaustive);
                    _graphContext.ForceUpdateView();
                });
            }
            else
            {
                Task.Run(() =>
                {
                    BFSClass b = new(ref _graphContext);
                    b.BFS(path, fileToSearch, exhaustive);
                    _graphContext.ForceUpdateView();
                });
            }


        }

    }
}
