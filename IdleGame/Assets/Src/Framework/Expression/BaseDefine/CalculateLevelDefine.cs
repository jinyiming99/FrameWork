using System.Collections.Generic;

using Expression;

public class CalculateLevelDefine
{
    public static int[] OperatorLevel = new int[]
    {
        -1,  //None
        5,   //Add
        5,   //Sub
        4,
        4,
        4,
        12,
        13,
        3,
        8,
        8,
        4,
        4,
        4,
        4,
        4
    };
    // {
    //     {(int)OperatorEnum.Add,5},
    //     {(int)OperatorEnum.Sub,5},
    //     {(int)OperatorEnum.Mul,4},
    //     {(int)OperatorEnum.Div,4},
    //     {(int)OperatorEnum.Mod,4},
    //     {(int)OperatorEnum.And,12},
    //     {(int)OperatorEnum.Or,13},
    //     {(int)OperatorEnum.Not,3},
    //     {(int)OperatorEnum.Equal,8},
    //     {(int)OperatorEnum.NotEqual,8},
    //     {(int)OperatorEnum.Greater,4},
    //     {(int)OperatorEnum.Less,4},
    //     {(int)OperatorEnum.GreaterEqual,4},
    //     {(int)OperatorEnum.LessEqual,4},
    // };
}