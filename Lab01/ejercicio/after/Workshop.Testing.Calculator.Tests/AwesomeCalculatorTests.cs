using System;
using Xunit;

namespace Workshop.Testing.Calculator.Tests
{
    public class AwesomeCalculatorTests
    {
        private AwesomeCalculator systemUnderTest;
        public AwesomeCalculatorTests()
        {
            systemUnderTest = new AwesomeCalculator();
        }

        [Theory]
        [InlineData(9,2,11)]
        [InlineData(7,3,10)]
        [InlineData(5.1, 2.2, 7.3)]
        public void Should_Add_Two_Numbers(double operator1,double operator2,double expected)
        {            
            // Act
            var result = systemUnderTest.Add(operator1, operator2);
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(7,3,4)]
        [InlineData(2,2,0)]
        [InlineData(-5.2,1.3,-6.5)]        
        public void Should_Substract_Two_Numbers(double operator1, double operator2, double expected)
        {            
            // Act
            var result = systemUnderTest.Substract(operator1, operator2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(2,4,8)]
        [InlineData(8, 7, 56)]
        [InlineData(-7, -7, 49)]
        public void Should_Multiply_Two_Numbers(double operator1, double operator2, double expected)
        {            
            // Act
            var result = systemUnderTest.Multiply(operator1, operator2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(40,20,2)]
        [InlineData(5, 2, 2.5)]
        [InlineData(-10,-5,2)]
        public void Should_Divide_Two_Numbers(double operator1, double operator2, double expected)
        {            
            // Act
            var result = systemUnderTest.Divide(operator1, operator2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Should_Throw_DivideByZeroException_When_Trying_To_Divide_By_Zero()
        {            
            // Arrange
            double operator1 = 2;
            double operator2 = 0;            

            // Act
            Action divide = () => systemUnderTest.Divide(operator1, operator2);

            // Assert
            Assert.Throws<DivideByZeroException>(divide);
        }
    }
}
