using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Microsoft.Msagl.Drawing;
using FolderCrawler;

public class DFSClass
{
    bool found;
    GraphContext graphContext;
    private List<string> _result;

    public DFSClass(ref GraphContext context)
    {
        graphContext = context;
        _result = new List<string>();
    }

    public void DFS(string absolutepath, string namaFolderSaatIni, string file_yang_dicari, bool all)
    {
        if (absolutepath == namaFolderSaatIni)
        {
            graphContext.AddNode(absolutepath, absolutepath);
            graphContext.UpdateView();
        }

        string parentPath = absolutepath;

        /*
        absolutepath - path absolute dari folder yang ingin dicari isinya
        namaFolderSaatIni - mengambil nama terahir dari sebuah absolute path, digunakan untuk menyambungkan graf
        file_yang_dicari - nama dari file yang dicari
        all - True jika ingin mencari semua file, false jika tidak
        */

        //Kalo File Belum Ketemu atau mode yang dicari adalah mecari semua file 
        if (!found || all)
        {
            //Ngecek Folder (Bagian Rekursif)
            string[] Folders = Directory.GetDirectories(absolutepath);
            for (int i = 0; i < Folders.Length; i++)
            {
                string folderPath = Folders[i];
                string folderBasename = Path.GetRelativePath(absolutepath, folderPath);

                // TODO: Enqueue absolute path
                //graphContext.EnqueueAddEdge(namaFolderSaatIni, FolderName);
                //graphContext.RequestUpdateView();


                graphContext.AddNode(folderPath, folderBasename);
                graphContext.AddEdge(parentPath, folderPath);

                DFS(Folders[i], folderBasename, file_yang_dicari, all);

                //graphContext.EnqueueColorEdge(namaFolderSaatIni, folderBasename,
                //    new Color(0, 0, 255));

                if (found && !all)
                {
                    break;
                }
                else
                {
                    graphContext.ColorizeEdge(folderPath, ColorPalette.RED);
                }
            }

            if (!found || all)
            {
                string[] files = Directory.GetFiles(absolutepath, "*");
                foreach (var filePath in files)
                {
                    if (!found || all)
                    {
                        string fileName = Path.GetRelativePath(absolutepath, filePath);



                        // TODO: Enqueue absolute path

                        graphContext.AddNode(filePath, fileName);
                        graphContext.AddEdge(parentPath, filePath);

                        if (fileName == file_yang_dicari)
                        {
                            found = true;
                            _result.Add(filePath);
                        }
                        else
                        {
                            graphContext.ColorizeEdge(filePath, ColorPalette.RED);
                        }

                        //graphContext.EnqueueAddEdge(namaFolderSaatIni, FileName);
                        //graphContext.RequestUpdateView();
                    }
                }

            }
        }

        if (absolutepath == namaFolderSaatIni)
        {
            _result.ForEach(
                (string filePath) =>
                {
                    graphContext.ColorizePath(filePath, ColorPalette.BLUE);
                });
        }
    }

    public List<string> Result()
    {
        return _result;
    }

}
