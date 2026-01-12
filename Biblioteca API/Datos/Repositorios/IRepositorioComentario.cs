using Biblioteca_API.Entidades;

namespace Biblioteca_API.Datos.Repositorios
{
    public interface IRepositorioComentario
    {
        Task<IEnumerable<Comentario>> GetAllAsync(int libroId);
        Task<Comentario?> GetByIdAsync(Guid comentarioId);
        Task CreateAsync(Comentario comentario);
        Task UpdateAsync(Guid comentarioId,Comentario comentario);
        Task<bool> DeleteAsync(Guid comentarioId);
        Task<bool> ExisteComentarioAsync(Guid comentarioId);
        Task<bool> ExisteLibroAsync(int libroId);
    }
}
