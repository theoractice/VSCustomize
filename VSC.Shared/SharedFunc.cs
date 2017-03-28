using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace VSC.Shared
{
    public class SharedFunc
    {
        public static string vsName = "2013";
        public static string vsVersion = "12.0";
        public static string programFiles = "Program Files";

        public static bool isValidPath(string path)
        {
            //if (path.ToLower().Contains("x64"))
            //    return false;
            //if (path.ToLower().Contains("ia64"))
            //    return false;
            //if (path.ToLower().Contains("win64"))
            //    return false;
            //if (path.ToLower().Contains("amd64"))
            //    return false;
            //if (path.ToLower().Contains("silverlight"))
            //    return false;
            //if (path.ToLower().Contains("microsoft.expression"))
            //    return false;
            //if (path.ToLower().Contains("blend"))
            //{
            //    int idx = path.ToLower().IndexOf("blend");
            //    return char.IsLetter(path[idx - 1]) && char.IsLetter(path[idx + 3]);
            //}
            if (path.ToLower().Contains("vmware")) return false;
            if (path.ToLower().Contains("thinprint")) return false;
            if (path.ToLower().Contains("group policy")) return false;

            if (path.ToLower().Contains("windows phone")) return false;
            if (path.ToLower().Contains("winrt")) return false;
            if (path.ToLower().Contains("setupcache")) return false;
            if (path.ToLower().Contains("arm"))
            {
                int idx = path.ToLower().IndexOf("arm");
                return char.IsLetter(path[idx - 1]) && char.IsLetter(path[idx + 3]);
            }

            //if (path.Contains("ProgramData\\Microsoft\\Windows\\Start Menu\\Programs")) return false;
            if (path.Contains("ProgramData\\Package Cache")) return false;
            if (path.Contains("C:\\Users")) return false;
            if (path.Contains("Windows\\Installer")) return false;
            if (path.Contains("Windows\\Microsoft.NET\\Framework\\v4.0.30319\\SetupCache")) return false;
            if (path.Contains("Windows\\Panther")) return false;
            if (path.Contains("Windows\\rescache")) return false;
            if (path.Contains("Windows\\ServiceProfiles")) return false;

            if (path.Contains("SOFTWARE\\Classes\\Installer")) return false;
            if (path.Contains("SOFTWARE\\Microsoft\\RADAR")) return false;
            if (path.Contains("SOFTWARE\\Microsoft\\WBEM")) return false;
            if (path.Contains("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Installer")) return false;
            if (path.Contains("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall")) return false;
            if (path.Contains("SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Installer")) return false;
            if (path.Contains("SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall")) return false;
            if (path.Contains("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\NetworkList")) return false;
            if (path.Contains("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Print")) return false;
            if (path.Contains("SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment\\Path")) return false;
            if (path.Contains("SYSTEM\\CurrentControlSet\\Control\\Class")) return false;
            if (path.Contains("SYSTEM\\CurrentControlSet\\Control\\Lsa")) return false;
            if (path.Contains("SYSTEM\\CurrentControlSet\\Control\\WMI\\Autologger")) return false;
            if (path.Contains("Root: HKU")) return false;
            /*
HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Installer
HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WBEM
HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Installer
HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall
HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\NetworkList
HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Print
HKEY_USERS
HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment\Path 删除并利用脚本添加
HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet 只保留control和services
            */

            return true;
        }

        public static string pathReplace(string vsVersion, string line)
        {
            line = line.Replace("Sky123.Org", "Administrator");
            line = line.Replace("C:\\" + programFiles + "\\Microsoft Visual Studio " + vsVersion, "{src}");
            line = line.Replace("C:/" + programFiles + "/Microsoft Visual Studio " + vsVersion, "{src}");
            line = line.Replace("C:\\" + programFiles + "\\Common Files", "{cf}");
            line = line.Replace("C:/" + programFiles + "/Common Files", "{cf}");
            line = line.Replace("C:\\" + programFiles + "", "{pf}");
            line = line.Replace("C:/" + programFiles + "", "{pf}");
            line = line.Replace("C:\\Windows\\System32", "{sys}");
            line = line.Replace("C:/Windows/System32", "{sys}");
            line = line.Replace("C:\\Windows", "{win}");
            line = line.Replace("C:/Windows", "{win}");
            line = line.Replace("C:\\ProgramData", "{commonappdata}");
            line = line.Replace("C:/ProgramData", "{commonappdata}");
            return line;
        }
    }
}
