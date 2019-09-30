using System;
using UnityEngine;
using UnityEngine.UI;

public class DateControl : MonoBehaviour {

    private DateTime dateEnd = new DateTime();
    private DateTime dayEnd  = new DateTime();

    private int daysLeft;
    public double minutesLeft;

    public void Load ()
    {
        // Сколько осталось дней до конца месяца
        if (!PlayerPrefs.HasKey("DateEnd"))
        {
            dateEnd = DateTime.Today.AddMonths(1);
            PlayerPrefs.SetString("DateEnd", dateEnd.ToString());
        }
        else dateEnd = DateTime.Parse(PlayerPrefs.GetString("DateEnd"));
        daysLeft = dateEnd.Subtract(DateTime.Today).Days;

        // Сколько осталось минут до конца дня
        if (!PlayerPrefs.HasKey("DayEnd"))
        {
            dayEnd = DateTime.Today.AddDays(1);
            PlayerPrefs.SetString("DayEnd", dayEnd.ToString());
        }
        else dayEnd = DateTime.Parse(PlayerPrefs.GetString("DayEnd"));
        minutesLeft = dayEnd.Subtract(DateTime.Now).TotalMinutes;
    }

    public void Render () { gameObject.GetComponent<Text>().text = daysLeft.ToString(); }

    public void UpdateDate()
    {
        dateEnd = DateTime.Today.AddMonths(1);
        PlayerPrefs.SetString("DateEnd", dateEnd.ToString());
        daysLeft = dateEnd.Subtract(DateTime.Today).Days;
    }

    public void UpdateDay()
    {
        dayEnd = DateTime.Today.AddDays(1);
        PlayerPrefs.SetString("DayEnd", dayEnd.ToString());
        minutesLeft = dayEnd.Subtract(DateTime.Now).TotalMinutes;
    }
    
    public bool HasMonthPassed () { return daysLeft <= 0; }
    public bool IsDayEnd () { return minutesLeft <= 0; }
}
