using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace lnk2iss
{
	class Program
	{
		static void Main(string[] args)
		{
			string[] files = Directory.GetFiles(".", "*.lnk", SearchOption.AllDirectories);

			using (FileStream nfs = new FileStream("icons.iss", FileMode.Create))
			using (StreamWriter sw = new StreamWriter(nfs, Encoding.UTF8))
			{
				sw.WriteLine("[Icons]");

				foreach (string file in files)
				{
					ProcessStartInfo info = new ProcessStartInfo(
                        "lnk_parser_cmd.exe"
                        , string.Format("-w \"{0}\"", file));
					info.CreateNoWindow = true;
					info.WindowStyle = ProcessWindowStyle.Hidden;
					Process proc = new Process();
					proc.StartInfo = info;
					proc.Start();
					proc.WaitForExit();

					string csvFile = Directory.GetFiles(".", "*.htm", SearchOption.TopDirectoryOnly)[0];
					using (FileStream fs = new FileStream(csvFile, FileMode.Open))
					using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
					{
						string html = sr.ReadLine();
						string comment = "";
						string arguments = "";
						string path = "";
						string workingdir = "";
						MatchCollection mc;

						string name = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));

						mc = Regex.Matches(html, "Comment \\(UNICODE\\)\\</div\\>\\<div id=\\\"d\\\"\\>(.*?)\\</div\\>");
						if (mc.Count != 0)
						{
							comment = mc[0].Groups[1].Value;
						}

						mc = Regex.Matches(html, "Arguments \\(UNICODE\\)\\</div\\>\\<div id=\\\"d\\\"\\>(.*?)\\</div\\>");
						if (mc.Count != 0)
						{
							arguments = mc[0].Groups[1].Value;
						}

						mc = Regex.Matches(html, "Local path \\(ASCII\\)\\</div\\>\\<div id=\\\"d\\\"\\>(.*?)\\</div\\>");
						if (mc.Count != 0)
						{
							path = mc[0].Groups[1].Value;
						}

						path = path.Replace(@"C:\Windows\System32\cmd.exe", "%comspec%");
						mc = Regex.Matches(html, "Working Directory \\(UNICODE\\)\\</div\\>\\<div id=\\\"d\\\"\\>(.*?)\\</div\\>");
						if (mc.Count != 0)
						{
							workingdir = mc[0].Groups[1].Value;
						}

						string line = string.Format(@"Name: ""{0}""; FileName: ""{1}""; Parameters: ""{2}""; WorkingDir: ""{3}""; Comment: ""{4}""; "
                        , name.Replace(@".\ProgramData\Microsoft\Windows\Start Menu\Programs", "{commonprograms}")
                        .Replace(@".\Visual Studio 2015", "{src}")
						.Replace(@".\Program Files\Microsoft Visual Studio 14.0", "{src}")
                        , path
                        , arguments.Replace("\"", "\"\"")
                        , workingdir
                        , comment.Replace("\"", "\"\"")
                        );

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

                        sw.WriteLine(line);
                    }
                    File.Delete(csvFile);
                }
            }
        }
    }
}
