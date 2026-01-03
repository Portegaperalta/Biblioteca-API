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

        public AutorServicio(IRepositorioAutor repositorioAutor,AutorMapper autorMapper)
        {
            _repositorioAutor = repositorioAutor;
            _autorMapper = autorMapper;
        }

        public async Task<IEnumerable<AutorDTO>> GetAutoresDtoAsync()
        {
            var autores = await _repositorioAutor.GetAutoresAsync();
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
            await _repositorioAutor.CreateAutorAsync(autor);
        }

        public async Task UpdateAutorAsync(AutorPutDTO autorPutDto)
        {
            var autor = _autorMapper.MapAutorPutDtoToAutor(autorPutDto);
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
