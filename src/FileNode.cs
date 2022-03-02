using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Folder_Crawling_WPF
{
    class FileNode
    {
        FileNode parentDirectory;
        List<FileNode> files;
    }
}
