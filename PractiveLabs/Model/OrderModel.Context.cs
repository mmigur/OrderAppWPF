﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PractiveLabs.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Practice_421_MigurEntities1 : DbContext
    {
        public Practice_421_MigurEntities1()
            : base("name=Practice_421_MigurEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        private static Practice_421_MigurEntities1 _context;

        public static Practice_421_MigurEntities1 GetContext() 
        {
            if (_context == null)
            {
                _context = new Practice_421_MigurEntities1();
            }
            return _context;
        }

        public virtual DbSet<category> category { get; set; }
        public virtual DbSet<order> order { get; set; }
        public virtual DbSet<product> product { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<user> user { get; set; }
    }
}
