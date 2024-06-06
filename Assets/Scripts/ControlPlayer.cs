using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine.EventSystems;
using Spine.Unity;
using webgl;

public class ControlPlayer : MonoBehaviour
{
    public static ControlPlayer instance;
    public Shader myShader;
    public Material spineMat;
    public GameObject Player;
    SkeletonAnimation ske;
    [SerializeField] GameObject thongBaoPerfect;
    [SerializeField] GameObject FxGlow1, FxGlow2;
    private Animator animThongBao;
    [SerializeField] Transform pointLeft;
    [SerializeField] Transform pointRight;
    [SerializeField] CircleCollider2D boxLeft, boxRight;
    [Tooltip("false: handLeft, true: handRight")]
    bool checkHand;
    bool checkCanClimb;
    public bool checkDiamond;
    public bool checkFinish;
    GameObject obRotate;
    GameObject obWait;
    GameObject objectNext;
    GameObject objectFail;
    Tween myTween;
    Rigidbody2D rigi;
    public float durationOld = 3f;

    [SerializeField] Transform[] pathEnd = new Transform[5];
    Vector3 localPosLeft, localPosRight;

    Quaternion rotateRight, rotateLeft;
    string currentAnim;
    public string anim_XoayTrai = "xoaytrai";
    public string anim_XoayPhai = "xoay_phai";
    public string anim_Nhay = "nhay";
    public string anim_finish = "finish";
    public string anim_idle = "Idle";
    public string anim_dieRight = "die_phai";
    public string anim_dieLeft = "die_trai";
    public Transform obSave;
    public Transform obSave2;
    [SerializeField] SpriteRenderer nocNha;
    bool spawn;
    private void Awake()
    {
        instance = this;
        ske = Player.GetComponent<SkeletonAnimation>();
        rigi = Player.GetComponent<Rigidbody2D>();
        animThongBao = thongBaoPerfect.transform.GetChild(0).GetComponent<Animator>();
    }
    void Start()
    {
        localPosLeft = boxLeft.transform.localPosition;
        localPosRight = boxRight.transform.localPosition;
        rotateLeft = boxLeft.transform.localRotation;
        rotateRight = boxRight.transform.localRotation;
        obSave = SpawnObject.ins.posStart;
        Time.timeScale = 1;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject(0) && !checkFinish)
        {
            ChangeHand();
        }
        //if (!spawn && Input.GetKey(KeyCode.Space))
        //{
        //    spawn = true;
        //    StartCoroutine(AutoSpawn());
        //}
    }

    /// <summary>
    /// bay len va roi xuong hon da dau tien, dung khi bat dau choi, tao duong cong giua 5 diem
    /// </summary>
    public void StartGroundClimb()
    {
        begin = true;
        //ChangeHand();
    }
    bool begin;
    /// <summary>
    /// bay len va roi xuong mai nha, ket thuc game
    /// </summary>
    public void EndGame()
    {
        Sound.ins.win();
        checkFinish = true;
        Material newMaterial = new Material(spineMat);
        newMaterial.shader = myShader;
        ske.CustomMaterialOverride.Add(spineMat, newMaterial);
        ske.GetComponent<MeshRenderer>().material.shader = myShader;
        ChangeHand();
        StartCoroutine(changeIdle());
    }

    IEnumerator changeIdle()
    {
        TangGiamDuration(1.5f);
        yield return new WaitForSeconds(1.5f);
        ProCamera2D.Instance.RemoveAllCameraTargets();
        ProCamera2D.Instance.AddCameraTarget(Player.transform);
        DOTween.KillAll();
        SetAnimation(anim_finish, false);
        Player.transform.DORotateQuaternion(Quaternion.identity, 1f);
        Player.transform.DOMove(pathEnd[0].position, 1.5f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            Player.transform.DOMove(pathEnd[1].position, .5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                nocNha.sortingLayerName = "mid";
                ProCamera2DShake.Instance.Shake(0);
            });
        });
        yield return new WaitForSeconds(ske.skeletonDataAsset.GetSkeletonData(true).FindAnimation(anim_finish).duration);
        ske.timeScale = .7f;
        SetAnimation(anim_idle, true);
        yield return new WaitForSeconds(4f);
        GameManager.ins.Win();
    }
    public int idList = 0;
    float time;
    float timeNew;

    /// <summary>
    /// thay doi sang hon da khac
    /// </summary>
    public void ChangeHand()
    {
        ProCamera2D.Instance.RemoveAllCameraTargets();
        if (checkCanClimb)
        {
            Sound.ins.ChangeHand();
            //thay đổi đối tượng target camera và thay sprite của đá diamond
            ProCamera2D.Instance.AddCameraTarget(objectNext.transform);
            GameManager.ins.GiamMeter(objectNext.transform.position.y);
            //SpawnObject.ins.ShowEffect(objectNext.transform.position, 1);
            //if (idList > 10)
            //{
            //    int rd = Random.Range(0, 2);
            //    if (rd == 0)
            //        SpawnObject.ins.RoiDH(obSave2);
            //}
            obSave2 = obSave;
            obSave = objectNext.transform;
            //tat collider khi da cham vao
            //objectNext.GetComponent<CircleCollider2D>().enabled = false;
            //SpawnObject.ins.ChangeSpriteDH(objectNext, true);
            if (checkDiamond)
            {
                SpawnObject.ins.ChangeRockDiamond(objectNext);
                // tang so luong kim cuong
                GameManager.ins.TangKimCuong(1);
            }
            if (checkHand)
            {
                if (myTween != null)
                    myTween.Kill();
                // hand right
                Player.transform.SetParent(pointRight);
                //set doi tuong chua hero va quay 
                obRotate = pointRight.gameObject;
                if (begin)
                    if (Vector2.Distance(boxRight.transform.position, pointRight.position) < .2f)
                    {
                        ShowNotification(pointRight.position, true);
                    }
                    else
                    {
                        ShowNotification(pointRight.position, false);
                    }
                MovePlayerToCenterRock(false);
                //set doi tuong chua hero o lan nhay tiep theo
                obWait = pointLeft.gameObject;
                checkHand = false;
                // change animation
                SetAnimation(anim_XoayPhai, false);
                myTween = obRotate.transform.DORotate(new Vector3(0, 0, -360f), durationOld, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).SetRelative();
                boxRight.enabled = false;
                boxLeft.enabled = true;
            }
            else
            {
                // hand left
                if (myTween != null)
                    myTween.Kill();
                Player.transform.SetParent(pointLeft);
                obRotate = pointLeft.gameObject;
                if (begin)
                    if (Vector2.Distance(boxLeft.transform.position, pointLeft.position) < .1f)
                    {
                        ShowNotification(pointLeft.position, true);
                    }
                    else
                    {
                        ShowNotification(pointLeft.position, false);
                    }
                MovePlayerToCenterRock(true);
                obWait = pointRight.gameObject;
                checkHand = true;
                SetAnimation(anim_XoayTrai, false);
                myTween = obRotate.transform.DORotate(new Vector3(0, 0, 360f), durationOld, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).SetRelative();
                boxRight.enabled = true;
                boxLeft.enabled = false;
            }
            SaveInfo();
            checkCanClimb = false;        
        }
        else
        {
            Die();
        }

    }

    public void Die()
    {
        if (!checkFinish)
        {
            Sound.ins.die();
            checkFinish = true;
            myTween.Kill();
            if (checkHand)
                SetAnimation(anim_dieRight, false);
            else
                SetAnimation(anim_dieLeft, false);
            Player.transform.DOMoveY(Player.transform.position.y - 15f, 1.5f).SetEase(Ease.Linear);
            StartCoroutine(delay());
        }
        FxGlow1.SetActive(false);
        FxGlow2.SetActive(false);
    }
    int demXemVideo;
    IEnumerator delay()
    {
        yield return new WaitForSeconds(1.3f);
        ProCamera2DShake.Instance.Shake(1);
        yield return new WaitForSeconds(.5f);
        GameManager.ins.OpenDialogVideo();
    }
    /// <summary>
    /// thay đổi timeScale của tween để tốc độ vòng quay hợp với tốc độ duration 
    /// </summary>
    /// <param name="durationNew"></param>
    public void TangGiamDuration(float durationNew)
    {
        // durationNew = durationOld / (tween.timeScale * Time.timeScale)
        // truong hop global timescale=1;
        myTween.timeScale = durationOld / durationNew;
    }
    /// <summary>
    /// thông báo Perfect
    /// </summary>
    /// <param name="pos">vị trí hiện thông báo</param>
    void ShowNotification(Vector2 pos, bool checkPerfect)
    {
        GameObject ob = Instantiate(thongBaoPerfect, pos, Quaternion.identity);
        if (checkPerfect)
        {
            ob.transform.GetChild(0).GetComponent<Text>().text = TextTranslator.GetText("Идеально х", "Perfect x") + point;
            point++;
            ob.transform.GetChild(0).GetComponent<Text>().color = new Color32(245, 250, 6, 255);
            ob.transform.GetChild(0).GetComponent<Text>().fontSize = 55;
            SpawnObject.ins.ShowEffect(pos, 2);
        }
        else
        {
            ob.transform.GetChild(0).GetComponent<Text>().text =TextTranslator.GetText("Хорошо", "Good");
            point = 1;
            ob.transform.GetChild(0).GetComponent<Text>().color = new Color32(255, 255, 255, 255);
            ob.transform.GetChild(0).GetComponent<Text>().fontSize = 55;
            SpawnObject.ins.ShowEffect(pos, 4);
        }
        Destroy(ob, 2f);
        GameManager.ins.IncreaseScore(point);
    }
    int point = 1;
    public void ThroughRock(Vector2 target, GameObject ob)
    {
        objectNext = ob;
        checkCanClimb = true;
        if (!ReferenceEquals(obWait, null))
            obWait.transform.position = target;
    }
    /// <summary>
    /// không chạm vào đá
    /// </summary>
    public void FailRock()
    {
        checkCanClimb = false;
    }
    /// <summary>
    /// chạm vào đá đểu
    /// </summary>
    public void RockFail(GameObject ob)
    {
        objectFail = ob;
        checkCanClimb = false;
    }
    /// <summary>
    /// cho nhân vật bám vào giữa hòn đá
    /// </summary>
    /// <param name="left"></param>
    void MovePlayerToCenterRock(bool left)
    {
        if (left)
        {
            boxLeft.transform.position = pointLeft.position;
            Player.transform.position = boxLeft.transform.position;


            boxLeft.transform.RotateAround(boxLeft.transform.position, boxLeft.transform.forward, -rotateLeft.eulerAngles.z);
            boxLeft.transform.RotateAround(boxLeft.transform.position, boxLeft.transform.right, -rotateLeft.eulerAngles.x);
            //boxLeft.transform.RotateAround(boxLeft.transform.position, boxLeft.transform.up, -rotateLeft.eulerAngles.y);
            transform.rotation = boxLeft.transform.rotation;

            Player.transform.position += -transform.right * localPosLeft.x;
            Player.transform.position += -transform.up * localPosLeft.y;

            boxLeft.transform.localPosition = localPosLeft;
            boxLeft.transform.localRotation = rotateLeft;
        }
        else
        {
            boxRight.transform.position = pointRight.position;
            Player.transform.position = boxRight.transform.position;

            boxRight.transform.RotateAround(boxRight.transform.position, boxRight.transform.forward, -rotateRight.eulerAngles.z);
            boxRight.transform.RotateAround(boxRight.transform.position, boxRight.transform.right, -rotateRight.eulerAngles.x);
            // boxRight.transform.RotateAround(boxRight.transform.position, boxRight.transform.up, -rotateRight.eulerAngles.y);
            transform.rotation = boxRight.transform.rotation;

            Player.transform.position += -transform.right * localPosRight.x;
            Player.transform.position += -transform.up * localPosRight.y;

            boxRight.transform.localPosition = localPosRight;
            boxRight.transform.localRotation = rotateRight;
        }
    }
    /// <summary>
    /// tự động chạy game, hàm này dùng để dựng map
    /// </summary>
    /// <returns></returns>
    IEnumerator AutoRotate()
    {
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            if (checkHand)
            {
                SpawnObject.ins.SpawnRockAtTarget(boxRight.transform.position);
                yield return new WaitForSeconds(.02f);
                ChangeHand();
            }
            else
            {
                SpawnObject.ins.SpawnRockAtTarget(boxLeft.transform.position);
                yield return new WaitForSeconds(.02f);
                ChangeHand();
            }
        }
    }
    IEnumerator AutoSpawn()
    {
        if (checkHand)
        {
            SpawnObject.ins.SpawnRockAtTarget(boxRight.transform.position);
            yield return new WaitForSeconds(.02f);
            ChangeHand();
        }
        else
        {
            SpawnObject.ins.SpawnRockAtTarget(boxLeft.transform.position);
            yield return new WaitForSeconds(.02f);
            ChangeHand();
        }
        yield return new WaitForSeconds(.5f);
        spawn = false;
    }
    /// <summary>
    /// chuyển đổi animation
    /// </summary>
    /// <param name="anim">tên animation</param>
    /// <param name="loop">true: lặp, false: không lặp</param>
    void SetAnimation(string anim, bool loop)
    {
        if (currentAnim != anim)
        {
            ske.loop = loop;
            ske.AnimationName = anim;
            currentAnim = anim;
        }
    }
    Quaternion rotateOB;
    Quaternion rotatePlayer;
    /// <summary>
    /// lưu thông tin về thời gian nhạc, góc quay sau mỗi lần click platform mới
    /// </summary>
    void SaveInfo()
    {
        rotateOB = obRotate.transform.rotation;
        rotatePlayer = Player.transform.rotation;
    }
    /// <summary>
    /// đưa nhân vật về đúng vị trí và góc quay để chơi lại
    /// </summary>
    public void Revival()
    {
        checkFinish = false;
        DOTween.KillAll();
        //obRotate.transform.position = objectNext.transform.position;
        obRotate.transform.rotation = rotateOB;
        Player.transform.rotation = rotatePlayer;
        if (checkHand)
        {
            boxRight.enabled = true;
            boxLeft.enabled = false;
            SetAnimation(anim_XoayTrai, false);
            MovePlayerToCenterRock(true);
        }
        else
        {
            boxRight.enabled = false;
            boxLeft.enabled = true;
            SetAnimation(anim_XoayPhai, false);
            MovePlayerToCenterRock(false);
        }
    }
    /// <summary>
    /// click để bắt đầu chơi lại
    /// </summary>
    public void ContinueRotate()
    {

        SpawnObject.ins.ChangeSpriteDH(obSave.gameObject, true);
        //durationOld = 3f;
        if (checkHand)
        {
            myTween = obRotate.transform.DORotate(new Vector3(0, 0, 360f), durationOld, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).SetRelative();
        }
        else
        {
            myTween = obRotate.transform.DORotate(new Vector3(0, 0, -360f), durationOld, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).SetRelative();
        }
        FxGlow1.SetActive(true);
        FxGlow2.SetActive(true);
    }
}
