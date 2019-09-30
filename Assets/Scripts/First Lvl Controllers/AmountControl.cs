using UnityEngine;
using UnityEngine.UI;

public class AmountControl : MonoBehaviour {
    // Profit
    [Header("Profit")]
    [SerializeField] Text profitRuble;
    [SerializeField] Text profitDollar;
    [SerializeField] Text profitEuro;

    // Bank
    [Header("Curencys")]
    [SerializeField] Text rublesText;
    [SerializeField] Text dollarsText;
    [SerializeField] Text euresText;

    // Current month
    [Header("Generals")]
    [Header("Incomes")]
    [SerializeField] Text generalIncomeRuble;
    [SerializeField] Text generalIncomeDollar;
    [SerializeField] Text generalIncomeEuro;

    [Header("Costs")]
    [SerializeField] Text generalCostRuble;
    [SerializeField] Text generalCostDollar;
    [SerializeField] Text generalCostEuro;


    // Показатели за сегодня
    [Header("Todays")]
    [Header("Incomes")]
    [SerializeField] Text todayIncomeRuble;
    [SerializeField] Text todayIncomeDollar;
    [SerializeField] Text todayIncomeEuro;

    [Header("Costs")]
    [SerializeField] Text todayCostRuble;
    [SerializeField] Text todayCostDollar;
    [SerializeField] Text todayCostEuro;

    private Saves saves;

    // Other scripts
    [Header("Dependencies")]
    [SerializeField] readonly DateControl dateControl;

    public void Load ()
    {
        if (PlayerPrefs.HasKey("Amount")) saves = JsonUtility.FromJson<Saves>(PlayerPrefs.GetString("Amount"));
        else JsonUtility.ToJson(saves = new Saves());
        saves.Save();
    }

    public void Render()
    {
        RenderAmount();
        RenderProfit();
        GeneralsRender();
        TodayRender();
    }

    public void Clear ()
    {
        ClearProfit();
        ClearGenerals();
        ClearToday();
    }

    // Управление банком
    public void AddValue(double value, char curency)
    {
        if      (curency == Curencys.ruble)  saves.Amount.Rubles += value;
        else if (curency == Curencys.dollar) saves.Amount.Dollars += value;
        else if (curency == Curencys.euro)   saves.Amount.Eures += value;
        saves.Save();
        RenderAmount();
    }

    public void Add(string tag, char curency, double value, bool isToday)
    {
        if (tag == "Income")
        {
            GenIncomeAdd(value, curency);
            if (isToday) TodayIncomeAdd(value, curency);
        }
        else
        {
            GenCostAdd(value, curency);
            if (isToday) TodayCostAdd(value, curency);
            value = -value;
        }
        
        AddValue(value, curency);
        AddProfit(value, curency);

        saves.Save();
    }
    
    public void RenderAmount()
    {
        rublesText.text  = saves.Amount.Rubles.ToString() + ' ' + Curencys.ruble;
        dollarsText.text = saves.Amount.Dollars.ToString() + ' ' + Curencys.dollar;
        euresText.text   = saves.Amount.Eures.ToString() + ' ' + Curencys.euro;
    }

    // Управление прибылью
    private void AddProfit(double value, char curency)
    {
        if (curency == Curencys.ruble) saves.Profit.Rubles += value;
        else if (curency == Curencys.dollar) saves.Profit.Dollars += value;
        else if (curency == Curencys.euro) saves.Profit.Eures += value;
        saves.Save();
        RenderProfit();
    }

    private void RenderProfit()
    {
        profitRuble.text = (saves.Profit.Rubles > 0 ? "+" : "") + saves.Profit.Rubles.ToString() + ' ' + Curencys.ruble;
        profitDollar.text = (saves.Profit.Dollars > 0 ? "+" : "") + saves.Profit.Dollars.ToString() + ' ' + Curencys.dollar;
        profitEuro.text = (saves.Profit.Eures > 0 ? "+" : "") + saves.Profit.Eures.ToString() + ' ' + Curencys.euro;
    }

    public void ClearProfit()
    {
        saves.Profit.Rubles = 0;
        saves.Profit.Dollars = 0;
        saves.Profit.Eures = 0;

        saves.Save();
        RenderProfit();
    }

    // Управление месячными показателями
    private void GeneralsRender()
    {
        GenIncomeRender();
        GenCostRender();
    }

    private void ClearGenerals()
    {
        saves.Generals.Incomes.Rubles = 0;
        saves.Generals.Incomes.Dollars = 0;
        saves.Generals.Incomes.Eures = 0;

        saves.Generals.Costs.Rubles = 0;
        saves.Generals.Costs.Dollars = 0;
        saves.Generals.Costs.Eures = 0;

        saves.Save();
        GeneralsRender();
    }

    // Доходы
    private void GenIncomeAdd(double newValue, char curency)
    {
        if (curency == Curencys.ruble) saves.Generals.Incomes.Rubles += newValue;
        else if (curency == Curencys.dollar) saves.Generals.Incomes.Dollars += newValue;
        else if (curency == Curencys.euro) saves.Generals.Incomes.Eures += newValue;

        saves.Save();
        GenIncomeRender();
    }

