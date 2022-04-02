using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimateManagement
{
	public class GridSlot : MonoBehaviour
	{
		[SerializeField] private GameObject spawnPoint = default;

		public void SetUpGridItem(GameObject gridItem) 
        {
			if (spawnPoint.transform.childCount > 0)
            {
				Destroy(spawnPoint.transform.GetChild(0).gameObject);
            }

			Instantiate(gridItem, spawnPoint.transform);
        }
	}
}
