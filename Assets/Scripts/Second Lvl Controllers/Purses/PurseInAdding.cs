using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PurseInAdding : MonoBehaviour {
    [SerializeField] Animation parentAnim;
    [SerializeField] Text valueS;
    private double valueInPurse;
    private double lastValue = 0;
    public bool isOpen = false;

    private void Start() { valueInPurse = MainControl.ReadTheValue(valueS.text); }
    
    public void Switch()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            parentAnim.Play("On");
            GetComponent<InputField>().ActivateInputField();
        }
        else
        {
            parentAnim.Play("Off");
            GetComponent<InputField>().text = "0";
            OnEndEdit();
        }
    }

    public void OnEndEdit()
    {
        InputField input = GetComponent<InputField>();

        if (input.text == string.Empty)
        {
            Switch();
            return;
        }

        input.text = Math.Round(double.Parse(input.text), 2).ToString();

        double inputValue = double.Parse(input.text);
        double newValueInPurse;
        double newValueToZero;

        if (AddingControl.Tag == "Income")
        {
            newValueInPurse = Math.Round(valueInPurse + inputValue, 2);
            newValueToZero = Math.Round(SecondStep.valueNeedToZero + lastValue - inputValue, 2);

            if (newValueToZero < 0)
            {
                inputValue = Math.Round(SecondStep.valueNeedToZero + lastValue, 2);
                newValueInPurse = Math.Round(valueInPurse + inputValue, 2);
                input.text = inputValue.ToString();
                newValueToZero = 0;
            }
        }
        else
        {
            newValueInPurse = Math.Round(valueInPurse - inputValue, 2);
            newValueToZero = Math.Round(SecondStep.valueNeedToZero + lastValue - inputValue, 2);
            
            if (newValueToZero < 0)
            {
                inputValue = Math.Round(SecondStep.valueNeedToZero + lastValue, 2);
                newValueInPurse = Math.Round(valueInPurse - inputValue, 2);
                input.text = inputValue.ToString();
                newValueToZero = 0;
            }

            if (newValueInPurse < 0) StartCoroutine(ChangeValueColor(MyColors.Pomegranate));
            else StartCoroutine(ChangeValueColor(Color.white));
        }

        SecondStep.valueNeedToZero = newValueToZero;

        valueS.text = newValueInPurse.ToString() + " " + AddingControl.Curency;
        GameObject.FindGameObjectWithTag("SecondStep").GetComponent<SecondStep>().value.text = newValueToZero.ToString() + " " + AddingControl.Curency;
        lastValue = inputValue;
    }

    private IEnumerator ChangeValueColor (Color color)
    {
        for (var i = 0f; i < 1; i += 0.05f)
        {
            valueS.color = Color.Lerp(valueS.color, color, i);
            yield return new WaitForSeconds(0.015f);
        }
    }
}
