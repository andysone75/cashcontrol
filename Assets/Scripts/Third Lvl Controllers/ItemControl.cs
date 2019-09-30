using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemControl : MonoBehaviour {
    MainControl mainControl;
    AmountControl amountController;

    [SerializeField] Text t_title;
    [SerializeField] Text t_value;

    public string Title { get; private set; }
    public double Value { get; private set; }
    public char Curency { get; private set; }
    public DateTime CreateDate { get; private set; }
    public int[] Ids;
    public double[] ValuesOnEachPurse;

    void Start()
    {
        mainControl = GameObject.FindGameObjectWithTag("MainControl").GetComponent<MainControl>();
        amountController = GameObject.FindWithTag("AmountController").GetComponent<AmountControl>();
    }

    public void Init(int[] ids, double[] valuesOnEachPurse, string title, double value, char curency, DateTime createDate)
    {
        Ids = ids;
        ValuesOnEachPurse = valuesOnEachPurse;
        Title = title;
        Value = value;
        Curency = curency;
        CreateDate = createDate;
        Render();
    }

    private void Render()
    {
        t_title.text = Title;
        string value = Value.ToString();
        t_value.text = ((tag == "Income") ? value : "- " + value) + " " + Curency;
    }

    public void ShowItemInfo ()
    {
        string value = Value.ToString() + " " + Curency;
        mainControl.itemInfo.Show(int.Parse(name), tag, Ids, ValuesOnEachPurse, Title, value, CreateDate);
    }
    
    public void Edit (string title)
    {
        Title = title;
        Render();
    }
    
    public void Delete ()
    {
        var index = int.Parse(name);
        bool isToday;
        GameObject[] items = GameObject.FindGameObjectsWithTag(tag);
        RectTransform rect;
        Vector2 posVector;

        for (var i = index + 1; i < items.Length - 1; i++)
        {
            items[i].name = (i-1).ToString();
            rect = items[i].GetComponent<RectTransform>();
            posVector = rect.anchoredPosition;
            posVector.y += 150;
            rect.anchoredPosition = posVector;
        }

        if (tag == "Income")
        {
            isToday = mainControl.incomes.IsToday[index];
            mainControl.incomes.RemoveAt(index);
        }
        else
        {
            isToday = mainControl.costs.IsToday[index];
            mainControl.costs.RemoveAt(index);
        }

        amountController.Add(tag, Curency, -Value, isToday);
        Destroy(gameObject);
    }
}
