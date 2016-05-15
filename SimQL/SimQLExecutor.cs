using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace SimQLTask
{
    class SimQLExecutor
    {
        private readonly JObject data;

        public SimQLExecutor(JObject data)
        {
            this.data = data;
        }


        public string ExecuteQuery(string query)
        {
            Match m;

            if (Match(@"sum\((?<path>.*)\)", query, out m))
                return $"{query} = {data.Sum(TransformPath(m.Groups["path"].Value))}";
            if (Match(@"min\((?<path>.*)\)", query, out m))
                return $"{query} = {data.Min(m.Groups["path"].Value)}";
            if (Match(@"max\((?<path>.*)\)", query, out m))
                return $"{query} = {data.Max(m.Groups["path"].Value)}";
            throw new ArgumentException("incorrect query");
        }


        private string TransformPath(string path)
        {
            StringBuilder result = new StringBuilder().Append("$");
            var parts = path.Split('.');
            var currentToken = data.SelectToken("$");
            foreach (var part in parts)
            {
                currentToken = currentToken.SelectToken(part);
                if(currentToken.Type == JTokenType.Array)
                result.Append($".{part}[*]");
                else
                result.Append($".{part}");
            }
            return result.ToString();
        }

        private bool Match(string pattern, string command, out Match m)
        {
            m = Regex.Match(command, pattern);
            return m.Success;
        }
    }
}