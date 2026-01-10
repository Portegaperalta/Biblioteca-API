using Biblioteca_API.DTOs;
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

        public async Task<IEnumerable<Autor>> GetAutoresAsync(PaginacionDTO paginacionDTO)
        {
            return await _context.Autores
                         .Skip((paginacionDTO.pagina - 1) * paginacionDTO.recordsPorPagina)
                         .Take(paginacionDTO.recordsPorPagina)
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

        public async Task<Autor?> GetAutorAsNoTrackingAsync(int id)
        {
            return await _context.Autores
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(a => a.Id == id);
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
            _context.Autores.Add(autor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAutorAsync(Autor autor)
        {
            _context.Autores.Update(autor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAutorAsync(Autor autor)
        {
            _context.Autores.Remove(autor);
            await _context.SaveChangesAsync();
        }
    }
}
