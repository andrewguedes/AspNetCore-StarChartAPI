﻿using System;
using Microsoft.EntityFrameworkCore;
using StarChart.Models;

namespace StarChart.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<CelestialObject> CelestialObjects { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        internal bool FirstOrDefault(int id)
        {
            throw new NotImplementedException();
        }
    }
}
