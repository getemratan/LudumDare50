using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimateManagement
{
	public class ScreensManager : MonoBehaviour
	{
        public static event System.Action OnGameStart;

        [SerializeField] private SelectionScreenManager selectionScreen = default;
        [SerializeField] private PlaceableButton[] placeableButtons = default;
        [SerializeField] private CooldownTimer cooldownTimer = default;
        [SerializeField] private GameObject titleScreen = default;
        [SerializeField] private TweenMove[] tweenMoves = default;
        [SerializeField] private TileGenerator tileGenerator = default;
        [SerializeField] private GameObject gameOverScreen = default;
        [SerializeField] private GameOver gameOver = default;
        [SerializeField] private CalendarController calendarController = default;

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
            cooldownTimer.SetYearCompleteBool(false);
            foreach (var button in placeableButtons)
            {
                Debug.Log(button.TileType);
                Debug.Log(tileType);

                if (button.TileType == tileType)
                {
                    button.UpdateAmount(amount);
                    break;
                }
            }
        }


        private void Update()
        {
            if (titleScreen.activeInHierarchy && Input.anyKeyDown)
            {
                titleScreen.SetActive(false);
                foreach (var item in tweenMoves)
                {
                    item.gameObject.SetActive(true);
                }
                //tileGenerator.gameObject.SetActive(true);
                OnGameStart?.Invoke();
            }
        }

        public void GameOver()
        {
            gameOver.SetScore(calendarController.CurrentYear);
            gameOverScreen.SetActive(true);
        }
    }
}
