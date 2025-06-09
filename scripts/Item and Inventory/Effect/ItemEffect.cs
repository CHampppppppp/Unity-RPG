using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect : ScriptableObject
{
    public virtual void ExecuteEffect()
    {
        Debug.Log("effect executed!");
    }
}
