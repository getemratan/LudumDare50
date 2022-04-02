using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public static class CustomUtilities
{
    // a timer function
    //use as follows:
    //StartCoroutine(CallbackTimer(*time value*, *function name without ()*);
    //no parameters can be passed to callback though :/
    public static IEnumerator CallbackTimer(float time, Action callback)
    {
        //Debug.Log("CallbackTimer called!!");
        yield return new WaitForSeconds(time);
        //Debug.Log("After Yield Return");
        callback();
    }

    public static IEnumerator BoolTimer(float time, Action<bool> boolFunc)
    {
        boolFunc(false);
        yield return new WaitForSeconds(time);
        boolFunc(true);
    }


    //rounds down float position to corresponding integer/grid position
    public static Vector2Int GetGridPosition(Vector2 originalPosition)
    {
        return new Vector2Int((int)Mathf.Floor(originalPosition.x),
                              (int)Mathf.Floor(originalPosition.y));
    }

    public class CustomPool<TObject> where TObject : new()
    {
        public List<TObject> activeObjects = new List<TObject>();
        public List<TObject> sleepingObjects = new List<TObject>();

        public CustomPool(int count)
        {
            for (int i = 0; i < count; i++) sleepingObjects.Add(new TObject());
        }
    }

    public class GameObjectPool
    {
        public GameObject prefab { get; private set; }
        public Transform objectParent { get; private set; }

        private List<GameObject> _activeObjects = new List<GameObject>();
        private Queue<GameObject> _sleepingObjects = new Queue<GameObject>();

        public IReadOnlyCollection<GameObject> activeObjects { get { return _activeObjects; } }

        public GameObjectPool(GameObject p_prefab, Transform parent, int count)
        {
            prefab = p_prefab;
            objectParent = parent;
            InstantiateObjects(count);
        }

        public GameObject GetObject()
        {
            if(_sleepingObjects.Count < 1)
                InstantiateObjects(1);

            GameObject awakened = _sleepingObjects.Dequeue();
            awakened.SetActive(true);
            _activeObjects.Add(awakened);
            return awakened;
        }

        public void ReleaseObject(GameObject returned)
        {
            if (_activeObjects.Contains(returned))
            {
                _activeObjects.Remove(returned);
                returned.SetActive(false);
                _sleepingObjects.Enqueue(returned);
            }

            else Debug.LogError("Returned an object that is not part of the pool!");
        }

        public void InstantiateObjects(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject newGameObject = MonoBehaviour.Instantiate(prefab);
                newGameObject.transform.SetParent(objectParent);
                newGameObject.SetActive(false);
                _sleepingObjects.Enqueue(newGameObject);
            }
        }
    }

    public static int RoundToNearestMultipleOf(float value, int factor)
    {
        int lower = Mathf.RoundToInt(Mathf.Floor(value / factor));
        int upper = lower + 1;

        return (Mathf.Abs(value - lower * factor) < Mathf.Abs(value - upper * factor))
            ?
            lower * factor : upper * factor;
    }

    public static void SetImageAlpha(Image image, float alpha)
    {
        var newColor = image.color;
        newColor.a = alpha;
        image.color = newColor;
    }

    public static IEnumerator FadeOutImage(Image fadeImage, float fadeTime, float targetAlpha = 0)
    {
        fadeTime = Mathf.Clamp(fadeTime, 0, 100);
        targetAlpha = Mathf.Clamp(targetAlpha, 0, fadeImage.color.a);
        float deltaAlpha = fadeImage.color.a - targetAlpha;

        while (fadeImage.color.a > targetAlpha)
        {
            float newAlpha = fadeImage.color.a - deltaAlpha * (Time.deltaTime / fadeTime);
            SetImageAlpha(fadeImage, newAlpha);
            yield return null;
        }
        SetImageAlpha(fadeImage, targetAlpha);
    }

    public static IEnumerator FadeInImage(Image fadeImage, float fadeTime, float targetAlpha = 1)
    {
        fadeTime = Mathf.Clamp(fadeTime, 0, 100);
        targetAlpha = Mathf.Clamp(targetAlpha, fadeImage.color.a, 1);
        float deltaAlpha = targetAlpha - fadeImage.color.a;

        while (fadeImage.color.a <= targetAlpha)
        {
            float newAlpha = fadeImage.color.a + deltaAlpha * (Time.deltaTime / fadeTime);
            SetImageAlpha(fadeImage, newAlpha);
            yield return null;
        }
        SetImageAlpha(fadeImage, targetAlpha);
    }

    public class Timer
    {
        public bool isFinished { get { CheckTimer(); return _isFinished; } }
        private bool _isFinished;

        public bool isRunning { get { CheckTimer(); return _isRunning; } }
        private bool _isRunning;

        private float startTime;
        private float timerValue;

        public Timer()
        {
            Reset();
        }

        public void Start(float time)
        {
            _isFinished = false;
            timerValue = time;
            startTime = Time.time;
            _isRunning = true;
        }

        public void Reset()
        {
            _isRunning = false;
            _isFinished = false;
        }

        public void SetFinished()
        {
            _isRunning = false;
            _isFinished = true;
        }

        private void CheckTimer()
        {
            if (_isRunning && Time.time >= startTime + timerValue)
            {
                _isFinished = true;
                _isRunning = false;
            }
        }
    }

    public static string[] AddStringToArray(string[] array, string newElement)
    {
        string[] retval = new string[array.Length + 1];
        for (int i = 0; i < array.Length; i++)
            retval[i] = array[i];
        retval[array.Length] = newElement;
        return retval;
    }

    public static Transform GetTopParent(Transform transform)
    {
        if (transform.parent != null)
            return GetTopParent(transform.parent);
        else return transform;
    }

    public static string DebugLogList<T>(List<T> list)
    {
        string retval = "";
        foreach (var val in list) retval += ", " + val.ToString();
        return retval;
    }

    public static bool IsInRange(float value, float min, float max)
    {
        return (value > min && value < max);
    }

    [Serializable]
    public struct ScalableCurve
    {
        public AnimationCurve curve;
        public float valueScale;
        public float timeScale;

        public float Evaluate(float time)
        {
            return curve.Evaluate(time/timeScale) * valueScale;
        }
    }

}

