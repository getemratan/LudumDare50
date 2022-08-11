using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ClimateManagement
{
	public class ScreensManager : MonoBehaviour
	{
        public static event System.Action OnGameStart;
        public static event Action<bool> IsPaused;

        [SerializeField] private SelectionScreenManager selectionScreen = default;
        [SerializeField] private PlaceableButton[] placeableButtons = default;
        [SerializeField] private CooldownTimer cooldownTimer = default;
        [SerializeField] private GameObject titleScreen = default;
        [SerializeField] private GameObject helpScreen = default;
        [SerializeField] private TweenMove[] tweenMoves = default;
        [SerializeField] private TileGenerator tileGenerator = default;
        [SerializeField] private GameObject gameOverScreen = default;
        [SerializeField] private GameOver gameOver = default;
        [SerializeField] private CalendarController calendarController = default;
        [SerializeField] private Image currentTileIcon = default;
        [SerializeField] private TitlTypeSpriteDictionary tileSprites = default;
        [SerializeField] private GameObject efficiencyBar = default;
        [SerializeField] private GameObject windEffectOne = default;
        [SerializeField] private GameObject windEffectTwo = default;

        private void Start()
        {
            //currentTileIcon
        }

        private void OnEnable()
        {
            CalendarController.OnYearComplete += OpenSelectionScreen;
            SelectableButton.OnTileAmountUpdated += UpdateTileCount;
            PlaceableButton.OnTileTypeSelected += UpdateTileIcon;
        }

        private void OnDisable()
        {
            CalendarController.OnYearComplete -= OpenSelectionScreen;
            SelectableButton.OnTileAmountUpdated -= UpdateTileCount;
            PlaceableButton.OnTileTypeSelected -= UpdateTileIcon;
        }

        private void OpenSelectionScreen(int year)
        {
            selectionScreen.gameObject.SetActive(true);
            selectionScreen.SetYearText(year);
            IsPaused?.Invoke(true);
        }

        private void UpdateTileIcon(TileType newTileType)
        {
            Vector3 initialScale = new Vector3(1f, 1f, 1f);
            currentTileIcon.transform.localScale = initialScale;
            currentTileIcon.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 1.2f).SetEase(Ease.Linear);
            currentTileIcon.sprite = tileSprites[newTileType];
        }

        private void UpdateTileCount(int amount, TileType tileType)
        {
            selectionScreen.gameObject.SetActive(false);
            cooldownTimer.SetYearCompleteBool(false);
            foreach (var button in placeableButtons)
            {
                if (button.TileType == tileType)
                {
                    button.UpdateAmount(amount);
                    break;
                }
            }
            IsPaused?.Invoke(false);
        }


        private void Update()
        {
            if (titleScreen.activeInHierarchy && Input.anyKeyDown)
            {
                titleScreen.SetActive(false);

                StartCoroutine(EnableHelpScreen());
            }
        }

        IEnumerator EnableHelpScreen()
        {
            helpScreen.SetActive(true);

            yield return new WaitForSeconds(0.1f);

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

            helpScreen.SetActive(false);

            foreach (var item in tweenMoves)
            {
                item.gameObject.SetActive(true);
            }
            efficiencyBar.SetActive(true);
            windEffectOne.SetActive(true);
            windEffectTwo.SetActive(true);
            //tileGenerator.gameObject.SetActive(true);
            OnGameStart?.Invoke();
        }

        public void GameOver()
        {
            gameOver.SetScore(calendarController.CurrentYear);
            gameOverScreen.SetActive(true);
            IsPaused?.Invoke(true);
        }
    }

    [Serializable]
    public class TileSprite
    {
        public TileType tile;
        public Sprite tileSprite;
    }
}
