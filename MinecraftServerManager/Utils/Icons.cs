using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MinecraftServerManager.Utils
{
    public class Icons
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        private class Win32
        {
            public const uint SHGFI_ICON = 0x100;
            public const uint SHGFI_LARGEICON = 0x0; // 'Large icon
            public const uint SHGFI_SMALLICON = 0x1; // 'Small icon

            [DllImport("shell32.dll")]
            public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
        }

        public static Bitmap GetSmallFileIcon(string filename)
        {
            SHFILEINFO shinfo = new SHFILEINFO();
            Win32.SHGetFileInfo(filename, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_ICON | Win32.SHGFI_SMALLICON);
            Icon i = Icon.FromHandle(shinfo.hIcon);
            Bitmap b = i.ToBitmap();
            return b;
        }

        public static Bitmap GetTransparentIcon(Icon i)
        {
            return i.ToBitmap();
        }

        public static Bitmap GetLargeFileIcon(string filename)
        {
            SHFILEINFO shinfo = new SHFILEINFO();
            Win32.SHGetFileInfo(filename, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_ICON | Win32.SHGFI_LARGEICON);
            Icon i = Icon.FromHandle(shinfo.hIcon);
            Bitmap b = i.ToBitmap();
            return b;
        }

        public static Bitmap AddColor(Bitmap image, Color color)
        {
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Width; y++)
                {
                    image.SetPixel(x, y, Color.FromArgb(image.GetPixel(x, y).A, color.R, color.G, color.B));
                }
            }
            return image;
        }

        public static Bitmap AddColor(Icon image, Color color)
        {
            return AddColor(image.ToBitmap(), color);
        }
    }
}
