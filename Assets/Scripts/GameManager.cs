using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using webgl;

public class GameManager : MonoBehaviour
{
    public static GameManager ins;
    [SerializeField] GameObject dialogLose;
    [SerializeField] GameObject dialogWin;
    [SerializeField] GameObject dialogVideo;
    [SerializeField] GameObject panelContinue;
    [SerializeField] GameObject txtThongBao;
    public int score;
    [SerializeField] Text txtScrore;
    [SerializeField] Text txtDiamond;
    [SerializeField] Text txtMeter;
    int slDiamond;
    [SerializeField] Text txtMeterWin, txtDiamondwin;
    [SerializeField] Text txtMeterLose, txtDiamondLose;
    int meter, meterMax;
    float temp = 3.15f;
    private void Awake()
    {
        ins = this;
        score = 0;
        IncreaseScore(0);
    }
    private void Start()
    {
        ProCamera2DTransitionsFX.Instance.TransitionEnter();
        meter = Mathf.RoundToInt(SpawnObject.ins.RockParent.transform.GetChild(SpawnObject.ins.RockParent.transform.childCount - 1).position.y - SpawnObject.ins.RockParent.transform.GetChild(0).position.y);
        meterMax = meter;
        txtMeter.text = meter + "M";
        slDiamond = PlayerPrefs.GetInt("diamond");
        txtDiamond.text = slDiamond.ToString();
    }
    public void GiamMeter(float y)
    {
        meter -= Mathf.RoundToInt(y - temp);
        temp = y;
        txtMeter.text = meter + "M";
    }
    public void CLickToPlay()
    {
        ControlPlayer.instance.StartGroundClimb();
    }
    bool viewVideoContineu;
    public void Win()
    {
        //try { MyAdvertisement.ShowFullNormal(); } catch { }
        if (PlayerPrefs.GetInt("level") == SceneManager.GetActiveScene().buildIndex && PlayerPrefs.GetInt("level") < 25)
        {
            PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        }
        meter = 0;
        txtMeterWin.text = meter.ToString();
        txtDiamondwin.text = slDiamond.ToString();
        dialogWin.SetActive(true);
    }
    public void Lose()
    {
        txtMeterLose.text = meter.ToString();
        txtDiamondLose.text = slDiamond.ToString();
        dialogLose.SetActive(true);

    }
    public void ReturnHome()
    {
        Sound.ins.Click();
        SceneManager.LoadScene(0);
        Ads.Instance.ShowInterstitial();
    }
    public void Replay()
    {

        Sound.ins.Click();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Ads.Instance.ShowInterstitial();
    }
    public void Next()
    {
        if (SceneManager.GetActiveScene().buildIndex < 23)
        {
            Sound.ins.Click();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
        Ads.Instance.ShowInterstitial();
    }
    public void IncreaseScore(int point)
    {
        score += point;
        txtScrore.text = score.ToString();
    }
    public void TangKimCuong(int i)
    {
        slDiamond += i;
        txtDiamond.text = slDiamond.ToString();
        PlayerPrefs.SetInt("diamond", PlayerPrefs.GetInt("diamond") + i);
    }
    public void OpenDialogVideo()
    {
        dialogVideo.SetActive(true);
    }
    public void CloseDialogVideo()
    {
        dialogVideo.SetActive(false);
        Lose();
    }
    public void WatchVideo()
    {
        Ads.Instance.ShowReward(Revival);
        // if (MobileRewardAd.instance.rewardBasedVideoAd.IsLoaded())
        // {
        //     MobileRewardAd.instance.rewardBasedVideoAd.Show();
        // }
        // else
        // {
        //     GameObject obj = Instantiate(txtThongBao, dialogVideo.transform.position, Quaternion.identity);
        //     Destroy(obj, 1f);
        // }
    }
    
    public void Revival()
    {
        dialogVideo.SetActive(false);
        panelContinue.SetActive(true);
        ControlPlayer.instance.Revival();
    }
    public void ClickToContinue()
    {
        panelContinue.SetActive(false);
        ControlPlayer.instance.ContinueRotate();
    }
}
