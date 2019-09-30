using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondStep : MonoBehaviour {
    public Text value;
    public GameObject list;
    public GameObject prefab;
    
    [Header("Dependencies")]
    [SerializeField] AddingControl addingControl;
    [SerializeField] PursesControl pursesControl;
    
    public static double valueNeedToZero;

    public void Init()
    {
        if      (AddingControl.Curency == Curencys.dollar) ShowDollarsInput();
        else if (AddingControl.Curency == Curencys.ruble)  ShowRublesInput();
        else ShowEuresInput();
    }

    public void OnSubmit()
    {
        if (valueNeedToZero != 0) return;

        GameObject[] inputs = GameObject.FindGameObjectsWithTag("PurseInAddingInput");
        GameObject[] pursesInAdding = GameObject.FindGameObjectsWithTag("PurseInAdding");
        GameObject[] purses = GameObject.FindGameObjectsWithTag("Purse");

        for (var i = 0; i < inputs.Length; i++)
        {
            if (inputs[i].GetComponent<PurseInAdding>().isOpen)
            {
                var newValue = double.Parse(inputs[i].GetComponent<InputField>().text);
                purses[int.Parse(pursesInAdding[i].name)].GetComponent<OnePurse>().Add(AddingControl.Tag == "Income" ? newValue : -newValue);
            }
        }

        string title = addingControl.title.text;
        double value;
        char curency = AddingControl.Curency;

        if (curency == Curencys.dollar) value = double.Parse(addingControl.dolInput.text);
        else if (curency == Curencys.ruble) value = double.Parse(addingControl.rubInput.text);
        else value = double.Parse(addingControl.eurInput.text);

        var mainControl = GameObject.FindGameObjectWithTag("MainControl").GetComponent<MainControl>();

        List<int> ids  = new List<int>();
        List<double> vals = new List<double>();
        for (var i = 0; i < pursesInAdding.Length; i++)
        {
            if (inputs[i].GetComponent<PurseInAdding>().isOpen)
            {
                ids.Add(mainControl.purses.Ids[int.Parse(pursesInAdding[i].name)]);
                vals.Add(double.Parse(inputs[i].GetComponent<InputField>().text));
            }
        }
        int[] idsArr = new int[ids.Count];
        double[] valsArr = new double[vals.Count];

        for (var i = 0; i < ids.Count; i++)
        {
            idsArr[i] = ids[i];
            valsArr[i] = vals[i];
        }

        if (AddingControl.Tag == "Income") mainControl.AddIncome(idsArr, valsArr, title, value, curency);
        else mainControl.AddCost(idsArr, valsArr, title, value, curency);

        StartCoroutine(Hide());
    }

    private void ShowDollarsInput()
    {
        ClearList();
        valueNeedToZero = double.Parse(addingControl.dolInput.text);
        value.text = valueNeedToZero.ToString() + " $";
        pursesControl.RenderSpecificCurencyInAdding(Curencys.dollar);
    }
    private void ShowRublesInput()
    {
        ClearList();
        valueNeedToZero = double.Parse(addingControl.rubInput.text);
        value.text = valueNeedToZero.ToString() + " ₽";
        pursesControl.RenderSpecificCurencyInAdding(Curencys.ruble);
    }
    private void ShowEuresInput()
    {
        ClearList();
        valueNeedToZero = double.Parse(addingControl.eurInput.text);
        value.text = valueNeedToZero.ToString() + " €";
        pursesControl.RenderSpecificCurencyInAdding(Curencys.euro);
    }

    private void ClearColor (Image image)
    {
        Color c = image.color;
        if (AddingControl.Tag == "Income") c.r = c.b = 1;
        else c.g = c.b = 1;
        image.color = c;
    }

    public void Back () {
        addingControl.GetComponent<Animation>().Play("ToFirstStep");
    }

    private IEnumerator Hide ()
    {
        addingControl.dolInput.text = addingControl.rubInput.text = addingControl.eurInput.text = string.Empty;
        Back();
        yield return new WaitForSeconds(.25f);
        addingControl.GetComponent<AddingControl>().Hide();
    }

    private void ClearList ()
    {
        GameObject[] purses = GameObject.FindGameObjectsWithTag("PurseInAdding");
        foreach (GameObject purse in purses)
            Destroy(purse);
    }
}
