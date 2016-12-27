using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PortableHelper
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (FileStream fs = new FileStream("VS2015 - 修改.txt", FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            {
                string curDir = "";
                //for (int i = 0; i < 20000; i++)
                //    sr.ReadLine();
                do
                {
                    string line = sr.ReadLine();
                    if (line.Contains("(文件夹)"))
                    {
                        curDir = line.Replace("    (+)(文件夹) ", "")
                            .Replace("    (文件夹) ", "");
                    }
                    //else if (line.Contains("(文件)"))
                    //{
                    //    line = line.Replace("    (+)(文件) ", "")
                    //        .Replace("    (*)(文件) ", "");
                    else if (line.Contains("    (+)(文件) "))
                    {
                        line = line.Replace("    (+)(文件) ", "");
                        line = line.Substring(0, line.LastIndexOf('=') - 1).Trim();
                        string path = Path.Combine(curDir, line);
                        try
                        {
                            if (!isValidPath(path))
                            {
                                continue;
                            }
                            Directory.CreateDirectory(Path.GetDirectoryName(path.Replace("C:", "Z:\\E\\VS")));
                            File.Copy(path, path.Replace("C:", "Z:\\E\\VS"), true);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(path);
                            Console.WriteLine(e.Message.ToString());
                        }
                    }
                }
                while (!sr.EndOfStream);
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
            if (path.ToLower().Contains("windows phone"))
            {
                return false;
            }
            if (path.ToLower().Contains("winrt"))
            {
                return false;
            }
            if (path.ToLower().Contains("arm"))
            {
                int idx = path.ToLower().IndexOf("arm");
                return char.IsLetter(path[idx - 1]) && char.IsLetter(path[idx + 3]);
            }
            return true;
        }
    }
}