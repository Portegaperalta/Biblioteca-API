using Biblioteca_API.Datos.Repositorios;
using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;
using Biblioteca_API.Mappers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

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

        public async Task UpdateAutorAsync(AutorPutDTO autorPutDto)
        {
            var autorDb = await _repositorioAutor.GetAutorAsync(autorPutDto.Id);
            var autor = _autorMapper.MapAutorPutDtoToAutor(autorPutDto);

            if (autorPutDto.Foto is not null)
            {
                var fotoActual = autorDb.Foto;
                var url = await _almacenadorArchivos
                                .Editar(fotoActual, contenedor, autorPutDto.Foto);

                autor.Foto = url;
            }

            await _repositorioAutor.UpdateAutorAsync(autor);
        }

        public async Task<int> DeleteAutorAsync(int autorId)
        {
            var registrosBorrados = await _repositorioAutor.DeleteAutorAsync(autorId);
            return registrosBorrados;
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
