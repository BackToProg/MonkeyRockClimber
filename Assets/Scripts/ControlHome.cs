using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using webgl;

public class ControlHome : MonoBehaviour
{
    [SerializeField] GameObject dialogSelectLevel;
    [SerializeField] GameObject dialogExit;
    [SerializeField] GameObject content;
    [SerializeField] Sprite on, off;
    [SerializeField] Image img;
    void Start()
    {
        Time.timeScale = 1;
        if (!PlayerPrefs.HasKey("level"))
        {
            PlayerPrefs.SetInt("level", 1);
            PlayerPrefs.SetInt("diamond", 0);
        }
        for(int i = 0; i < 25; i++)
        {
            content.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            if (i <= PlayerPrefs.GetInt("level")-1)
            {
                content.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
                content.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                content.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                content.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
            }
        }
        if (Sound.ins.mute)
        {
            img.sprite = off;
        }
        else
        {
            img.sprite = on;
        }
    }

    public void BtnOpenSelectLevel()
    {
        Sound.ins.Click();
        dialogSelectLevel.SetActive(true);
    }
    public void BtnCloseSelectLevel()
    {
        Sound.ins.Click();
        dialogSelectLevel.SetActive(false);
    }
    public void SelectLevel(int i)
    {
        if(i <= PlayerPrefs.GetInt("level"))
        {
            Sound.ins.Click();
            SceneManager.LoadScene(i);
        }
        else
        {
            Debug.Log("Chưa mở !");
        }
    }
    public void OpenExit()
    {
        Sound.ins.Click();
        dialogExit.SetActive(true);
    }
    public void CloseExit()
    {
        Sound.ins.Click();
        dialogExit.SetActive(false);
    }
    public void Exit()
    {
        dialogExit.transform.GetChild(1).gameObject.SetActive(true);
        try
        {
            //MyAdvertisement.ShowFullNormal();
            Ads.Instance.ShowInterstitial();
        }
        catch
        {
            
        }
        Application.Quit();
    }
    public void Music()
    {
        Sound.ins.OnOffMusic();
        if (Sound.ins.mute)
        {
            img.sprite = off;
        }
        else
        {
            img.sprite = on;
        }
    }
}
