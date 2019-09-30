using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsBG : MonoBehaviour {
    public void GoToOptions () { SceneManager.LoadScene("Options"); }
    public void Disactivate () { gameObject.SetActive(false); }
}
