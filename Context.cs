    using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace DependencyCheck
{
    class Context : DbContext
    {
        public Context()
            : base("DbConnection")
        { }

        public DbSet<DependencyVulnerabilityDB> dependencyVulnerabilityDBs { get; set; }
        public DbSet<DependencyDB> dependencyDBs { get; set; }
        public DbSet<VulnerabilityDB> vulnerabilityDBs { get; set; }
    }
}
