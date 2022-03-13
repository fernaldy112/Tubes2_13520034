using System;
using Microsoft.Msagl.Drawing;

namespace FolderCrawler
{
    public abstract class ColorPalette
    {
        public static readonly Color RED = new(Byte.MaxValue, 0, 0);
        public static readonly Color BLUE = new(0, 0xb0, Byte.MaxValue);
    }
}
