using System;


namespace Expression
{
    
    public class Expression
    {
        public string _expression;
        public static char[] _spanBytes = new char[256];
        
        
        public static ValueNode Parse(string expression)
        {
            if (IsValidExpression(expression,0))
            {
                int length = 0;
                for (var i = 0; i < expression.Length; i++)
                {
                    char c = expression[i];
                    if (c != ' ')
                        length++;
                }
                Span<char> span1 = new Span<char>(_spanBytes);
                var index = 0;
                for (var i = 0; i < expression.Length; i++)
                {
                    char c = expression[i];
                    if (c != ' ' )
                    {
                        if (c >= 'A' && c <= 'Z')
                            c = (char) (c + 32);
                        span1[index++] = c;
                    }
                }
                var result = ParseExpression(span1, 0, length);
                return result.GetValue();
            }
            else
            {
                throw new Exception("表达式不合法");
            }
        }
        
        private static bool IsValidExpression(ReadOnlySpan<char> expression,int start)
        {
            int count = 0;
            for (int i = start; i < expression.Length; i++)
            {
                char c = expression[i];
                if (c == '(')
                {
                    count++;
                }
                else if (c == ')')
                {
                    count--;
                }
                if (count < 0)
                {
                    return false;
                }
            }
            return count == 0;
        }
        
        private static ExpressionNode ParseExpression(ReadOnlySpan<char> expression,int start,int end)
        {
            ExpressionNode node = ExpressionNode.Create();
            
            int offset = start;
            
            for (int i = start; i < end; i++)
            {
                char c = expression[i];
                if (c == '(')
                {
                    if (i >= offset)
                    {
                        var child = ParseExpression(expression, i + 1, end);
                        offset = i;
                        i = child.endIndex + 1;
                        node._CalculateNodes.Add(child);
                    }
                }
                else if (c == ')')
                {
                    node.endIndex = i - 1;
                    return node;
                }
                else if (c == ' ')
                {
                    continue;
                }
                else if (char.IsDigit(c))
                {
                    ValueNode v = ParseValue(expression, i);

                    if (v._type != ValueType.None)
                    {
                        i = v._endIndex;
                        node._CalculateNodes.Add(v);
                    }
                }
                else
                {
                    var o = ParseOperator(expression, i);
                    if (o._enum != OperatorEnum.None)
                    {
                        i = o.endIndex ;
                        node._CalculateNodes.Add(o);
                    }
                    
                }
            }
            return node;
        }
        private static ValueNode ParseValue(ReadOnlySpan<char> expression, int start)
        {
            if (!char.IsDigit(expression[start]))
                return ValueNode.Null;
            ValueNode node = new ValueNode();
            int number = 0;
            int index = start;
            bool isFloat = false;
            char c = expression[index];
            double dNumber = double.NegativeInfinity;
            int pow = -1;
            while (index < expression.Length)
            {
                c = expression[index];
                if (!(char.IsDigit(c) || c == '.'))
                {
                    break;
                }
                if (c == '.')
                {
                    isFloat = true;
                    dNumber = number;
                    index++;
                    continue;
                }

                if (isFloat)
                {
                    dNumber = dNumber + (c - '0') * 1f * Math.Pow(10, pow--);
                }
                else
                    number = number * 10 + (expression[index] - '0');
                index++;
            }
            node._endIndex = index - 1;
            if (isFloat)
            {
                node._floatValue = (float)dNumber;
                node._type = ValueType.FLOAT;
            }
            else
            {
                node._intValue = number;
                node._type = ValueType.INT;
            }
            return node;
        }
        private static OperatorNode ParseOperator(ReadOnlySpan<char> expression, int index)
        {
            char c1 = expression[index];
            char c2 = index + 1 >= expression.Length ? ' ' : expression[index + 1];
            char c3 = index + 2 >= expression.Length ? ' ' : expression[index + 2];
            if (c1 == '(' || c1 == ')')
                return OperatorNode.Null;
            OperatorNode node = new OperatorNode();
            if (c1 == '+' )
            {
                node.endIndex = index;
                node._enum = OperatorEnum.Add;
            }
            else if (c1 == '-')
            {
                node.endIndex = index;
                node._enum = OperatorEnum.Sub;
            }
            else if (c1 == '/')
            {
                node.endIndex =index ;
                node._enum = OperatorEnum.Div;
            }
            else if (c1 == '*')
            {
                node.endIndex =index ;
                node._enum = OperatorEnum.Mul;
            }
            else if (c1 == '!' && c2 == '=')
            {
                node.endIndex =index +1 ;
                node._enum = OperatorEnum.NotEqual;
            }
            else if ((c1 == '=' && c2 == '='))
            {
                node.endIndex =index +1;
                node._enum = OperatorEnum.Equal;
            }
            else if (c1 == '=')
            {
                node.endIndex =index;
                node._enum = OperatorEnum.Equal;
            }
            else if (c1 == '>')
            {
                if (c2 == '=')
                {
                    node.endIndex =index +1;
                    node._enum = OperatorEnum.GreaterEqual;
                }
                else
                {
                    node.endIndex =index ;
                    node._enum = OperatorEnum.Greater;
                }
            }
            else if (c1 == '<')
            {
                if (c2 == '=')
                {
                    node.endIndex =index +1;
                    node._enum = OperatorEnum.LessEqual;
                }
                else if (c2 == '>')
                {
                    node.endIndex =index +1;
                    node._enum = OperatorEnum.LessOrGreater;
                }
                else
                {
                    node.endIndex =index ;
                    node._enum = OperatorEnum.Less;
                }
            }
            else if (c1 == '!')
            {
                node.endIndex =index ;
                node._enum = OperatorEnum.Not;
            }
            else if (c1 == '&')
            {
                node.endIndex =index ;
                node._enum = OperatorEnum.And;
            }
            else if (c1 == '|')
            {
                node.endIndex =index ;
                node._enum = OperatorEnum.Or;
            }
            else if (c1 == '%')
            {
                node.endIndex =index ;
                node._enum = OperatorEnum.Mod;
            }
            else if (c1 == 'a' && c2 == 'n' && c3 == 'd')
            {
                node.endIndex =index +2;
                node._enum = OperatorEnum.And;
            }
            else if (c1 == 'o' && c2 == 'r')
            {
                node.endIndex =index +1;
                node._enum = OperatorEnum.Or;
            }
            else if (c1 == 'n' && c2 == 'o' && c3 == 't')
            {
                node.endIndex =index +2;
                node._enum = OperatorEnum.Not;
            }
            else
            {
                return OperatorNode.Null;
            }

            return node;
        }
    }
}