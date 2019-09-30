using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class MyColors
{
    public static Color Pomegranate = new Color(0.7529412f,  0.2235294f, 0.1686275f);
    public static Color Nephritis   = new Color(0.1529412f,  0.682353f,  0.3764706f);
    public static Color GreenSea    = new Color(0.08627451f, 0.627451f,  0.5215687f);
}

public class Curencys
{
    public static char dollar = '$';
    public static char ruble  = '₽';
    public static char euro   = '€';
}

public class MainControl : MonoBehaviour
{
    // Скрипты
    public CostControl    costControl;
    public IncomeControl  incomeControl;
    public PursesControl  pursesControl;
    public AmountControl  amountControl;
    public DateControl    dateControl;
    public ItemInfo       itemInfo;
    public PurseInfo      purseInfo;

    // Lang System
    [SerializeField] Text[] tMenuOther;
    [SerializeField] Text[] tCostsInfo;
    [SerializeField] Text[] tPursesInfo;
    [SerializeField] Text[] tIncomesInfo;
    [SerializeField] Text[] tSwitcher;
    [SerializeField] Text[] tItemInfo;
    [SerializeField] Text[] tPurseInfo;
    [SerializeField] Text[] tAddingControl;
    [SerializeField] Text[] tAddingPurse;
    [SerializeField] Text[] tNoPursesModal;

    // Сейвы
    public Incomes incomes = new Incomes();
    public Costs   costs   = new Costs();
    public Purses  purses  = new Purses();

    void Start ()
    {
        LangSystem.Init();
        dateControl.Load();
        amountControl.Load();

        // Загрузка сохранений
        if (PlayerPrefs.HasKey("Incomes")) incomes = JsonUtility.FromJson<Incomes>(PlayerPrefs.GetString("Incomes"));
        if (PlayerPrefs.HasKey("Costs"))   costs   = JsonUtility.FromJson<Costs>(PlayerPrefs.GetString("Costs"));
        if (PlayerPrefs.HasKey("Purses"))  purses  = JsonUtility.FromJson<Purses>(PlayerPrefs.GetString("Purses"));

        if (dateControl.HasMonthPassed()) UpdateMonth();
        else if (dateControl.IsDayEnd()) UpdateDayStat();
        
        dateControl.Render();
        amountControl.Render();

        incomeControl.LoadAndRender();
        costControl.LoadAndRender();
        pursesControl.LoadAndRender();

        // Language Loading
        //Lang lang = LangSystem.Load();
        LangSystem.Init();
        LangSystem.Load();

        for (int i = 0; i < tMenuOther.Length;     i++) tMenuOther[i].text     = LangSystem.lang.Menu_Other[i];
        for (int i = 0; i < tCostsInfo.Length;     i++) tCostsInfo[i].text     = LangSystem.lang.CostsInfo[i];
        for (int i = 0; i < tPursesInfo.Length;    i++) tPursesInfo[i].text    = LangSystem.lang.PursesInfo[i];
        for (int i = 0; i < tIncomesInfo.Length;   i++) tIncomesInfo[i].text   = LangSystem.lang.IncomesInfo[i];
        for (int i = 0; i < tSwitcher.Length;      i++) tSwitcher[i].text      = LangSystem.lang.Switcher[i];
        for (int i = 0; i < tItemInfo.Length;      i++) tItemInfo[i].text      = LangSystem.lang.ItemInfo[i];
        for (int i = 0; i < tPurseInfo.Length;     i++) tPurseInfo[i].text     = LangSystem.lang.PurseInfo[i];
        for (int i = 0; i < tAddingControl.Length; i++) tAddingControl[i].text = LangSystem.lang.AddingControl[i];
        for (int i = 0; i < tAddingPurse.Length;   i++) tAddingPurse[i].text   = LangSystem.lang.AddingPurse[i];
        for (int i = 0; i < tNoPursesModal.Length; i++) tNoPursesModal[i].text = LangSystem.lang.NoPursesModal[i];
    }

    public void UpdateMonth()
    {
        amountControl.ClearProfit();
        dateControl.UpdateDate();
        dateControl.UpdateDay();
    }

    public void UpdateDayStat ()
    {
        incomes.UpdateDay();
        costs.UpdateDay();
        amountControl.ClearToday();
        dateControl.UpdateDay();
    }

    public void AddIncome(int[] ids, double[] valuesOnEachPurse, string title, double value, char curency)
    {
        incomes.Add(ids, valuesOnEachPurse, title, value, curency);
        amountControl.Add("Income", curency, value, true);
        int index = incomes.Titles.Count - 1;
        incomeControl.Render(ids, valuesOnEachPurse, title, value, curency, DateTime.Parse(incomes.CreateDates[index]), index);
        StartCoroutine(incomeControl.UpdateContentHeight());
    }

    public void AddCost (int[] ids, double[] valuesOnEachPurse, string title, double value, char curency)
    {
        costs.Add(ids, valuesOnEachPurse, title, value, curency);
        amountControl.Add("Cost", curency, value, true);
        int index = costs.CreateDates.Count - 1;
        costControl.Render(ids, valuesOnEachPurse, title, value, curency, DateTime.Parse(costs.CreateDates[index]), index);
        StartCoroutine(costControl.UpdateContentHeight());
    }

