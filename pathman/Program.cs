using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pathman
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                switch (args[0])
                {
                    case "/as":
                        Environment.SetEnvironmentVariable("Path",
                            Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine) + ";" + args[1],
                            EnvironmentVariableTarget.Machine);
                        break;
                    case "/rs":
                        string[] path = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine).Split(new char[] { ';' });
                        var keys = path.Select(t => { if (t != args[1]) return t; else return ""; }).ToArray();
                        string newpath = "";
                        foreach (var key in keys)
                        {
                            if (key != "")
                                newpath += key + ";";
                        }
                        Environment.SetEnvironmentVariable("Path", newpath, EnvironmentVariableTarget.Machine);
                        break;
                    case "/2017":
                        string jsonPath = @"C:\ProgramData\Microsoft\VisualStudio\Packages\_Instances\5e3ad804\state.json";
                        string installPath = args[1].Replace(@"\", @"\\");
                        string text = File.ReadAllText(jsonPath);
                        text = text.Replace(@"C:\\Program Files\\Microsoft Visual Studio 15.0", installPath);
                        File.WriteAllText(jsonPath, text);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
