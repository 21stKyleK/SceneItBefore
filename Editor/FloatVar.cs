using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//namespace MrNobody.FlatRpg.Vars {
    [CreateAssetMenu(menuName = "Scriptable Variables/Float")]
    public class FloatVar : GenericNumVar<float>
    {
        //public float Value;

        //public void SetValue(float v)
        //{
        //    Value = v;
        //}

        //public void SetValue(FloatVar v)
        //{
        //    Value = v.Value;
        //}

        public void AddValue(float v)
        {
            Value += v;
        }
        public void AddValue(FloatVar v)
        {
            Value += v.Value;
        }
    }
//}
