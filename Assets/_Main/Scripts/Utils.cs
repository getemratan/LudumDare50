using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimateManagement
{
    public static class Utils
    {
        public static int GetRandomValue(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }
    }
}