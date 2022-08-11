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
        [SerializeField] protected AudioClip treePlaceClip = default;
        [SerializeField] protected AudioClip recyclePlaceClip = default;
        [SerializeField] protected AudioClip windmillPlaceClip = default;

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
            if (obj == TileType.Tree)
            {
                sfx.PlayOneShot(treePlaceClip);
            }
            else if (obj == TileType.Windmill)
            {
                sfx.PlayOneShot(windmillPlaceClip);
            }
            else if (obj == TileType.WasteCollection)
            {
                sfx.PlayOneShot(recyclePlaceClip);
            }
        }
    }
}
