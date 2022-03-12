using System;
using System.Windows;
using System.Windows.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Documents;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;

namespace FolderCrawler
{
    public class GraphContext
    {
        private AutomaticGraphLayoutControl Control;
        private Graph NullGraph = new();

        private Queue<Tuple<string, string>> EdgeQueue;
        private Queue<EdgeColorQueueItem> EdgeColorQueue;
        private Queue<Tuple<string, string>> NodeColorQueue;
        Dictionary<string, Dictionary<string, Edge>> Edges;

        // TODO: Implement nodes & edges store
        private Dictionary<string, Node> _nodes;
        private Dictionary<string, Edge> _edges;
        private Dictionary<string, string> _parents;


        private bool _isUpdatingView;
        private DispatcherOperation UpdateViewOperation;

        public GraphContext(
            ref AutomaticGraphLayoutControl control
            )
        {
            Control = control;
            EdgeQueue = new Queue<Tuple<string, string>>();
            EdgeColorQueue = new Queue<EdgeColorQueueItem>();
            NodeColorQueue = new Queue<Tuple<string, string>>();
            Edges = new Dictionary<string, Dictionary<string, Edge>>();

            _nodes = new Dictionary<string, Node>();
            _edges = new Dictionary<string, Edge>();
            _parents = new Dictionary<string, string>();
        }


        public void RequestUpdateView(bool forced = false)
        {
            if (forced && _isUpdatingView)
            {
                UpdateViewOperation.Wait();
            }
            if (!_isUpdatingView)
            {
                //Trace.WriteLine("Called RequestUpdateView");
                _isUpdatingView = true;
                UpdateViewOperation = Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                () =>
                {
                    _updateEdge();
                    UpdateEdgeColor();
                    _isUpdatingView = false;
                });
            }

        }

        public void ForceUpdateView()
        {
            RequestUpdateView(true);
        }

        public void ResetGraph()
        {
            Control.Graph = new Graph();
            _nodes = new Dictionary<string, Node>();
            _edges = new Dictionary<string, Edge>();
            _parents = new Dictionary<string, string>();
            UpdateView();
        }

        public void EnqueueAddEdge(string source, string target)
        {
            EdgeQueue.Enqueue(Tuple.Create(source, target));
            //Application.Current.Dispatcher.BeginInvoke(
            //    DispatcherPriority.Background,
            //    () =>
            //    {
            //        Graph g = Control.Graph;
            //        //g.AddNode(FolderName);
            //        g.AddEdge(source, target);
            //        Control.Graph = NullGraph;
            //        Control.Graph = g;
            //    });
        }

        public void AddNode(string path, string label)
        {
            Node node;
            if (!_nodes.TryGetValue(path, out node))
            {
                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    () =>
                    {
                        node = new(path);
                        node.LabelText = label;
                        _nodes.Add(path, node);
                        Control.Graph.AddNode(node);
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
                        Graph graph = Control.Graph;
                        Edge edge = graph.AddEdge(sourceNode, destNode);
                        _edges.Add(destNode, edge);

                        UpdateView();
                    }).Wait();
            }
        }

        public void ColorizeNode(string path, Color color)
        {
            if (!_nodes.TryGetValue(path, out Node node))
            {
                throw new KeyNotFoundException("Invalid node(s)");
            }

            node.Attr.Color = color;
            UpdateView();
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

        public void EnqueueColorEdge(string source, string target, Color color)
        {
            EdgeColorQueue.Enqueue(
                new EdgeColorQueueItem(source, target, color)
                );
        }

        public void EnqueueColorNode(string node, string color)
        {
            NodeColorQueue.Enqueue(Tuple.Create(node, color));
        }

        private void _updateEdge()
        {
            // TODO: Construct nodes and edges manually instead of using AddEdge()

            int edgeCount = EdgeQueue.Count;
            Graph g = Control.Graph;

            while (edgeCount-- != 0)
            {
                Dictionary<string, Edge> destNodeEdges;
                Tuple<string, string> edgeData = EdgeQueue.Dequeue();
                if (!Edges.TryGetValue(edgeData.Item1, out destNodeEdges))
                {
                    destNodeEdges = new Dictionary<string, Edge>();
                    Edges.Add(edgeData.Item1, destNodeEdges);
                }

                Edge edge = g.AddEdge(edgeData.Item1, edgeData.Item2);
                edge.Attr.Color = new Color(255, 0, 0);

                if (!destNodeEdges.TryGetValue(edgeData.Item2, out Edge _))
                {
                    destNodeEdges.Add(edgeData.Item2, edge);
                }

            }

            UpdateView();


            //Control.Graph = NullGraph;
            //Control.Graph = g;

        }

        private void UpdateEdgeColor()
        {
            // TODO: Use _edges list instead of dictionary

            int edgeCount = EdgeColorQueue.Count;

            while (edgeCount-- != 0)
            {
                Dictionary<string, Edge> destNodeEdges;
                EdgeColorQueueItem edgeColorData = EdgeColorQueue.Dequeue();
                if (Edges.TryGetValue(edgeColorData.Source, out destNodeEdges))
                {
                    if (destNodeEdges.TryGetValue(edgeColorData.Target, out Edge edge))
                    {
                        edge.Attr.Color = edgeColorData.Color;
                    }
                }

                //UpdateView();
            }

            UpdateView();


            //Graph g = Control.Graph;
            //Control.Graph = NullGraph;
            //Control.Graph = g;
        }

        public void UpdateView()
        {
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                () =>
                {
                    Graph g = Control.Graph;
                    Control.Graph = NullGraph;
                    Control.Graph = g;
                });
        }
    }

    public class EdgeColorQueueItem
    {
        public string Source;
        public string Target;
        public Color Color;

        public EdgeColorQueueItem(
            string source,
            string target,
            Color color
            )
        {
            Source = source;
            Target = target;
            Color = color;
        }
    }
}
