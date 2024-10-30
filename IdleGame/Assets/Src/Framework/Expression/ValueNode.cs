using System;

namespace Expression
{
    public struct ValueNode :ICalculateNode
    {
        public NodeType nodeType => NodeType.Value;
        public int _intValue;
        public float _floatValue;
        public bool _boolValue;
        public int _endIndex;
        public ValueType _type;

        public static ValueNode Null
        {
            get => new ValueNode(){_type = ValueType.None};
        }
        public ValueNode GetValue()
        {
            return this;
        }

        public override string ToString()
        {
            switch (nodeType)
            {
                case NodeType.Value:
                {
                    switch (_type)
                    {
                        case ValueType.INT:
                            return $"_type is int ,value is {_intValue}";
                        case ValueType.BOOL:
                            return $"_type is BOOL ,value is {_boolValue}";
                        case ValueType.FLOAT:
                            return $"_type is FLOAT ,value is {_floatValue}";
                    }
                    break;
                }
            }
            return $"node type is {nodeType}";
        }

        public ValueNode(bool value)
        {
            _boolValue = value;
            _floatValue = 0f;
            _intValue = 0;
            _endIndex = 0;
            _type = ValueType.BOOL;
        }
        public ValueNode(int value)
        {
            _intValue = value;
            _boolValue = false;
            _floatValue = 0f;
            _endIndex = 0;
            _type = ValueType.INT;
        }
        public ValueNode(float value)
        {
            _floatValue = value;
            _boolValue = false;
            _intValue = 0;
            _endIndex = 0;
            _type = ValueType.FLOAT;
        }

        public static ValueNode operator +(ValueNode left,ValueNode right)
        {
            if (left._type == ValueType.INT && right._type == ValueType.INT)
            {
                return new ValueNode() { _intValue = left._intValue + right._intValue ,_type = ValueType.INT};
            }
            else if (left._type == ValueType.FLOAT && right._type == ValueType.FLOAT)
            {
                return new ValueNode() { _floatValue = left._floatValue + right._floatValue ,_type = ValueType.FLOAT};
            }
            else if (left._type == ValueType.INT && right._type == ValueType.FLOAT)
            {
                return new ValueNode() { _floatValue = left._intValue + right._floatValue ,_type = ValueType.FLOAT};
            }
            else if (left._type == ValueType.FLOAT && right._type == ValueType.INT)
            {
                return new ValueNode() { _floatValue = left._floatValue + right._intValue ,_type = ValueType.FLOAT};
            }
            else
            {
                throw new Exception("类型错误");
            }
        }
        public static ValueNode operator -(ValueNode left,ValueNode right)
        {
            if (left._type == ValueType.INT && right._type == ValueType.INT)
            {
                return new ValueNode() { _intValue = left._intValue - right._intValue ,_type = ValueType.INT};
            }
            else if (left._type == ValueType.FLOAT && right._type == ValueType.FLOAT)
            {
                return new ValueNode() { _floatValue = left._floatValue - right._floatValue,_type = ValueType.FLOAT };
            }
            else if (left._type == ValueType.INT && right._type == ValueType.FLOAT)
            {
                return new ValueNode() { _floatValue = left._intValue - right._floatValue ,_type = ValueType.FLOAT};
            }
            else if (left._type == ValueType.FLOAT && right._type == ValueType.INT)
            {
                return new ValueNode() { _floatValue = left._floatValue - right._intValue ,_type = ValueType.FLOAT};
            }
            else
            {
                throw new Exception("类型错误");
            }
        }
        public static ValueNode operator *(ValueNode left,ValueNode right)
        {
            if (left._type == ValueType.INT && right._type == ValueType.INT)
            {
                return new ValueNode(left._intValue * right._intValue);
            }
            else if (left._type == ValueType.FLOAT && right._type == ValueType.FLOAT)
            {
                return new ValueNode(left._floatValue * right._floatValue);
            }
            else if (left._type == ValueType.INT && right._type == ValueType.FLOAT)
            {
                return new ValueNode(left._intValue * right._floatValue);
            }
            else if (left._type == ValueType.FLOAT && right._type == ValueType.INT)
            {
                return new ValueNode(left._floatValue * right._intValue);
            }
            else
            {
                throw new Exception("类型错误");
            }
        }
        public static ValueNode operator /(ValueNode left,ValueNode right)
        {
            if (left._type == ValueType.INT && right._type == ValueType.INT)
            {
                return new ValueNode(left._intValue / right._intValue);
            }
            else if (left._type == ValueType.FLOAT && right._type == ValueType.FLOAT)
            {
                return new ValueNode(left._floatValue / right._floatValue);
            }
            else if (left._type == ValueType.INT && right._type == ValueType.FLOAT)
            {
                return new ValueNode(left._intValue * 1f / right._floatValue);
            }
            else if (left._type == ValueType.FLOAT && right._type == ValueType.INT)
            {
                return new ValueNode(left._floatValue / right._intValue);
            }
            else
            {
                throw new Exception("类型错误");
            }
        }
        /// <summary>
        /// equal
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public ValueNode Equal(ValueNode right)
        {
            if (this._type == ValueType.INT && right._type == ValueType.INT)
            {
                return new ValueNode(this._intValue == right._intValue);
            }
            else if (this._type == ValueType.FLOAT && right._type == ValueType.FLOAT)
            {
                return new ValueNode(this._floatValue == right._floatValue);
            }
            else if (this._type == ValueType.BOOL && right._type == ValueType.BOOL)
            {
                return new ValueNode(this._boolValue == right._boolValue);
            }
            else
            {
                return new ValueNode(){_boolValue = false};
            }
        }
        
