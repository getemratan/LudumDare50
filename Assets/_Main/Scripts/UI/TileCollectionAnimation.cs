using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

namespace ClimateManagement
{
	public class TileCollectionAnimation : MonoBehaviour
	{
		[SerializeField] private GameObject selectableIconPrefab = default;
		[SerializeField] private TitlTypeSpriteDictionary tileSprites = default;
		[SerializeField] private Transform target = default;
		[SerializeField] private Transform origin = default;
		//[SerializeField] private float spread;

		[Space]
		[Header("Animation settings")]
		[SerializeField] [Range(0.5f, 0.9f)] private float minAnimDuration;
		[SerializeField] [Range(0.9f, 2f)] private float maxAnimDuration;

		[SerializeField] private Ease easeType;

		Vector3 targetPosition;
		Vector3 originPosition;

		void Awake()
		{
			originPosition = origin.localPosition;
		}

        public void Animate(int totalAmount, TileType tileType)
		{
			targetPosition = target.position;

			for (int i = 0; i < totalAmount; i++)
			{
				var icon = Instantiate(selectableIconPrefab, transform);
				//+new Vector3(Random.Range(-spread, spread), 0f, 0f);
				icon.transform.localPosition = originPosition;
				icon.GetComponent<Image>().sprite = tileSprites[tileType];

				float duration = Random.Range(minAnimDuration, maxAnimDuration);

				icon.transform.DOMove(targetPosition, duration)
				.SetEase(easeType)
				.OnComplete(() => {
						//executes whenever icon reach target position
						Destroy(icon);
				});
			}
		}
	}
}
