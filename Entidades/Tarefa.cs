namespace GerenciadorDeTarefas.Entidades;

public class Tarefa
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public DateTime Data { get; set; }
    public int Status { get; set; }

    public int SetStatus(EnumStatusTarefa status) => Status = (int)status;


    public void AlterarStatus(EnumStatusTarefa enumStatusTarefa)
    {
        SetStatus(enumStatusTarefa);
    }
}