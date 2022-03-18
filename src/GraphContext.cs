using System.Windows;
using System.Windows.Threading;
using System.Collections.Generic;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;

namespace FolderCrawler
{
    /**
     * <summary>
     *  <c>GraphContext</c> acts as a context regarding
     *  graph modifications. This class provides just enough
     *  necessary methods to modify the graph needed by
     *  the application. The class also holds nodes, edges,
     *  and node parents data to enable same node labeling &
     *  graph edge backtracking.
     * </summary>
     */
    public class GraphContext
    {
        private readonly AutomaticGraphLayoutControl _control;
        private readonly Graph _nullGraph = new();

        private Dictionary<string, Node> _nodes;
        private Dictionary<string, Edge> _edges;
        private Dictionary<string, string> _parents;

        /**
         * <summary>
         *  Creates a new <c>GraphContext</c> object from a
         *  given graph control and initializes every data to
         *  empty.
         * </summary>
         *
         * <param name="control">
         *  the graph control to use and modify by the context
         * </param>
         */
        public GraphContext(
            ref AutomaticGraphLayoutControl control
            )
        {
            _control = control;

            _nodes = new Dictionary<string, Node>();
            _edges = new Dictionary<string, Edge>();
            _parents = new Dictionary<string, string>();
        }

        /**
         * <summary>
         *  Resets the graph in the control to an empty
         *  graph (null graph), i.e. having no nodes
         *  at all.
         * </summary>
         */
        public void ResetGraph()
        {
            _control.Graph = new Graph();
            _nodes = new Dictionary<string, Node>();
            _edges = new Dictionary<string, Edge>();
            _parents = new Dictionary<string, string>();
            UpdateView();
        }

        /**
         * <summary>
         *  Adds a new node to the graph with a given path
         *  and label.
         * </summary>
         *
         * <param name="path">
         *  the file or directory path the node resembles
         * </param>
         *
         * <param name="label">
         *  the label of the node displayed in the GUI
         * </param>
         */
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

        /**
         * <summary>
         *  Adds a new edge to the graph from a given source node
         *  path and destination node path.
         * </summary>
         *
         * <param name="sourceNode">
         *  the source node path
         * </param>
         *
         * <param name="destNode">
         *  the destination node path
         * </param>
         */
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

        /**
         * <summary>
         *  Colorizes an edge that points to a specific
         *  destination node with a specific color.
         * </summary>
         *
         * <param name="destNode">
         *  the destination node path
         * </param>
         *
         * <param name="color">
         *  the color to colorize the node
         * </param>
         */
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

        /**
         * <summary>
         *  Recursively colorizes the edges from the starting node
         *  to a specific destination node with a specific color.
         * </summary>
         *
         * <param name="destNode">
         *  the destination node path
         * </param>
         *
         * <param name="color">
         *  the color to colorize the node
         * </param>
         */
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

        /**
         * <summary>
         *  Update the view of the graph displayed in the GUI
         *  immediately.
         * </summary>
         */
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
