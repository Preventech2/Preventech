using System;
using Microsoft.EntityFrameworkCore;
using Preventech.Core.Models;

namespace Preventech.Core.DatabaseContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Equipamento> Equipamentos { get; set; }

    // Agora não precisamos do OnModelCreating porque usamos Data Annotations!
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     // Configurações automáticas via Data Annotations
    // }
}

