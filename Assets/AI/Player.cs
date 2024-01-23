using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float velocidade = 5f; // Velocidade de movimento do jogador
    private float forcaSalto = 6; // For�a do salto do jogador
    public float alturaMaxima = 10f;
    private Rigidbody2D rb; // Refer�ncia ao componente Rigidbody2D do jogador
    private bool noChao; // Flag para verificar se o jogador est� no ch�o
    AI player = new AI(100, 5);
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Obt�m os valores dos eixos horizontal (esquerda/direita)
        float moveHorizontal = Input.GetAxis("Horizontal");

        // Verifica se a tecla "W" (ou seta para cima) est� pressionada e o jogador est� no ch�o
        if (Input.GetKeyDown(KeyCode.Space) && noChao)
        {
            // Aplica uma for�a de salto no eixo Y
            rb.velocity = new Vector2(rb.velocity.x, forcaSalto);
        }

        // Calcula o vetor de movimento com base nos valores dos eixos, apenas no plano XY (ignorando o Z)
        Vector2 movimento = new Vector2(moveHorizontal, 0f) * velocidade * Time.deltaTime;

        // Move o jogador pela cena (apenas no plano XY)
        transform.position += (Vector3)movimento;

        // Verifica se o jogador est� no ch�o (baseado em um pequeno raio abaixo do jogador)
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f);
        noChao = hit.collider != null;
    }
}
