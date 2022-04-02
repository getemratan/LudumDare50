using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrassColors", menuName = "GrassColors")]
public class GrassColors : ScriptableObject
{
    public List<Color> colors = default;
}