using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace ClimateManagement
{
    public class Highlights : MonoBehaviour
    {
        [SerializeField] GameObject plusVFXPrefab;
        [SerializeField] float animationTime;
        [SerializeField] float timeBeforeDisappearance;
        [SerializeField] float yRiseDistance;

        //public override void Awake()
        //{
        //    base.Awake();
        //    //plusVFXPool = new GameObjectPool(plusVFXPrefab, 5, transform);
        //}

        //public static GameObject SpawnVFX(Vector3 position)
        //{
        //    GameObject VFXGo = Instance.plusVFXPool.GetObject();
        //    VFXGo.transform.position = position;

        //    //TMP_Text textValue = VFXGo.GetComponentInChildren<TMP_Text>();
        //    //textValue.text = "+" + value.ToString();
        //    //textValue.color = new Color(textValue.color.r, textValue.color.g, textValue.color.b, 1);

        //    //Image heartImage = VFXGo.GetComponentInChildren<Image>();
        //    //heartImage.color = Color.white;

        //    Sequence sequence = DOTween.Sequence();
        //    sequence.Append(VFXGo.transform.DOMoveY(position.y + Instance.yRiseDistance, Instance.animationTime)
        //        .SetEase(Ease.OutCubic));
        //    //sequence.Append(heartImage.DOFade(0, Instance.timeBeforeDisappearance));
        //    sequence.AppendCallback(() => Instance.plusVFXPool.ReturnObject(VFXGo));

        //    return VFXGo;
        //}

        public void InstantiatePlus(Vector3 position)
        {
            GameObject VFXGo = Instantiate(plusVFXPrefab, transform);
            VFXGo.transform.position = position;

            //TMP_Text textValue = VFXGo.GetComponentInChildren<TMP_Text>();
            //textValue.text = "+" + value.ToString();
            //textValue.color = new Color(textValue.color.r, textValue.color.g, textValue.color.b, 1);

            //Image heartImage = VFXGo.GetComponentInChildren<Image>();
            //heartImage.color = Color.white;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(VFXGo.transform.DOMoveY(position.y + yRiseDistance, animationTime)
                .SetEase(Ease.OutCubic));
            //sequence.Append(heartImage.DOFade(0, Instance.timeBeforeDisappearance));
            sequence.AppendCallback(() => Destroy(VFXGo));
        }
    }
}