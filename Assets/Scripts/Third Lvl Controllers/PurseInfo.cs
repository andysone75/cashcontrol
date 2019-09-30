using System;
using UnityEngine;
using UnityEngine.UI;

public class PurseInfo : MonoBehaviour {
    public int Index { get; private set; }
    public string Title { get; private set; }
    public double Value { get; private set; }
    public char Curency { get; private set; }
    DateTime CreatingDate;

    [Header("Text")]
    [SerializeField] Text t_Title;
    public Text t_Value;
    [SerializeField] Text t_CreateDate;
    [SerializeField] Text closeBtnText;
    [SerializeField] PursesInTransfer pursesInTransfer;
    [SerializeField] GameObject pursePrefab;
    [SerializeField] Image UpperBtnImage;
    [SerializeField] Sprite deleteIcon;
    [SerializeField] Sprite checkMark;
    [SerializeField] GameObject renameInput;
    
    Vector2 posVector;
    bool isTransferWindowOpen = false;

    public void Show (int index, string title, double value, char curency, DateTime creatingData)
    {
        Index = index;
        Title = title;
        Value = value;
        Curency = curency;
        CreatingDate = creatingData;

        Render();

        gameObject.SetActive(true);
        GetComponent<Animation>().Play("FadeIn");
    }

    public void Close()
    {
        if (renameInput.activeSelf) CloseRenameInput();
        if (isTransferWindowOpen) DeclineTransfers();
        else GetComponent<Animation>().Play("FadeOut");
    }
    public void Disactive () { gameObject.SetActive(false); }
    public void CloseRenameInput() { renameInput.SetActive(false); }

    public void ShowRenameInput()
    {
        renameInput.SetActive(true);
        renameInput.GetComponent<InputField>().text = Title;
    }
    
    public void Rename()
    {
        var newName = renameInput.GetComponent<InputField>().text;

        Title = newName;
        Render();

        GameObject.FindGameObjectsWithTag("Purse")[Index].GetComponent<OnePurse>().UpdateItem(newName);

        CloseRenameInput();
    }

    public void CloseTransferWindow()
    {
        closeBtnText.text = "ЗАКРЫТЬ";
        GetComponent<Animation>().Play("CloseTransferWindow");
        isTransferWindowOpen = false;
        foreach (GameObject purse in GameObject.FindGameObjectsWithTag("PurseInTransfer"))
        {
            purse.GetComponent<Animation>().Play("Hide");
            purse.GetComponent<PurseInTransfer>().HideInput();
        }
    }

    public void Clear() { pursesInTransfer.Clear(); }

    private void DeclineTransfers()
    {
        GameObject[] allPursesInTransfer = GameObject.FindGameObjectsWithTag("PurseInTransfer");
        foreach (GameObject purse in allPursesInTransfer)
        {
            var purseInTransfer = purse.GetComponent<PurseInTransfer>();
            purseInTransfer.input.text = string.Empty;
            purseInTransfer.OnEndEdit();
            purseInTransfer.lastInput = 0;
        }
        GameObject.FindGameObjectWithTag("PursesInTransfer").GetComponent<PursesInTransfer>().sumToTransfer = 0;
        CloseTransferWindow();
    }

    private void Render ()
    {
        t_Title.text = Title;
        t_Value.text = Value.ToString() + ' ' + Curency;
        t_CreateDate.text = string.Format("{0}.{1}.{2} {3}", CreatingDate.Day, CreatingDate.Month, CreatingDate.Year, CreatingDate.TimeOfDay);
    }

    public void ShowTransferWindow ()
    {
        int count = 0;
        GameObject[] purses = GameObject.FindGameObjectsWithTag("Purse");
        foreach (GameObject purse in purses)
        {
            if (purse.GetComponent<OnePurse>().Curency == Curency) count++;
        }
        if (count == 1) return; 
        pursesInTransfer.ShowPursesByCurency();
        closeBtnText.text = "ОТМЕНИТЬ";
        GetComponent<Animation>().Play("ShowTransferWindow");
        isTransferWindowOpen = true;
    }

    public void SetCheckMark()  { UpperBtnImage.sprite = checkMark; }
    public void SetDeleteMark() { UpperBtnImage.sprite = deleteIcon; }

    public void OnUpperBtnClick()
    {
        if (isTransferWindowOpen)
        {
            Value = MainControl.ReadTheValue(t_Value.text);
            GameObject.FindGameObjectsWithTag("Purse")[Index].GetComponent<OnePurse>().UpdateByTransfer(Value);
            CloseTransferWindow();
        }
        else
        {
            GameObject.FindGameObjectsWithTag("Purse")[Index].GetComponent<OnePurse>().Delete();
            Close();
        }
    }
}
