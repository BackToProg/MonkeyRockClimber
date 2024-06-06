using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOptionAnim : MonoBehaviour {

    [SerializeField] RuntimeAnimatorController[] animators;
	void Start () {
        GetComponent<Animator>().runtimeAnimatorController = animators[SpawnObject.ins.option];
    }
	
	
}
