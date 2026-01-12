using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;

namespace Biblioteca_API.Servicios
{
    public interface IComentarioServicio
    {
      Task<IEnumerable<ComentarioDTO>> GetAll(int libroId);
      Task<ComentarioDTO?> GetById(Guid comentarioId);
      Task<Comentario?> GetEntityByIdAsync(Guid comentarioId);
      Task Create(int libroId, Comentario comentario);
      Task Update(Guid comentarioId, Comentario comentario);
      Task<bool> Delete(Guid comentarioId);
    }
}