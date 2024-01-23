using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    List<GameObject> pos = new List<GameObject>();
    int move = 2;
    float posInit;
    public float distance;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.gameObject.tag == "platformjump")
        {
            move = move * -1;
        }
    }
    private void Start()
    {
        posInit = transform.position.x;
    }

    private void Update()
    {
        distance = Mathf.Abs( transform.position.x - posInit);
        if (distance > 4) move = move * -1;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 movimento = new Vector2(1, 0f) * move * Time.deltaTime;

        // Move o jogador pela cena (apenas no plano XY)
        transform.position += (Vector3)movimento;
    }
}
