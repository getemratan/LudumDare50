using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClimateManagement
{
    public class EfficiencyBar : MonoBehaviour
    {
        [SerializeField] private Slider slider = default;
        [SerializeField] private List<Sprite> facesImages = default;
        [SerializeField] private Image handleIcon = default;

        private void OnEnable()
        {
            TileInput.OnTileExit += SetDefaultSliderValue;
        }

        private void OnDisable()
        {
            TileInput.OnTileExit -= SetDefaultSliderValue;
        }

        public void UpdateSlider(int value)
        {
            handleIcon.sprite = value > (slider.maxValue / 2) ? facesImages[2] : facesImages[1];

            if (value == 0)
                handleIcon.sprite = facesImages[0];

            slider.value = value;
        }

        public void SetDefaultSliderValue()
        {
            slider.value = 0;
            handleIcon.sprite = facesImages[0];
        }
    }
}
