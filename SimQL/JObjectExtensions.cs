using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace SimQLTask
{
    public static class JObjectExtensions
    {
        public static double Sum(this JObject data, string path)
        {
            var tokens = data.SelectTokens(path).ToList();

            return tokens.Select(x => (double)x).Sum();
        }

        public static double Min(this JObject data, string path)
        {
            return data.SelectTokens(path).Select(x => (double)x).Min();
        }

        public static double Max(this JObject data, string path)
        {
            return data.SelectTokens(path).Select(x => (double)x).Max();
        }

        public static IEnumerable<double> GetSequence(this JObject data, string path)
        {
            var pathNodes = path.Split('.');
            var stack = new Stack<JToken>();
            int nodeIndex = 0;

            stack.Push(data.SelectToken(pathNodes[nodeIndex]));

            while (stack.Count > 0)
            {
                var nextToken = stack.Pop();

                if (nodeIndex < pathNodes.Length - 1)
                {
                    foreach (var child in nextToken.SelectTokens(pathNodes[++nodeIndex]))
                    {
                        stack.Push(child);
                    }
                }
                else
                
                if (nextToken.HasValues)
                {
                    foreach (var child in nextToken.Children())
                    {
                        yield return child.Value<double>();
                    }
                }
                else
                {
                    yield return nextToken.Value<double>();
                }
            }

        }

    }

    [TestFixture]
    public class JobjectExtensions_Shoud
    {
        JObject data;
        string path;

        [SetUp]
        public void Setup()
        {
            data = (JObject)JObject.Parse(@"{'data': {'a':{'x':3.14, 'b':[{'c': 15}, {'c':9}]}, 'z':[2.65, 35]},
                                                        'queries': [ 'sum(a.b.c)']}")["data"];

            path = @"a.b.c";
        }

        [Test]
        public void PathTest()
        {
            var result = data.Sum(path);

            Assert.AreEqual(24, result);
        }

    }
}