    private void GenIncomeRender()
    {
        generalIncomeRuble.text = "+" + saves.Generals.Incomes.Rubles.ToString() + ' ' + Curencys.ruble;
        generalIncomeDollar.text = "+" + saves.Generals.Incomes.Dollars.ToString() + ' ' + Curencys.dollar;
        generalIncomeEuro.text = "+" + saves.Generals.Incomes.Eures.ToString() + ' ' + Curencys.euro;
    }

    // Расходы
    private void GenCostAdd(double newValue, char curency)
    {
        if (curency == Curencys.ruble) saves.Generals.Costs.Rubles += newValue;
        else if (curency == Curencys.dollar) saves.Generals.Costs.Dollars += newValue;
        else if (curency == Curencys.euro) saves.Generals.Costs.Eures += newValue;

        saves.Save();
        GenCostRender();
    }

    private void GenCostRender()
    {
        generalCostRuble.text = (saves.Generals.Costs.Rubles != 0 ? "-" : "") + saves.Generals.Costs.Rubles.ToString() + ' ' + Curencys.ruble;
        generalCostDollar.text = (saves.Generals.Costs.Dollars != 0 ? "-" : "") + saves.Generals.Costs.Dollars.ToString() + ' ' + Curencys.dollar;
        generalCostEuro.text = (saves.Generals.Costs.Eures != 0 ? "-" : "") + saves.Generals.Costs.Eures.ToString() + ' ' + Curencys.euro;
    }

    // Управление сегодняшними показателями
    private void TodayRender ()
    {
        TodayIncomeRender();
        TodayCostRender();
    }
    private void TodayIncomeRender ()
    {
        todayIncomeRuble.text = "+" + saves.Today.Incomes.Rubles.ToString() + ' ' + Curencys.ruble;
        todayIncomeDollar.text = "+" + saves.Today.Incomes.Dollars.ToString() + ' ' + Curencys.dollar;
        todayIncomeEuro.text = "+" + saves.Today.Incomes.Eures.ToString() + ' ' + Curencys.euro;
    }
    private void TodayCostRender ()
    {
        todayCostRuble.text = (saves.Today.Costs.Rubles != 0 ? "-" : "") + saves.Today.Costs.Rubles.ToString() + ' ' + Curencys.ruble;
        todayCostDollar.text = (saves.Today.Costs.Dollars != 0 ? "-" : "") + saves.Today.Costs.Dollars.ToString() + ' ' + Curencys.dollar;
        todayCostEuro.text = (saves.Today.Costs.Eures != 0 ? "-" : "") + saves.Today.Costs.Eures.ToString() + ' ' + Curencys.euro;
    }

    private void TodayIncomeAdd (double newValue, char curency)
    {
        if (curency == Curencys.ruble) saves.Today.Incomes.Rubles += newValue;
        else if (curency == Curencys.dollar) saves.Today.Incomes.Dollars += newValue;
        else if (curency == Curencys.euro) saves.Today.Incomes.Eures += newValue;

        saves.Save();
        TodayIncomeRender();
    }

    private void TodayCostAdd (double newValue, char curency)
    {
        if (curency == Curencys.ruble) saves.Today.Costs.Rubles += newValue;
        else if (curency == Curencys.dollar) saves.Today.Costs.Dollars += newValue;
        else if (curency == Curencys.euro) saves.Today.Costs.Eures += newValue;

        saves.Save();
        TodayCostRender();
    }
    
    public void ClearToday ()
    {
        saves.Today.Incomes.Rubles = 0;
        saves.Today.Incomes.Dollars = 0;
        saves.Today.Incomes.Eures = 0;

        saves.Today.Costs.Rubles = 0;
        saves.Today.Costs.Dollars = 0;
        saves.Today.Costs.Eures = 0;

        saves.Save();
    }
}

public class Saves
{
    public Amount Amount = new Amount();
    public Profit Profit = new Profit();
    public Generals Generals = new Generals();
    public Today Today = new Today();

    public void Save() { PlayerPrefs.SetString("Amount", JsonUtility.ToJson(this)); }
}

[System.Serializable]
public class Amount
{
    public double Rubles = 0;
    public double Dollars = 0;
    public double Eures = 0;
}

[System.Serializable]
public class Profit
{
    public double Rubles = 0;
    public double Dollars = 0;
    public double Eures = 0;
}

[System.Serializable]
public class Generals
{
    public AIncomes Incomes = new AIncomes();
    public ACosts Costs = new ACosts();
}

[System.Serializable]
public class Today
{
    public AIncomes Incomes = new AIncomes();
    public ACosts Costs = new ACosts();
}

[System.Serializable]
public class AIncomes
{
    public double Rubles = 0;
    public double Dollars = 0;
    public double Eures = 0;
}

[System.Serializable]
public class ACosts
{
    public double Rubles = 0;
    public double Dollars = 0;
    public double Eures = 0;
}