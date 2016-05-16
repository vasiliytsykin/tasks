using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace SimQLTask
{
    public static class JObjectExtensions
    {
        public static IEnumerable<double> GetSequence(this JObject data, string path)
        {
            var pathNodes = path.Split('.');
            var currentTokens = new List<JToken>();
            currentTokens.Add(data);

            foreach (var pathNode in pathNodes)
                currentTokens = currentTokens.SelectMany(x => GetTokens(x, pathNode)).ToList();

            return GetResult(currentTokens);
        }

        private static IEnumerable<JToken> GetTokens(JToken token, string path)
        {
            if (token.Type == JTokenType.Array)
                return token.Children().SelectMany(x => x.SelectTokens(path));

            return token.SelectTokens(path);
        }

        private static IEnumerable<double> GetResult(IEnumerable<JToken> currentTokens)
        {
            foreach (var token in currentTokens)
                if (token.Type == JTokenType.Array)
                    foreach (var child in token.Children())
                        yield return (double) child;
                else
                    yield return (double) token;
        }       
    }
}