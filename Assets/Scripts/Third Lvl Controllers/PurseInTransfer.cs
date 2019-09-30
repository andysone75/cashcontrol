using System;
using UnityEngine;
using UnityEngine.UI;

public class PurseInTransfer : MonoBehaviour {
    public string Name { get; private set; }
    public double Value { get; private set; }
    public char Curency { get; private set; }

    bool isOpen;
    public double lastInput = 0;

    [SerializeField] Text t_Name;
    [SerializeField] Text t_Value;
    public InputField input;

    public void Init(string name, double value, char curency)
    {
        Name = name;
        Value = value;
        Curency = curency;
        Render();
    }

    private void Render()
    {
        t_Name.text = Name;
        t_Value.text = Value.ToString() + " " + Curency;
    }

    public void OnClick()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            GetComponent<Animation>().Play("On");
            input.ActivateInputField();
        }
        else
        {
            if (input.text != string.Empty && double.Parse(input.text) > 0)
            {
                isOpen = true;
                return;
            }
            GetComponent<Animation>().Play("Off");
        }
    }

    public void HideInput()
    {
        if (isOpen) GetComponent<Animation>().Play("Off");
    }

    public void OnEndEdit()
    {
        double value;
        if (input.text == string.Empty) value = 0;
        else value = Math.Round(double.Parse(input.text), 2);
        if (value < 0) input.text = (value = 0).ToString();

        value -= lastInput;

        GameObject.FindGameObjectWithTag("PursesInTransfer").GetComponent<PursesInTransfer>().AddToTransfer(value);
        GameObject.FindGameObjectsWithTag("Purse")[int.Parse(name)].GetComponent<OnePurse>().AddByTransfer(value);

        Value += value;
        Render();

        lastInput = value + lastInput;
    }
}
