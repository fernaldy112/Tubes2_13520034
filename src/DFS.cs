using System.IO;
using Microsoft.Msagl.Drawing;
using FolderCrawler;

public class DFSClass
{
    bool found;
    GraphContext graphContext;

    public DFSClass(ref GraphContext context)
    {
        graphContext = context;
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

                // TODO: Enqueue absolute path
                graphContext.EnqueueAddEdge(namaFolderSaatIni, FolderName);
                graphContext.RequestUpdateView();
                DFS(Folders[i], FolderName, file_yang_dicari, all);

                graphContext.EnqueueColorEdge(namaFolderSaatIni, FolderName,
                    new Color(0, 0, 255));

                if (this.found && !all)
                {
                    break;
                }
            }

            if (!found || all)
            {
                string[] files = Directory.GetFiles(absolutepath, "*");
                foreach (var file in files)
                {
                    if (!this.found)
                    {
                        string FileName = Path.GetRelativePath(absolutepath, file);

                        // TODO: Enqueue absolute path
                        graphContext.EnqueueAddEdge(namaFolderSaatIni, FileName);
                        graphContext.RequestUpdateView();
                    }
                }

            }
        }
    }
}
