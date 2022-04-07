using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace ClimateManagement
{
	public class SelectableButton : MonoBehaviour
	{
		public static event Action<int, TileType> OnTileAmountUpdated;

		[SerializeField] private Vector3 finalScale = default;
		[SerializeField] private float tweenDelay = default;
		[SerializeField] private TileType tileType = default;
		[SerializeField] private TextMeshProUGUI amountText = default;
		[SerializeField] private TileCollectionAnimation tileCollectionAnimation = default;

		private Vector3 originalScale;

		public int Amount { get; set; }

		private void Start()
		{
			amountText.text = $"+{Amount}";
			originalScale = transform.localScale;
		}

		public void OnSelectableButtonClicked()
		{
			transform.DOScale(originalScale, tweenDelay).SetEase(Ease.Linear);
			OnTileAmountUpdated?.Invoke(Amount, tileType);
			tileCollectionAnimation.Animate(Amount, tileType);
		}

		public void OnSelectablePointerEntered()
		{
			transform.DOScale(finalScale, tweenDelay).SetEase(Ease.Linear);
		}

		public void OnSelectablePointerExit()
		{
			transform.DOScale(originalScale, tweenDelay).SetEase(Ease.Linear);
		}
	}
}
