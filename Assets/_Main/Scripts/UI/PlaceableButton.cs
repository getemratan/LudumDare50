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

		public int currAmount;


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
				currAmount = Mathf.Clamp(currAmount, 0, 100);
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
			//Debug.Log("entered");
			transform.DOScale(finalScale, tweenDelay).SetEase(Ease.Linear);
		}

		public void OnPointerExit()
		{
			Debug.Log("exit");
			transform.DOScale(originalScale, tweenDelay).SetEase(Ease.Linear);
		}

        private void OnMouseOver()
        {
			Debug.Log("entered");
		}

        public void UpdateAmount(int value)
        {
			currAmount += value;
			currAmount = Mathf.Clamp(currAmount, 0, 100);
			amount.text = currAmount.ToString();
        }
	}
}
