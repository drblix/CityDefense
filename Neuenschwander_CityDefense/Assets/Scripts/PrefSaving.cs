using UnityEngine;

public class PrefSaving
{
    private const string hKey = "hScore";

    public static void SaveData(int hs)
    {
        PlayerPrefs.SetInt(hKey, hs);
        PlayerPrefs.Save();
    }

    public static int GetData()
    {
        int hs = PlayerPrefs.GetInt(hKey);
        return hs;
    }
}
