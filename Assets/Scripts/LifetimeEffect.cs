using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifetimeEffect : MonoBehaviour {
    public float time;
    private void OnEnable()
    {
        StartCoroutine(delay());
    }
    IEnumerator delay()
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

}
