
using TarefasApi.Models.Dtos;
using TarefasApi.Models.Entities;

namespace TarefasApi.Repository.Interfaces
{
    public interface ITarefaRepository : IBaseRepository
    {
        Task<IEnumerable<TarefaDto>> GetTarefas();
        Task<Tarefa?> GetTarefaById(int id);
    }
}