        public ValueNode NotEqual(ValueNode right)
        {
            if (this._type == ValueType.INT && right._type == ValueType.INT)
            {
                return new ValueNode(this._intValue != right._intValue);
            }
            else if (this._type == ValueType.FLOAT && right._type == ValueType.FLOAT)
            {
                return new ValueNode(this._floatValue != right._floatValue);
            }
            else if (this._type == ValueType.BOOL && right._type == ValueType.BOOL)
            {
                return new ValueNode(this._boolValue != right._boolValue);
            }
            else
            {
                return new ValueNode(false);
            }
        }
        public ValueNode And(ValueNode right)
        {
            if (this._type == ValueType.BOOL && right._type == ValueType.BOOL)
            {
                return new ValueNode(this._boolValue && right._boolValue);
            }
            else
            {
                return new ValueNode(false);
            }
        }
        public ValueNode Or(ValueNode right)
        {
            if (this._type == ValueType.BOOL && right._type == ValueType.BOOL)
            {
                return new ValueNode(this._boolValue || right._boolValue);
            }
            else
            {
                return new ValueNode(false);
            }
        }
        public ValueNode Not()
        {
            if (this._type == ValueType.BOOL)
            {
                return new ValueNode(!this._boolValue);
            }
            else
            {
                return new ValueNode(false);
            }
        }
        
        public ValueNode Greater(ValueNode right)
        {
            if (this._type == ValueType.INT && right._type == ValueType.INT)
            {
                return new ValueNode(this._intValue > right._intValue);
            }
            else if (this._type == ValueType.FLOAT && right._type == ValueType.FLOAT)
            {
                return new ValueNode(this._floatValue > right._floatValue);
            }
            else
            {
                return new ValueNode(false);
            }
        }
        public ValueNode Less(ValueNode right)
        {
            if (this._type == ValueType.INT && right._type == ValueType.INT)
            {
                return new ValueNode(this._intValue < right._intValue);
            }
            else if (this._type == ValueType.FLOAT && right._type == ValueType.FLOAT)
            {
                return new ValueNode(this._floatValue < right._floatValue);
            }
            else
            {
                return new ValueNode(false);
            }
        }
        public ValueNode GreaterEqual(ValueNode right)
        {
            if (this._type == ValueType.INT && right._type == ValueType.INT)
            {
                return new ValueNode(this._intValue >= right._intValue);
            }
            else if (this._type == ValueType.FLOAT && right._type == ValueType.FLOAT)
            {
                return new ValueNode(this._floatValue >= right._floatValue);
            }
            else
            {
                return new ValueNode(false);
            }
        }
        public ValueNode LessEqual(ValueNode right)
        {
            if (this._type == ValueType.INT && right._type == ValueType.INT)
            {
                return new ValueNode(this._intValue <= right._intValue);
            }
            else if (this._type == ValueType.FLOAT && right._type == ValueType.FLOAT)
            {
                return new ValueNode(this._floatValue <= right._floatValue);
            }
            else
            {
                return new ValueNode(false);
            }
        }

        public ValueNode LessOrGreater(ValueNode right)
        {
            if (this._type == ValueType.INT && right._type == ValueType.INT)
            {
                return new ValueNode(this._intValue < right._intValue || this._intValue > right._intValue);
            }
            else if (this._type == ValueType.FLOAT && right._type == ValueType.FLOAT)
            {
                return new ValueNode(this._floatValue < right._floatValue || this._floatValue > right._floatValue);
            }
            else
            {
                return new ValueNode(false);
            }
        }
    }
}