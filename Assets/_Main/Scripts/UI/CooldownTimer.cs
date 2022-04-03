using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClimateManagement
{
	public class CooldownTimer : MonoBehaviour
	{
        [SerializeField] private float maxTime = default;
        [SerializeField] private Image fillImage = default;

        private bool timerIsRunning = false;

        private float timerTime;

        private void OnEnable()
        {
            StartCoolDownTimer();
        }

        // Update is called once per frame
        void Update()
        {
            if (timerIsRunning)
            {
                if (timerTime > 0)
                {
                    timerTime -= Time.deltaTime;
                    fillImage.fillAmount = timerTime / maxTime;
                }
                else
                {
                    Debug.Log("Time has run out!");
                    timerTime = 0;
                    timerIsRunning = false;
                    GetComponentInParent<Button>().interactable = true;
                    gameObject.SetActive(false);
                }
            }
        }

        public void StartCoolDownTimer()
        {
            timerTime = maxTime;
            timerIsRunning = true;
            CursorManager.Instance.SetActiveCursorType(CursorType.CantSelect);
            GetComponentInParent<Button>().interactable = false;
        }
    }
}
