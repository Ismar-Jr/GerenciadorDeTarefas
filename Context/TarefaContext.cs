using Microsoft.EntityFrameworkCore;
using GerenciadorDeTarefas.Entidades;
namespace GerenciadorDeTarefas.Context;

public class TarefaContext : DbContext
{
    public TarefaContext(DbContextOptions<TarefaContext> options) : base(options) 
    { 

    }  
    public DbSet<Tarefa> Tarefas { get; set; }


}