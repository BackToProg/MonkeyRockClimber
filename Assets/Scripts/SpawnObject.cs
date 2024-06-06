using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SpawnObject : MonoBehaviour
{
    public static SpawnObject ins;
    public GameObject RockParent;
    public GameObject rockNormal;
    public GameObject rockDiamond;
    public GameObject rockFake;
    public GameObject DHRoi;
    public Transform posStart;
    [SerializeField] GameObject[] effect;
    [SerializeField] GameObject PointLight;
    [SerializeField] GameObject ObFinish;
    public int option;
    [SerializeField] Sprite[] dhBt;
    [SerializeField] Sprite[] dhHong;
    [SerializeField] Sprite nenDH;
    [SerializeField] GameObject[] Deco;
    private ParticleSystem ps;
    [SerializeField] Sprite dh6;
    int childCound;
    public GameObject bat;
    public GameObject Plane;
    public GameObject ballon;
    [SerializeField] Transform[] posLeft, posRight, poscloud;
   // [SerializeField] GameObject vong;
    [SerializeField] GameObject cloud;
    [SerializeField] Transform posBallonLeft, posBallonRight;
    private void Awake()
    {
        ins = this;
        //option = PlayerPrefsController.MapGame;
        //if (option == 0)
        //    Camera.main.backgroundColor = new Color32(107, 74, 163, 255);
        //if (option == 1)
        //    Camera.main.backgroundColor = new Color32(84, 142, 245, 255);
        //Deco[PlayerPrefsController.MusicPlay].SetActive(true);
        //RockParent = Instantiate(SoundManager.Instance.dataSong.songs[PlayerPrefsController.MusicPlay].Platform, Vector3.zero, Quaternion.identity);
        posStart = RockParent.transform.GetChild(0);
        ObFinish.transform.position = RockParent.transform.GetChild(RockParent.transform.childCount - 1).position;
        ps = PointLight.GetComponent<ParticleSystem>();
        childCound = RockParent.transform.childCount;
    }

    public void ChangeRockDiamond(GameObject rock)
    {
        rock.transform.GetChild(0).gameObject.SetActive(false);
        ShowEffect(rock.transform.position, 0);
    }
    int yRandom;
    GameObject obInstantiate;

    public void SpawnRockAtTarget(Vector2 target)
    {
        int rd = Random.Range(0, 100);
        if (rd < 10)
            obInstantiate = Instantiate(rockDiamond, target, Quaternion.identity);
        else
            obInstantiate = Instantiate(rockNormal, target, Quaternion.identity);
        obInstantiate.transform.SetParent(RockParent.transform);
    }
    GameObject obFirst;
    public void RoiDH(Transform ob)
    {
        StartCoroutine(delay(ob));
        ShowEffect(ob.position, 1);
    }
    IEnumerator delay(Transform ob)
    {
        yield return new WaitForSeconds(Random.Range(.1f, .3f));
        ob.gameObject.GetComponent<SpriteRenderer>().sprite = nenDH;
        ob.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 160);
        GameObject obj = Instantiate(DHRoi, ob.position, Quaternion.identity);
    }
    /// <summary>
    /// id=0: pick diamond, id=1: bụi rơi, 2hieuUngperfect, 3 phao hoa
    /// </summary>
    /// <param name="pos">vị trí sinh ra hiệu ứng</param>
    /// <param name="id">id hiệu ứng</param>
    public void ShowEffect(Vector2 pos, int id)
    {
        //effect[id].transform.position = pos;
        //effect[id].SetActive(true);
        GameObject ob = Instantiate(effect[id], pos, Quaternion.identity);
        Destroy(ob, 1f);
    }
    int i = 0;
    IEnumerator ChangeDh(float time)
    {
        yield return new WaitForSeconds(time);
        changeDH(RockParent.transform.GetChild(i).gameObject);
    }
    public void ChangeSpriteDH(GameObject ob, bool checkHong)
    {
        ob.transform.localScale = new Vector3(0.7f, 0.7f, 0);
        //if (checkHong)
        //{
        //    ob.GetComponent<SpriteRenderer>().sprite = dhHong[option];
        //}
        //else
        //{
        //    ob.GetComponent<SpriteRenderer>().sprite = dhBt[option];
        //}
    }
    public void changeDH(GameObject gameObject)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = dh6;
        obj = gameObject;
    }
    public void ChangeDhDefault()
    {
        //if (obj != null)
        //    obj.GetComponent<SpriteRenderer>().sprite = dhBt[option];
    }
    GameObject obj;
    GameObject plane;
    GameObject obBallon;
    void RandomPlane()
    {
        int rd2 = Random.Range(0, 2);
        if (rd2 == 0)
        {
            plane = Instantiate(Plane, posRight[Random.Range(0, 3)].position, Quaternion.identity);
            Destroy(plane, 6f);
        }
        else
        {
            plane = Instantiate(Plane, posLeft[Random.Range(0, 3)].position, Quaternion.identity);
            plane.transform.localScale = new Vector3(-1, 1, 1);
            Destroy(plane, 6f);
        }
    }
    void RandomBallon()
    {
        int rd2 = Random.Range(0, 2);
        obBallon = Instantiate(ballon, poscloud[Random.Range(0, 3)].position, Quaternion.identity);
        if (rd2 == 0)
        {
            obBallon = Instantiate(ballon, posBallonLeft.position, Quaternion.identity);
            ballon.transform.localScale = new Vector3(-1, 1, 1);
            Destroy(obBallon, 10f);
        }
        else
        {
            obBallon = Instantiate(ballon, posBallonRight.position, Quaternion.identity);
            Destroy(obBallon, 10f);
        }
    }
}
