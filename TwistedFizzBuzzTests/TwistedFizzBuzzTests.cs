using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TwistedFizzBuzz;

namespace TwistedFizzBuzzTests
{
    [TestClass]
    public class TwistedFizzBuzzTests
    {
        // ***********************************************
        // ******* GetOutput for standard FizzBuzz *******

        [TestMethod]
        public void GetOutput_StartIsGreaterThanEnd_ReturnsCorrectResult()
        {
            // Arrange
            var start = 10;
            var end = 5;
            var expectedOutput = new[] { "Buzz", "Fizz", "8", "7", "Fizz", "Buzz" };
            TwistedFizzBuzz.TwistedFizzBuzz.SetTokens(new Dictionary<int, string> { { 3, "Fizz" }, { 5, "Buzz" } });

            // Act
            var result = TwistedFizzBuzz.TwistedFizzBuzz.GetOutput(start, end).Select(_ => _).ToArray();

            // Assert
            CollectionAssert.AreEqual(expectedOutput, result);
        }

        [TestMethod]
        public void GetOutput_NonSequentialNumbers_ReturnsCorrectResult()
        {
            // Arrange
            var numbers = new[] { 5, 39, 23, 768, 454, 1, 8, 64, 15 };
            var expectedOutput = new[] { "Buzz", "Fizz", "23", "Fizz", "454", "1", "8", "64", "FizzBuzz" };
            TwistedFizzBuzz.TwistedFizzBuzz.SetTokens(new Dictionary<int, string> { { 3, "Fizz" }, { 5, "Buzz" } });

            // Act
            var result = TwistedFizzBuzz.TwistedFizzBuzz.GetOutput(numbers).Select(_ => _).ToArray();

            // Assert
            CollectionAssert.AreEqual(expectedOutput, result);
        }

        [TestMethod]
        public void GetOutput_NullNumbers_ReturnsEmptyOutput()
        {
            // Arrange
            int[] numbers = null;
            var expectedOutput = new string[] { };

            // Act
            var result = TwistedFizzBuzz.TwistedFizzBuzz.GetOutput(numbers).Select(_ => _).ToArray();

            // Assert
            CollectionAssert.AreEqual(expectedOutput, result);
        }

        [TestMethod]
        public void GetOutput_ReturnsCorrectResult()
        {
            // Arrange
            var start = 1;
            var end = 20;
            var expectedOutput = new[] { "1", "2", "Fizz", "4", "Buzz", "Fizz", "7", "8", "Fizz", "Buzz", "11", "Fizz", "13", "14", "FizzBuzz", "16", "17", "Fizz", "19", "Buzz" };
            TwistedFizzBuzz.TwistedFizzBuzz.SetTokens(new Dictionary<int, string> { { 3, "Fizz" }, { 5, "Buzz" } });
            // Act
            var result = TwistedFizzBuzz.TwistedFizzBuzz.GetOutput(start, end).Select(_ => _).ToArray();

            // Assert
            CollectionAssert.AreEqual(expectedOutput, result);
        }

        // **************************************************
        // ******* GetOutput for alternative FizzBuzz *******

        [TestMethod]
        public void GetOutput_AlternativeFizzBuzz_NonSequentialNumbers_ReturnsCorrectResult()
        {
            // Arrange
            var numbers = new[] { 5, 39, 119, 23, 768, 21, 454, 1, 357, 8, 64, 15 };
            var expectedOutput = new[] { "5", "College", "PoemWriter", "23", "College", "PoemCollege", "454", "1", "PoemWriterCollege", "8", "64", "College" };
            TwistedFizzBuzz.TwistedFizzBuzz.SetTokens(new Dictionary<int, string> { { 7, "Poem" }, { 17, "Writer" }, { 3, "College" } });

            // Act
            var result = TwistedFizzBuzz.TwistedFizzBuzz.GetOutput(numbers).Select(_ => _).ToArray();

            // Assert
            CollectionAssert.AreEqual(expectedOutput, result);
        }

        [TestMethod]
        public void GetOutput_AlternativeFizzBuzz_ReturnsCorrectResult()
        {
            // Arrange
            var start = 1;
            var end = 20;
            var expectedOutput = new[] { "1", "2", "College", "4", "5", "College", "Poem", "8", "College", "10", "11", "College", "13", "Poem", "College", "16", "Writer", "College", "19", "20" };
            TwistedFizzBuzz.TwistedFizzBuzz.SetTokens(new Dictionary<int, string> { { 7, "Poem" }, { 17, "Writer" }, { 3, "College" } });

            // Act
            var result = TwistedFizzBuzz.TwistedFizzBuzz.GetOutput(start, end).Select(_ => _).ToArray();

            // Assert
            CollectionAssert.AreEqual(expectedOutput, result);
        }

        // **************************************************
        // ******* GetOutput for API generated tokens *******

        [TestMethod]
        public async Task GetOutput_APIGeneratedTokens_NonSequentialNumbers_ReturnsCorrectResult()
        {
            // Arrange
            var numbers = new[] { 5, 39, 119, 23, 768, 21, 454, 1, 357, 8, 64, 15 };
            var expectedOutput = new[] { "5", "39", "119", "23", "hello", "21", "454", "1", "357", "hello", "hello", "15" };
            await TwistedFizzBuzz.TwistedFizzBuzz.SetAPIgeneratedTokens(GetMockedHttpClient(4, "hello"));

            // Act
            var result = TwistedFizzBuzz.TwistedFizzBuzz.GetOutput(numbers).Select(_ => _).ToArray();

            var r = JsonConvert.SerializeObject(result);

            // Assert
            CollectionAssert.AreEqual(expectedOutput, result);
        }

        [TestMethod]
        public async Task GetOutput_APIGeneratedTokens_ReturnsCorrectResult()
        {
            // Arrange
            var start = 1;
            var end = 20;
            var expectedOutput = new[] { "1", "2", "3", "hello", "5", "6", "7", "hello", "9", "10", "11", "hello", "13", "14", "15", "hello", "17", "18", "19", "hello" };
            await TwistedFizzBuzz.TwistedFizzBuzz.SetAPIgeneratedTokens(GetMockedHttpClient(4, "hello"));

            // Act
            var result = TwistedFizzBuzz.TwistedFizzBuzz.GetOutput(start, end).Select(_ => _).ToArray();

            // Assert
            CollectionAssert.AreEqual(expectedOutput, result);
        }

        HttpClient GetMockedHttpClient(int multiple, string word) 
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();

            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync", 
                    ItExpr.Is<HttpRequestMessage>(t => t.Method == HttpMethod.Get && t.RequestUri.ToString() == "https://rich-red-cocoon-veil.cyclic.app/random"), 
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(new ApiToken
                    {
                        Multiple = multiple,
                        Word = word
                    }))
                });

            return new HttpClient(mockMessageHandler.Object);
        }
    }
}
