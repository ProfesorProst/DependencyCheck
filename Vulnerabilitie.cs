﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DependencyCheck
{

    public class DependencyVulnerabilityDB
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? id { get; set; }

        [Index("DependencyANDVulnerability", 1, IsUnique = true)]
        public DependencyDB dependency { get; set; }
        public virtual ICollection<VulnerabilityDB> vulnerabilityDBs { get; set; }
        public DependencyVulnerabilityDB()
        {
            this.vulnerabilityDBs = new HashSet<VulnerabilityDB>();
        }
        [Index("DependencyANDVulnerability", 2, IsUnique = true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? timeSpan { get; set; }
    }

    public class VulnerabilityDB
    {
        public VulnerabilityDB()
        {
            this.dependencyVulnerabilityDBs = new HashSet<DependencyVulnerabilityDB>();
        }
        public virtual ICollection<DependencyVulnerabilityDB> dependencyVulnerabilityDBs { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? id { get; set; }
        [MaxLength(128), Index(IsUnique = true)]
        public string? name { get; set; }
        public double? num1 { get; set; }
        public double? num2 { get; set; }
        public double? num3 { get; set; }
        public double? num4 { get; set; }
        public double? rezult { get; set; }
        public string? description { get; set; }
    }

    public class DependencyDB
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? id { get; set; }
        [MaxLength(128)]
        [Index("NameANDFileName", 1, IsUnique = true)]
        public string? name { get; set; }
        [MaxLength(128)]
        [Index("NameANDFileName", 2, IsUnique = true)]
        public string? fileName { get; set; }
        [MaxLength(256)]
        public string? filePath { get; set; }
    }
}
