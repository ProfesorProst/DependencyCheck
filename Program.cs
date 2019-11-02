using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
                    { name = x.Name, num1 = x.Cvssv3.BaseScore, description = x.Description }).ToList()
                    ,   dateTime = DateTime.Now
                })).ToList();

            Console.WriteLine(dependencyVulnerabilityDBs.First().id);
            using (Context db = new Context())
            {
                
                /*
                var depvuln = (from depvul in dependencyVulnerabilityDBs
                              from depvulne in (db.dependencyVulnerabilityDBs).ToList()
                               where depvul.timeSpan != depvulne.timeSpan && depvul.dependency != depvulne.dependency
                              select depvulne).ToList();
                 */
                //var depvuln = db.dependencyVulnerabilityDBs.ToList();

                /*
                foreach (DependencyVulnerabilityDB dependencyVulnerabilityDB in dependencyVulnerabilityDBs)
                    db.dependencyVulnerabilityDBs.AddOrUpdate(dependencyVulnerabilityDB);
                */

                var depvulnDB = db.dependencyVulnerabilityDBs.ToList();
                var dependencies = (from dv in dependencyVulnerabilityDBs
                                    select dv.dependency).ToList();
                /*
                foreach(DependencyDB dependency in dependencies)
                try
                {
                    db.dependencyDBs.Add(dependency);
                    db.SaveChanges();
                }
                catch(Exception e) { Console.WriteLine(e.ToString()); }

                List<VulnerabilityDB> vulnerabilities = (from dv in dependencyVulnerabilityDBs
                                                            from vulns in dv.vulnerabilityDBs
                                                            select vulns).ToList();

                foreach (VulnerabilityDB vulns in vulnerabilities)
                    try
                {
                    db.vulnerabilityDBs.Add(vulns);
                    db.SaveChanges();
                }
                catch (Exception e) { Console.WriteLine(e.ToString()); }
                */
                foreach (DependencyVulnerabilityDB depvul in dependencyVulnerabilityDBs)
                    try
                {
                        ICollection<VulnerabilityDB> vulners = new List<VulnerabilityDB>();
                        foreach (VulnerabilityDB vul in depvul.vulnerabilityDBs)
                            vulners.Add(db.vulnerabilityDBs.ToList().Find(x => x.name == vul.name));

                        HashSet<string> diffids = new HashSet<string>(db.vulnerabilityDBs.ToList().Select(s => s.name));
                        var results = depvul.vulnerabilityDBs.Where(m => !diffids.Contains(m.name)).ToList();

                        depvul.vulnerabilityDBs.Clear();
                        foreach (VulnerabilityDB vul in results)
                            vulners.Add(vul);

                        foreach (VulnerabilityDB vul in vulners)
                            depvul.vulnerabilityDBs.Add(vul);


                        DependencyDB dp = db.dependencyDBs.ToList().Find(x => x.name == depvul.dependency.name && x.fileName == depvul.dependency.fileName);

                        //db.dependencyDBs.Attach(dp);
                        depvul.dependency = dp==null? depvul.dependency : dp;
                        //db.Entry(dp).State = dp.id == null ? EntityState.Added : EntityState.Modified;

                        //db.dependencyDBs.AddOrUpdate(dp);
                        //db.Entry(depvul.dependency).State = EntityState.Modified;
                        //db.dependencyVulnerabilityDBs.Add(depvul);
                        //db.SaveChanges();
                        //depvul.vulnerabilityDBs = null;
                        db.dependencyVulnerabilityDBs.AddOrUpdate(depvul);
                    db.SaveChanges();
                }
                catch (Exception e) 
                { 
                    Console.WriteLine(e.ToString());
                }









                var users = db.dependencyVulnerabilityDBs;
                Console.WriteLine("Список объектов:");
                foreach (DependencyVulnerabilityDB u in users)
                {
                    Console.WriteLine("{0}.{1} - {2}", u.id, u.dependency.name, u.vulnerabilityDBs);
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
