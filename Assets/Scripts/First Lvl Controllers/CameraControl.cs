using System.Collections;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    private Camera mainCamera;

    private void Start() { mainCamera = GetComponent<Camera>(); }

    public void ChangeBgToIncome () { StartCoroutine(ChangeBgColor(MyColors.Nephritis)); }
    public void ChangeBgToCost() { StartCoroutine(ChangeBgColor(MyColors.Pomegranate)); }
    public void ChangeBgToPurse() { StartCoroutine(ChangeBgColor(MyColors.GreenSea)); }

    private IEnumerator ChangeBgColor (Color color)
    {
        for (var i = 0f; i < 1; i += 0.05f)
        {
            mainCamera.backgroundColor = Color.Lerp(mainCamera.backgroundColor, color, i);
            yield return new WaitForSeconds(0.015f);
        }
    }
}
