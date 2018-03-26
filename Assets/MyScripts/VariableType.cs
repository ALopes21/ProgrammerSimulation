using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableType : MonoBehaviour {

    public enum Type
    {
        Float,
        Bool,
        GameObject,
        Sprite,
        Any,
        None
    }

    public Type type = Type.Float;
}
