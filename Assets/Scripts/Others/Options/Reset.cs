using UnityEngine;

public class Reset : MonoBehaviour {
    public void ResetApp()
    {
        PlayerPrefs.DeleteAll();
        LangSystem.Init();
        GetComponent<Modal>().Hide();
        GameObject.Find("OptionsManager").GetComponent<Options>().GoBack();
    }
}
