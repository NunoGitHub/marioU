using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    //what can i do when i see the platform 
    //and what can i do when i cannot see the platform

    public float recordTime = 300;
    public float recordTimeAux = 0;
    public int DNALenght = 20;
    public float jumpforce = 0;
    public float timeAlive;
    public DNA dna;
    public GameObject eyes;
    bool alive = true;
    bool seeGround = true, seeEnemie =true;
    float  jumpForceAux = 7.5f, timeToJump= 0.1f;
    public float timeWalking = 0;
    public Rigidbody2D rb;
    public float initPos = 0;
    public float distance = 0;
    public bool ground = false, platformJump = false;
    public float positiveThings = 0;
   public int numberActions = 4;
    bool canKill = true;


    /*private ThirdPersonCharacter m_Character;
    private Vector3 m_Move;
    private bool m_Jump;
    private Vector3 posInit = new Vector3();
    private Vector3 posfinal = new Vector3();
    public float distanceTotal = 0;*/


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platform" || collision.gameObject.tag == "platformjump")
        {
            ground = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platform" || collision.gameObject.tag == "platformjump")
        {
            ground = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "dead")
        {
            alive = false;
            timeAlive = 0;
            timeWalking = -50;
            distance = distance-10;
            positiveThings = 0;
            recordTime = 0;
        }
        int kills = 0;
        if (collision.gameObject.GetComponent<Type>()!=null)
        {
            if (alive && collision.gameObject.name != "kill" && collision.gameObject.GetComponent<Type>().movementType == Type.MovementType.goomba)
            {
                alive = false;
                timeAlive = 0;
                timeWalking = -10;
                distance = distance - 10;
                positiveThings -= 10;
                recordTime = 0;
                canKill = false;
                return;
            }
            if (collision.gameObject.name == "kill" && canKill==true)
            {
                Destroy(collision.transform.parent.gameObject);
            }
            
           

            positiveThings += (int)(collision.gameObject.GetComponent<Type>().movementType);
        }


    }

    bool isFinished = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.GetComponent<Type>()!=null && !isFinished)
        {

            positiveThings += (int)(collision.gameObject.GetComponent<Type>().movementType);
            recordTimeAux = recordTime;
            isFinished=true;
        }
    }
    /* private void OnCollisionStay2D(Collision2D collision)
     {
         if (collision.gameObject.tag == "platform")
         {
             ground = true;
         }
     }

     private void OnCollisionExit2D(Collision2D collision)
     {
         if (collision.gameObject.tag == "platform")
         {
             ground = false;
         }
     }*/
    RaycastHit2D hit;
    RaycastHit2D hit2, hit3, hit4;
    Vector2 upDir, downDir, downDir2;
    int layerMask;
    public void Init()
    {

        layerMask = ~(1 << 11 ); // Ignora as camadas 11 e 2
        

        initPos = transform.position.x;

         rb = GetComponent<Rigidbody2D>();
        //initialize DNA
        /*
        0 move foward
        1 left
        2 right
        3 jump
         */

        dna = new DNA(DNALenght, numberActions);//the 6  is number values explain above, like 0= foward and etc..
                                    // m_Character = GetComponent<ThirdPersonCharacter>();
        timeAlive = 0;
        alive = true;

    }
    private void Update()
    {

        if (!alive) return;

        if(PopulationManager.elapse>= PopulationManager.trialTimeAux && isFinished==false)
        {
            timeAlive = 0;
            timeWalking = 0;
            distance -= 30;
            positiveThings = 0;
            recordTime -= 30;
        }

        recordTime -= Time.deltaTime;
        distance = transform.position.x -initPos  ;

        // Debug.Log(distance);

        upDir = Quaternion.Euler(0, 0, 30) * eyes.transform.right;
        downDir = Quaternion.Euler(0, 0, -60) * eyes.transform.right;
        downDir2 = Quaternion.Euler(0, 0, 27) * eyes.transform.right;

        Debug.DrawRay(eyes.transform.position, eyes.transform.right * 5, Color.red);
        Debug.DrawRay(eyes.transform.position, upDir*2, Color.red);
        Debug.DrawRay(eyes.transform.position, downDir * 5, Color.red);
        Debug.DrawRay(new Vector3(eyes.transform.position.x-0.51f, transform.position.y-0.43f, transform.position.z), downDir2 *0.45f, Color.red);
        seeGround = false;
        platformJump = false;
        seeEnemie = false;

        //Vector3 upwardDir = Quaternion.AngleAxis(30, Vector3.forward) * Vector3.up;
        
        hit = Physics2D.Raycast(eyes.transform.position, eyes.transform.right, 5, layerMask);
        hit2 = Physics2D.Raycast(eyes.transform.position, upDir, 2, layerMask);
        hit3 = Physics2D.Raycast(eyes.transform.position, downDir, 5, layerMask);
        hit4 = Physics2D.Raycast(new Vector3(eyes.transform.position.x - 0.51f, transform.position.y - 0.43f, transform.position.z), downDir2 , 0.5f, layerMask);


        if (Physics2D.Raycast(eyes.transform.position, eyes.transform.right, 5) || Physics2D.Raycast(eyes.transform.position, upDir, 2) || Physics2D.Raycast(eyes.transform.position, downDir, 3))
        {
            


            if (hit.collider!=null && hit.collider.gameObject.tag == "platform" || hit2.collider != null && hit2.collider.gameObject.tag == "platform" || hit3.collider != null && hit3.collider.gameObject.tag == "platform")
            {
                seeGround = true;
                
            }
            if (hit.collider != null && hit.collider.gameObject.tag == "platformjump" || hit2.collider != null && hit2.collider.gameObject.tag == "platformjump" || hit3.collider != null && hit3.collider.gameObject.tag == "platformjump")
            {
                platformJump = true;

            }
            if ( hit3.collider != null && hit3.collider.gameObject.tag == "goomba" || hit.collider != null && hit.collider.gameObject.tag == "goomba" || hit2.collider != null && hit2.collider.gameObject.tag == "goomba")
            {
                seeEnemie = true;

            }
        }






        timeAlive = PopulationManager.elapse;


       
        //rb.velocity = new Vector2(rb.velocity.x, jumpforce);

        //transform.Translate(0, 0, move * Time.deltaTime * vel);
        //transform.Rotate(0, turn * Time.deltaTime, 0);
        //rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);

    }

    //Detect collisions between the GameObjects with Colliders attached

    
    private void FixedUpdate()
    {
        if (!alive) return;
        //read DNA
        //h = turn
        float turn = 0;
        //direction, go foward or backward
        float move = 0;
        jumpforce = 0;

      

        if (hit.collider != null && hit.collider.gameObject.CompareTag("platform"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(0) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(0) == 1) move = -3;
            else if (dna.GetGene(0) == 2 ) jumpforce = jumpForceAux;
            else if (dna.GetGene(0) == 3 )
            {
                jumpforce = jumpForceAux;
                move = 3;
            }

        }

        if (hit2.collider != null && hit2.collider.gameObject.CompareTag("platform"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(1) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(1) == 1) move = -3;
            else if (dna.GetGene(1) == 2 ) jumpforce = jumpForceAux;
            else if (dna.GetGene(1) == 3)
            {
                jumpforce = jumpForceAux;
                move = 3;
            }

        }
        if (hit3.collider != null && hit3.collider.gameObject.CompareTag("platform"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(2) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(2) == 1) move = -3;
            else if (dna.GetGene(2) == 2 ) jumpforce = jumpForceAux;
            else if (dna.GetGene(2) == 3 )
            {
                jumpforce = jumpForceAux;
                move = 3;
            }

        }
        if (hit4.collider != null && hit4.collider.gameObject.CompareTag("platform"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(3) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(3) == 1) move = -3;
            else if (dna.GetGene(3) == 2) jumpforce = jumpForceAux;
            else if (dna.GetGene(3) == 3)
            {
                jumpforce = jumpForceAux;
                move = 3;
            }

        }

        if (hit.collider != null && hit.collider.gameObject.CompareTag("platformjump"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(4) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(4) == 1) move = -3;
            else if (dna.GetGene(4) == 2 ) jumpforce = jumpForceAux;
            else if (dna.GetGene(4) == 3 )
            {
                jumpforce = jumpForceAux;
                move = 3;
            }

        }


        if (hit2.collider != null && hit2.collider.gameObject.CompareTag("platformjump"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(5) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(5) == 1) move = -3;
            else if (dna.GetGene(5) == 2 ) jumpforce = jumpForceAux;
            else if (dna.GetGene(5) == 3 )
            {
                jumpforce = jumpForceAux;
                move = 3;
            }

        }

        if (hit3.collider != null && hit3.collider.gameObject.CompareTag("platformjump"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(6) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(6) == 1) move = -3;
            else if (dna.GetGene(6) == 2 ) jumpforce = jumpForceAux;
            else if (dna.GetGene(6) == 3 )
            {
                jumpforce = jumpForceAux;
                 move = 3;
            }

        }
        if (hit4.collider != null && hit4.collider.gameObject.CompareTag("platformjump"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(7) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(7) == 1) move = -3;
            else if (dna.GetGene(7) == 2) jumpforce = jumpForceAux;
            else if (dna.GetGene(7) == 3)
            {
                jumpforce = jumpForceAux;
                move = 3;
            }

        }

        if (hit.collider != null && hit.collider.gameObject.CompareTag("goomba"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(8) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(8) == 1) move = -3;
            else if (dna.GetGene(8) == 2 ) jumpforce = jumpForceAux;
            else if (dna.GetGene(8) == 3 )
            {
                jumpforce = jumpForceAux;
                move = 3;
            }

        }

        if (hit2.collider != null && hit2.collider.gameObject.CompareTag("goomba"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(9) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(9) == 1) move = -3;
            else if (dna.GetGene(9) == 2 ) jumpforce = jumpForceAux;
            else if (dna.GetGene(9) == 3 )
            {
                jumpforce = jumpForceAux;
                move = 3;
            }

        }
        if (hit3.collider != null && hit3.collider.gameObject.CompareTag("goomba"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(10) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(10) == 1) move = -3;
            else if (dna.GetGene(10) == 2 ) jumpforce = jumpForceAux;
            else if (dna.GetGene(10) == 3 )
            {
                jumpforce = jumpForceAux;
                move = 3;
            }

        }
        if (hit4.collider != null && hit4.collider.gameObject.CompareTag("goomba"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(11) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(11) == 1) move = -3;
            else if (dna.GetGene(11) == 2) jumpforce = jumpForceAux;
            else if (dna.GetGene(11) == 3)
            {
                jumpforce = jumpForceAux;
                move = 3;
            }

        }

        if (hit.collider != null && hit.collider.gameObject.CompareTag("dead"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(12) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(12) == 1) move = -3;
            else if (dna.GetGene(12) == 2) jumpforce = jumpForceAux;
            else if (dna.GetGene(12) == 3)
            {
                jumpforce = jumpForceAux;
                move = 3;
            }

        }
        if (hit2.collider != null && hit2.collider.gameObject.CompareTag("dead"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(13) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(13) == 1) move = -3;
            else if (dna.GetGene(13) == 2) jumpforce = jumpForceAux;
            else if (dna.GetGene(13) == 3)
            {
                jumpforce = jumpForceAux;
                move = 3;
            }

        }
        if (hit3.collider != null && hit3.collider.gameObject.CompareTag("dead"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(14) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(14) == 1) move = -3;
            else if (dna.GetGene(14) == 2) jumpforce = jumpForceAux;
            else if (dna.GetGene(14) == 3)
            {
                jumpforce = jumpForceAux;
                move = 3;
            }

        }
        if (hit4.collider != null && hit4.collider.gameObject.CompareTag("dead"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(15) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(15) == 1) move = -3;
            else if (dna.GetGene(15) == 2) jumpforce = jumpForceAux;
            else if (dna.GetGene(15) == 3)
            {
                jumpforce = jumpForceAux;
                move = 3;
            }

        }


        if (hit.collider != null && hit.collider.gameObject.CompareTag("kill"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(16) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(16) == 1) move = -3;
            else if (dna.GetGene(16) == 2) jumpforce = jumpForceAux;
            else if (dna.GetGene(16) == 3)
            {
                jumpforce = jumpForceAux;
                move = 3;
            }

        }
        if (hit2.collider != null && hit2.collider.gameObject.CompareTag("kill"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(17) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(17) == 1) move = -3;
            else if (dna.GetGene(17) == 2) jumpforce = jumpForceAux;
            else if (dna.GetGene(17) == 3)
            {
                jumpforce = jumpForceAux;
                move = 3;
            }

        }
        if (hit3.collider != null && hit3.collider.gameObject.CompareTag("kill"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(18) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(18) == 1) move = -3;
            else if (dna.GetGene(18) == 2) jumpforce = jumpForceAux;
            else if (dna.GetGene(18) == 3)
            {
                jumpforce = jumpForceAux;
                move = 3;
            }

        }
        if (hit4.collider != null && hit4.collider.gameObject.CompareTag("kill"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(19) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(19) == 1) move = -3;
            else if (dna.GetGene(19) == 2) jumpforce = jumpForceAux;
            else if (dna.GetGene(19) == 3)
            {
                jumpforce = jumpForceAux;
                move = 3;
            }

        }



        if (hit.collider != null && hit.collider.gameObject.CompareTag("Untagged"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(20) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(20) == 1) move = -3;
            else if (dna.GetGene(20) == 2) jumpforce = jumpForceAux;
            else if (dna.GetGene(20) == 3)
            {
                jumpforce = jumpForceAux;
                move = 3;
            }

        }
        if (hit2.collider != null && hit2.collider.gameObject.CompareTag("Untagged"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(21) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(21) == 1) move = -3;
            else if (dna.GetGene(21) == 2) jumpforce = jumpForceAux;
            else if (dna.GetGene(21) == 3)
            {
                jumpforce = jumpForceAux;
                move = 3;
            }

        }
        if (hit3.collider != null && hit3.collider.gameObject.CompareTag("Untagged"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(22) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(22) == 1) move = -3;
            else if (dna.GetGene(22) == 2) jumpforce = jumpForceAux;
            else if (dna.GetGene(22) == 3)
            {
                jumpforce = jumpForceAux;
                move = 3;
            }

        }
        if (hit4.collider != null && hit4.collider.gameObject.CompareTag("Untagged"))
        {
            //make move relative to the character and always move foward
            //just move foward
            if (dna.GetGene(23) == 0)
            {
                move = 3;
                timeWalking++;
            }
            //turn -90
            else if (dna.GetGene(23) == 1) move = -3;
            else if (dna.GetGene(23) == 2) jumpforce = jumpForceAux;
            else if (dna.GetGene(23) == 3)
            {
                jumpforce = jumpForceAux;
                move = 3;
            }

        }






        /* if (!seeGround)
         {
             //second gene has the information if we cant see the ground
             if (dna.GetGene(1) == 0)
             {
                 move = 3;
                 timeWalking++;
             }
             else if (dna.GetGene(1) == 1) move = -3;
             else if (dna.GetGene(1) == 2 && ground) jumpforce = jumpForceAux;
             else if (dna.GetGene(1) == 3 && ground)
             {
                 jumpforce = jumpForceAux;
                 move = 3;
             }
         }
         if (platformJump)
         {
             //second gene has the information if we cant see the ground
             if (dna.GetGene(2) == 0)
             {
                 move = 3;
                 timeWalking++;
             }
             else if (dna.GetGene(2) == 1) move = -3;
             else if (dna.GetGene(2) == 2 && ground) jumpforce = jumpForceAux;
             else if (dna.GetGene(2) == 3 && ground)
             {
                 jumpforce = jumpForceAux;
                 move = 3;
             }
         }
         if (!platformJump)
         {
             //second gene has the information if we cant see the ground
             if (dna.GetGene(3) == 0)
             {
                 move = 3;
                 timeWalking++;
             }
             else if (dna.GetGene(3) == 1) move = -3;
             else if (dna.GetGene(3) == 2 && ground) jumpforce = jumpForceAux;
             else if (dna.GetGene(3) == 3 && ground)
             {
                 jumpforce = jumpForceAux;
                 move = 3;
             }
         }

         if (seeEnemie)
         {
             //second gene has the information if we cant see the ground
             if (dna.GetGene(4) == 0)
             {
                 move = 3;
                 timeWalking++;
             }
             else if (dna.GetGene(4) == 1) move = -3;
             else if (dna.GetGene(4) == 2 && ground) jumpforce = jumpForceAux;
             else if (dna.GetGene(4) == 3 && ground)
             {
                 jumpforce = jumpForceAux;
                 move = 3;
             }
         }*/

        // Calcula o vetor de movimento com base nos valores dos eixos, apenas no plano XY (ignorando o Z)
        Vector2 movimento = new Vector2(1, 0f) * move * Time.deltaTime;

        // Move o jogador pela cena (apenas no plano XY)
        transform.position += (Vector3)movimento;
        // GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
        JumpRoutine();

    }
    public bool canJumps = true;
    public float time = 0;
    public string namejump;
    private void JumpRoutine()
    {
        

        if (this.canJumps && hit4.collider!=null)
        {
            namejump= hit4.collider.name;
            
            rb.AddForce(Vector2.up * this.jumpforce, ForceMode2D.Impulse);
            this.canJumps = false;
        }
       
        if (timeToJump > time)
        {
            this.canJumps = false;
            this.time += Time.deltaTime;

        }
        if(this.time > this.timeToJump)
        {
            this.canJumps = true;
            this.time = 0;
        }
    }






}
