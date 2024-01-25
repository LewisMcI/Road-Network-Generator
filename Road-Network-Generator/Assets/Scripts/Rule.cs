using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(menuName = "Road Generator/Rule")]
public class Rule : ScriptableObject
{
    // Letter to look for
    public char alphabet;
    // Value that the alphabet is replaced with
    public string value;

    public string GetValue()
    {
        return this.value;
    }

    public override string ToString()
    {
        return alphabet + " -> " + value;
    }
}
