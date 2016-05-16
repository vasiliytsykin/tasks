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
                return $"{query} = {data.GetSequence(m.Groups["path"].Value).Sum()}";
            if (Match(@"min\((?<path>.*)\)", query, out m))
                return $"{query} = {data.GetSequence(m.Groups["path"].Value).Min()}";
            if (Match(@"max\((?<path>.*)\)", query, out m))
                return $"{query} = {data.GetSequence(m.Groups["path"].Value).Max()}";
            throw new ArgumentException("incorrect query");
        }


     

        private bool Match(string pattern, string command, out Match m)
        {
            m = Regex.Match(command, pattern);
            return m.Success;
        }
    }
}