using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DependencyCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            // dependency-check.bat --project "index" --scan "C:\Users\profe\Desktop\index" --format "JSON"
            string strCmdText = " cd " + System.AppContext.BaseDirectory + " ";

            //executeCommand(strCmdText,true, false);
            //executeCommand("dependency-check.bat --project \"index\" --scan \"C: \\Users\\profe\\Desktop\\index\" --format \"JSON\"", true, false);

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.StartInfo = startInfo;
            startInfo.FileName = "cmd.exe";

            //startInfo.Arguments = " cd " + System.AppContext.BaseDirectory + " ";

            startInfo.Arguments = "/c C:\\Users\\profe\\source\\repos\\DependencyCheck\\resoures\\dependency-check\\bin\\dependency-check.bat --project \"index\" --scan \"C:\\Users\\profe\\Desktop\\index\" --format \"JSON\"";
            process.Start();
            Console.WriteLine(startInfo.Arguments);

            startInfo.Arguments = "/C dir";
            process.Start();
            Console.WriteLine(startInfo.Arguments);


            string msg = File.ReadAllText(@"C:\Users\profe\source\repos\DependencyCheck\resoures\dependency-check\bin\dependency-check-report.json");
            //convert to json instance
            JObject obj = JObject.Parse(msg);
            //return event array
            var token = (JArray)obj.SelectToken("dependencies");

            Welcome test = JsonConvert.DeserializeObject<Welcome>(msg);

            List<DependencyVulnerabilityDB> dependencyVulnerabilityDBs = (test.Dependencies.Where(x => x.Vulnerabilities != null)
                .Select((x) => new DependencyVulnerabilityDB
                {
                    dependency = (new DependencyDB { fileName = x.FileName, filePath = x.FilePath, name = x.Packages.First().Id }),
                    vulnerabilityDBs = x.Vulnerabilities.Select(x => new VulnerabilityDB
                    { name = x.Name, num1 = x.Cvssv3.BaseScore, description = x.Description }).ToList(),
                    timeSpan = DateTime.Now
                })).ToList();

            Console.Read();
        }

        internal static void executeCommand(string command, bool waitForExit = true, bool hideWindow = true, bool runAsAdministrator = false)
        {
            System.Diagnostics.ProcessStartInfo psi =
            new System.Diagnostics.ProcessStartInfo("cmd", "/C " + command);

            if (hideWindow)
            {
                psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            }

            if (runAsAdministrator)
            {
                psi.Verb = "runas";
            }

            if (waitForExit)
            {
                System.Diagnostics.Process.Start(psi).WaitForExit();
            }
            else
            {
                System.Diagnostics.Process.Start(psi);
            }
        }
    }
}
