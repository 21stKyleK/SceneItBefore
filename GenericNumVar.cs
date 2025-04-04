using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace MrNobody.FlatRpg.Vars
//{
    public abstract class GenericNumVar<T> : ScriptableObject
    {
        public T Value;

        public void SetValue(T v)
        {
            Value = v;
        }

        public void SetValue(GenericNumVar<T> v)
        {
            Value = v.Value;
        }

        public static implicit operator T(GenericNumVar<T> v)
        {
            return v.Value;
        }
    }

//}
