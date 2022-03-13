using System;
using Microsoft.Msagl.Drawing;

namespace FolderCrawler
{
    public abstract class ColorPalette
    {
        public static readonly Color RED = new Color(Byte.MaxValue, 0, 0);
        public static readonly Color BLUE = new Color(0, 0xb0, Byte.MaxValue);
    }
}
