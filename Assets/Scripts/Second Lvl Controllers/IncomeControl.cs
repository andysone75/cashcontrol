using System;
using System.Collections;
using UnityEngine;

public class IncomeControl : MonoBehaviour {
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
        if (!PlayerPrefs.HasKey("Incomes")) return;
        Incomes incomes = GameObject.FindGameObjectWithTag("MainControl").GetComponent<MainControl>().incomes;
        for (int i = 0; i < incomes.Titles.Count; i++)
        {
            string[] ids = incomes.Ids[i].Split(' ');
            string[] valuesOnEachPurse = incomes.ValuesOnEachPurse[i].Split(' ');

            int[] idsInt = new int[ids.Length];
            double[] valsInt = new double[valuesOnEachPurse.Length];

            for (var k = 0; k < ids.Length; k++)
            {
                idsInt[k] = int.Parse(ids[k]);
                valsInt[k] = double.Parse(valuesOnEachPurse[k]);
            }

            Render(idsInt, valsInt, incomes.Titles[i], incomes.Values[i], incomes.Curencys[i], DateTime.Parse(incomes.CreateDates[i]), i);
        }
        StartCoroutine(UpdateContentHeight());
    }

    public void Render(int[] ids, double[] valuesOnEachPurse, string title, double value, char curency, DateTime date, int i)
    {
        GameObject income = Instantiate(prefab, transform);
        ItemControl itemControl = income.GetComponent<ItemControl>();
        float incomeHeight = 100 + spacing;

        income.name = i.ToString();
        income.tag = "Income";
        income.transform.localPosition = new Vector2(income.transform.localPosition.x, income.transform.localPosition.y - i * incomeHeight);    

        itemControl.Init(ids, valuesOnEachPurse, title, value, curency, date);
        GameObject.FindGameObjectsWithTag("Income")[i].GetComponent<Animation>().Play("FadeIn");

        height = incomeHeight * (i + 1);
    }
}
