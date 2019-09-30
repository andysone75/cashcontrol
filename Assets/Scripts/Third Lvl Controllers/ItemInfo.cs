using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour {
    [SerializeField] GameObject renameInput;

    [Header("Sprites")]
    [SerializeField] Sprite income;
    [SerializeField] Sprite cost;

    [Header("Info")]
    [SerializeField] Text Title;
    [SerializeField] Text Value;
    [SerializeField] Text CreateDate;

    private int Index;
    private int[] Ids;
    private double[] Vals;

    public void Show(int index, string Tag, int[] ids, double[] vals, string title, string value, DateTime date)
    {
        Index = index;
        tag = Tag;
        Ids = ids;
        Vals = vals;
        gameObject.SetActive(true);
        GetComponent<Image>().sprite = tag == "Income" ? income : cost;
        Title.text = title;
        Value.text = value;
        CreateDate.text = string.Format("{0}.{1}.{2} {3}", date.Day, date.Month, date.Year, date.TimeOfDay);
        GetComponent<Animation>().Play("FadeIn");
    }

    public void Close()
    {
        if (renameInput.activeSelf) CloseRenameInput();
        GetComponent<Animation>().Play("FadeOut");
    }

    public void ShowRenameInput ()
    {
        renameInput.SetActive(true);
        renameInput.GetComponent<InputField>().text = Title.text;
    }
    public void Rename ()
    {
        var newName = renameInput.GetComponent<InputField>().text;
        var mainControl = GameObject.FindGameObjectWithTag("MainControl").GetComponent<MainControl>();

        Title.text = newName;
        if (tag == "Income") mainControl.incomes.Rename(Index, newName);
        else mainControl.costs.Rename(Index, newName);

        CloseRenameInput();
    }
    public void CloseRenameInput() { renameInput.SetActive(false); }

    public void Delete ()
    {
        var mainControl = GameObject.FindGameObjectWithTag("MainControl").GetComponent<MainControl>();
        for (var i = 0; i < Ids.Length; i++)
        {
            var index = mainControl.purses.Ids.IndexOf(Ids[i]);
            if (index != -1) GameObject.FindGameObjectsWithTag("Purse")[index].GetComponent<OnePurse>().Add(tag == "Income" ? -Vals[i] : Vals[i]);
        }
        GameObject.FindGameObjectsWithTag(tag)[Index].GetComponent<ItemControl>().Delete();

        Close();
    }
    
    public void Disactive () { gameObject.SetActive(false); }
}
