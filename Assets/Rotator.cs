using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimateManagement
{
	public class Rotator : MonoBehaviour
	{
        [SerializeField] private Vector3 angularVelocity = default;
        [SerializeField] private Space space = Space.Self;

        void Update()
        {
            transform.Rotate(angularVelocity * Time.deltaTime, space);
        }
    }
}
