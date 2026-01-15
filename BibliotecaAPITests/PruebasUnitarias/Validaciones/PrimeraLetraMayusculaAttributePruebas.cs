using System.ComponentModel.DataAnnotations;
using Biblioteca_API.Validaciones;

namespace BibliotecaAPITests.PruebasUnitarias.Validaciones
{
    [TestClass]
    public class PrimeraLetraMayusculaAttributePruebas
    {
        [TestMethod]
        [DataRow("")]
        [DataRow("  ")]
        [DataRow(null)]
        [DataRow("Texto")]
        public void IsValid_RetornaExitoso_siValueEsVacioONull(string value)
        {
            //Preparacion
            var primeraLetraMayusculaAttribute = new PrimeraLetraMayusculaAttribute();
            var validationContext = new ValidationContext(new object { });

            //Prueba
            var resultado = primeraLetraMayusculaAttribute.GetValidationResult(value, validationContext);

            //Verificacion
            Assert.AreEqual(expected: ValidationResult.Success, actual: resultado);
        }

        [TestMethod]
        [DataRow("texto")]
        public void IsValid_RetornaValidationResult_siPrimeraLetraNoMayuscula(string value)
        {
            //Preparacion
            var primeraLetraMayusculaAttribute = new PrimeraLetraMayusculaAttribute();
            var validationContext = new ValidationContext(new object { });

            //Prueba
            var resultado = primeraLetraMayusculaAttribute.GetValidationResult(value, validationContext);

            //Verificacion
            Assert.AreEqual(expected: "La primera letra debe ser Mayuscula", resultado!.ErrorMessage);
        }
    }
}
