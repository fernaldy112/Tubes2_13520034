using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Msagl.Drawing;
using Microsoft.WindowsAPICodePack.Dialogs;
using DispatcherPriority = System.Windows.Threading.DispatcherPriority;

namespace FolderCrawler
{
    /// <summary>
    /// <c>MainWindow</c> class is the code-behind of
    /// the main GUI window.
    /// </summary>
    public partial class MainWindow : Window
    {

        /**
         * <summary>
         *  Graph context object to pass to searcher object.
         * </summary>
         */
        private GraphContext _graphContext;

        /**
         * <summary>
         *  Creates the main window to display to the user.
         * </summary>
         */
        public MainWindow()
        {
            InitializeComponent();
            _graphContext = new(ref GraphControl);
            _graphContext.ResetGraph();
        }

        /**
         * <summary>
         *  Will be called when the user press <c>Choose Directory ...</c>
         *  button.
         * </summary>
         */
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

        /**
         * <summary>
         *  Will be called when the user press the <c>SEARCH</c> button.
         * </summary>
         */
        private void OnSearch(object sender, RoutedEventArgs e)
        {
            HideErrorMsg();
            ResultView.Visibility = Visibility.Collapsed;
            _graphContext.ResetGraph();
            ResultView.Items.Clear();

            if (ValidateInput())
            {
                bool useDfs = DfsModeButton.IsChecked ?? false;
                string fileToSearch = SearchInput.Text;
                string path = DirectoryPath.Text;

                bool exhaustive = ExhaustiveCheckBox.IsChecked ?? false;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                if (useDfs)
                {
                    Task.Run(() =>
                    {
                        Dfs searcher = new(ref _graphContext);
                        searcher.Search(path, fileToSearch, exhaustive);
                        stopwatch.Stop();
                        ShowSearchResult(searcher.Result(), stopwatch.ElapsedMilliseconds);
                    });
                }
                else
                {
                    Task.Run(() =>
                    {
                        Bfs searcher = new(ref _graphContext);
                        searcher.Search(path, fileToSearch, exhaustive);
                        stopwatch.Stop();
                        ShowSearchResult(searcher.Result(), stopwatch.ElapsedMilliseconds);
                    });
                }
            }
        }

        /**
         * <summary>
         *  Retrieves user searching input data & validates them.
         * </summary>
         *
         * <returns>
         *  true if all necessary input exists & valid, false
         *  otherwise
         * </returns>
         */
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

        /**
         * <summary>
         *  Hides all error messages. Always called before searching
         *  process starts.
         * </summary>
         */
        private void HideErrorMsg()
        {
            ModeErrorMsg.Visibility = Visibility.Collapsed;
            SearchErrorMsg.Visibility = Visibility.Collapsed;
            DirectoryErrorMsg.Visibility = Visibility.Collapsed;
            NotFoundMsg.Visibility = Visibility.Collapsed;
        }

        /**
         * <summary>
         *  Dynamically creates Hyperlink objects from searching results
         *  and add them to the GUI.
         * </summary>
         *
         * <param name="result">
         *  the searching result
         * </param>
         *
         * <param name="time">
         *  total searching duration
         * </param>
         */
        private void ShowSearchResult(List<string> result, long time)
        {
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                () =>
                {
                    if (result.Count != 0)
                    {
                        result.ForEach(path =>
                        {
                            Hyperlink link = CreateNewResultItem(path);
                            ResultView.Items.Add(link);
                        });
                        if (time < 100000)
                        {
                            ResultView.Items.Add("Waktu Pencarian: " + time + " ms");
                        }
                        else
                        {
                            time /= 100000;
                            ResultView.Items.Add("Waktu Pencarian: " + time + " s");
                        }
                        ResultView.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        NotFoundMsg.Visibility = Visibility.Visible;
                    }
                });
        }

        /**
         * <summary>
         *  Creates a new Hyperlink item from a path.
         *  The resulting hyperlink will link to the path.
         * </summary>
         *
         * <param name="path">
         *  the path to point by the hyperlink
         * </param>
         *
         * <returns>
         *  the hyperlink object that points to the path
         * </returns>
         */
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
