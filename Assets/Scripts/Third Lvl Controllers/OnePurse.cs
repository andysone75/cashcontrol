using System;
using UnityEngine;
using UnityEngine.UI;

public class OnePurse : MonoBehaviour {
    [Header("Text")]
    [SerializeField] Text nameTxt;
    [SerializeField] Text valueTxt;

    public string Name { get; private set; }
    public double Value { get; private set; }
    public double StartValue { get; private set; }
    public char Curency { get; private set; }
    public DateTime CreatingDate;

    public void Init(string name, double value, double startValue, char curency, DateTime creatingDate)
    {
        Name = name;
        Value = value;
        StartValue = startValue;
        Curency = curency;
        CreatingDate = creatingDate;
        Render();
    }

    public void Render()
    {
        nameTxt.text = Name;
        valueTxt.text = Value.ToString() + " " + Curency;
    }

    public void UpdateItem(string newName, double value)
    {
        Name = newName;
        Value = value;
        GameObject.FindGameObjectWithTag("MainControl").GetComponent<MainControl>().purses.UpdateAt(int.Parse(name), newName, value);
        Render();
    }
    public void UpdateItem(double value) {
        Value = value;
        GameObject.FindGameObjectWithTag("MainControl").GetComponent<MainControl>().purses.UpdateAt(int.Parse(name), value);
        Render();
    }
    public void UpdateItem(string newName)
    {
        Name = newName;
        GameObject.FindGameObjectWithTag("MainControl").GetComponent<MainControl>().purses.UpdateAt(int.Parse(name), newName);
        Render();
    }

    public void UpdateByTransfer(double value)
    {
        StartValue = Value = value;
        GameObject.FindGameObjectWithTag("MainControl").GetComponent<MainControl>().purses.UpdateAt(int.Parse(name), value);
        Render();
    }

    public void Add(double value)
    {
        Value += value;
        GameObject.FindGameObjectWithTag("MainControl").GetComponent<MainControl>().purses.UpdateAt(int.Parse(name), Value);
        Render();
    }

    public void AddByTransfer(double value)
    {
        Value += value;
        StartValue += value;
        GameObject.FindGameObjectWithTag("MainControl").GetComponent<MainControl>().purses.UpdateAt(int.Parse(name), Value);
        Render();
    }

    public void ShowInfo ()
    {
        GameObject.FindGameObjectWithTag("MainControl").GetComponent<MainControl>().purseInfo.Show(int.Parse(name), Name, Value, Curency, CreatingDate);
    }

    public void Delete()
    {
        var index = int.Parse(name);
        var purses = GameObject.FindGameObjectsWithTag("Purse");
        RectTransform rect;
        Vector2 posVector;

        GameObject.FindGameObjectWithTag("MainControl").GetComponent<MainControl>().purses.RemoveAt(index);
        GameObject.FindGameObjectWithTag("AmountController").GetComponent<AmountControl>().AddValue(-StartValue, Curency);

        for (var i = index + 1; i < purses.Length; i++)
        {
            purses[i].name = (i - 1).ToString();
            rect = purses[i].GetComponent<RectTransform>();
            posVector = rect.anchoredPosition;
            posVector.y += 150;
            rect.anchoredPosition = posVector;
        }

        Destroy(gameObject);
    }
}
