#region

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

#endregion

namespace TfsCommunity.Collaboration.Skype.Helpers
{
    public static class Logger
    {
        public static void Write(string message, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
        {
            Trace.WriteLine(string.Format("Providername:{0} Membername:{3} Message:{4} File:{1} Line:{2} ", Resources.Resources.ProviderName, file, line, member, message));
        }

        public static void WriteExceptionDetails(string source, Exception ex, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
        {
            Write(string.Format("Providername:{0} Membername:{3} Exception was thrown at {4} File:{1} Line:{2}", Resources.Resources.ProviderName, file, line, member, source));
            Write(string.Format("Providername:{0} Membername:{3} Exception message:{4}  File:{1} Line:{2}", Resources.Resources.ProviderName, file, line, member, ex.Message));
            Write(string.Format("Providername:{0} Membername:{3} Exception stacktrace:{4}  File:{1} Line:{2}", Resources.Resources.ProviderName, file, line, member, ex.StackTrace));
        }
    }
}