using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Biblioteca_API.Swagger
{
    public class ConvencionAgrupaPorVersion : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var namespaceDelControlador = controller.ControllerType.Namespace;
            var version = namespaceDelControlador!.Split(".").Last().ToLower();
            controller.ApiExplorer.GroupName = version;
        }
    }
}
