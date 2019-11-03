using DependencyCheck.Controller;
using DependencyCheck.Data;
using DependencyCheck.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace DependencyCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            IdentifyVulnerabilities identifyVulnerabilities = new IdentifyVulnerabilities();
            List<DependencyVulnerabilityDB> dependencyVulnerabilityDBs = identifyVulnerabilities.OWASPDependencyCheck("index", "C:\\Users\\profe\\Desktop\\index", "JSON");

            ContrDepenVulnDB cdv = new ContrDepenVulnDB();
            cdv.SaveList(dependencyVulnerabilityDBs);
            Console.Read();
        }
    }
}
