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
        [SerializeField] CalendarController calendarController = default;

        private bool timerIsRunning = false;

        private float timerTime;

        private bool yearComplete = false;

        private void Start()
        {
            StartCoolDownTimer();
        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

        public void SetYearCompleteBool(bool value)
        {
            if (!value)
            {
                StartCoolDownTimer();
            }

            yearComplete = value;
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
                    calendarController.ChangeMonth();
                    //GetComponentInParent<Button>().interactable = true;
                    //gameObject.SetActive(false);
                    fillImage.fillAmount = 1;
                    if (!yearComplete)
                    {
                        StartCoolDownTimer();
                    }
                }
            }
        }

        public void StartCoolDownTimer()
        {
            timerTime = maxTime;
            timerIsRunning = true;
            //CursorManager.Instance.SetActiveCursorType(CursorType.CantSelect);
            //GetComponentInParent<Button>().interactable = false;
        }
    }
}
