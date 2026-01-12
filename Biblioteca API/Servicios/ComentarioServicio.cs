using Biblioteca_API.Datos.Repositorios;
using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;

namespace Biblioteca_API.Servicios
{
    public class ComentarioServicio : IComentarioServicio
    {
        private readonly RepositorioComentario _repositorioComentario;

        public ComentarioServicio(RepositorioComentario repositorioComentario)
        {
            _repositorioComentario = repositorioComentario;
        }

        public async Task<IEnumerable<ComentarioDTO>> GetAll(int libroId)
        {
            bool existeLibro = await _repositorioComentario.ExisteLibroAsync(libroId);

            if (existeLibro == false)
            {
                throw new ArgumentException($"El libro con id {libroId} no existe");
            }

            var comentarios = await _repositorioComentario.GetComentariosDtoAsync(libroId);
            var comentariosDTO = comentarios.Select(c => new ComentarioDTO
            {
                UsuarioId = c.UsuarioId,
                Usuario = c.Autor,
                UsuarioEmail = c.Usuario!.Email!,
                Cuerpo = c.Cuerpo,
                FechaPublicacion = c.FechaPublicacion
            });

            return comentariosDTO;
        }

        public async Task<ComentarioDTO?> GetById(Guid comentarioId)
        {
            var comentario = await _repositorioComentario.GetComentarioDtoByIdAsync(comentarioId);
            
            if (comentario is null)
            {
                return null;
            }

            var comentarioDTO = new ComentarioDTO
            {
                Id = comentario.Id,
                UsuarioId = comentario.UsuarioId,
                Usuario = comentario.Autor,
                UsuarioEmail = comentario.Usuario!.Email!,
                Cuerpo = comentario.Cuerpo,
                FechaPublicacion = comentario.FechaPublicacion
            };

            return comentarioDTO;
        }

        public async Task Create(int libroId,Comentario comentario)
        {
            bool existeLibro = await _repositorioComentario.ExisteLibroAsync(libroId);

            if (existeLibro == false)
            {
                throw new ArgumentException($"El libro con id {libroId} no existe");
            }

            await _repositorioComentario.CreateComentarioAsync(comentario);
        }

        public async Task Update(Guid comentarioId,Comentario comentario)
        {
            bool existeLibro = await _repositorioComentario.ExisteLibroAsync(comentario.LibroId);

            if (existeLibro == false)
            {
                throw new InvalidOperationException($"El libro con id: {comentario.LibroId} no existe");
            }

            await _repositorioComentario.UpdateComentarioAsync(comentarioId, comentario);
        }

        public async Task<bool> Delete(Guid comentarioId)
        {
            bool estaBorrado = await _repositorioComentario.DeleteComentarioAsync(comentarioId);

            if (estaBorrado == false)
            {
                return false;
            }

            return true;
        }
    }
}
