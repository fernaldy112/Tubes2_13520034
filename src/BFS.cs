using System;
using System.Collections.Generic;
using System.IO;


namespace FolderCrawler
{
    public class BFSClass
    {
        private GraphContext _graphContext;

        private List<string> _result;

        public BFSClass(ref GraphContext Context)
        {
            _graphContext = Context;
            _result = new List<string>();
        }

        public void BFS(string dir, string file_yang_dicari, bool all)
        {
            Queue<string> dirQueue = new Queue<string>();
            string curDir, FileName, FolderName;
            string[] files, folders;
            bool found = false;
            int i, n;


            dirQueue.Enqueue(dir);
            while (!(dirQueue.Count == 0) && !(found && !all))
            {
                curDir = dirQueue.Dequeue();

                _graphContext.AddNode(curDir,
                    new DirectoryInfo(curDir).Name);

                files = Directory.GetFiles(curDir, "*");

                // TODO: Enqueue node & edge
                for (i = 0; i < files.Length; i++)
                {
                    string parent = curDir;
                    string filePath = files[i];

                    _graphContext.AddNode(filePath,
                        new DirectoryInfo(filePath).Name);
                    _graphContext.AddEdge(parent, filePath);
                }

                i = 0;
                n = files.Length;
                while (i < n && !(found && !all))
                {
                    FileName = Path.GetRelativePath(curDir, files[i]);
                    Console.WriteLine(FileName);
                    if (FileName == file_yang_dicari)
                    {
                        found = true;
                        _graphContext.ColorizeEdge(files[i], ColorPalette.BLUE);
                        _result.Add(files[i]);
                    }
                    else
                    {
                        // TODO: Color dead end node edge
                        _graphContext.ColorizeEdge(files[i], ColorPalette.RED);
                    }
                    i++;

                }

                folders = Directory.GetDirectories(curDir);
                i = 0;
                n = folders.Length;
                while (i < n && !(found && !all))
                {
                    //Console.WriteLine(FolderName);
                    string folderPath = folders[i];
                    dirQueue.Enqueue(folderPath);

                    // TODO: Enqueue node & edge
                    string parent = curDir;
                    _graphContext.AddNode(folderPath,
                        new DirectoryInfo(folderPath).Name);
                    _graphContext.AddEdge(parent, folderPath);

                    _graphContext.ColorizeEdge(folderPath, ColorPalette.RED);

                    i++;
                }
            }

            _result.ForEach(
                (string filePath) =>
                {
                    _graphContext.ColorizePath(filePath, ColorPalette.BLUE);
                });
        }

        public List<string> Result()
        {
            return _result;
        }

    }


}