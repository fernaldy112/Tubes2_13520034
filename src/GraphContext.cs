using System.Windows;
using System.Windows.Threading;
using System.Collections.Generic;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;

namespace FolderCrawler
{
    public class GraphContext
    {
        private readonly AutomaticGraphLayoutControl _control;
        private readonly Graph _nullGraph = new();

        private Dictionary<string, Node> _nodes;
        private Dictionary<string, Edge> _edges;
        private Dictionary<string, string> _parents;

        public GraphContext(
            ref AutomaticGraphLayoutControl control
            )
        {
            _control = control;

            _nodes = new Dictionary<string, Node>();
            _edges = new Dictionary<string, Edge>();
            _parents = new Dictionary<string, string>();
        }

        public void ResetGraph()
        {
            _control.Graph = new Graph();
            _nodes = new Dictionary<string, Node>();
            _edges = new Dictionary<string, Edge>();
            _parents = new Dictionary<string, string>();
            UpdateView();
        }

        public void AddNode(string path, string label)
        {
            if (!_nodes.TryGetValue(path, out Node node))
            {
                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    () =>
                    {
                        node = new(path);
                        node.LabelText = label;
                        _nodes.Add(path, node);
                        _control.Graph.AddNode(node);
                    }).Wait();
            }
        }

        public void AddEdge(string sourceNode, string destNode)
        {
            if (!_nodes.TryGetValue(sourceNode, out Node _)
                || !_nodes.TryGetValue(destNode, out Node _))
            {
                throw new KeyNotFoundException("Invalid node(s)");
            }

            if (!_edges.TryGetValue(destNode, out Edge _))
            {
                _parents.Add(destNode, sourceNode);
                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    () =>
                    {
                        Graph graph = _control.Graph;
                        Edge edge = graph.AddEdge(sourceNode, destNode);
                        _edges.Add(destNode, edge);

                        UpdateView();
                    }).Wait();
            }
        }

        public void ColorizeEdge(string destNode, Color color)
        {
            if (!_edges.TryGetValue(destNode, out Edge edge))
            {
                throw new KeyNotFoundException("Invalid node(s)");
            }

            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                () =>
                {
                    edge.Attr.Color = color;
                    UpdateView();
                }).Wait();
        }

        public void ColorizePath(string destNode, Color color)
        {
            string path = destNode;
            ColorizeEdge(path, color);
            while (_parents.TryGetValue(path, out path))
            {
                if (_parents.TryGetValue(path, out _))
                {
                    ColorizeEdge(path, color);
                }
            }
        }

        public void UpdateView()
        {
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                () =>
                {
                    Graph g = _control.Graph;
                    _control.Graph = _nullGraph;
                    _control.Graph = g;
                });
        }
    }
}
