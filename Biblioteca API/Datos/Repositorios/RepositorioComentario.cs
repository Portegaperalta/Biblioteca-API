using Biblioteca_API.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_API.Datos.Repositorios
{
    public class RepositorioComentario: IRepositorioComentario
    {
        private readonly ApplicationDbContext _context;

        public RepositorioComentario(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comentario>> GetComentariosAsync(int libroId)
        {
            var comentarios = await _context.Comentarios
                              .Include(c => c.Usuario)
                              .Where(x => x.LibroId == libroId)
                              .OrderByDescending(x => x.FechaPublicacion)
                              .ToListAsync();

            return comentarios;
        }

        public async Task<Comentario?> GetComentarioAsync(Guid comentarioId)
        {
            var comentario = await _context.Comentarios
                                   .Include(c => c.Usuario)
                                   .FirstOrDefaultAsync(x => x.Id == comentarioId);
            return comentario;
        }

        public async Task CreateComentarioAsync(Comentario comentario)
        {
            _context.Add(comentario);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateComentarioAsync(Guid comentarioId,Comentario comentario)
        {
            _context.Update(comentario);
            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteComentarioAsync(Guid comentarioId)
        {
            var registrosBorrados = await _context.Comentarios
                                   .Where(x => x.Id == comentarioId)
                                   .ExecuteDeleteAsync();

            return registrosBorrados;
        }

        public async Task<bool> ExisteComentarioAsync(Guid comentarioId)
        {
            return await _context.Comentarios.AnyAsync(x => x.Id == comentarioId);
        }

         public async Task<bool> ExisteLibroAsync(int libroId)
        {
            return await _context.Libros.AnyAsync(x => x.Id == libroId);
        }
    }
}
