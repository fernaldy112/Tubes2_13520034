using System;
using System.Collections.Generic;
using System.IO;


namespace FolderCrawler
{
    /**
     * <summary>
     *  <c>Bfs</c> class is a searcher class to search files
     *  in a given directory using breadth-first search algorithm.
     * </summary>
     */
    public class Bfs
    {
        private readonly GraphContext _graphContext;
        private readonly List<string> _result;

        /**
         * <summary>
         *  Creates a new <c>Bfs</c> object with a given
         *  <c>GraphContext</c> object to visualize the
         *  search.
         * </summary>
         */
        public Bfs(ref GraphContext context)
        {
            _graphContext = context;
            _result = new List<string>();
        }

        /**
         * <summary>
         *  Searches for a specific file name in a given directory.
         *  Searching process can be exhaustive, if necessary.
         * </summary>
         *
         * <param name="dir">
         *  searching process starting directory
         * </param>
         *
         * <param name="fileToFind">
         *  file name to find in the directory
         * </param>
         *
         * <param name="exhaustive">
         *  flag indicating whether the search should be exhaustive
         * </param>
         */
        public void Search(string dir, string fileToFind, bool exhaustive)
        {
            Queue<string> dirQueue = new();
            bool found = false;

            dirQueue.Enqueue(dir);
            while (dirQueue.Count != 0 && !(found && !exhaustive))
            {
                string currentDir = dirQueue.Dequeue();
                _graphContext.AddNode(currentDir,
                    new DirectoryInfo(currentDir).Name);
                string[] files = Directory.GetFiles(currentDir, "*");

                foreach (string file in files)
                {
                    _graphContext.AddNode(file,
                        new DirectoryInfo(file).Name);
                    _graphContext.AddEdge(currentDir, file);
                }

                int i = 0;
                int n = files.Length;
                while (i < n && !(found && !exhaustive))
                {
                    string fileName = Path.GetRelativePath(currentDir, files[i]);
                    if (fileName == fileToFind)
                    {
                        found = true;
                        _graphContext.ColorizeEdge(files[i], ColorPalette.BLUE);
                        _result.Add(files[i]);
                    }
                    else
                    {
                        _graphContext.ColorizeEdge(files[i], ColorPalette.RED);
                    }
                    i++;

                }

                string[] folders = Directory.GetDirectories(currentDir);
                i = 0;
                n = folders.Length;
                while (i < n && !(found && !exhaustive))
                {
                    dirQueue.Enqueue(folders[i]);

                    _graphContext.AddNode(folders[i],
                        new DirectoryInfo(folders[i]).Name);
                    _graphContext.AddEdge(currentDir, folders[i]);

                    _graphContext.ColorizeEdge(folders[i], ColorPalette.RED);

                    i++;
                }
            }

            _result.ForEach(
                filePath =>
                {
                    _graphContext.ColorizePath(filePath, ColorPalette.BLUE);
                });
        }

        /**
         * <summary>
         *  Retrieves the searching result. Should only be called
         *  after calling <c>Search()</c>.
         * </summary>
         *
         * <returns>
         *  a list of path to the searched file to find
         * </returns>
         */
        public List<string> Result()
        {
            return _result;
        }

    }


}