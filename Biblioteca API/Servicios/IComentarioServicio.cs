using Biblioteca_API.DTOs.Comentario;
using Biblioteca_API.Entidades;

namespace Biblioteca_API.Servicios
{
    public interface IComentarioServicio
    {
      Task<IEnumerable<ComentarioDTO>> GetAllAsync(int libroId);
      Task<ComentarioDTO?> GetByIdAsync(Guid comentarioId);
      Task<Comentario?> GetEntityByIdAsync(Guid comentarioId);
      Task CreateAsync(int libroId, Comentario comentario);
      Task UpdateAsync(Comentario comentario);
      Task<bool> DeleteAsync(Guid comentarioId);
    }
}