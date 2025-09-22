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
    public DbSet<Usuario> Usuarios { get; set; }

    public DbSet<OrdemServico> OrdensServico { get; set; }
    
    public DbSet<Localizacao> Localizacoes { get; set; }

    public DbSet<Peca> Pecas { get; set; }

    // ================ existe a chance de que os relacionamentos não sejam descobertos ================
    // Agora não precisamos do OnModelCreating porque usamos Data Annotations!
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     // Configurações automáticas via Data Annotations
    // }
}

