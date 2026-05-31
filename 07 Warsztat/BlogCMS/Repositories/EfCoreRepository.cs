using BlogCMS.Data;
using BlogCMS.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace BlogCMS.Repositories
{
    public class EfCoreRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly BlogDbContext _context;
        private DbSet<T> _entities;

        public EfCoreRepository(BlogDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _entities.ToListAsync();
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.Where(predicate).ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id) => await _entities.FindAsync(id);

        public async Task<int> AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _entities.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            _entities.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
