namespace Expression
{
    public interface ICalculateNode
    {
        //计算节点
        public ValueNode GetValue();

        public NodeType nodeType { get; }
    }
}
