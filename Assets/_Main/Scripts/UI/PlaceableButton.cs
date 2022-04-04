using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ClimateManagement
{
	public class PlaceableButton : MonoBehaviour
	{
		public static event Action<TileType> OnTileTypeSelected;

		[SerializeField] private Vector3 finalScale = default;
		[SerializeField] private float tweenDelay = default;
		[SerializeField] private TileType tileType = default;

		private Vector3 originalScale;

        private void Start()
        {
			originalScale = transform.localScale;
        }

        public void OnPlaceableButtonClicked()
		{
			transform.DOScale(originalScale, tweenDelay).SetEase(Ease.Linear);
			transform.GetChild(0).gameObject.SetActive(true);
			OnTileTypeSelected?.Invoke(tileType);
		}

		public void OnPointerEntered()
        {
			if (transform.GetChild(0).gameObject.activeInHierarchy)
            {
				//CursorManager.Instance.SetActiveCursorType(CursorType.CantSelect);
            }
            else
            {
				transform.DOScale(finalScale, tweenDelay).SetEase(Ease.Linear);
			}	
        }

		public void OnPointerExit()
		{
			if (transform.GetChild(0).gameObject.activeInHierarchy)
			{
				//CursorManager.Instance.SetActiveCursorType(CursorType.Arrow);
			}
			transform.DOScale(originalScale, tweenDelay).SetEase(Ease.Linear);
		}
	}
}
