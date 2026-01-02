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
            return await _context.Libros.Include(l => l.Autores)
                                        .FirstOrDefaultAsync(l => l.Id == libroId);
        }

        public async Task<IEnumerable<Autor>> GetLibroAutores(int libroId)
        {
            var libroAutores = await _context.Autores
                                             .Where(autor => autor.Libros
                                             .Any(libro => libro.LibroId == libroId)).ToListAsync();
            return libroAutores;
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

        public async Task<IEnumerable<int>> GetLibroAutoresId (LibroCreacionDTO libroCreacionDTO)
        {
            var autoresId = await _context.Autores
                                       .Where(autor => libroCreacionDTO.AutoresIds
                                       .Contains(autor.Id))
                                       .Select(autor => autor.Id)
                                       .ToListAsync();

            return autoresId;
        }
    }
}
