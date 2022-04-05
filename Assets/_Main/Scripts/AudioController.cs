using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimateManagement
{
	public class AudioController : MonoBehaviour
	{
        [SerializeField] private AudioSource sfx = default;
        [SerializeField] protected AudioClip calenderClip = default;
        [SerializeField] protected AudioClip replaceClip = default;

        private void Awake()
        {
            CalendarController.OnYearComplete += OnYearComplete;
            TileController.OnTilePlaced += OnTilePlaced;
        }

        private void OnDestroy()
        {
            CalendarController.OnYearComplete -= OnYearComplete;
            TileController.OnTilePlaced -= OnTilePlaced;
        }

        private void Start()
        {
            Audiomanager.FadeIn("BG", 1f);
        }

        private void OnYearComplete(int obj)
        {
            sfx.PlayOneShot(calenderClip);
        }

        private void OnTilePlaced(TileType obj)
        {
            sfx.PlayOneShot(replaceClip);
        }
    }
}
