using Biblioteca_API.Entidades;

namespace Biblioteca_API.Datos.Repositorios
{
    public interface IRepositorioComentario
    {
        Task<IEnumerable<Comentario>> GetComentariosAsync(int libroId);
        Task<Comentario?> GetComentarioAsync(Guid comentarioId);
        Task CreateComentarioAsync(int libroId, Comentario comentario);
        Task UpdateComentarioAsync(Guid comentarioId,Comentario comentario);
        Task<int> DeleteComentarioAsync(Guid comentarioId);
        Task<bool> ExisteLibroAsync(int libroId);
    }
}
