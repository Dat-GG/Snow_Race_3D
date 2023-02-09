using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase.Base.Controller;
namespace UnityBase.Base.UI.Effect
{
    public class EffectFlyShow : BehaviourController,IEffect
    {
        [SerializeField]
        float firstY, lastY;

        public void HideEffect()
        {
            transform.position = new Vector3(transform.position.x, lastY, transform.position.z);
            Vector3 newPos = new Vector3(transform.position.x, firstY, transform.position.z);

            MoveUpdateLocal(newPos);
        }

        public void ShowEffect()
        {
            transform.position = new Vector3(transform.position.x, firstY, transform.position.z);
            Vector3 newPos = new Vector3(transform.position.x,lastY,transform.position.z);
            
            MoveUpdateLocal(newPos);
        }
    }

}
