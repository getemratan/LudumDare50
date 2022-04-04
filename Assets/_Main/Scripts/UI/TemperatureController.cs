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

        private enum TemperatureUnit
        {
            Celcius,
            Fahrenheit
        }

        private TemperatureUnit currentTemperatureUnit;

        private int houseCounter = 0;

        private int currentTemperature = 0;

        private void Awake()
        {
            
        }

        private void OnDestroy()
        {
            
        }

        private void Start()
        {
            currentTemperature = defaultTemperature;
            temperatureDisplay.text = $"{defaultTemperature}°C";
            currentTemperatureUnit = TemperatureUnit.Celcius;
            temperatureButton.onClick.AddListener(() => OnTemperatureButtonClicked());
        }

        private void OnHouseCounterIncreased(int value)
        {
            houseCounter += value;
            if (houseCounter >= requiredCountOfHouses)
            {
                currentTemperature++;
                temperatureDisplay.text = $"{currentTemperature}°C" ;
                houseCounter = 0;
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
