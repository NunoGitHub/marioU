using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JUMP : MonoBehaviour
{
    GameObject player;
    Brain b;
    private void Start()
    {
        player = transform.parent.gameObject;
        b =player.GetComponent<Brain>();


    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platform" || collision.gameObject.tag== "platformjump")
        {
            b.ground = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platform" || collision.gameObject.tag == "platformjump")
        {
           b.ground = false;
        }
    }
}
