﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Plotnikov_PR_21_102_DocumentManager.Entity
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities1 : DbContext
    {
        public Entities1()
            : base("name=Entities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<customers> customers { get; set; }
        public DbSet<document_types> document_types { get; set; }
        public DbSet<documents> documents { get; set; }
        public DbSet<files> files { get; set; }
        public DbSet<posts> posts { get; set; }
        public DbSet<project_workers> project_workers { get; set; }
        public DbSet<projects> projects { get; set; }
        public DbSet<roles> roles { get; set; }
        public DbSet<test_case_results> test_case_results { get; set; }
        public DbSet<test_cases> test_cases { get; set; }
        public DbSet<workers> workers { get; set; }
    }
}