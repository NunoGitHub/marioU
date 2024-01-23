using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI 
{
    int health=-1;
    int attack = -1;
    Velocity velocity = new Velocity(0, 0);

    public AI(int health, int attack/*, int velX, int velY*/){
        this.health = health;
        this.attack = attack;
        /*velocity.x = velX;
        velocity.y = velY;*/

    }
}
struct Velocity
{
    public float x; // Membro x da velocidade
    public float y; // Membro y da velocidade

    // Construtor para inicializar a estrutura
    public Velocity(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}
