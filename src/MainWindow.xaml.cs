﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Microsoft.Msagl.Drawing;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Threading.Tasks;
using System.Windows.Documents;
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
            _graphContext.ResetGraph();

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

        private void ShowSearchResult(List<string> result)
        {
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                () =>
                {
                    result.ForEach(path =>
                    {
                        Hyperlink link = CreateNewResultItem(path);
                        ResultView.Items.Add(link);
                    });
                });
        }

        private Hyperlink CreateNewResultItem(string path)
        {
            Hyperlink link = new();
            link.Inlines.Add(path);
            link.Click += (_, _) =>
            {
                System.Diagnostics.Process.Start("explorer.exe",
                    Path.GetDirectoryName(path));
            };
            return link;
        }

    }
}
