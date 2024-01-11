using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace Rpnw.acceptance.Test
{
    [TestFixture(Category ="Acceptance")]
    public class StackAcceptanceTests
    {
        private HttpClient _client;
        private string _baseUri = "https://localhost:7144/api/v1/stacks";

        [SetUp]
        public void SetUp()
        {
            _client = new HttpClient();
        }

        [Test]
        public async Task CreateStack_ShouldReturnNewStack()
        {
            // Arrange
            var uri = _baseUri;

            // Act
            var response = await _client.PostAsync(uri, null);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            var stackDto = JsonConvert.DeserializeObject<StackDto>(jsonString);
            Assert.IsNotNull(stackDto);
            Assert.IsNotNull(stackDto.Id);
        }

        [Test]
        public async Task DeleteStack_AfterCreating_ShouldReturnNoContent()
        {
            // Arrange
            var createResponse = await _client.PostAsync(_baseUri, null);
            createResponse.EnsureSuccessStatusCode();
            var createContent = await createResponse.Content.ReadAsStringAsync();
            var createdStack = JsonConvert.DeserializeObject<StackDto>(createContent);
            var uri = $"{_baseUri}/{createdStack.Id}";

            // Act
            var response = await _client.DeleteAsync(uri);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task PerformCalculation_ShouldReturnCorrectResult()
        {
            // Arrange
            var createResponse = await _client.PostAsync(_baseUri, null);
            createResponse.EnsureSuccessStatusCode();
            var createContent = await createResponse.Content.ReadAsStringAsync();
            var createdStack = JsonConvert.DeserializeObject<StackDto>(createContent);
            var uri = $"{_baseUri}/{createdStack.Id}";

            await _client.PostAsync($"{uri}/operands", JsonContent.Create(new OperandDto { Value = 10 }));
            await _client.PostAsync($"{uri}/operands", JsonContent.Create(new OperandDto { Value = 12 }));
            await _client.PostAsync($"{uri}/operands", JsonContent.Create(new OperandDto { Value = 2 }));

            // Act
            await _client.PostAsync($"{uri}/operators", JsonContent.Create(new AddOperatorDto { OperatorId = 1 }));
            await _client.PostAsync($"{uri}/operators", JsonContent.Create(new AddOperatorDto { OperatorId = 3 }));

            // Assert
            var getResultResponse = await _client.GetAsync(uri);
            getResultResponse.EnsureSuccessStatusCode();
            var getResultContent = await getResultResponse.Content.ReadAsStringAsync();
            var stackResult = JsonConvert.DeserializeObject<StackDto>(getResultContent);
            var resultValue = stackResult.Elements.Last().Value; 
            Assert.AreEqual("140", resultValue); //10*(12+2)
        }

        [Test]
        public async Task UndoOperation_ShouldRevertLastOperation()
        {
            // Arrange
            var createResponse = await _client.PostAsync(_baseUri, null);
            createResponse.EnsureSuccessStatusCode();
            var createContent = await createResponse.Content.ReadAsStringAsync();
            var createdStack = JsonConvert.DeserializeObject<StackDto>(createContent);
            var uri = $"{_baseUri}/{createdStack.Id}";

            // Act
            await _client.PostAsync($"{uri}/operands", JsonContent.Create(new OperandDto { Value = 10 }));
            await _client.PostAsync($"{uri}/operands", JsonContent.Create(new OperandDto { Value = 12 }));
            await _client.PostAsync($"{uri}/operands", JsonContent.Create(new OperandDto { Value = 2 }));
            await _client.PostAsync($"{uri}/operators", JsonContent.Create(new AddOperatorDto { OperatorId = 1 })); // 1 est une Addition
            await _client.PostAsync($"{uri}/operators", JsonContent.Create(new AddOperatorDto { OperatorId = 3 })); // 3 est Multiplication

            // Act
            var undoResponse = await _client.PostAsync($"{uri}/operators/undo", null);
            undoResponse.EnsureSuccessStatusCode();
            var undoContent = await undoResponse.Content.ReadAsStringAsync();
            var stackAfterUndo = JsonConvert.DeserializeObject<StackDto>(undoContent);

            // Assert
            var currentValues = stackAfterUndo.Elements.Select(e => e.Value).ToArray();
            Assert.AreEqual(new string[] { "14", "10" }, currentValues, "La pile finale doit contenir les chiffre 10,14.");

            // Act
            undoResponse = await _client.PostAsync($"{uri}/operators/undo", null);
            undoResponse.EnsureSuccessStatusCode();
            undoContent = await undoResponse.Content.ReadAsStringAsync();
            stackAfterUndo = JsonConvert.DeserializeObject<StackDto>(undoContent);

            // Assert
            currentValues = stackAfterUndo.Elements.Select(e => e.Value).ToArray(); 
            Assert.AreEqual(new string[] { "2","12","10" }, currentValues, "La pile finale doit contenir les chiffre 2,12,10 .");

        }

       
    }
}