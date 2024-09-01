using TarefasApi.Context;
using TarefasApi.Repository.Interfaces;
using System.Threading.Tasks;

namespace TarefasApi.Repository
{
    public class BaseRepository : IBaseRepository
    {
        private readonly TarefasContext _context;

        public BaseRepository(TarefasContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
    }
}
