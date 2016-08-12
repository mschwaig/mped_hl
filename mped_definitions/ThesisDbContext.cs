﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace at.mschwaig.mped.definitions
{
    public class ThesisDbContext : DbContext
    {
        public ThesisDbContext() : base() { }

        public DbSet<GeneratedProblem> Problems { get; set; }

        // public DbSet<Solution> Solutions { get; set; }
    }
}