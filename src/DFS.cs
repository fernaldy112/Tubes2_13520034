using System.IO;
using System.Collections.Generic;

namespace FolderCrawler
{
    public class Dfs
    {
        private bool _found;
        private readonly GraphContext _graphContext;
        private readonly List<string> _result;

        public Dfs(ref GraphContext context)
        {
            _graphContext = context;
            _result = new List<string>();
        }

        public void Search(string dir, string fileToFind, bool exhaustive)
        {
            _graphContext.AddNode(dir, dir);
            _graphContext.UpdateView();

            RecursiveSearch(dir, fileToFind, exhaustive);

            _result.ForEach(
                filePath =>
                {
                    _graphContext.ColorizePath(filePath, ColorPalette.BLUE);
                });
        }

        private void RecursiveSearch(
            string dir,
            string fileToFind,
            bool exhaustive
            )
        {

            /*
            dir - path absolute dari folder yang ingin dicari isinya
            namaFolderSaatIni - mengambil nama terahir dari sebuah absolute path, digunakan untuk menyambungkan graf
            file_yang_dicari - nama dari file yang dicari
            exhaustive - True jika ingin mencari semua file, false jika tidak
            */

            //Kalo File Belum Ketemu atau mode yang dicari adalah mecari semua file 
            if (!_found || exhaustive)
            {
                //Ngecek Folder (Bagian Rekursif)
                string[] files = Directory.GetFiles(dir, "*");
                foreach (var filePath in files)
                {
                    if (!_found || exhaustive)
                    {
                        string fileName = Path.GetRelativePath(dir, filePath);
                        _graphContext.AddNode(filePath, fileName);
                        _graphContext.AddEdge(dir, filePath);

                        if (fileName == fileToFind)
                        {
                            _found = true;
                            _result.Add(filePath);
                        }
                        else
                        {
                            _graphContext.ColorizeEdge(filePath, ColorPalette.RED);
                        }
                    }
                }
                

                if (!_found || exhaustive)
                {
                    string[] folders = Directory.GetDirectories(dir);
                    for (int i = 0; i < folders.Length; i++)
                    {
                        string folderPath = folders[i];
                        string folderBasename = Path.GetRelativePath(dir, folderPath);

                        _graphContext.AddNode(folderPath, folderBasename);
                        _graphContext.AddEdge(dir, folderPath);

                        RecursiveSearch(folders[i], fileToFind, exhaustive);

                        if (_found && !exhaustive)
                        {
                            break;
                        }
                        else
                        {
                            _graphContext.ColorizeEdge(folderPath, ColorPalette.RED);
                        }
                    }
                }
            }
        }

        public List<string> Result()
        {
            return _result;
        }

    }
}

