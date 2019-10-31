using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyCheck
{
    public class DependencyVulnerabilityDB
    {
        public int id { get; set; }
        public List<VulnerabilityDB> vulnerabilityDBs { get; set; }

        public DependencyDB dependency { get; set; }

        public DateTime timeSpan { get; set; }
    }

    public class VulnerabilityDB
    {
        public string name { get; set; }
        public double num1 { get; set; }
        public double num2 { get; set; }
        public double num3 { get; set; }
        public double num4 { get; set; }
        public double rezult { get; set; }
        public string description { get; set; }
    }

    public class DependencyDB
    {
        public string name { get; set; }
        public string fileName { get; set; }
        public string filePath { get; set; }
    }
}
