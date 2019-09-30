using System;
using System.Collections;
using UnityEngine;

public class PursesControl : MonoBehaviour {
    [SerializeField] RectTransform ScrollView;
    [SerializeField] SecondStep addingSecondStep;
    [SerializeField] GameObject prefab;
    [SerializeField] float animationSpeed = 15;

    const int itemSpacing = 3;
    float height = 150;
    Purses pursesSave;

    public IEnumerator UpdateContentHeight()
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);

        Vector2 vector = rect.anchoredPosition;
        float lastHeight = float.MinValue;

        while (lastHeight < rect.anchoredPosition.y)
        {
            lastHeight = rect.anchoredPosition.y;
            vector.y = Mathf.SmoothStep(rect.anchoredPosition.y, rect.sizeDelta.y - ScrollView.sizeDelta.y, Time.deltaTime * animationSpeed);
            rect.anchoredPosition = vector;
            yield return new WaitForFixedUpdate();
        }
    }

    public void LoadAndRender()
    {
        if (!PlayerPrefs.HasKey("Purses")) return;
        pursesSave = GameObject.FindGameObjectWithTag("MainControl").GetComponent<MainControl>().purses;
        for (int i = 0; i < pursesSave.Titles.Count; i++)
            Render(i, transform, prefab);
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -75);
    }

    public void RenderSpecificCurency(char curency, Transform content, GameObject prefab)
    {
        int count = 0;
        for (var i = 0; i < pursesSave.Titles.Count; i++)
        {
            if (pursesSave.Curencys[i] == curency)
                Render(count++, content, prefab);
        }
    }

    public void RenderSpecificCurencyInAdding(char curency)
    {
        int count = 0;
        Transform transform = addingSecondStep.list.GetComponent<Transform>();
        for (var i = 0; i < pursesSave.Titles.Count; i++)
        {
            if (pursesSave.Curencys[i] == curency)
                Render(count++, i, transform);
        }
    }

    // Вывод в главный список кошельков
    public void Render(int i)
    {
        pursesSave = GameObject.FindGameObjectWithTag("MainControl").GetComponent<MainControl>().purses;

        string title = pursesSave.Titles[i];
        double value = pursesSave.Values[i];
        double startValue = pursesSave.StartValues[i];
        char curency = pursesSave.Curencys[i];
        DateTime creatingDate = DateTime.Parse(pursesSave.CreatingDates[i]);

        GameObject purse = Instantiate(prefab, transform);
        OnePurse purseCtrl = purse.GetComponent<OnePurse>();

        purse.name = i.ToString();
        purse.transform.localPosition = new Vector2(purse.transform.localPosition.x, purse.transform.localPosition.y - i * (150 + itemSpacing));

        purseCtrl.Init(title, value, startValue, curency, creatingDate);
        purseCtrl.Render();

        height = (150 + itemSpacing) * (i + 1);
    }
    // Вывод конкретной формы кошелька в конкретный список
    public void Render(int i, Transform transform, GameObject prefab)
    {
        string title = pursesSave.Titles[i];
        double value = pursesSave.Values[i];
        double startValue = pursesSave.StartValues[i];
        char curency = pursesSave.Curencys[i];
        DateTime creatingDate = DateTime.Parse(pursesSave.CreatingDates[i]);

        GameObject purse = Instantiate(prefab, transform);
        OnePurse purseCtrl = purse.GetComponent<OnePurse>();

        purse.name = i.ToString();
        purse.transform.localPosition = new Vector2(purse.transform.localPosition.x, purse.transform.localPosition.y - i * (150 + itemSpacing));

        purseCtrl.Init(title, value, startValue, curency, creatingDate);
        purseCtrl.Render();

        height = (150 + itemSpacing) * (i + 1);
    }
    // Рендер для случаев, когда выводятся не все кошельки
    public void Render(int count, int i, Transform transform)
    {
        string title = pursesSave.Titles[i];
        double value = pursesSave.Values[i];
        double startValue = pursesSave.StartValues[i];
        char curency = pursesSave.Curencys[i];
        DateTime creatingDate = DateTime.Parse(pursesSave.CreatingDates[i]);

        var rect = addingSecondStep.list.GetComponent<RectTransform>();

        GameObject purse = Instantiate(addingSecondStep.prefab, transform);
        OnePurse purseCtrl = purse.GetComponent<OnePurse>();

        purse.name = i.ToString();
        purse.transform.localPosition = new Vector2(purse.transform.localPosition.x, purse.transform.localPosition.y - count * (150 + itemSpacing));

        purseCtrl.Init(title, value, startValue, curency, creatingDate);
        purseCtrl.Render();

        var vector = new Vector2(rect.sizeDelta.x, (150 + itemSpacing) * (count + 1));
        rect.sizeDelta = vector;
    }
}
    