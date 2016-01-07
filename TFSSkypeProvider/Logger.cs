using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SKYPE4COMLib;

namespace TfsCommunity.Collaboration.Skype
{
    public static class Logger
    {
        public static void Write(string message)
        {
            Trace.WriteLine(string.Format("{0}",message),Properties.Resources.ProviderName);
        }

        public static void WriteExceptionDetails(string source, Exception ex)
        {
            Write(string.Format("Exception was thrown at {0}",source));
            Write(string.Format("Exception message:{0}", ex.Message));
            Write(string.Format("Exception stacktrace:{0}", ex.StackTrace));
        }
    }
}
