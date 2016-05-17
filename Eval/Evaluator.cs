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

    public class Evaluator
    {

        private readonly Dictionary<string, Func<double, double, double>> operations;
        private readonly Dictionary<string, Priority> priorities;

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
                {"/", Priority.High }
            };
        }

        public double Evaluate(string expression)
        {
            var polishNotation = ConvertToPolishNotation(expression);
            return CalculateFromPolishNotation(polishNotation);
        }

        private string ConvertToPolishNotation(string expression)
        {
            var outQueue = new StringBuilder();
            var operators = new Stack<string>();

            foreach (var token in expression)
            {
                if(char.IsDigit(token)) outQueue.Append(token + " ");                
                else if(operations.ContainsKey(token.ToString()))
                HandleSituation(outQueue, operators, token.ToString());
            }

            while (operators.Count != 0)
            {
                outQueue.Append(operators.Pop() + " ");
            }

            return outQueue.ToString();
        }

        private void HandleSituation(StringBuilder outQueue, Stack<string> operators, string token)
        {
            if (operators.Count != 0 && priorities[token] <= priorities[operators.Peek()])          
                outQueue.Append(operators.Pop() + " ");               
            
            operators.Push(token);
        }

        private double CalculateFromPolishNotation(string expression)
        {
            var tokens = expression.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries);
            var operands = new Stack<double>();

            foreach (var token in tokens)
            {
                operands.Push(operations.ContainsKey(token)
                    ? operations[token](operands.Pop(), operands.Pop())
                    : double.Parse(token));
            }

            return operands.Pop();

        }
    }
}