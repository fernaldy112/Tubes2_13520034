using System;
using Microsoft.Msagl.Drawing;

namespace FolderCrawler
{
    /**
     * <summary>
     *  <c>ColorPalette</c> class provides constants that
     *  hold Color data to color Msagl graph nodes or edges.
     * </summary>
     */
    public abstract class ColorPalette
    {
        public static readonly Color RED = new(Byte.MaxValue, 0, 0);
        public static readonly Color BLUE = new(0, 0xb0, Byte.MaxValue);
    }
}
