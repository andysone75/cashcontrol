using System;
using System.Collections;
using UnityEngine;

public class CostControl : MonoBehaviour {
    [SerializeField] RectTransform ScrollView;
    [SerializeField] GameObject prefab;
    [SerializeField] float animationSpeed = 15;
    [SerializeField] int spacing = 3;

    float height = 100;
    
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
        if (!PlayerPrefs.HasKey("Costs")) return;
        Costs costs = GameObject.FindGameObjectWithTag("MainControl").GetComponent<MainControl>().costs;
        for (int i = 0; i < costs.Titles.Count; i++)
        {
            string[] ids = costs.Ids[i].Split(' ');
            string[] valuesOnEachPurse = costs.ValuesOnEachPurse[i].Split(' ');

            int[] idsInt = new int[ids.Length];
            double[] valsInt = new double[valuesOnEachPurse.Length];

            for (var k = 0; k < ids.Length; k++)
            {
                idsInt[k] = int.Parse(ids[k]);
                valsInt[k] = double.Parse(valuesOnEachPurse[k]);
            }

            Render(idsInt, valsInt, costs.Titles[i], costs.Values[i], costs.Curencys[i], DateTime.Parse(costs.CreateDates[i]), i);
        }
        StartCoroutine(UpdateContentHeight());
    }

    public void Render(int[] ids, double[] valuesOnEachPurse, string title, double value, char curency, DateTime date, int i)
    {
        GameObject cost = Instantiate(prefab, transform);
        ItemControl itemControl = cost.GetComponent<ItemControl>();
        float costHeight = 100 + spacing;

        cost.name = i.ToString();
        cost.tag = "Cost";
        cost.transform.localPosition = new Vector2(cost.transform.localPosition.x, cost.transform.localPosition.y - i * costHeight);

        itemControl.Init(ids, valuesOnEachPurse, title, value, curency, date);
        cost.GetComponent<Animation>().Play("FadeIn");

        height = costHeight * (i + 1);
    }
}
