using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NiceTest
{
    internal sealed class Operation
    {
        private readonly int precedence;
        private readonly string name;
        private readonly Func<Expression, Expression, Expression> operation;

        public static readonly Operation Addition = new Operation(2, Expression.Add, "Addition");
        public static readonly Operation Subtraction = new Operation(2, Expression.Subtract, "Subtraction");
        public static readonly Operation Multiplication = new Operation(1, Expression.Multiply, "Multiplication");
        public static readonly Operation Division = new Operation(1, Expression.Divide, "Division");

        private static readonly Dictionary<char, Operation> Operations = new Dictionary<char, Operation>
    {
        { '+', Addition },
        { '-', Subtraction },
        { '*', Multiplication},
        { '/', Division }
    };

        private Operation(int precedence, Func<Expression, Expression, Expression> operation, string name)
        {
            this.precedence = precedence;
            this.operation = operation;
            this.name = name;
        }

        public int Precedence
        {
            get { return precedence; }
        }

        public static explicit operator Operation(char operation)
        {
            Operation result;

            if (Operations.TryGetValue(operation, out result))
            {
                return result;
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        public Expression Apply(Expression left, Expression right)
        {
            return operation(left, right);
        }

        public static bool IsDefined(char operation)
        {
            return Operations.ContainsKey(operation);
        }
    }
}
