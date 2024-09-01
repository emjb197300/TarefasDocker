namespace TarefasApi.Models.Dtos
{
    public class TarefaAdicionarDto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public bool Status { get; set; }
    }
}
