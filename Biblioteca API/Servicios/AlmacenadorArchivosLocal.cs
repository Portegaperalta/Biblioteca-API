
namespace Biblioteca_API.Servicios
{
    public class AlmacenadorArchivosLocal : IAlmacenadorArchivos
    {
        private readonly IWebHostEnvironment _webHostEnv;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AlmacenadorArchivosLocal(IWebHostEnvironment webHostEnv,
            IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnv = webHostEnv;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> Almacenar(string contenedor, IFormFile archivo)
        {
            var extension = Path.GetExtension(archivo.FileName);
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(_webHostEnv.WebRootPath, contenedor);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string ruta = Path.Combine(folder, nombreArchivo);
            using (var ms = new MemoryStream())
            {
                await archivo.CopyToAsync(ms);
                var contenido = ms.ToArray();
                await File.WriteAllBytesAsync(ruta, contenido);
            }

            var request = _httpContextAccessor.HttpContext!.Request;
            var url = $"{request.Scheme}://{request.Host}";
            var urlArchivo = Path.Combine(url, contenedor, nombreArchivo).Replace("\\", "/");

            return urlArchivo;
        }

        public Task Borrar(string? ruta, string contenedor)
        {
            if (string.IsNullOrEmpty(ruta))
            {
                return Task.CompletedTask;
            }

            var nombreArchivo = Path.GetFileName(ruta);
            var directorioArchivo = Path.Combine(_webHostEnv.WebRootPath, contenedor, nombreArchivo);
            
            if (File.Exists(directorioArchivo))
            {
                File.Delete(directorioArchivo);
            }

            return Task.CompletedTask;
        }
    }
}
