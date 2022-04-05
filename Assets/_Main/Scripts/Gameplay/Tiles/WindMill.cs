using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimateManagement
{
    public class WindMill : Tile, IGoodTile
    {
        [SerializeField] private Transform fanTransform = default;
        [SerializeField] private float fanSpeed = default;

        private void Update()
        {
            fanTransform.Rotate(Vector3.forward * Time.deltaTime * fanSpeed);
        }
    }
}