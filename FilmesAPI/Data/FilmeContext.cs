using FilmesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmesAPI.Data;

public class FilmeContext : DbContext

{
    // Digitando ctor tab + tab ele cria o construtor automaticamente
    public FilmeContext(DbContextOptions<FilmeContext> opts) : base(opts)
    {

    }

    public DbSet<Filme> Filmes { get; set; }
}
