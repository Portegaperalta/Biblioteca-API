using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_API.Datos.Repositorios
{
    public class RepositorioLibro : IRepositorioLibro
    {
        private readonly ApplicationDbContext _context;

        public RepositorioLibro(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Libro>> GetLibrosAsync()
        {
            return await _context.Libros.ToListAsync();
        }

        public async Task<Libro?> GetLibroAsync(int id)
        {
            return await _context.Libros.
                         FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Libro?> GetLibroConAutorAsync(int libroId)
        {
            return await _context.Libros.Include(libro => libro.Autores).
                         Where(libro => libro.Autores.Any(autor =>
                         autor.LibroId == libroId)).FirstOrDefaultAsync();
        }

        public async Task CreateLibroAsync(Libro libro)
        {
            _context.Add(libro);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLibroAsync(Libro libro)
        {
            _context.Update(libro);
            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteLibroAsync(int id)
        {
            var registrosBorrados = await _context.Libros.
                Where(x => x.Id == id).ExecuteDeleteAsync();

            return registrosBorrados;
        }

        public async Task<bool> ExistenAutores(List<int> autoresIds)
        {   
            var autoresDb = await _context.Autores.ToListAsync();
            List<int> autoresDbIds = [];

            foreach (Autor autorDb in autoresDb)
            {
                foreach (int autorId in autoresIds)
                {
                    if (autorDb.Id == autorId)
                    {
                        autoresDbIds.Add(autorDb.Id);
                    }
                }
            }

            if (autoresDb.Count == 0)
            {
                return false;
            }

            return true;
        }
    }
}
