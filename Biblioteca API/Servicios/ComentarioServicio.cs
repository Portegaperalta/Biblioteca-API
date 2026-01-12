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

        public async Task<IEnumerable<ComentarioDTO>> GetAllAsync(int libroId)
        {
            bool existeLibro = await _repositorioComentario.ExisteLibroAsync(libroId);

            if (existeLibro == false)
            {
                throw new ArgumentException($"El libro con id {libroId} no existe");
            }

            var comentarios = await _repositorioComentario.GetAllAsync(libroId);
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

        public async Task<ComentarioDTO?> GetByIdAsync(Guid comentarioId)
        {
            var comentario = await _repositorioComentario.GetByIdAsync(comentarioId);
            
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

        public async Task<Comentario?> GetEntityByIdAsync(Guid comentarioId)
        {
            var comentario = await _repositorioComentario.GetByIdAsync(comentarioId);
            
            if (comentario is null)
            {
                return null;
            }

            return comentario;
        }

        public async Task CreateAsync(int libroId,Comentario comentario)
        {
            bool existeLibro = await _repositorioComentario.ExisteLibroAsync(libroId);

            if (existeLibro == false)
            {
                throw new ArgumentException($"El libro con id {libroId} no existe");
            }

            await _repositorioComentario.CreateAsync(comentario);
        }

        public async Task UpdateAsync(Comentario comentario)
        {
            bool existeLibro = await _repositorioComentario.ExisteLibroAsync(comentario.LibroId);

            if (existeLibro == false)
            {
                throw new InvalidOperationException($"El libro con id: {comentario.LibroId} no existe");
            }

            await _repositorioComentario.UpdateAsync(comentarioId, comentario);
        }

        public async Task<bool> DeleteAsync(Guid comentarioId)
        {
            bool estaBorrado = await _repositorioComentario.DeleteAsync(comentarioId);

            if (estaBorrado == false)
            {
                return false;
            }

            return true;
        }
    }
}
