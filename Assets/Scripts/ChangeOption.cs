using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOption : MonoBehaviour {

    [SerializeField] Sprite[] option;
	void Start () {
        GetComponent<SpriteRenderer>().sprite = option[SpawnObject.ins.option];
	}
	
}
