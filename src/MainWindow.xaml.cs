using System;
using System.Windows;
using Microsoft.Msagl.Drawing;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using Folder_Crawling_WPF;

namespace Folder_Crawling_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public Graph graph = new();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoad;

        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            //graph.AddEdge("Octagon", "Hexagon");
            //graph.FindNode("Octagon").Attr.Shape = Shape.Octagon;
            //graph.FindNode("Hexagon").Attr.Shape = Shape.Hexagon;
            //graph.Attr.LayerDirection = LayerDirection.TB;
            //graph.AddNode("Something");
            //graph.AddEdge("Hexagon", "Something");

            graphContext.Graph = graph;

            
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
                string path = dialog.FileName;
                DFSClass d = new(ref graphContext);
                d.DFS(path, path, "something", false);

                //Graph g = graphContext.Graph;
                //graphContext.Graph = new Graph();
                //graphContext.Graph = g;
                
            }


        }

        
    }
}
