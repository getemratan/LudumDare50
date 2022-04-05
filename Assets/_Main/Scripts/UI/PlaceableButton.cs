using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace ClimateManagement
{
	public class PlaceableButton : MonoBehaviour
	{
		public static event Action<TileType> OnTileTypeSelected;

		[SerializeField] private Vector3 finalScale = default;
		[SerializeField] private float tweenDelay = default;
		[SerializeField] private TileType tileType = default;
		[SerializeField] private TextMeshProUGUI amount = default;
		[SerializeField] private int defaultAmount = default;
		[SerializeField] private TileController tileController = default;

		private Vector3 originalScale;

		public TileType TileType { get => tileType; }

		private int currAmount;


		private void Start()
		{
			currAmount = defaultAmount;
			originalScale = transform.localScale;
			amount.text = currAmount.ToString();
		}

        private void OnEnable()
        {
			TileController.OnTilePlaced += DeductAmount;
        }

        private void OnDisable()
        {
			TileController.OnTilePlaced -= DeductAmount;
		}

		private void DeductAmount(TileType currTileType)
        {
			if (currTileType == tileType)
            {
				currAmount--;
				amount.text = currAmount.ToString();
			}
        }

        public void OnPlaceableButtonClicked()
		{
			transform.DOScale(originalScale, tweenDelay).SetEase(Ease.Linear);
			OnTileTypeSelected?.Invoke(tileType);
		}

		public void OnPointerEntered()
		{
			transform.DOScale(finalScale, tweenDelay).SetEase(Ease.Linear);
		}

		public void OnPointerExit()
		{
			transform.DOScale(originalScale, tweenDelay).SetEase(Ease.Linear);
		}

		public void UpdateAmount(int value)
        {
			currAmount += value;
			amount.text = currAmount.ToString();
        }
	}
}
