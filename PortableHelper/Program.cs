using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using VSC.Shared;

namespace PortableHelper
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string vsName = SharedFunc.vsName;

            using (FileStream fs = new FileStream("VS"+ vsName + " - 修改.txt", FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            {
                string curDir = null;

                Stopwatch sw = new Stopwatch();
                sw.Start();
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
                            if (!SharedFunc.isValidPath(path))
                            {
                                continue;
                            }
                            Directory.CreateDirectory(Path.GetDirectoryName(path.Replace("C:", "Z:\\E\\VS" + vsName)));
                            File.Copy(path, path.Replace("C:", "Z:\\E\\VS" + vsName), true);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(path);
                            Console.WriteLine(e.Message.ToString());
                        }
                    }
                }
                while (!sr.EndOfStream);

                Console.WriteLine("Time: " + sw.Elapsed);
                Console.ReadLine();
            }
        }
    }
}