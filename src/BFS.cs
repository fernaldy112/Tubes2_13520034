using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;


namespace BFS
{

    public class BFSClass
    {

        public static void Main()
        {
            new BFSClass().BFS("abc.pdf", false);
        }

        public void BFS(string file_yang_dicari, bool all)
        {
            Queue<string> dirQueue = new Queue<string>();
            string dir, curDir, FileName, FolderName;
            string[] files, folders;
            bool found = false;
            int i, n;

            Console.Write("Input directory root: ");
            dir = Console.ReadLine();
            Console.WriteLine(dir);
            dirQueue.Enqueue(dir);
            while (!(dirQueue.Count == 0) && !(found && !all))
            {
                curDir = dirQueue.Dequeue();
                
                files = Directory.GetFiles(curDir, "*");
                i = 0;
                n = files.Length;
                while (i<n && !(found&&!all))
                {
                    FileName = Path.GetRelativePath(curDir, files[i]);
                    Console.WriteLine(FileName);
                    if (FileName == file_yang_dicari)
                    {
                        found = true;
                    }
                    else
                    {
                        i++;
                    }
                }

                folders = Directory.GetDirectories(curDir);
                i = 0;
                n = folders.Length;
                while (i < n && !(found && !all))
                {
                    FolderName = Path.GetRelativePath(curDir, folders[i]);
                    Console.WriteLine(FolderName);
                    dirQueue.Enqueue(folders[i]);
                    i++;
                }
            }
        }

    }

    
}