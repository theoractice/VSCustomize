using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using VSC.Shared;

namespace RegIssPostProc
{
    class Program
    {
        static void Main(string[] args)
        {
            string vsVersion = SharedFunc.vsVersion;

            using (FileStream fs = new FileStream("reg.iss", FileMode.Open))
            using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gb2312")))
            using (FileStream nfs = new FileStream("newreg.iss", FileMode.Create))
            using (StreamWriter sw = new StreamWriter(nfs, Encoding.UTF8))
            {
                do
                {
                    string line = sr.ReadLine();
                    if (line.Contains("[Code]")) break;
                    if (!SharedFunc.isValidPath(line)) continue;

                    line = SharedFunc.pathReplace(vsVersion, line);

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

                        if(newline.Contains("Wow6432Node"))
                        {
                            newline = newline + "; Check: IsWin64;";
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
    }
}
