using UnityEngine;
using UnityEngine.SceneManagement;

public class Canvas : MonoBehaviour {
    public void GoBack() { SceneManager.LoadScene("SampleScene"); }
}
