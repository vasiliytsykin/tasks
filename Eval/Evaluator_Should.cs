using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace EvalTask
{
    [TestFixture]
    class Evaluator_Should
    {
        private readonly Evaluator evaluator;

        public Evaluator_Should()
        {
            evaluator = new Evaluator();
        }

        [SetUp]
        public void SetUp()
        {
            
        }

        [TestCase("3 + 2 - 1", Result = 4)]
        [TestCase("6 / 2 + 6", Result = 9)]
        public double DoSimpleMathWithoutBrackets(string input)
        {
            var res = evaluator.Evaluate(input);
            return res;
        }


        [TestCase("3 + (2 - 1)", Result = 4)]
        [TestCase("8 / (2 + 6)", Result = 1)]
        public double DoSimpleMathWithBrackets(string input)
        {
            var res = evaluator.Evaluate(input);
            return res;
        }

        [TestCase("-3 + (2 - 1)", Result = -2)]
        [TestCase("8 / (-2)", Result = -4)]
        public double DoUnaryMinus(string input)
        {
            var res = evaluator.Evaluate(input);
            return res;
        }

        [TestCase("3.5 + sqrt(16 + 9)", Result = 8.5)]
        [TestCase("8 + (200 + 60)%", Result = 10.6)]
        public double DoComplexMathWithBrackets(string input)
        {
            var res = evaluator.Evaluate(input);
            return res;
        }
    }
}
