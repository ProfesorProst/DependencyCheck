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
            string baseDirectory = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
            // dependency-check.bat --project "index" --scan "C:\Users\profe\Desktop\index" --format "JSON"
            string strCmdText = baseDirectory + "resoures\\dependency-check\\bin\\dependency-check.bat " +
                "--project \"index\" --scan \"C:\\Users\\profe\\Desktop\\index\" --out "+ baseDirectory+" --format \"JSON\"";

            //executeCommand(strCmdText);
            Console.WriteLine("Done");
                   
            string msg = File.ReadAllText(baseDirectory + "dependency-check-report.json");
            JObject obj = JObject.Parse(msg);
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


            using (Context db = new Context())
            {
                db.dependencyVulnerabilityDBs.AddRange(dependencyVulnerabilityDBs);
                db.SaveChanges();


                var users = db.dependencyVulnerabilityDBs;
                Console.WriteLine("Список объектов:");
                foreach (DependencyVulnerabilityDB u in users)
                {
                    Console.WriteLine("{0}.{1} - {2}", u.id, u.dependency, u.vulnerabilityDBs);
                }
            }
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
