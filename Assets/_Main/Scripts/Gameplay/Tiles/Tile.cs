using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ClimateManagement
{
    public class Tile : MonoBehaviour
    {
        private float animateTime = default;

        private void Awake()
        {
            animateTime = 0.5f;
            AnimateChildren();
        }

        private void AnimateChildren()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                Vector3 childScale = child.localScale;
                child.localScale = Vector3.zero;
                child.DOScale(childScale, 0.3f).SetDelay(i * (animateTime / (float)transform.childCount));
            }
        }
    }
}