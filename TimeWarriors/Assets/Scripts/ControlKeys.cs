using System.Collections.Generic;
using UnityEngine;
using System;

public class ControlKeys : MonoBehaviour {


    public string p1_axisHorizontal = "HorizontalPlayer1"; // References to the Unity Input Axis
    public string p1_jump = "JumpPlayer1";
    public string p1_firstAttack = "FirstAttackPlayer1";
    public string p1_secondAttack = "SecondAttackPlayer1";
    public string p1_extra = "ExtraPlayer1";
    public string p1_crouch = "CrouchPlayer1";

    public string p2_axisHorizontal = "HorizontalPlayer2"; // References to the Unity Input Axis
    public string p2_jump = "JumpPlayer2";
    public string p2_firstAttack = "FirstAttackPlayer2";
    public string p2_secondAttack = "SecondAttackPlayer2";
    public string p2_extra = "ExtraPlayer2";
    public string p2_crouch = "CrouchPlayer2";


    // Function to deliver the list of the keys by player to the UserControl Class
    public List<string> getKeysForPlayer(PlayerID pID) {
        List<string> keyList = new List<string>();
        if (pID == PlayerID.P1) {
            keyList.Add(p1_jump);
            keyList.Add(p1_firstAttack);
            keyList.Add(p1_secondAttack);
            keyList.Add(p1_extra);
            keyList.Add(p1_crouch);
        }
        else if (pID == PlayerID.P2) {
            keyList.Add(p2_jump);
            keyList.Add(p2_firstAttack);
            keyList.Add(p2_secondAttack);
            keyList.Add(p2_extra);
            keyList.Add(p2_crouch);
        }
        else {
            throw new Exception("Time Warriors does not support a number of Players > 2");
        }
        return keyList;
    }

    public string getAxisforPlayer(PlayerID pID) {
        if (pID == PlayerID.P1) {
            return p1_axisHorizontal;
        }
        else if (pID == PlayerID.P2) {
            return p2_axisHorizontal;
        }
        else {
            throw new Exception("Time Warriors does not support a number of Players > 2");
        }
    }
}
