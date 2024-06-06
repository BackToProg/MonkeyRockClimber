using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DhRoi : MonoBehaviour {
    [SerializeField] Sprite[] spr;
	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().sprite = spr[SpawnObject.ins.option];
        transform.DOMoveY(transform.position.y - 5f, 1f);
        int rd = Random.Range(0, 2);
        if (rd == 0)
            transform.DORotate(new Vector3(0, 0, 100), 3f);
        else
            transform.DORotate(new Vector3(0, 0, -100), 3f);
    }
	
	
}
