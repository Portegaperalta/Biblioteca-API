using Biblioteca_API.Entidades;

namespace Biblioteca_API.Datos.Repositorios
{
    public interface IRepositorioComentario
    {
        Task<IEnumerable<Comentario>> GetComentariosAsync(int libroId);
        Task<Comentario?> GetComentarioAsync(Guid comentarioId);
        Task CreateComentarioAsync(Comentario comentario);
        Task UpdateComentarioAsync(Guid comentarioId,Comentario comentario);
        Task<bool> DeleteComentarioAsync(Guid comentarioId);
        Task<bool> ExisteComentarioAsync(Guid comentarioId);
        Task<bool> ExisteLibroAsync(int libroId);
    }
}
