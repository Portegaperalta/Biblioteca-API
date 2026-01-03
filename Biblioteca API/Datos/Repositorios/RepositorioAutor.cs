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
            return await _context.Autores
                         .Include(x => x.Libros)
                         .ThenInclude(l => l.Libro)
                         .ToListAsync();
        }

        public async Task<Autor?> GetAutorAsync(int id)
        {
            return await _context.Autores
                         .Include(x => x.Libros)
                         .ThenInclude(l => l.Libro)
                         .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Autor?> GetAutorSinLibrosAsync(int autorId)
        {
            var autorSinLibros = await _context.Autores
                                       .FirstOrDefaultAsync(a => a.Id == autorId);

            if (autorSinLibros is null) return null;

            return autorSinLibros;
        }

        public async Task CreateAutorAsync(Autor autor)
        {
            _context.Add(autor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAutorAsync(Autor autor)
        {
            _context.Update(autor);
            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAutorAsync(int id)
        {
            int registrosBorrados = await _context.Autores.Where(x => x.Id == id).ExecuteDeleteAsync();
            return registrosBorrados;
        }
    }
}
