using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tile : MonoBehaviour
{
    public int tileId = default;
    [SerializeField] private MeshRenderer grassRenderer = default;
    [SerializeField] private int grassMatIndex = default;
    [SerializeField] private float animateTime = default;
    [SerializeField] private GrassColors grassColors = default;

    private MaterialPropertyBlock propertyBlock;

    private void Awake()
    {
        //UpdateGrassColor();
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

    private void UpdateGrassColor()
    {
        propertyBlock = new MaterialPropertyBlock();
        grassRenderer.GetPropertyBlock(propertyBlock, grassMatIndex);

        int rColor = Random.Range(0, grassColors.colors.Count);
        propertyBlock.SetColor("_BaseColor", grassColors.colors[rColor]);

        grassRenderer.SetPropertyBlock(propertyBlock, grassMatIndex);
    }
}