using System;
using System.Collections.Generic;
using System.Text;

namespace EvalTask
{

    enum Priority
    {
        Low = 0,
        High
    }

    enum Lexem
    {
        Add,
        Subtract,
        Multiply,
        Divide,
        Sqrt,
        Percent,
        OpBracket,
        ClBracket
    }

    public class Evaluator
    {

        private readonly Dictionary<string, Func<double, double, double>> operations;
        private readonly Dictionary<string, Priority> priorities;
        private readonly string[] lexems = {"sqrt", "+", "-", "*", "/", "%", "(", ")"};

        public Evaluator()
        {
            operations = new Dictionary<string, Func<double, double, double>>()
            {
                {"+", (a, b) => a + b},
                {"-", (a, b) => b - a},
                {"*", (a, b) => a*b},
                {"/", (a, b) => (int) b/a}
            };

            priorities = new Dictionary<string, Priority>()
            {
                {"+", Priority.Low },
                {"-", Priority.Low },
                {"*", Priority.High },
                {"/", Priority.High },
                {"sqrt", Priority.High },
                {"%", Priority.High }
            };
        }

        public double Evaluate(string expression)
        {
            var polishNotation = ConvertToPolishNotation(expression);
            return CalculateFromPolishNotation(polishNotation);
        }            

        private double CalculateFromPolishNotation(List<string> tokens)
        {            
            var operands = new Stack<double>();

            foreach (var token in tokens)
            {
                operands.Push(operations.ContainsKey(token)
                    ? operations[token](operands.Pop(), operands.Pop())
                    : double.Parse(token));
            }

            return operands.Pop();

        }

        private List<string> ConvertToPolishNotation(string expression)
        {
            var res = new List<string>();
            var stack = new Stack<string>();

            return ConvertToPolishNotation(expression, res, stack, 0);
        }

        private List<string> ConvertToPolishNotation(string exp, List<string> res, Stack<string> stack , int startIndex)
        {
            if (startIndex == exp.Length)
            {
                while (stack.Count != 0)
                {
                    res.Add(stack.Pop());
                }
                return res;
            }

            var index = startIndex;

            foreach (var lexem in lexems)
            {
                var strRestLength = exp.Substring(startIndex).Length;

                if (lexem.Length <= strRestLength && lexem == exp.Substring(startIndex, lexem.Length))
                {
                    if (IsUnaryMinus(stack, lexem) || lexem == "sqrt" || lexem == "%")
                    {
                        res.Add("0");
                        stack.Push(lexem);
                    }

                    else HandleSituation(res, stack, lexem);

                    startIndex += lexem.Length;
                }
            }

            if (startIndex == index)
                res.Add(ReadNumber(exp, ref startIndex));

            SkipWhiteSpaces(exp, ref startIndex);

            return ConvertToPolishNotation(exp, res, stack, startIndex);
        }

        private void HandleSituation(List<string> res, Stack<string> stack, string lexem)
        {

            if (lexem == "(")
            {
                stack.Push(lexem);
                return;
            }

            if (lexem == ")")
            {
                while (stack.Peek() != "(")
                {
                    res.Add(stack.Pop());
                }

                stack.Pop();
                return;
            }

            if (stack.Count != 0 && priorities[lexem] <= priorities[stack.Peek()])
                res.Add(stack.Pop());

            stack.Push(lexem);
        }

        private void SkipWhiteSpaces(string exp, ref int startIndex)
        {
            while (startIndex < exp.Length && char.IsWhiteSpace(exp[startIndex]))        
                startIndex++;          
        }

        private string ReadNumber(string exp, ref int startIndex)
        {
            StringBuilder str = new StringBuilder();

            while (startIndex < exp.Length && (char.IsDigit(exp[startIndex]) || char.IsPunctuation(exp[startIndex])))
            {
                str.Append(exp[startIndex]);
                startIndex++;
            }

            return str.ToString();
        }

        private static bool IsUnaryMinus(Stack<string> stack, string lexem)
        {
            return lexem == "-" && (stack.Count == 0 || stack.Peek() == "(");
        }
    }
}