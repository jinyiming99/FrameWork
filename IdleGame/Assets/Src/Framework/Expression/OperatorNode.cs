using System;

namespace Expression
{
    public struct OperatorNode :ICalculateNode
    {
        public NodeType nodeType => NodeType.Operator;
        public int endIndex;
        public OperatorEnum _enum;
        public ValueNode GetValue()
        {
            throw new NotImplementedException();
        }
        public static OperatorNode Null => new OperatorNode(){_enum = OperatorEnum.None};
        
    }
}