using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TwistedFizzBuzz
{
    public static class TwistedFizzBuzz
    {
        private static IReadOnlyDictionary<int, string> _tokens = new Dictionary<int, string>();

        public static void SetTokens(IReadOnlyDictionary<int, string> tokens)
        {
            _tokens = tokens ?? new Dictionary<int, string>();
        }

        public static async Task SetAPIgeneratedTokens(HttpClient httpClient)
        {
            try
            {
                var response = await httpClient.GetAsync("https://rich-red-cocoon-veil.cyclic.app/random");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var apiToken = JsonConvert.DeserializeObject<ApiToken>(json);
                SetTokens(new Dictionary<int, string> { { apiToken.Multiple, apiToken.Word } });
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to retrieve API-generated tokens.", ex);
            }
        }

        public static IEnumerable<string> GetOutput(int start, int end)
        {
            bool executingStatement(int i) => end < start ? i >= end : i <= end;
            var stepSize = end < start ? -1 : 1;

            for (var i = start; executingStatement(i); i += stepSize) yield return FindToken(i);
        }

        public static IEnumerable<string> GetOutput(int[] numbers)
        {
            foreach (var i in numbers ?? new int[] { }) yield return FindToken(i);
        }

        static string FindToken(int i)
        {
            var tokens = _tokens.Where(t => i % t.Key == 0).Select(t => t.Value);
            return tokens.Any() ? String.Join(String.Empty, tokens) : i.ToString();
        }
    }

    public class ApiToken
    {
        [JsonProperty(PropertyName = "multiple")]
        public int Multiple { get; set; }

        [JsonProperty(PropertyName = "word")]
        public string Word { get; set; }
    }
}
