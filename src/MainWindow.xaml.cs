using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Msagl.Drawing;
using Microsoft.WindowsAPICodePack.Dialogs;
using DispatcherPriority = System.Windows.Threading.DispatcherPriority;

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

        private void OnChooseDir(object sender, RoutedEventArgs e)
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
            HideErrorMsg();
            ResultView.Visibility = Visibility.Collapsed;
            _graphContext.ResetGraph();

            if (ValidateInput())
            {
                bool useDfs = DfsModeButton.IsChecked ?? false;
                string fileToSearch = SearchInput.Text;
                string path = DirectoryPath.Text;

                bool exhaustive = ExhaustiveCheckBox.IsChecked ?? false;

                if (useDfs)
                {
                    Task.Run(() =>
                    {
                        Dfs searcher = new(ref _graphContext);
                        searcher.Search(path, fileToSearch, exhaustive);
                        ShowSearchResult(searcher.Result());
                    });
                }
                else
                {
                    Task.Run(() =>
                    {
                        Bfs searcher = new(ref _graphContext);
                        searcher.Search(path, fileToSearch, exhaustive);
                        ShowSearchResult(searcher.Result());
                    });
                }
            }
        }

        private bool ValidateInput()
        {
            bool valid = true;

            bool useDfs = DfsModeButton.IsChecked ?? false;
            bool useBfs = BfsModeButton.IsChecked ?? false;

            if (!useDfs && !useBfs)
            {
                ModeErrorMsg.Visibility = Visibility.Visible;
                valid = false;
            }

            string fileToSearch = SearchInput.Text;

            if (fileToSearch == "")
            {
                SearchErrorMsg.Visibility = Visibility.Visible;
                valid = false;
            }

            string path = DirectoryPath.Text;

            if (!Directory.Exists(path))
            {
                DirectoryErrorMsg.Visibility = Visibility.Visible;
                valid = false;
            }

            return valid;
        }

        private void HideErrorMsg()
        {
            ModeErrorMsg.Visibility = Visibility.Collapsed;
            SearchErrorMsg.Visibility = Visibility.Collapsed;
            DirectoryErrorMsg.Visibility = Visibility.Collapsed;
            NotFoundMsg.Visibility = Visibility.Collapsed;
        }

        private void ShowSearchResult(List<string> result)
        {
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                () =>
                {
                    if (result.Count != 0)
                    {
                        ResultView.Items.Clear();
                        result.ForEach(path =>
                        {
                            Hyperlink link = CreateNewResultItem(path);
                            ResultView.Items.Add(link);
                        });
                        ResultView.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        NotFoundMsg.Visibility = Visibility.Visible;
                    }
                });
        }

        private Hyperlink CreateNewResultItem(string path)
        {
            Hyperlink link = new();
            link.Inlines.Add(path);
            link.FontFamily = new("Segoe UI");
            link.FontSize = 14;
            link.FontWeight = FontWeights.DemiBold;

            link.Click += (_, _) =>
            {
                System.Diagnostics.Process.Start("explorer.exe",
                    Path.GetDirectoryName(path));
            };
            return link;
        }

    }
}
