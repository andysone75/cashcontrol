using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LangSystem : MonoBehaviour {
    public static Lang lang;
    
    [SerializeField] Image langBtnImage;
    [SerializeField] Sprite[] flags;
    [SerializeField] Text[] optionsTexts;
    [SerializeField] Text[] resetConfirmModal;

    private int index = 0;
    private string[] langsArr = new string[] { "ru_RU", "en_US" };

    public static void Init ()
    {
        if (!PlayerPrefs.HasKey("Language"))
        {
            if (Application.systemLanguage == SystemLanguage.Russian || Application.systemLanguage == SystemLanguage.Ukrainian || Application.systemLanguage == SystemLanguage.Belarusian)
                PlayerPrefs.SetString("Language", "ru_RU");
            else PlayerPrefs.SetString("Language", "en_US");
        }
    }

    private void SetLang()
    {
        for (var i = 0; i < optionsTexts.Length; i++) optionsTexts[i].text = lang.options[i];
        for (var i = 0; i < resetConfirmModal.Length; i++) resetConfirmModal[i].text = lang.ResetConfirmModal[i];
    }

    public static void Load()
    {
        string json;
        string path = string.Format("{0}/Languages/{1}.json", Application.streamingAssetsPath, PlayerPrefs.GetString("Language"));
#if UNITY_EDITOR
        json = File.ReadAllText(path);
#else
        WWW reader = new WWW(path);
        while (!reader.isDone) { }
        json = reader.text;
#endif
        lang = JsonUtility.FromJson<Lang>(json);
    }

    public void Switch ()
    {
        if (++index == langsArr.Length) index = 0;
        PlayerPrefs.SetString("Language", langsArr[index]);
        langBtnImage.sprite = flags[index];
        Load();
        SetLang();
    }
}

public class Lang
{
    public string[] options = new string[3];
    public string[] ResetConfirmModal = new string[4];

    public string[] Menu_Other = new string[3];
    public string[] CostsInfo = new string[3];
    public string[] PursesInfo = new string[1];
    public string[] IncomesInfo = new string[3];
    public string[] Switcher = new string[3];
    public string[] ItemInfo = new string[1];
    public string[] PurseInfo = new string[2];
    public string[] AddingControl = new string[3];
    public string[] AddingPurse = new string[4];
    public string[] NoPursesModal = new string[4];
}