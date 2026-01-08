namespace Biblioteca_API.DTOs
{
    public record PaginacionDTO(int pagina = 1, int recordsPorPagina = 10)
    {
        private const int CantidadMaximaRecordsPorPagina = 50;

        public int Pagina { get; init; } = Math.Max(1, pagina);
        public int RecordsPorPagina { get; init; } = 
            Math.Clamp(recordsPorPagina, 1, CantidadMaximaRecordsPorPagina);
    }
}
