//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ClimateManagement
{
	public class SelectionScreenManager : MonoBehaviour
	{
		[SerializeField] private SelectableButton[] selectableButtons = default;
		[SerializeField] private int minAmountValue = default;
		[SerializeField] private int maxAmountValue = default;
        [SerializeField] private TextMeshProUGUI yearText = default;

        private void OnEnable()
        {
            SetUpSelectableButtons();
        }

        private void SetUpSelectableButtons()
        {
            foreach (var button in selectableButtons)
            {
                button.Amount = Random.Range(minAmountValue, maxAmountValue);
            }
        }

        public void SetYearText(int year)
        {
            yearText.text = $"YEAR {year}";
        }
    }
}
