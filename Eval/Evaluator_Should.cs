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


        [TestCase("1 2 + 4 *", Result = 12)]
        [TestCase("6 2 / 6 +", Result = 9)]
        [TestCase("3 2 + 1 -", Result = 4)]
        public double DoSimpleMath_FromPolishNotation(string input)
        {
            var res = evaluator.Evaluate(input);
            return res;
        }
    }
}
