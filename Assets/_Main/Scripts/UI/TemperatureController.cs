using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

namespace ClimateManagement
{
	public class TemperatureController : MonoBehaviour
	{
        [SerializeField] private int defaultTemperature = default;
		[SerializeField] private int thresholdTemperature = default;
		[SerializeField] private int requiredCountOfHouses = default;
        [SerializeField] private TextMeshProUGUI temperatureDisplay = default;
        [SerializeField] private Button temperatureButton = default;
        [SerializeField] private Vector3 buttonShrinkScale = default;
        [SerializeField] private float tweenDelay = default;
        [SerializeField] private ScreensManager screensManager = default;
        [SerializeField] private TileGenerator tileGenerator = default;

        private enum TemperatureUnit
        {
            Celcius,
            Fahrenheit
        }

        private TemperatureUnit currentTemperatureUnit;

        private int houseCounter = 0;

        private int currentTemperature = 0;
        private float tempPerc;

        private void Awake()
        {
            //TileController.OnHousePopUp += OnHouseCounterIncreased;
            TileController.OnTilePlaced += OnTilePlaced;
        }

        private void OnDestroy()
        {
            //TileController.OnHousePopUp -= OnHouseCounterIncreased;
            TileController.OnTilePlaced -= OnTilePlaced;
        }

        private void Start()
        {
            currentTemperature = defaultTemperature;
            temperatureDisplay.text = $"{defaultTemperature}°C";
            currentTemperatureUnit = TemperatureUnit.Celcius;
            temperatureButton.onClick.AddListener(() => OnTemperatureButtonClicked());
            //OnTilePlaced(TileType.None);
        }

        private void OnTilePlaced(TileType tileType)
        {
            int totalCount = tileGenerator.GetAllTiles();
            int goodCount = tileGenerator.GetAllGoodTiles();
            int badCount = tileGenerator.GetAllBadTiles();

            if (goodCount > badCount)
            {
                int diff = goodCount - badCount;
                tempPerc = 1f - ((float)diff / (float)totalCount);
            }
            else if(goodCount < badCount)
            {
                int diff = badCount - goodCount;
                tempPerc = ((float)diff / (float)totalCount);
            }
            else
            {
                tempPerc = 0.5f;
            }
            Debug.Log("Temp: " + tempPerc);
            currentTemperature = Mathf.CeilToInt(Mathf.Lerp(defaultTemperature, thresholdTemperature, tempPerc));
            temperatureDisplay.text = $"{currentTemperature}°C";

            if (tempPerc >= 1f)
            {
                screensManager.GameOver();
            }
        }

        private void OnHouseCounterIncreased()
        {
            houseCounter++;
            if (houseCounter >= requiredCountOfHouses)
            {
                currentTemperature++;
                temperatureDisplay.text = $"{currentTemperature}°C" ;
                houseCounter = 0;
            }

            if (currentTemperature >= thresholdTemperature)
            {
                screensManager.GameOver();
            }
        }

        private void OnTemperatureButtonClicked()
        {
            if (currentTemperatureUnit == TemperatureUnit.Celcius)
            {
                int fahrenheit = (currentTemperature * 9 / 5) + 32;
                temperatureDisplay.text = $"{fahrenheit}°F";
                currentTemperatureUnit = TemperatureUnit.Fahrenheit;
            }
            else
            {
                temperatureDisplay.text = $"{currentTemperature}°C";
                currentTemperatureUnit = TemperatureUnit.Celcius;
            }

            Vector3 originalButtonScale = temperatureButton.transform.localScale;

            Sequence mySequence = DOTween.Sequence();

            mySequence.Append(temperatureButton.gameObject.transform
                .DOScale(buttonShrinkScale, tweenDelay).SetEase(Ease.Linear));
            mySequence.Append(temperatureButton.gameObject.transform
                .DOScale(originalButtonScale, tweenDelay).SetEase(Ease.Linear));
        }
	}
}
