using System;
using System.Collections.Generic;

namespace Expression
{
    public class ExpressionNode : ICalculateNode
    {
        public NodeType nodeType => NodeType.Expression;
        public int endIndex;
        public List<ICalculateNode> _CalculateNodes;
        //public static ClassPool<ExpressionNode> pool = new ClassPool<ExpressionNode>();
        public static ExpressionNode Create()
        {
            var node = new ExpressionNode(); //pool.Pop();////ClassPoolManager.GetPool<ExpressionNode>().Pop();
            if (node._CalculateNodes == null)
                node._CalculateNodes = new List<ICalculateNode>();
            node.Clear();
            return node;
        }

        public void Clear()
        {
            _CalculateNodes.Clear();
        }

        public ValueNode GetValue()
        {

            for (int i = 0; i < _CalculateNodes.Count; i++)
            {
                var node = _CalculateNodes[i];
                if (node.nodeType == NodeType.Expression)
                {
                    _CalculateNodes[i] = node.GetValue();
                }
            }
            
            while (_CalculateNodes.Count > 1)
            {
                OperatorNode operatorNode = OperatorNode.Null ;
                ValueNode left = ValueNode.Null;
                ValueNode right = ValueNode.Null;
                int calculateLevel = 999;
                int index = 0;
                for (int i = 0 ; i< _CalculateNodes.Count; i++)
                {
                    if (_CalculateNodes[i].nodeType == NodeType.Operator)
                    {
                        var tempOperatorNode = (OperatorNode)_CalculateNodes[i];
                        var level = CalculateLevelDefine.OperatorLevel[(int)tempOperatorNode._enum];
                        if (calculateLevel > level)
                        {
                            calculateLevel = level;
                            operatorNode = tempOperatorNode;
                            if (tempOperatorNode._enum != OperatorEnum.Not)
                                left = _CalculateNodes[i - 1].GetValue();
                            if (_CalculateNodes[i + 1].nodeType  != NodeType.Operator)
                                right = _CalculateNodes[i + 1].GetValue();
                            index = i;
                        }
                    }
                }
                
                ValueNode valueNode = GetValue(left, operatorNode, right);
                if (operatorNode._enum == OperatorEnum.Not)
                {
                    _CalculateNodes[index] = valueNode;
                    RemoveAt(index+1);
                }
                // else if (operatorNode._enum == OperatorEnum.Add && !valueNode._boolValue)
                //     return new ValueNode(false);
                else
                {
                    _CalculateNodes[index - 1] = valueNode;
                    RemoveAt(index);
                    RemoveAt(index);
                }
                
                if (_CalculateNodes.Count == 1)
                {
                    return _CalculateNodes[0].GetValue();
                }
            }
            return default;
        }

        private void RemoveAt(int index)
        {
            // if (_CalculateNodes[index].nodeType == NodeType.Expression)
            //     pool.Push((ExpressionNode)_CalculateNodes[index]);
            _CalculateNodes.RemoveAt(index);
        }

        public ValueNode GetValue(ValueNode left, OperatorNode operatorNode, ValueNode right)
        {
            ValueNode valueNode = new ValueNode();
            switch (operatorNode._enum)
            {
                case OperatorEnum.Add:
                    valueNode = left + right;
                    break;
                case OperatorEnum.Sub:
                    valueNode = left - right;
                    break;
                case OperatorEnum.Mul:
                    valueNode = left * right;
                    break;
                case OperatorEnum.Div:
                    valueNode = left / right;
                    break;
                case OperatorEnum.Equal:
                    valueNode = left.Equal(right);
                    break;
                case OperatorEnum.NotEqual:
                    valueNode = left.NotEqual(right);
                    break;
                case OperatorEnum.And:
                    valueNode = left.And(right);
                    break;
                case OperatorEnum.Or:
                    valueNode = left.Or(right);
                    break;
                case OperatorEnum.Not:
                    valueNode = right.Not();
                    break;
                case OperatorEnum.LessEqual:
                    valueNode = left.LessEqual(right);
                    break;
                case OperatorEnum.LessOrGreater:
                    valueNode = left.LessOrGreater(right);
                    break;
                case OperatorEnum.Less:
                    valueNode = left.Less(right);
                    break;
                case OperatorEnum.GreaterEqual:
                    valueNode = left.GreaterEqual(right);
                    break;
                case OperatorEnum.Greater:
                    valueNode = left.Greater(right);
                    break;
            }

            return valueNode;
        }
    }
}