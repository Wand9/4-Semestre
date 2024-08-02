using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculos.Test
{
    public class CalculoUnitTest
    {
        //AAA : Act, Arrange, Assert
        //AAA : Agir, Organizar e Assertir
        [Fact]
        public void TestarMetodoSomar()
        {
            //Arrange : Organizar
            var x1 = 4.1;
            var x2 = 5.9;
            var ValorEsperado = 10;

            //Act : Agir
            var soma = Calculo.Somar(x1, x2); 

            //Assert : Provar
            Assert.Equal(ValorEsperado, soma);

        }

        [Fact]
        public void TestarMetodoSubtrair()
        {
            var x1 = 20;
            var x2 = 10;
            var ValorEsperado = 10;

            var subtrair = Calculo.Subtrair(x1, x2);

            Assert.Equal(ValorEsperado, subtrair);
        }

        [Fact]
        public void TestarMetodoDividir()
        {
            var x1 = 10;
            var x2 = 2;
            var ValorEsperado = 5;

            var dividir = Calculo.Dividir(x1, x2);

            Assert.Equal(ValorEsperado, dividir);
        }

        [Fact]
        public void TestarMetodoMultiplicar()
        {
            var x1 = 5;
            var x2 = 10;
            var ValorEsperado = 50;

            var multiplicar = Calculo.Multiplicar(x1, x2);

            Assert.Equal(ValorEsperado, multiplicar);
        }
    }
}
