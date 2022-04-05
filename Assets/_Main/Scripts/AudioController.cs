using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimateManagement
{
	public class AudioController : MonoBehaviour
	{
        private void Start()
        {
            Audiomanager.FadeIn("BG", 1f);
        }
    }
}
