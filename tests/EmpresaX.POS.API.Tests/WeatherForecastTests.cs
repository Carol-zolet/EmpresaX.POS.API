using Xunit;
using FluentAssertions;
using System.Linq;

namespace EmpresaX.POS.API.Tests
{
    public class WeatherForecastTests
    {
        [Fact]
        public void TemperatureF_Calculation_IsCorrect()
        {
            // Arrange
            var forecast = new WeatherForecast(System.DateOnly.FromDateTime(System.DateTime.Now), 0, "Cool");

            // Act
            var tempF = forecast.TemperatureF;

            // Assert
            tempF.Should().Be(32);
        }
    }

    // Copiado do projeto principal para teste
    public record WeatherForecast(System.DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
