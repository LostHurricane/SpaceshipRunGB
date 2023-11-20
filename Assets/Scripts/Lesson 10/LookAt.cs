using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace SpaceshipRunGB.Lesson10
{
    [ExecuteInEditMode]
    public class LookAt : MonoBehaviour
    {
        public Vector3 LookAtPoint = Vector3.zero;
        public void Update()
        {
            transform.LookAt(LookAtPoint);
        }
    }

}