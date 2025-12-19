using Biblioteca_API.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_API.Datos.Repositorios
{
    public class RepositorioAutor : IRepositorioAutor
    {
        private readonly ApplicationDbContext _context;

        public RepositorioAutor(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Autor>> GetAutoresAsync()
        {
            return await _context.Autores.ToListAsync();
        }

        public async Task<Autor?> GetAutor(int id)
        {
            return await _context.Autores.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Autor> GetPrimerAutor()
        {
            return await _context.Autores.FirstAsync();
        }
    }
}
