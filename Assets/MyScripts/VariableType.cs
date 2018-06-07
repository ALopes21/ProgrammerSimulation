using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableType : MonoBehaviour {

    public enum Type
    {
        Float,
        Char,
        Bool,
        GameObject,
        Sprite,
        Any,
        None,
        Condition,
        Loop
    }

    public Type type = Type.Float;
}
