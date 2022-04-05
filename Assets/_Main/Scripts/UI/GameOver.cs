using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ClimateManagement
{
	public class GameOver : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI scoreText = default;
		[SerializeField] private Button replayButton = default;

        private void Start()
        {
            replayButton.onClick.AddListener(() => OnReplayButtonClicked());
        }

        private void OnReplayButtonClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void SetScore(int score)
        {
			scoreText.text = $"you delayed climate change by\n{score} Years";
        }
	}
}
