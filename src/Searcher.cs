using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Folder_Crawling_WPF;
using Microsoft.Msagl.WpfGraphControl;
using Microsoft.Msagl.Drawing;

namespace Folder_Crawling_WPF
{
    class Searcher
    {

        public static void BFS(ref AutomaticGraphLayoutControl graphContext)
        {
            Graph g = graphContext.Graph;
            g.AddNode("123");
            Edge e = g.AddEdge("Hexagon", "123");
            graphContext.Graph = new Graph();
            graphContext.Graph = g;
        }
    }
}
