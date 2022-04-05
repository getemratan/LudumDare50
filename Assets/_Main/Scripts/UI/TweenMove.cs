using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ClimateManagement
{
	public class TweenMove : MonoBehaviour
	{
		[SerializeField] private float finalDestination = default;
		[SerializeField] private float tweenDelay = default;
		[SerializeField] private bool isX = default;
		[SerializeField] private bool isCooldown = default;
		[SerializeField] private GameObject cooldown = default;

		public void Start()
        {
			RectTransform rectTransform = transform as RectTransform;
			if (isX && isCooldown)
            {
				rectTransform.DOAnchorPosX(finalDestination, tweenDelay).SetEase(Ease.Linear)
					.OnComplete(() => SetCooldown());
			}
			else if (isX)
            {
				rectTransform.DOAnchorPosX(finalDestination, tweenDelay).SetEase(Ease.Linear);
			}
            else
            {
				rectTransform.DOAnchorPosY(finalDestination, tweenDelay).SetEase(Ease.Linear);
			}
        }

		private void SetCooldown()
        {
			cooldown.SetActive(true);
        }
	}
}
