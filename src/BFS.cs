using System;
using System.Collections.Generic;
using System.IO;


namespace FolderCrawler
{
    public class BFSClass
    {
        private GraphContext _graphContext;
        public BFSClass(ref GraphContext Context)
        {
            _graphContext = Context;
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
                files = Directory.GetFiles(curDir, "*");

                // TODO: Enqueue node & edge

                i = 0;
                n = files.Length;
                while (i < n && !(found && !all))
                {
                    FileName = Path.GetRelativePath(curDir, files[i]);
                    Console.WriteLine(FileName);
                    if (FileName == file_yang_dicari)
                    {
                        found = true;
                        // TODO: Color edge path to file
                    }
                    else
                    {
                        i++;
                        // TODO: Color dead end node edge
                    }
                }

                folders = Directory.GetDirectories(curDir);
                i = 0;
                n = folders.Length;
                while (i < n && !(found && !all))
                {
                    FolderName = Path.GetRelativePath(curDir, folders[i]);
                    //Console.WriteLine(FolderName);
                    dirQueue.Enqueue(folders[i]);

                    // TODO: Enqueue node & edge

                    i++;
                }
            }
        }

    }


}