using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayerLmao : MonoBehaviour
{
    GameObject Player;
    Vector3 mouseVector;
    Vector3 vectorMan;
    float fuckAngle;

    void Start()
    {
        Player = GameObject.Find("newPlayer");
    }

    void Update()
    {
        transform.position = Player.transform.position + new Vector3(0,0,-1);

        mouseVector = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        vectorMan = Camera.main.ScreenToWorldPoint(mouseVector) - transform.position;

        if (vectorMan.y > 0 && vectorMan.x > 0)
        {
            fuckAngle = (180 / Mathf.PI) * Mathf.Atan(vectorMan.y / vectorMan.x);
        }
        if (vectorMan.y > 0 && vectorMan.x < 0)
        {
            fuckAngle = 180 + (180 / Mathf.PI) * Mathf.Atan(vectorMan.y / vectorMan.x);
        }
        if (vectorMan.y < 0 && vectorMan.x < 0)
        {
            fuckAngle = (180 / Mathf.PI) * Mathf.Atan(vectorMan.y / vectorMan.x) + 180;
        }
        if (vectorMan.y < 0 && vectorMan.x > 0)
        {
            fuckAngle = 90 + (180 / Mathf.PI) * Mathf.Atan(vectorMan.y / vectorMan.x) + 270;
        }

        Quaternion actualRotation = Quaternion.Euler(0, -fuckAngle + 220, 0);

        transform.rotation = actualRotation;
        transform.Rotate(-50, 0, 0, Space.World);
    }
}
