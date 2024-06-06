using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag("rock"))
        {
            ControlPlayer.instance.ThroughRock(target.transform.position, target.gameObject);
            ControlPlayer.instance.checkDiamond = false;
        }
        else if (target.CompareTag("rockfail"))
        {
            ControlPlayer.instance.RockFail(target.gameObject);
            ControlPlayer.instance.checkDiamond = false;
        }
        else if (target.CompareTag("rockdiamond"))
        {
            ControlPlayer.instance.ThroughRock(target.transform.position, target.gameObject);
            ControlPlayer.instance.checkDiamond = true;
        }
        else if (target.CompareTag("rockend"))
        {
            ControlPlayer.instance.ThroughRock(target.transform.position, target.gameObject);
            ControlPlayer.instance.checkDiamond = false;
            ControlPlayer.instance.checkFinish = true;
            ControlPlayer.instance.EndGame();
        }
    }
    private void OnTriggerExit2D(Collider2D target)
    {
        if (target.CompareTag("rock") /*|| target.CompareTag("rockfail")*/ || target.CompareTag("rockdiamond"))
        {
            ControlPlayer.instance.FailRock();
            //if (ControlPlayer.instance.obSave != target.transform && ControlPlayer.instance.obSave2 != target.transform)
            //{
            //    ControlPlayer.instance.Die();
            //}
        }
        if (target.CompareTag("rockfail"))
            ControlPlayer.instance.FailRock();
    }
}
