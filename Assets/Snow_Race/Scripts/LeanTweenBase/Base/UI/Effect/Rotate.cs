using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeMonster
{
    public class Rotate : MonoBehaviour
    {
        [SerializeField] float speed = 10f;
    
        private void LateUpdate()
        {
            transform.Rotate(0, 0, -speed);
        }
    }
}
