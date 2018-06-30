using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableType : MonoBehaviour {

    public enum Type
    {
        Vector2,
        Bool,
        GameObject,
        Sprite,
        Any,
        None,
        Layered,
        Int
    }

    public Type type = Type.Vector2;
}
