using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudRandom : MonoBehaviour
{
    [SerializeField] Sprite[] option2;
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = option2[Random.Range(0, option2.Length)];
        int i = Random.Range(0, 2);
        if (i == 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }


}
