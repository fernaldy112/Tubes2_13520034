using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Msagl.WpfGraphControl;
using Microsoft.Msagl.Drawing;
using System.Windows;

public class DFSClass
{
    bool found;
    AutomaticGraphLayoutControl graphContext;
    //static void Main(string[] args)
    //{
    //    string path = "D:\\git\\ITB\\Sem4\\Stima\\Tucil2_13520034";
    //    string namaFolderSaatIni = path;
    //    string tujuan = "main.py";
    //    bool all = false;   //True jika ingin mencari semua file
    //    new DFSClass().DFS(path, namaFolderSaatIni, tujuan, all); 
    //}

    public DFSClass(ref AutomaticGraphLayoutControl context)
    {
        this.graphContext = context;
    }

    public void DFS(string absolutepath, string namaFolderSaatIni, string file_yang_dicari, bool all)
    {
        //System.Threading.Thread.Sleep(250);
        /*
        absolutepath - path absolute dari folder yang ingin dicari isinya
        namaFolderSaatIni - mengambil nama terahir dari sebuah absolute path, digunakan untuk menyambungkan graf
        file_yang_dicari - nama dari file yang dicari
        all - True jika ingin mencari semua file, false jika tidak
        */

        //Kalo File Belum Ketemu atau mode yang dicari adalah mecari semua file 
        if (!this.found || all)
        {
            //Ngecek Folder (Bagian Rekursif)
            string[] Folders = Directory.GetDirectories(absolutepath);
            for (int i = 0; i < Folders.Length; i++)
            {
                string FolderName = Path.GetRelativePath(absolutepath, Folders[i]);
                Graph g = graphContext.Graph;
                //g.AddNode(FolderName);
                g.AddEdge(namaFolderSaatIni, FolderName);
                graphContext.Graph = new Graph();
                graphContext.Graph = g;
                //addnode(relativePath);
                //addedge(namaFolderSaatIni, relativePath);
                Console.WriteLine(FolderName);
                DFS(Folders[i], FolderName, file_yang_dicari, all);
                if (this.found && !all)
                {
                    break;
                }
            }
            //Ngecek File jika belum ketemu atau yang dicari adalah seluruh file
            if (!this.found || all)
            {
                string[] files = Directory.GetFiles(absolutepath, "*");
                foreach (var file in files)
                {
                    if (!this.found)
                    {
                        string FileName = Path.GetRelativePath(absolutepath, file);
                        Graph g = graphContext.Graph;
                        //g.AddNode(FileName);
                        g.AddEdge(namaFolderSaatIni, FileName);
                        graphContext.Graph = new Graph();
                        graphContext.Graph = g;
                        //addnode(shortPath);
                        //addedge(namaFolderSaatIni, shortPath);
                        //if (FileName == file_yang_dicari)
                        //{
                        //    this.found = true;
                        //    //Console.WriteLine("File Found ^_^, Open in Explorer?(y/n): ");
                        //    char openInTerminal = Console.ReadKey().KeyChar;
                        //    if (openInTerminal == 'y')
                        //    {
                        //        System.Diagnostics.Process.Start(Environment.GetEnvironmentVariable("WINDIR") + @"\explorer.exe", absolutepath);
                        //    }
                        //}
                        Console.WriteLine(FileName);
                    }
                }

            }
        }
    }
}
