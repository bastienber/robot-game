using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Character : MonoBehaviour
{
    // Move player in 2D space
    public float speed = 3.4f;
    public float jumpHeight = 6.5f;
    


    private bool facingRight;
    float moveDirection = 0;
    bool isGrounded = false;
    bool AirWalled;  

    Vector3 cameraPos;
    Rigidbody2D r2d;
    Collider2D mainCollider;
    public GameObject[] PlayerMeshes;

    // Check every collider except Player and Ignore Raycast
    LayerMask layerMask = ~(1 << 2 | 1 << 8);
    



    private int JumpStack;
    private float jumpTimeCounter;
    public float jumpTime;
    public bool isJumping;
    public GameObject Spawn;


    private bool LandingParticles;

    GameObject FrontTrigger, UnderTrigger, TopTrigger;    

    private Animator anim;



    void Start()
   
    {

        r2d = GetComponent<Rigidbody2D>();
        r2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        mainCollider = GetComponent<PolygonCollider2D>();

        facingRight = transform.localRotation.y <= 0;
        JumpStack = 1;        
        anim = transform.GetChild(0).GetComponent<Animator>();
        PlayerMeshes = GameObject.FindGameObjectsWithTag("PlayerMesh");

        facingRight = transform.localScale.x > 0;

        FrontTrigger = transform.Find("TRIGGER/Front_Trigger").gameObject;
        UnderTrigger = transform.Find("TRIGGER/Under_Trigger").gameObject;
        TopTrigger = transform.Find("TRIGGER/Top_Trigger").gameObject;
        Spawn = GameObject.Find("SPAWN");
        gameObject.transform.position = Spawn.transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        
        // Movement controls
        if (Input.GetButton("Left") && ((FrontTrigger.GetComponent<SC_DetectCollision>().isCollided == false || FrontTrigger.GetComponent<SC_DetectCollision>().GameObjectsCollided[0].tag == "Platform") || facingRight == true))
        {
            moveDirection = -1;
            Moving();
        }
        else if (Input.GetButton("Left") && FrontTrigger.GetComponent<SC_DetectCollision>().isCollided == true && FrontTrigger.GetComponent<SC_DetectCollision>().GameObjectsCollided[0].tag != "Platform" && facingRight == false)
        {
            moveDirection = 0;
            Moving();
        }
        else if (Input.GetButton("Right") && ((FrontTrigger.GetComponent<SC_DetectCollision>().isCollided == false || FrontTrigger.GetComponent<SC_DetectCollision>().GameObjectsCollided[0].tag == "Platform") || facingRight == false))
        {
            moveDirection = 1;
            Moving();

        }
        else if (Input.GetButton("Right") && FrontTrigger.GetComponent<SC_DetectCollision>().isCollided == true && FrontTrigger.GetComponent<SC_DetectCollision>().GameObjectsCollided[0].tag != "Platform" && facingRight == true )
        {
            moveDirection = 0;
            Moving();
        }
        else if (moveDirection == 0)
        {
            anim.SetBool("isRunning", false);
        }           


        

        //Stopping the motion in case of stopped input
        else
        {
            if (isGrounded || r2d.velocity.magnitude < 0.01f)
            {
                moveDirection = 0;
            }
        }
        
        CharacRotate(); // Change facing direction

        // Jumping

        if (Input.GetButtonDown("Jump") && isGrounded && JumpStack > 0 && TopTrigger.GetComponent<SC_DetectCollision>().isCollided == false)
        {    
            if (UnderTrigger.GetComponent<SC_DetectCollision>().GameObjectsCollided[0].gameObject.tag != "Platform" || (UnderTrigger.GetComponent<SC_DetectCollision>().GameObjectsCollided[0].gameObject.tag == "Platform" && Input.GetButton("Down") == false))
            {
                isJumping = true;
                anim.SetTrigger("jump");

                jumpTimeCounter = jumpTime;
                r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
            }
      
        }

        if(Input.GetButton("Jump") && isJumping == true) //longer input jump higher
        {
            if(jumpTimeCounter > 0)
            {
                r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight); //stay input
                jumpTimeCounter -= Time.deltaTime;
                if (TopTrigger.GetComponent<SC_DetectCollision>().isCollided == true && TopTrigger.GetComponent<SC_DetectCollision>().GameObjectsCollided[0].tag != "Platform")
                {
                    jumpTimeCounter = 0;
                    isJumping = false;
                }                
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }

        if (UnderTrigger.GetComponent<SC_DetectCollision>().isCollided == true)
        {
            JumpStack = 1;  
            anim.SetBool("isFalling", false);
        }


        if (r2d.velocity.y < -0.1 && anim.GetBool("isFalling") == false && UnderTrigger.GetComponent<SC_DetectCollision>().isCollided == false)
        {
            anim.SetBool("isFalling", true);
        }

        if (UnderTrigger.GetComponent<SC_DetectCollision>().isCollided == true && LandingParticles == true)
        {
            GameObject.Find("P_GroundImpact").GetComponent<ParticleSystem>().Play();
            LandingParticles = false;
        }
    }

    void FixedUpdate()
    {
        Bounds colliderBounds = mainCollider.bounds;
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, 0.1f, 0);
        // Check if player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheckPos, 0.23f, layerMask);

        // Apply movement velocity
        r2d.velocity = new Vector2((moveDirection) * speed, r2d.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "SolidColliding")
        {
            GameObject.Find("Character_Landing").GetComponent<SC_SoundsEffect>().Play();
        }
    }
 
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (UnderTrigger.GetComponent<SC_DetectCollision>().isCollided == true)
        {
            if (collision.gameObject.tag != "Player" || collision.gameObject.tag != "Trigger")
            {
                JumpStack = 1; //Put value > 1 for double jumping
                anim.SetBool("isFalling", false);                
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (UnderTrigger.GetComponent<SC_DetectCollision>().isCollided == false)
        {
            JumpStack = -1;
        }
        
        LandingParticles = true;       
    }

    private void Moving()
    {
        anim.SetBool("isRunning", true);
    }

    private void CharacRotate()
    {
        if (moveDirection != 0)
        {
            if (moveDirection > 0 && !facingRight)
            {   
                facingRight = true;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            if (moveDirection < 0 && facingRight)
            {
                facingRight = false;
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
}
