using System;
using System.Collections.Generic;
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

            if (Match(@"sum\((?<path>[\w.]+)\s?(from\s(?<from>\d+))?\s?(top\s(?<top>\d+))?\)", query, out m))
                return $"{query} = { FilterSequence(data.GetSequence(m.Groups["path"].Value), m).Sum() }";
            if (Match(@"min\((?<path>[\w.]+)\s?(from\s(?<from>\d+))?\s?(top\s(?<top>\d+))?\)", query, out m))
                return $"{query} = { FilterSequence(data.GetSequence(m.Groups["path"].Value), m).Min() }";
            if (Match(@"max\((?<path>[\w.]+)\s?(from\s(?<from>\d+))?\s?(top\s(?<top>\d+))?\)", query, out m))
                return $"{query} = { FilterSequence(data.GetSequence(m.Groups["path"].Value), m).Max() }";
            throw new ArgumentException("incorrect query");
        }

        private IEnumerable<double> FilterSequence(IEnumerable<double> seq, Match m)
        {
            var from = m.Groups["from"];
            var top = m.Groups["top"];
            if (from.Length != 0)
                seq = seq.Where(x => x >= double.Parse(from.Value));
            if (top.Length != 0)
                seq = seq.Take(int.Parse(top.Value));
            
            return seq;
        }
     

        private bool Match(string pattern, string command, out Match m)
        {
            m = Regex.Match(command, pattern);
            return m.Success;
        }
    }
}