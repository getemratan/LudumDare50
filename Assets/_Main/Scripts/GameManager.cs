using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimateManagement
{
	public class GameManager : MonoBehaviour
	{
		public static event System.Action OnGameStart;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(2f);
            OnGameStart?.Invoke();
        }
    }
}