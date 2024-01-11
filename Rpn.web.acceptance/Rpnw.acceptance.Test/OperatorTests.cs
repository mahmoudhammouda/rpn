using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;

namespace Rpnw.acceptance.Test
{
    [TestFixture(Category ="Acceptance")]
    public class OperatorTests
    {
        private string _baseUri = "https://localhost:7144/api/v1/operators";
        private HttpClient _client;

        [OneTimeSetUp]
        public void Setup()
        {
            _client = new HttpClient();
        }

        [Test]
        public async Task GetAllOperators_ReturnsOkResponse()
        {
            // Arrange
            var uri = _baseUri;

            // Act
            var response = await _client.GetAsync(uri);

            // Assert
            Assert.That(response.IsSuccessStatusCode, Is.True);
            var responseString = await response.Content.ReadAsStringAsync();
            var operators = JsonConvert.DeserializeObject<IEnumerable<OperatorDto>>(responseString);
            Assert.NotNull(operators);
            Assert.IsNotEmpty(operators);
        }

        [Test]
        public async Task GetOperator_ReturnsOkResponse_WhenOperatorExists()
        {
            // Arrange
            var uri = $"{_baseUri}/1";

            // Act
            var response = await _client.GetAsync(uri);

            // Assert
            Assert.That(response.IsSuccessStatusCode, Is.True);
            var responseString = await response.Content.ReadAsStringAsync();
            var operatorDto = JsonConvert.DeserializeObject<OperatorDto>(responseString);
            Assert.NotNull(operatorDto);
            Assert.AreEqual(operatorDto.Id, 1);
        }

    }
}