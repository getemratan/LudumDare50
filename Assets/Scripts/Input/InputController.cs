//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimateManagement
{
	public class InputController : MonoBehaviour
	{
        [SerializeField] private string gridItemTag = default;
        [SerializeField] private GameObject[] gridItems = default;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform.CompareTag(gridItemTag))
                    {
                        hit.transform.GetComponentInParent<GridSlot>().SetUpGridItem(GetRandomGridItem());
                    }
                }
            }
        }

        private GameObject GetRandomGridItem()
        {
            var randomIndex = Random.Range(0, gridItems.Length);

            return gridItems[randomIndex];
        }
    }
}
