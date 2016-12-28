using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace RegIssPostProc
{
    class Program
    {
        static void Main(string[] args)
        {
            using (FileStream fs = new FileStream("reg.iss", FileMode.Open))
            using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gb2312")))
            using (FileStream nfs = new FileStream("newreg.iss", FileMode.Create))
            using (StreamWriter sw = new StreamWriter(nfs, Encoding.UTF8))
            {
                do
                {
                    string line = sr.ReadLine();
                    line = Regex.Replace(line, "Sky123.Org", "Administrator", RegexOptions.IgnoreCase);
                    line = Regex.Replace(line, "C:\\\\Program Files\\\\Microsoft Visual Studio 14.0", "{src}", RegexOptions.IgnoreCase);
                    line = Regex.Replace(line, "C:/Program Files/Microsoft Visual Studio 14.0", "{src}", RegexOptions.IgnoreCase);
                    line = Regex.Replace(line, "C:\\\\Program Files\\\\Common Files", "{cf}", RegexOptions.IgnoreCase);
                    line = Regex.Replace(line, "C:/Program Files/Common Files", "{cf}", RegexOptions.IgnoreCase);
                    line = Regex.Replace(line, "C:\\\\Program Files", "{pf}", RegexOptions.IgnoreCase);
                    line = Regex.Replace(line, "C:/Program Files", "{pf}", RegexOptions.IgnoreCase);
                    line = Regex.Replace(line, "C:\\\\Windows\\\\System32", "{sys}", RegexOptions.IgnoreCase);
                    line = Regex.Replace(line, "C:/Windows/System32", "{sys}", RegexOptions.IgnoreCase);
                    line = Regex.Replace(line, "C:\\\\Windows", "{win}", RegexOptions.IgnoreCase);
                    line = Regex.Replace(line, "C:/Windows", "{win}", RegexOptions.IgnoreCase);
                    line = Regex.Replace(line, "C:\\\\ProgramData", "{commonappdata}", RegexOptions.IgnoreCase);
                    line = Regex.Replace(line, "C:/ProgramData", "{commonappdata}", RegexOptions.IgnoreCase);

                    try
                    {
                        string strValueName = "";
                        string strValueData = "";
                        string beforeAll = "";
                        if (line.Contains(" ValueName: ") && line.Contains(" ValueData: "))
                        {
                            beforeAll = line.Split(
                               new string[] { " ValueName: " }, StringSplitOptions.None)[0];
                            strValueName = line.Split(
                                new string[] { " ValueName: " }, StringSplitOptions.None)[1]
                                .Split(
                                new string[] { " ValueData: " }, StringSplitOptions.None)[0]
                                .TrimEnd(new char[] { ';' });
                            strValueData = line.Split(
                                new string[] { " ValueData: " }, StringSplitOptions.None)[1]
                                .Split(
                                new string[] { " Flags: " }, StringSplitOptions.None)[0]
                                .TrimEnd(new char[] { ';' });
                        }
                        else if ((!line.Contains(" ValueName: ")) && line.Contains(" ValueData: "))
                        {
                            beforeAll = line.Split(
                               new string[] { " ValueData: " }, StringSplitOptions.None)[0];
                            strValueName = "";
                            strValueData = line.Split(
                                new string[] { " ValueData: " }, StringSplitOptions.None)[1]
                                .Split(
                                new string[] { " Flags: " }, StringSplitOptions.None)[0]
                                .TrimEnd(new char[] { ';' });
                        }
                        else if (line.Contains(" ValueName: ") && (!line.Contains(" ValueData: ")))
                        {
                            beforeAll = line.Split(
                               new string[] { " ValueName: " }, StringSplitOptions.None)[0];
                            strValueName = line.Split(
                                new string[] { " ValueName: " }, StringSplitOptions.None)[1]
                                .Split(
                                new string[] { " Flags: " }, StringSplitOptions.None)[0]
                                .TrimEnd(new char[] { ';' });
                            strValueData = "";
                        }
                        else if ((!line.Contains(" ValueName: ")) && (!line.Contains(" ValueData: ")))
                        {
                            throw new Exception();
                        }

                        string afterAll = line.Split(
                            new string[] { " Flags: " }, StringSplitOptions.None)[1];

                        if (strValueName.StartsWith(@"""")
                            && strValueName.EndsWith(@""""))
                        {
                            strValueName = strValueName.Substring(1, strValueName.Length - 2);
                        }

                        if (strValueData.StartsWith(@"""")
                            && strValueData.EndsWith(@""""))
                        {
                            strValueData = strValueData.Substring(1, strValueData.Length - 2);
                        }

                        if (strValueData.Contains("{break}"))
                        {
                            strValueData = strValueData.Replace("{break}", "{somethingthatwillneverappearinanyregfiles}");
                            strValueData = strValueData.Replace("{", "{{");
                            strValueData = strValueData.Replace("{{somethingthatwillneverappearinanyregfiles}", "{break}");
                        }

                        string newline = string.Format("{0} ValueName: \"{1}\"; ValueData: \"{2}\"; Flags: {3}"
                            , beforeAll
                            , strValueName
                            , strValueData
                            , afterAll);

                        if (newline.Contains(" SubKey: ") && newline.Contains(" ValueType: "))
                        {
                            newline = newline.Insert(newline.IndexOf("SubKey:") + 8, "\"");
                            newline = newline.Insert(newline.IndexOf("ValueType:") - 2, "\"");
                        }
                        sw.WriteLine(newline);
                    }
                    catch (Exception)
                    {
                        sw.WriteLine(line);
                    }
                }
                while (!sr.EndOfStream);
                Console.WriteLine("Completed.");
                Console.ReadLine();
            }
        }

        private static bool isValidPath(string path)
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
            if (path.ToLower().Contains("windows phone")) return false;
            if (path.ToLower().Contains("winrt")) return false;
            if (path.ToLower().Contains("arm"))
            {
                int idx = path.ToLower().IndexOf("arm");
                return char.IsLetter(path[idx - 1]) && char.IsLetter(path[idx + 3]);
            }
            return true;
        }
    }
}
