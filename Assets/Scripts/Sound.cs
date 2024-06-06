using UnityEngine;

public class Sound : MonoBehaviour
{
    public static Sound ins;
    [SerializeField] AudioSource sound, bg;
    [SerializeField] AudioClip click, platform, victory, lose;
    public bool mute;
    void Awake()
    {
        if (ins == null) ins = this;
        else if (ins != this) Destroy(gameObject);
        mute = PlayerPrefs.GetInt("Sound", 0) == 1;
        sound.mute = mute;
        bg.mute = mute;
        DontDestroyOnLoad(gameObject);
    }
    public void Click()
    {
        sound.PlayOneShot(click, 1);
    }
    public void ChangeHand()
    {
        sound.PlayOneShot(platform, 1);
    }
    public void win()
    {
        sound.PlayOneShot(victory, 1);
    }
    public void die()
    {
        sound.PlayOneShot(lose, 1);
    }
    public void OnOffMusic()
    {
        if (mute)
        {
            sound.mute = !sound.mute;
            bg.mute = !bg.mute;
            mute = false;
        }
        else
        {
            sound.mute = !sound.mute;
            bg.mute = !bg.mute;
            mute = true;
        }
        PlayerPrefs.SetInt("Sound", mute ? 1 : 0);
    }
}
