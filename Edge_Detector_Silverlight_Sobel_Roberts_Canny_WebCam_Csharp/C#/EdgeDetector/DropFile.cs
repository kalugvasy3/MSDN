using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace TransformMethods
{
    public class DropFile
    {
        public WriteableBitmap CreateImage(FileInfo dropFile, string fullPath) {
            if (Application.Current.HasElevatedPermissions)   // Application.Current.IsRunningOutOfBrowser &&
            {
                // msdn.microsoft.com/en-us/library/system.io.filestream(v=vs.95).aspx
                string strFullPath;
                if (dropFile.Extension.ToLower() == ".jpg" || dropFile.Extension.ToLower() == ".png")
                {
                    strFullPath = fullPath + dropFile.Name;  // @"C:\Users\vkalugin\Pictures\"
                }
                else {
                    return null;
                }

                FileStream fileStream = new FileStream(strFullPath, FileMode.Open, FileAccess.Read, FileShare.Read);

                WriteableBitmap bitmapImage = new WriteableBitmap(0, 0);
                bitmapImage.SetSource(fileStream);
                return bitmapImage;
            }
            return null;
        }

        //http://www.codeproject.com/Articles/45480/Silverlight-4-How-to-Drag-and-Drop-External-Files

        public WriteableBitmap CreateImage(FileInfo dropFile)
        {
            if (Application.Current.HasElevatedPermissions)   // Application.Current.IsRunningOutOfBrowser &&
            {
                // msdn.microsoft.com/en-us/library/system.io.filestream(v=vs.95).aspx
                string strFullPath;
                if (dropFile.Extension.ToLower() == ".jpg" || dropFile.Extension.ToLower() == ".png")
                {
                    strFullPath = dropFile.FullName;   
                }
                else
                {
                    return null;
                }

                FileStream fileStream = new FileStream(strFullPath, FileMode.Open, FileAccess.Read, FileShare.Read);

                WriteableBitmap bitmapImage = new WriteableBitmap(0,0);
                bitmapImage.SetSource(fileStream);
                return bitmapImage;
            }
            return null;
        }


    }
}