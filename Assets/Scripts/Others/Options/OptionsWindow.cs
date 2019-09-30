using UnityEngine;

public class OptionsWindow : MonoBehaviour {
    [SerializeField] GameObject bg;

    void Start () {
        if (!Options.flag) return;
        bg.SetActive(true);
        bg.GetComponent<Animation>().Play("FadeOut");
    }

    public void GoToOptions() {
        bg.SetActive(true);
        bg.GetComponent<Animation>().Play("FadeIn");
    }
}
