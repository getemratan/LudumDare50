using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace ClimateManagement
{
	public class CalendarController : MonoBehaviour
	{
		public static event Action<int> OnYearComplete;

		[SerializeField] private TextMeshProUGUI calendarDate = default;
		[SerializeField] private GameObject calendarFrame = default;
		[SerializeField] private string[] monthArray = default;
		[SerializeField] private float tweenDelay = default;
		[SerializeField] private CooldownTimer cooldownTimer = default;

		private int currMonthCounter = 0;

		private int currYear = 0;

		public int CurrentYear { get => currYear; }

		public void ChangeMonth()
		{
			if (currMonthCounter == 11)
            {
				cooldownTimer.SetYearCompleteBool(true);
				currYear++;
				OnYearComplete?.Invoke(currYear);
				currMonthCounter = 0;
				calendarDate.text = monthArray[currMonthCounter];
			}
            else
            {
				calendarDate.gameObject.SetActive(false);
				calendarFrame.transform.DOLocalRotate(new Vector3(0, 0, 360), tweenDelay
					, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear)
					.OnComplete(() => ChangeText());
			}
		}

			

		private void ChangeText()
        {
			currMonthCounter++;
			calendarDate.text = monthArray[currMonthCounter];
			calendarDate.gameObject.SetActive(true);
        }
	}
}
