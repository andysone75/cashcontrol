using UnityEngine;

public class MenuSwitch : MonoBehaviour
{
    bool isCost, isIncome, isPurse;
    Animation costsAnim, incomesAnim, pursesAnim;

    [SerializeField]
    Animation costs, incomes, purses;
    CameraControl mainCamera;

    private void Start()
    {
        isCost = true;

        costsAnim   = GameObject.Find("CostsInfo").GetComponent<Animation>();
        incomesAnim = GameObject.Find("IncomesInfo").GetComponent<Animation>();
        pursesAnim  = GameObject.Find("PursesInfo").GetComponent<Animation>();

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>();
    }

    public void IncomesClick ()
    {
        if (isIncome) return;

        if (isPurse)
        {
            purses.Play("Off");
            pursesAnim.Play("OutToLeft");
        }
        else
        {
            costs.Play("Off");
            costsAnim.Play("Out");
        }

        incomes.Play("On");
        incomesAnim.Play("In");

        isCost   = false;
        isPurse  = false;
        isIncome = true;
        
        mainCamera.ChangeBgToIncome();
    }

    public void CostsClick()
    {
        if (isCost) return;

        if (isPurse)
        {
            purses.Play("Off");
            pursesAnim.Play("OutToRight");
        }
        else
        {
            incomes.Play("Off");
            incomesAnim.Play("Out");
        }

        costs.Play("On");
        costsAnim.Play("In");

        isCost   = true;
        isPurse  = false;
        isIncome = false;

        mainCamera.ChangeBgToCost();
    }

    public void PursesClick()
    {
        if (isPurse) return;

        if (isCost)
        {
            costs.Play("Off");
            costsAnim.Play("Out");
            pursesAnim.Play("InFromRight");
        }
        else
        {
            incomes.Play("Off");
            incomesAnim.Play("Out");
            pursesAnim.Play("InFromLeft");
        }

        purses.Play("On");

        isCost   = false;
        isPurse  = true;
        isIncome = false;

        mainCamera.ChangeBgToPurse();
    }

    public bool IsIncomeActive ()  { return isIncome; }
    public bool IsCostActive   ()  { return isCost;   }
}
