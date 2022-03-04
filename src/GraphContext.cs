using System;
using System.Windows;
using System.Windows.Threading;
using System.Collections.Generic;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;

namespace FolderCrawler
{
    public class GraphContext
    {
        private AutomaticGraphLayoutControl Control;
        private Graph NullGraph = new();
        private bool IsUpdatingView = false;

        private Queue<Tuple<string, string>> EdgeQueue;
        private Queue<EdgeColorQueueItem> EdgeColorQueue;
        private Queue<Tuple<string, string>> NodeColorQueue;
        Dictionary<string, Dictionary<string, Edge>> Edges;

        private DispatcherOperation UpdateViewOperation = null;

        public GraphContext(
            AutomaticGraphLayoutControl control
            )
        {
            Control = control;
            EdgeQueue = new Queue<Tuple<string,string>>();
            EdgeColorQueue = new Queue<EdgeColorQueueItem>();
            NodeColorQueue = new Queue<Tuple<string, string>>();
            Edges = new Dictionary<string, Dictionary<string, Edge>>();
        }


        public void RequestUpdateView(bool forced = false)
        {
            if (forced && UpdateViewOperation != null)
            {
                UpdateViewOperation.Wait();
            }
            if (UpdateViewOperation != null)
            {
                UpdateViewOperation = Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                () =>
                {
                    Graph g = Control.Graph;
                    // TODO: Add Edge, Add Edge Color, Add Node Color
                    Control.Graph = NullGraph;
                    Control.Graph = g;
                    UpdateViewOperation = null;
                });   
            }
            
        }

        public void ResetGraph()
        {
            Control.Graph = new Graph();
            RequestUpdateView();
        }

        public void EnqueueAddEdge(string source, string target)
        {
            EdgeQueue.Enqueue(Tuple.Create(source, target));
        }

        public void EnqueueColorEdge(string source, string target, string color)
        {
            EdgeColorQueue.Enqueue(
                new EdgeColorQueueItem(source, target, color)
                );
        }

        public void EnqueueColorNode(string node, string color)
        {
            NodeColorQueue.Enqueue(Tuple.Create(node, color));
        }
    }

    public class EdgeColorQueueItem
    {
        public string Source;
        public string Target;
        public string Color;

        public EdgeColorQueueItem(
            string source,
            string target,
            string color
            )
        {
            Source = source;
            Target = target;
            Color = color;
        }
    }
}