    public void AddPurse (string title, double value, char curency)
    {
        purses.Add(title, value, curency);
        amountControl.AddValue(value, curency);
        amountControl.RenderAmount();
        int index = purses.Titles.Count - 1;
        pursesControl.Render(index);
        StartCoroutine(pursesControl.UpdateContentHeight());
    }
    
    public static double ReadTheValue(string input)
    {
        Regex regex;
        MatchCollection matches;
        regex = new Regex(@"\-?(\d\.?)+");
        matches = regex.Matches(input);
        regex = new Regex(@"\s");
        return double.Parse(regex.Replace(matches[0].Value, ""));
    }
}

public abstract class Items
{
    public List<string> Titles = new List<string>();
    public List<double> Values = new List<double>();
    public List<char> Curencys = new List<char>();
    public List<bool> IsToday = new List<bool>();
    public List<string> CreateDates = new List<string>();
    public double TodayStat = 0;
    public List<string> Ids = new List<string>();
    public List<string> ValuesOnEachPurse = new List<string>();

    public void Add (int[] ids, double[] valuesOnEachPurse, string title, double value, char curency)
    {
        string idsStr = ids[0].ToString();
        string valsStr = valuesOnEachPurse[0].ToString();

        for (var i = 1; i < ids.Length; i++)
        {
            idsStr  += " " + ids[i].ToString();
            valsStr += " " + valuesOnEachPurse[i].ToString();
        }

        Ids.Add(idsStr);
        ValuesOnEachPurse.Add(valsStr);
        Titles.Add(title);
        Values.Add(value);
        Curencys.Add(curency);
        CreateDates.Add(DateTime.Now.ToString());
        IsToday.Add(true);
        TodayStat += value;
        Save();
    }

    public void RemoveAt(int index)
    {
        Ids.RemoveAt(index);
        ValuesOnEachPurse.RemoveAt(index);
        Titles.RemoveAt(index);
        Values.RemoveAt(index);
        Curencys.RemoveAt(index);
        CreateDates.RemoveAt(index);
        IsToday.RemoveAt(index);
        Save();
    }

    public void UpdateDay ()
    {
        TodayStat = 0;
        for (var i = 0; i < IsToday.Count; i++)
            IsToday[i] = false;
        Save();
    }
    
    private void Clear ()
    {
        Titles.Clear();
        Values.Clear();
        Curencys.Clear();
        IsToday.Clear();
        CreateDates.Clear();
        TodayStat = 0;
        Ids.Clear();
        ValuesOnEachPurse.Clear();
    }

    public abstract void Rename(int index, string title);
    public abstract void Save();
}

public class Incomes : Items
{
    public override void Rename(int index, string title)
    {
        Titles[index] = title;
        Save();
        GameObject.FindGameObjectsWithTag("Income")[index].GetComponent<ItemControl>().Edit(title);
    }

    public override void Save() { PlayerPrefs.SetString("Incomes", JsonUtility.ToJson(this)); }
}

public class Costs : Items
{
    public override void Rename(int index, string title)
    {
        Titles[index] = title;
        Save();
        GameObject.FindGameObjectsWithTag("Cost")[index].GetComponent<ItemControl>().Edit(title);
    }

    public override void Save() { PlayerPrefs.SetString("Costs", JsonUtility.ToJson(this)); }
}

public class Purses
{
    public List<int>    Ids           = new List<int>();
    public List<string> Titles        = new List<string>();
    public List<double> Values        = new List<double>();
    public List<double> StartValues   = new List<double>();
    public List<char>   Curencys      = new List<char>();
    public List<string> CreatingDates = new List<string>();

    public void Add(string title, double value, char curency)
    {
        int id;
        if (PlayerPrefs.HasKey("Id")) id = PlayerPrefs.GetInt("Id");
        else PlayerPrefs.SetInt("Id", id = 0);
        Ids.Add(id++);
        PlayerPrefs.SetInt("Id", id);
        Titles.Add(title);
        Values.Add(value);
        StartValues.Add(value);
        Curencys.Add(curency);
        CreatingDates.Add(DateTime.Now.ToString());
        Save();
    }

    public void UpdateAt(int index, string title)
    {
        Titles[index] = title;
        Save();
    }
    public void UpdateAt(int index, double value)
    {
        Values[index] = value;
        Save();
    }
    public void UpdateAt(int index, string title, double value)
    {
        Titles[index] = title;
        Values[index] = value;
        Save();
    }

    public void RemoveAt(int index)
    {
        Ids.RemoveAt(index);
        Titles.RemoveAt(index);
        Values.RemoveAt(index);
        StartValues.RemoveAt(index);
        Curencys.RemoveAt(index);
        CreatingDates.RemoveAt(index);
        Save();
    }

    private void Save() { PlayerPrefs.SetString("Purses", JsonUtility.ToJson(this)); }
}