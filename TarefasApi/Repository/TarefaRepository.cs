using Microsoft.EntityFrameworkCore;
using TarefasApi.Context;
using TarefasApi.Models.Dtos;
using TarefasApi.Models.Entities;
using TarefasApi.Repository.Interfaces;

namespace TarefasApi.Repository
{
    public class TarefaRepository : BaseRepository, ITarefaRepository
    {
        private readonly TarefasContext _context;

        public TarefaRepository(TarefasContext context) : base(context)
        {
            _context = context;
            _context.Database.Migrate();
        }

        public async Task<IEnumerable<TarefaDto>> GetTarefas()
        {
            return await _context.Tarefas
                .Select(x => new TarefaDto { Id = x.Id, Descricao = x.Descricao, Data = x.Data, Status = x.Status }).ToListAsync();
        }

        public async Task<Tarefa?> GetTarefaById(int id)
        {
            return await _context.Tarefas.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
