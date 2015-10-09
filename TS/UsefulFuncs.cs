using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
namespace TS
{
    class UsefulFuncs
    {
        static Boolean Is64Bit()
        {

            return Marshal.SizeOf(typeof(IntPtr)) == 8;

        }

        public unsafe static char* my_strcpy(char* destination, int buffer, char* source)
        {
            char* p = destination;
            int x = 0;
            while (*source != '\0' && x < buffer)
            {
                *p++ = *source++;
                x++;
            }
            *p = '\0';
            return destination;
        }

        private static byte[] convertLPSTR(string _string)
        {
            List<byte> lpstr = new List<byte>();
            foreach (char c in _string.ToCharArray())
            {
                lpstr.Add(Convert.ToByte(c));
            }
            lpstr.Add(Convert.ToByte('\0'));

            return lpstr.ToArray();
        }

        public static unsafe PluginMenuItem* createMenuItem(PluginMenuType type, int id, String text, String icon)
        {

            PluginMenuItem* menuItem = (PluginMenuItem*)Marshal.AllocHGlobal(sizeof(PluginMenuItem));
            menuItem->type = type;
            menuItem->id = id;


            IntPtr i_ptr = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(icon);
            void* i_strPtr = i_ptr.ToPointer();
            char* i_cptr = (char*)i_strPtr;
            *menuItem->icon = *my_strcpy(menuItem->icon, 128, i_cptr);

            IntPtr t_ptr = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(text);
            void* t_strPtr = t_ptr.ToPointer();
            char* t_cptr = (char*)t_strPtr;
            my_strcpy(menuItem->text, 128, t_cptr);




            return menuItem;


        }

        public static unsafe uint GetChannelVariableAsStr(ulong serverConnectionHandlerID, ulong channelID, ChannelProperties property, ref string ptr)
        {
            var funcs = TSPlugin.Instance.Functions;
            IntPtr refs = IntPtr.Zero;
            uint reter =
                (funcs.getChannelVariableAsString(serverConnectionHandlerID, channelID, new IntPtr((int) property),
                    ref refs));
            ptr = Marshal.PtrToStringAnsi(refs);
            return reter;
        }

        public static unsafe Dictionary<string, ulong> GetAllChannelNamesAsDict(ulong serverConnectionHandlerID)
        {
            var funcs = TSPlugin.Instance.Functions;
            var dict = new Dictionary<string, ulong>();
            IntPtr res = IntPtr.Zero;
            if (funcs.getChannelList(serverConnectionHandlerID, ref res) != Errors.ERROR_ok)
            {
                funcs.logMessage("Failed", LogLevel.LogLevel_ERROR, "Plugin", serverConnectionHandlerID);
                return null;
            }
            ulong* ptr = (ulong*) res.ToPointer();
            for (ulong t = 0; ptr[t] != 0; t++)
            {
                var result = String.Empty;
                if (
                    (GetChannelVariableAsStr(serverConnectionHandlerID, ptr[t], ChannelProperties.CHANNEL_NAME,
                        ref result) != Errors.ERROR_ok))
                {
                    funcs.logMessage("Failed", LogLevel.LogLevel_ERROR, "Plugin", serverConnectionHandlerID);
                }
                if (result == null) return null;
                dict.Add(result, ptr[t]);
            }
            return dict;
        }
    }
}
