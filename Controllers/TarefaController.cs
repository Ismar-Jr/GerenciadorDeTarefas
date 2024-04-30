using System.Globalization;
using GerenciadorDeTarefas.Context;
using GerenciadorDeTarefas.Entidades;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GerenciadorDeTarefas.Controllers;

[ApiController]
[Route("[controller]")]
public class TarefaController : ControllerBase
{
    private readonly TarefaContext context;
    public TarefaController(TarefaContext context)
    {
        this.context = context;
    }
    
    [HttpPost]
    public IActionResult Create(Tarefa tarefa)
    {
        context.Add(tarefa);
        context.SaveChanges();
        return CreatedAtAction(nameof(ObterPorId), new {id = tarefa.Id}, tarefa);
    }
    
    [HttpGet("{id}")]
    public IActionResult ObterPorId(int id)
    {
        var tarefa = context.Tarefas.Find(id);
        if (tarefa == null)
            return NotFound();
        return Ok(tarefa);
    }
    
    [HttpGet("ObterPorTitulo")]
    public IActionResult ObterPorNome(string titulo)
    {
        var tarefas = context.Tarefas.Where(x => x.Titulo.Contains(titulo));
        return Ok(tarefas);
    }
    
    [HttpGet("ObterPorStatus")]
    public IActionResult ObterPorStatus(int status)
    {
        var tarefas = context.Tarefas.Where(x => x.Status == status);
        return Ok(tarefas);
    }

    
    [HttpGet("ObterPorData")]
    [SwaggerOperation(Summary = "Obtém tarefas pela data especificada no formato dd/MM/yyyy")]
    public IActionResult ObterPorData([FromQuery, SwaggerParameter(Description = "Data no formato dd/MM/yyyy")] string data)
    {
        if (!DateTime.TryParseExact(data, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
        {
            return BadRequest("Formato de data inválido. Use o formato dd/MM/yyyy.");
        }

        // Busca as tarefas que correspondem à data especificada.
        var tarefas = context.Tarefas.Where(x => x.Data.Date == parsedDate.Date)
            .Select(t => new {
                Id = t.Id,
                Titulo = t.Titulo,
                Descricao = t.Descricao,
                Data = t.Data.ToString("dd/MM/yyyy")
            });

        return Ok(tarefas);
    }

    [HttpGet("ObterTodas")]
    public IActionResult ObterTodas()
    {
        var tarefas = context.Tarefas.ToList();

        return Ok(tarefas);
    }

    
    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, Tarefa tarefa)
    {
        var tarefaBanco = context.Tarefas.Find(id);
        if (tarefaBanco == null)
        {
            return NotFound();
        }

        tarefaBanco.Titulo = tarefa.Titulo;
        tarefaBanco.Descricao = tarefa.Descricao;
        tarefaBanco.Data = tarefa.Data;
        tarefaBanco.Status = tarefa.Status;

        context.Tarefas.Update(tarefaBanco);
        context.SaveChanges();
        
        return Ok(tarefaBanco);
    }

    [HttpDelete("{id}")]
    public IActionResult Deletar(int id)
    {
        var tarefaBanco = context.Tarefas.Find(id);
        if (tarefaBanco == null)
        {
            return NotFound();
        }

        context.Tarefas.Remove(tarefaBanco);
        context.SaveChanges();
        return NoContent();

    }

    
}
