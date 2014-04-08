using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class EventLogger
    {
        public static string EventLogSourceName = Properties.Settings.Default.NazivServisa;

        // Info
        public static void WriteEventInfo(string message)
        {
            OtseciVisak(ref message);
            System.Diagnostics.EventLog.WriteEntry(EventLogSourceName, message, System.Diagnostics.EventLogEntryType.Information);
        }

        // Warning
        public static void WriteEventWarning(string message)
        {
            OtseciVisak(ref message);
            System.Diagnostics.EventLog.WriteEntry(EventLogSourceName, message, System.Diagnostics.EventLogEntryType.Warning);
        }

        // Error
        public static void WriteEventError(string message)
        {
            try
            {
                OtseciVisak(ref message);
                System.Diagnostics.EventLog.WriteEntry(EventLogSourceName, message, System.Diagnostics.EventLogEntryType.Error);
            }
            catch
            {
                // nemam vise gde da logujem!
            }
        }

        public static void WriteEventError(string message, Exception ex)
        {
            string exMessage;
            string exGetType;
            string trace;
            string wholeMessage;
            if (ex != null)
            {
                exMessage = ex.Message;
                exGetType = ex.GetType().ToString();
                trace = (new System.Diagnostics.StackTrace(ex)).ToString();
                wholeMessage = string.Format("{0}\r\nException message: {1}\r\nException type: {2}\r\n{3}",
                    message, exMessage, exGetType, trace);
            }
            else
            {
                exMessage = string.Empty;
                exGetType = string.Empty;
                trace = string.Empty;
                wholeMessage = message;
            }
            WriteEventError(wholeMessage);
        }

        private static bool DuzinaPrekoracena(ref string poruka)
        {
            return poruka.Length > Properties.Settings.Default.MaxVelicinaEventLoggera;
        }

        private static void OtseciVisak(ref string poruka)
        {
            if (DuzinaPrekoracena(ref poruka))
                poruka = poruka.Substring(0, Properties.Settings.Default.MaxVelicinaEventLoggera);
        }
    }

}
