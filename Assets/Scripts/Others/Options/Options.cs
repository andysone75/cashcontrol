using UnityEngine;

public class Options : MonoBehaviour
{
    [SerializeField] GameObject resetConfirm;
    [SerializeField] Animation anim;
    public static bool flag { get; protected set; }

    private void Start()
    {
        anim.Play("Start");
        flag = true;
    }

    public void ShowResetConfirm()
    {
        resetConfirm.SetActive(true);
        resetConfirm.GetComponent<Animation>().Play("ShowModal");
    }

    public void GoToPrivacyPolicy()
    {
        Application.OpenURL("https://docs.google.com/document/d/1OICVBueMgdGLPFj7SoyepZIwHIAeIEVHzmpI4Fev27A/edit#heading=h.g6dsppe494cv");
    }

    public void GoBack () { anim.Play("End"); }
}
