using System.IO;
using System.Collections.Generic;

namespace FolderCrawler
{
    /**
     * <summary>
     *  <c>Dfs</c> class is a searcher class to search files
     *  in a given directory using depth-first search algorithm.
     * </summary>
     */
    public class Dfs
    {
        private bool _found;
        private readonly GraphContext _graphContext;
        private readonly List<string> _result;

        /**
         * <summary>
         *  Creates a new <c>Dfs</c> object with a given
         *  <c>GraphContext</c> object to visualize the
         *  search.
         * </summary>
         */
        public Dfs(ref GraphContext context)
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
            _graphContext.AddNode(dir, dir);
            _graphContext.UpdateView();

            RecursiveSearch(dir, fileToFind, exhaustive);

            _result.ForEach(
                filePath =>
                {
                    _graphContext.ColorizePath(filePath, ColorPalette.BLUE);
                });
        }

        /**
         * <summary>
         *  Recursively searches for a specific file name in a given
         *  directory.
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
        private void RecursiveSearch(
            string dir,
            string fileToFind,
            bool exhaustive
            )
        {

            if (!_found || exhaustive)
            {

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

