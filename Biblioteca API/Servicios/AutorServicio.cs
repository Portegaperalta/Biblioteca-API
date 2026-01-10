using Biblioteca_API.Datos.Repositorios;
using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;
using Biblioteca_API.Mappers;
using Biblioteca_API.Migrations;
using Biblioteca_API.Utilidades;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_API.Servicios
{
    public class AutorServicio : IAutorServicio
    {
        private readonly IRepositorioAutor _repositorioAutor;
        private readonly AutorMapper _autorMapper;
        private readonly IAlmacenadorArchivos _almacenadorArchivos;
        private const string contenedor = "autores";

        public AutorServicio(IRepositorioAutor repositorioAutor,AutorMapper autorMapper,
                             IAlmacenadorArchivos almacenadorArchivos)
        {
            _repositorioAutor = repositorioAutor;
            _autorMapper = autorMapper;
            _almacenadorArchivos = almacenadorArchivos;
        }

        public async Task<IEnumerable<AutorDTO>> GetAutoresDtoAsync(PaginacionDTO paginacionDTO)
        {
            var autores = await _repositorioAutor.GetAutoresAsync(paginacionDTO);
            var autoresDto = autores.Select(autor => _autorMapper.MapToAutorDto(autor));
            return autoresDto;
        }

        public async Task<AutorDTO?> GetAutorDtoAsync(int autorId)
        {
            var autor = await _repositorioAutor.GetAutorAsync(autorId);

            if (autor is null) return null;

            var autorDto = _autorMapper.MapToAutorDto(autor);
            return autorDto;
        }

        public async Task<Autor?> GetAutorAsync(int autorId)
        {
            var autor = await _repositorioAutor.GetAutorAsync(autorId);

            if (autor is null) return null;

            return autor;
        }

        public async Task<AutorSinLibrosDTO?> GetAutorSinLibrosDtoAsync(int autorId)
        {
            var autorSinLibros = await _repositorioAutor.GetAutorSinLibrosAsync(autorId);

            if (autorSinLibros is null) return null;

            var autorSinLibrosDto = _autorMapper.MapToAutorSinLibrosDto(autorSinLibros);
            return autorSinLibrosDto;
        }

        public async Task<Autor?> GetAutorAsNoTrackingAsync(int autorId)
        {
            return await _repositorioAutor.GetAutorAsNoTrackingAsync(autorId);
        }

        public async Task CreateAutorAsync(AutorCreacionDTO autorCreacionDto)
        {
            var autor = _autorMapper.MapToAutor(autorCreacionDto);

            if (autorCreacionDto.Foto is not null)
            {
                var url = await _almacenadorArchivos.Almacenar(contenedor, autorCreacionDto.Foto);
                autor.Foto = url;
            }

            await _repositorioAutor.CreateAutorAsync(autor);
        }

        public async Task<IEnumerable<AutorDTO>> GetAutoresFiltro(AutorFiltroDTO autorFiltroDTO)
        {
            var queryable = _repositorioAutor.GetAutoresAsQueryable();

            //Si el nombre no es null o vacio se filtran autores por nombre
            if (!string.IsNullOrEmpty(autorFiltroDTO.Nombres))
            {
                queryable = queryable.Where(a => a.Nombres.Contains(autorFiltroDTO.Nombres));
            }

            //Si el apellido no es null o vacio se filtran autores por apellido
            if (!string.IsNullOrEmpty(autorFiltroDTO.Apellidos))
            {
                queryable = queryable.Where(a => a.Apellidos.Contains(autorFiltroDTO.Apellidos));
            }

            //Permite filtrar los autores con foto y los que no tienen foto.
            if (autorFiltroDTO.TieneFoto.HasValue)
            {
                if (autorFiltroDTO.TieneFoto.Value)
                {
                    queryable = queryable.Where(a => a.Foto != null);
                } else
                {
                    queryable = queryable.Where(a => a.Foto == null);
                }
            }

            var autores = await queryable.OrderBy(a => a.Nombres)
                                         .Paginar(autorFiltroDTO.PaginacionDTO)
                                         .ToListAsync();

            var autoresDTO = autores.Select(autor => _autorMapper.MapToAutorDto(autor));

            return autoresDTO;
        }

        public async Task UpdateAutorAsync(AutorPutDTO autorPutDto)
        {
           
            var autor = _autorMapper.MapAutorPutDtoToAutor(autorPutDto);

            if (autorPutDto.Foto is not null)
            {
                var fotoActual = await _repositorioAutor.GetFotoActualAutor(autorPutDto.Id);
                var url = await _almacenadorArchivos
                                .Editar(fotoActual, contenedor, autorPutDto.Foto);

                autor.Foto = url;
            }

            await _repositorioAutor.UpdateAutorAsync(autor);
        }

        public async Task<bool> DeleteAutorAsync(int autorId)
        {
            var autorDb = await _repositorioAutor.GetAutorAsync(autorId);

            if (autorDb is null)
            {
                return false;
            }

            await _repositorioAutor.DeleteAutorAsync(autorDb);
            await _almacenadorArchivos.Borrar(autorDb.Foto, contenedor);

            return true;
        }

        public AutorPatchDTO HandleAutorPatchDtoMapping(Autor autor)
        {
            var autorPatchDto = _autorMapper.MapToAutorPatchDto(autor);
            return autorPatchDto;
        }

        public AutorPutDTO HandleAutorPutDtoMapping(int autorId, AutorPatchDTO autorPatchDto)
        {
            var autorPutDto = _autorMapper.MapPatchDtoToPutDto(autorId, autorPatchDto);
            return autorPutDto;
        }
    }
}
