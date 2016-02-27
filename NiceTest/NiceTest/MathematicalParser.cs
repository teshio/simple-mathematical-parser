using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;

namespace NiceTest
{
    public class MathematicalParser
    {
        private Stack<Expression> expressionStack = new Stack<Expression>();
        private Stack<char> operatorStack = new Stack<char>();

        private Dictionary<string, string> mappings = new Dictionary<string, string>
        {
            { "a", "+" },
            { "b", "-" },
            { "c", "*" },
            { "d", "/" },
            { "e", "(" },
            { "f", ")" }
        };

        public int Parse(string input)
        {
            var convertedInput = input;

            foreach(var k in mappings.Keys)
            {
                convertedInput = convertedInput.Replace(k, mappings[k]);
            }

            return EvaluateParsedExpression(convertedInput);
        }

        private int EvaluateParsedExpression(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                return 0;
            }

            Debug.WriteLine(expression);

            operatorStack.Clear();
            expressionStack.Clear();

            using (var reader = new StringReader(expression))
            {
                int peek;
                while ((peek = reader.Peek()) > -1)
                {
                    var next = (char)peek;

                    if (char.IsDigit(next))
                    {
                        expressionStack.Push(ReadOperand(reader));
                        continue;
                    }

                    if (Operation.IsDefined(next))
                    {
                        var currentOperation = ReadOperation(reader);

                        //EvaluateWhile(() => operatorStack.Count > 0 && operatorStack.Peek() != '(' &&
                        //                  currentOperation.Precedence <= ((Operation)operatorStack.Peek()).Precedence);
                        EvaluateWhile(() => operatorStack.Count > 0 && operatorStack.Peek() != '(');

                        operatorStack.Push(next);
                        continue;
                    }

                    if (next == '(')
                    {
                        reader.Read();
                        operatorStack.Push('(');
                        continue;
                    }

                    if (next == ')')
                    {
                        reader.Read();
                        EvaluateWhile(() => operatorStack.Count > 0 && operatorStack.Peek() != '(');
                        operatorStack.Pop();
                        continue;
                    }

                    if (next != ' ')
                    {
                        throw new ArgumentException(string.Format("Encountered invalid character {0}", next), "expression");
                    }
                }
            }

            EvaluateWhile(() => operatorStack.Count > 0);

            var compiled = Expression.Lambda<Func<int>>(expressionStack.Pop()).Compile();
            return compiled();
        }


        private Expression ReadOperand(TextReader reader)
        {
            var operand = string.Empty;

            int peek;
            while ((peek = reader.Peek()) > -1)
            {
                var next = (char)peek;

                if (char.IsDigit(next))
                {
                    reader.Read();
                    operand += next;
                }
                else
                {
                    break;
                }
            }

            return Expression.Constant(int.Parse(operand));
        }

        private Operation ReadOperation(TextReader reader)
        {
            var operation = (char)reader.Read();
            return (Operation)operation;
        }

        private void EvaluateWhile(Func<bool> condition)
        {
            while (condition())
            {
                var right = expressionStack.Pop();
                var left = expressionStack.Pop();

                expressionStack.Push(((Operation)operatorStack.Pop()).Apply(left, right));
            }
        }
    }
}
