using UnityEngine;
using UnityEngine.UI;

public class Modal : MonoBehaviour {
    [SerializeField] Text title, leftBtn, rightBtn;

    public void Show()
    {
        gameObject.SetActive(true);
        GetComponent<Animation>().Play("ShowModal");
    }

    public void Hide() { GetComponent<Animation>().Play("HideModal"); }
    public void Disactive() { gameObject.SetActive(false); }
}
