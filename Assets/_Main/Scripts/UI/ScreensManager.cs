using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimateManagement
{
	public class ScreensManager : MonoBehaviour
	{
		[SerializeField] private SelectionScreenManager selectionScreen = default;
        [SerializeField] private PlaceableButton[] placeableButtons = default;

        private void OnEnable()
        {
            CalendarController.OnYearComplete += OpenSelectionScreen;
            SelectableButton.OnTileAmountUpdated += UpdateTileCount;
        }

        private void OnDisable()
        {
            CalendarController.OnYearComplete -= OpenSelectionScreen;
            SelectableButton.OnTileAmountUpdated -= UpdateTileCount;
        }

        private void OpenSelectionScreen(int year)
        {
            selectionScreen.gameObject.SetActive(true);
            selectionScreen.SetYearText(year);
        }

        private void UpdateTileCount(int amount, TileType tileType)
        {
            selectionScreen.gameObject.SetActive(false);

            foreach (var button in placeableButtons)
            {
                if (button.TileType == tileType)
                {
                    button.UpdateAmount(amount);
                    break;
                }
            }
        }
    }
}
