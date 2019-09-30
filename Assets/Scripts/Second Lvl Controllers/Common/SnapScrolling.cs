using UnityEngine;

public class SnapScrolling : MonoBehaviour {
    [Range(0, 50)]
    public float snapSpeed;
    
    [Range(0, 100)]
    [SerializeField] private float progressBar;
    [Range(0, 100)]
    [SerializeField] private float leftProgressBar;
    [Range(0, 100)]
    [SerializeField] private float midProgressBar;
    [Range(0, 100)]
    [SerializeField] private float rightProgressBar;

    private bool isScrolling;
    
    [SerializeField] private RectTransform leftPage;
    [SerializeField] private RectTransform midPage;
    [SerializeField] private RectTransform rightPage;
    private Vector2 leftVec, midVec, rightVec;
    
    RectTransform content;
    Vector2 contentVector;

    private enum States { Left = 800, Middle = 0, Right = -800 };
    private States state;

	void Start ()
    {
        content  = GetComponent<RectTransform>();
        leftVec  = new Vector2(15, 15);
        midVec   = new Vector2(25, 25);
        rightVec = new Vector2(15, 15);
        
    }
	
	void FixedUpdate ()
    {
        float pos = content.anchoredPosition.x;
        if (!isScrolling)
        {
            float deltaLeft   = Mathf.Abs((float)States.Left - pos);
            float deltaMiddle = Mathf.Abs((float)States.Middle - pos);
            float deltaRight  = Mathf.Abs((float)States.Right - pos);

            float min = Mathf.Min(deltaMiddle, Mathf.Min(deltaLeft, deltaRight));

            if (min == deltaLeft) state = States.Left;
            else if (min == deltaMiddle) state = States.Middle;
            else state = States.Right;

            contentVector.x = Mathf.SmoothStep(pos, (float)state, snapSpeed * Time.deltaTime);
            content.anchoredPosition = contentVector;
        }

        progressBar = (pos + 800) / 16; // (pos + 400) / 1600 * 100

        if (progressBar > 50)
        {
            leftProgressBar = (progressBar - 50) / 50 * 100;
            rightProgressBar = 0;
        }
        else
        {
            leftProgressBar = 0;
            rightProgressBar = (50 - progressBar) / 50 * 100;
        }

        midProgressBar = 100 - Mathf.Abs(progressBar - 50) / 50 * 100;

        leftVec.x   = leftVec.y   = 15 + leftProgressBar * 0.1f;
        midVec.x    = midVec.y    = 15 + midProgressBar * 0.1f;
        rightVec.x  = rightVec.y  = 15 + rightProgressBar * 0.1f;

        leftPage.sizeDelta = leftVec;
        midPage.sizeDelta = midVec;
        rightPage.sizeDelta = rightVec;
    }

    public void Scrolling (bool scroll) { isScrolling = scroll; }
}
