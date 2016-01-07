#region

using System.IO;
using System.Runtime.InteropServices;

#endregion

namespace TfsCommunity.Collaboration.Skype.Helpers
{
    public static class FileManagement
    {
        /// <summary>
        ///   Source: http://stackoverflow.com/questions/1304/how-to-check-for-file-lock-in-c
        /// </summary>
        /// <param name="exception"> </param>
        /// <returns> </returns>
        public static bool IsFileLocked(IOException exception)
        {
            int errorCode = Marshal.GetHRForException(exception) & ((1 << 16) - 1);
            return errorCode == 32 || errorCode == 33;
        }

        public static bool IsZeroSizeFile(string filename)
        {
            if (File.Exists(filename) && (new FileInfo(filename).Length == 0))
            {
                return true;
            }
            return false;
        }

        public static bool DeleteFile(string filename)
        {
            try
            {
                File.Delete(filename);
                return true;
            }
            catch (IOException ex)
            {
                if (IsFileLocked(ex))
                {
                    Logger.Write("Avatar file is locked and couldn't be deleted.");
                    Logger.Write(string.Format("Check file: {0}", filename));
                }
                Logger.WriteExceptionDetails(Resources.Resources.ProviderName, ex);
            }
            return false;
        }
    }
}