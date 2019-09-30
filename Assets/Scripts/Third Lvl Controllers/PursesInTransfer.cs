using System;
using UnityEngine;

public class PursesInTransfer : MonoBehaviour {
    [SerializeField] GameObject pursePrefab;

    public double sumToTransfer = 0;

    public void ShowPursesByCurency ()
    {
        GameObject[] purses = GameObject.FindGameObjectsWithTag("Purse");
        MainControl mainControl = GameObject.FindGameObjectWithTag("MainControl").GetComponent<MainControl>();
        PurseInfo purseInfo = mainControl.purseInfo;
        Vector2 posVector = new Vector2();
        int height = 0;
        for (int i = 0, count = 0; i < purses.Length; i++)
        {
            OnePurse onePurse = purses[i].GetComponent<OnePurse>();
            if (onePurse.Curency != purseInfo.Curency || i == purseInfo.Index) continue;
            GameObject purse = Instantiate(pursePrefab, transform);

            purse.GetComponent<Animation>().Play("Show");
            purse.name = i.ToString();
            posVector.x = purse.transform.localPosition.x;
            posVector.y = purse.transform.localPosition.y - (150 + 3) * count++;
            purse.transform.localPosition = posVector;
            
            purse.GetComponent<PurseInTransfer>().Init(onePurse.Name, onePurse.Value, purseInfo.Curency);

            height += 153;
        }
        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
    }

    public void Clear()
    {
        GameObject[] purses = GameObject.FindGameObjectsWithTag("PurseInTransfer");
        foreach (GameObject purse in purses)
            Destroy(purse);
    }

    public void AddToTransfer (double value)
    {
        sumToTransfer += value;
        PurseInfo purseInfo = GameObject.FindGameObjectWithTag("MainControl").GetComponent<MainControl>().purseInfo;
        double newVal = Math.Round(purseInfo.Value - sumToTransfer, 2);
        purseInfo.t_Value.text = (newVal).ToString() + " " + purseInfo.Curency;
    }
}
