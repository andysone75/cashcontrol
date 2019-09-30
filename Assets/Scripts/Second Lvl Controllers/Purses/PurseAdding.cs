using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PurseAdding : MonoBehaviour {
    [SerializeField] Animation anim;

    [Header("Inputs")]
    [SerializeField] InputField title;
    [SerializeField] InputField value;
    
    [Header("Curencys")]
    [Header("Dollars")]
    [SerializeField] Image dol_i;
    [SerializeField] Text dol_t;
    [Header("Rubles")]
    [SerializeField] Image rub_i;
    [SerializeField] Text rub_t;
    [Header("Eures")]
    [SerializeField] Image eur_i;
    [SerializeField] Text eur_t;

    private Color selected = new Color(0.3921569f, 0.8666667f, 0.09019608f);

    private char curency = '$';
    private MainControl mainControl;

    private void Start() { mainControl = GameObject.FindGameObjectWithTag("MainControl").GetComponent<MainControl>(); }

    public void Show()
    {
        gameObject.SetActive(true);
        anim.Play("FadeIn");
    }

    private void SetCurencysActive (bool dollar, bool ruble, bool euro)
    {
        SetDollarActive(dollar);
        SetRubleActive(ruble);
        SetEuroActive(euro);
    }

    private void SetDollarActive(bool active)
    {
        if (active)
        {
            StartCoroutine(ChangeImageColor(dol_i, selected));
            StartCoroutine(ChangeTextColor(dol_t, selected));
        }
        else
        {
            StartCoroutine(ChangeImageColor(dol_i, Color.white));
            StartCoroutine(ChangeTextColor(dol_t, Color.white));
        }
    }
    private void SetRubleActive(bool active)
    {
        if (active)
        {
            StartCoroutine(ChangeImageColor(rub_i, selected));
            StartCoroutine(ChangeTextColor(rub_t, selected));
        }
        else
        {
            StartCoroutine(ChangeImageColor(rub_i, Color.white));
            StartCoroutine(ChangeTextColor(rub_t, Color.white));
        }
    }
    private void SetEuroActive(bool active)
    {
        if (active)
        {
            StartCoroutine(ChangeImageColor(eur_i, selected));
            StartCoroutine(ChangeTextColor(eur_t, selected));
        }
        else
        {
            StartCoroutine(ChangeImageColor(eur_i, Color.white));
            StartCoroutine(ChangeTextColor(eur_t, Color.white));
        }
    }

    private IEnumerator ChangeImageColor (Image image, Color color)
    {
        for (var i = 0f; i < 1; i += 0.05f)
        {
            image.color = Color.Lerp(image.color, color, i);
            yield return new WaitForSeconds(0.015f);
        }
    }
    private IEnumerator ChangeTextColor(Text text, Color color)
    {
        for (var i = 0f; i < 1; i += 0.05f)
        {
            text.color = Color.Lerp(text.color, color, i);
            yield return new WaitForSeconds(0.015f);
        }
    }

    public void Hide() { anim.Play("FadeOut"); }

    public void Disactivate() {
        dol_i.color = dol_t.color = selected;
        rub_i.color = rub_t.color = Color.white;
        eur_i.color = eur_t.color = Color.white;
        title.text = value.text = string.Empty;
        curency = '$';
        gameObject.SetActive(false);
    }
     
    public void DollarsClick ()
    {
        if (curency == '$') return;
        SetCurencysActive(true, false, false);
        curency = '$';
    }
    public void RublesClick()
    {
        if (curency == '₽') return;
        SetCurencysActive(false, true, false);
        curency = '₽';
    }
    public void EuresClick()
    {
        if (curency == '€') return;
        SetCurencysActive(false, false, true);
        curency = '€';
    }

    public void OnEndEdit()
    {
        if (value.text == string.Empty) return;
        value.text = Math.Round(double.Parse(value.text), 2).ToString();
    }

    public void OnSubmit ()
    {
        double val = value.text == string.Empty ? 0 : double.Parse(value.text);
        if (title.text == string.Empty || val < 0) return;
        mainControl.AddPurse(title.text, val, curency);
        Hide();
    }
}
