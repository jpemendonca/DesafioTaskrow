using DesafioTaskrow.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace DesafioTaskrow.Domain.Models;

public class Log : Entidades.Log
{
    public static void Setup(ModelBuilder builder)
    {
        builder.Entity<Log>().ToTable("Logs");
        builder.Entity<Log>().HasKey(x => x.Id);
        builder.Entity<Log>().HasIndex(x => x.Id);
        builder.Entity<Log>().Property(x => x.Id).ValueGeneratedOnAdd();
    }
}