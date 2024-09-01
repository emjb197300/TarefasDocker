namespace TarefasApi.Models.Entities
{
    public class Tarefa : Base
    {
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public bool Status { get; set; }
    
    }
}
