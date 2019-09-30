using System;
using UnityEngine;
using UnityEngine.UI;

public class AddingControl : MonoBehaviour
{
    [SerializeField] Text head;
    public InputField title;

    [Header("Dependencies")]
    [SerializeField] MainControl mainControl;
    [SerializeField] MenuSwitch menuSwitch;
    [SerializeField] SecondStep SecondStep;

    [Header("Curencys")]
    [SerializeField] Animation dol;
    [SerializeField] Animation rub;
    [SerializeField] Animation eur;

    [Header("Inputs")]
    public InputField dolInput;
    public InputField rubInput;
    public InputField eurInput;

    [Header("Modal")]
    [SerializeField] Modal noPursesModal;

    int ItemIndex;
    public static string Tag { get; private set; }
    public static char Curency = 'n';

    public void ShowInputWindow(string tag)
    {
        Tag = tag;
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        GetComponent<Animation>().Play("FadeIn");
        if (Tag == "Income") head.text = "Новый доход";
        else head.text = "Новый расход";
        title.ActivateInputField();
    }

    public void OnEndEdit(InputField input)
    {
        if (input.text == string.Empty) return;
        input.text = Math.Round(double.Parse(input.text), 2).ToString();
    }

    public void OnSubmit()
    {
        if (title.text == string.Empty)
        {
            print("Необходимо ввести название");
            return;
        }

        if (Curency != Curencys.dollar && Curency != Curencys.ruble && Curency != Curencys.euro)
        {
            print("Необходимо выбрать валюту");
            return;
        }

        if (   Curency == Curencys.dollar && dolInput.text == string.Empty
            || Curency == Curencys.ruble  && rubInput.text == string.Empty
            || Curency == Curencys.euro   && eurInput.text == string.Empty)
        {
            print("Необходимо ввести значение");
            return;
        }

        ToSecondStep();
    }

    private void ToSecondStep()
    {
        SecondStep.Init();
        GetComponent<Animation>().Play("ToSecondStep");
    }

    public void Hide()
    {
        if (Curency == Curencys.dollar) dol.Play("Off");
        else if (Curency == Curencys.ruble) rub.Play("Off");
        else if (Curency == Curencys.euro) eur.Play("Off");

        Curency = 'n';
        title.text = dolInput.text = rubInput.text = eurInput.text = string.Empty;
        GetComponent<Animation>().Play("FadeOut");
    }

    public void Disactive() {
        gameObject.SetActive(false);
        title.text = string.Empty;
    }

    public void DollarsClick()
    {
        // Если нет кошельков с долларами, показать модальное окно
        var purses = GameObject.FindGameObjectsWithTag("Purse");
        var count = 0;
        foreach (GameObject purse in purses)
            if (purse.GetComponent<OnePurse>().Curency == '$') count++;
        if (count == 0)
        {
            noPursesModal.Show();
            return;
        }

        // Выключить доллары или включить доллары и выключить остальные валюты
        if (Curency != Curencys.dollar)
        {
            dol.Play("On");
            if (Curency == Curencys.ruble) rub.Play("Off");
            else if (Curency == Curencys.euro) eur.Play("Off");
        } else return;
        Curency = Curencys.dollar;
    }
    public void RublesClick()
    {
        // Если нет кошельков с рублями, показать модальное окно
        var purses = GameObject.FindGameObjectsWithTag("Purse");
        var count = 0;
        foreach (GameObject purse in purses)
            if (purse.GetComponent<OnePurse>().Curency == '₽') count++;
        if (count == 0)
        {
            noPursesModal.Show();
            return;
        }

        // Выключить рубли или включить рубли и выключить остальные валюты
        if (Curency != Curencys.ruble)
        {
            rub.Play("On");
            if (Curency == Curencys.dollar) dol.Play("Off");
            else if (Curency == Curencys.euro) eur.Play("Off");
        } else return;
        Curency = Curencys.ruble;
    }
    public void EuresClick()
    {
        // Если нет кошельков с евро, показать модальное окно
        var purses = GameObject.FindGameObjectsWithTag("Purse");
        var count = 0;
        foreach (GameObject purse in purses)
            if (purse.GetComponent<OnePurse>().Curency == '€') count++;
        if (count == 0)
        {
            noPursesModal.Show();
            return;
        }

        // Выключить евро или включить евро и выключить остальные валюты
        if (Curency != Curencys.euro)
        {
            eur.Play("On");
            if (Curency == Curencys.dollar) dol.Play("Off");
            else if (Curency == Curencys.ruble) rub.Play("Off");
        } else return;
        Curency = Curencys.euro;
    }
}