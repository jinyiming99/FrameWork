using System.Collections;
using System.Collections.Generic;
using Expression;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class test1
{
    // A Test behaves as an ordinary method
    [Test]
    public void test1SimplePasses()
    {
        var node = Expression.Expression.Parse($"5+5=11");
        Debug.Log(node.ToString());
        // Use the Assert class to test conditions
    }


}
