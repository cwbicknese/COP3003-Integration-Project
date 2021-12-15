using UnityEngine;
using UnityEngine.SceneManagement;

// this class contains the actions and movements the player can perform
// it is a subclass of CharacterStats

public class PlayerMovement : CharacterStats
{
    //model, character controller, and camera
    public GameObject playerModel;
    public CharacterController controller;
    public Transform cam;

    //movement fields
    private float moveSpeed = 25f; //speed the player moves on the ground
    private float turnSmoothTime = .1f;  //makes the player turn more smoothly
    private float turnSmoothVelocity;

    //jumping and gravity fields
    public Transform groundCheck; //checks the bottom of the character for collision with ground
    public LayerMask groundMask;
    Vector3 velocity;
    private float jumpHeight = 6f;
    private float gravity = -9.81f * 3; //gravity is tripled to make it feel less floaty
    private float groundDistance = .4f;
    bool isGrounded;

    //gliding fields
    public GameObject paraglider;
    private float airTime = 0f; //time in air, so that the player doesn't start gliding immediately when they jump
    private float gliding = 1f; //1 will be default so that I can multiply by this variable without changing the result if it is at the default value
    private float glideSpd = 3f; //this is what gliding will be set to when it is not the default value
    
    //spell costs
    private float fireMpCost = 5f;
    private float iceMpCost = 10f;

    //to reference SplashText class
    private GameObject objSplash;
    private SplashText textToWrite;

    private void Start()
    {
        objSplash = GameObject.Find("obj_splash_text_txt");
        textToWrite = objSplash.GetComponent<SplashText>();
    }

    // Update is called once per frame
    void Update()
    {
        //checks if the player is colliding with ground beneath them
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //stops gravity from continuously increasing if player is grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //paraglider-like system that allows the player to hover
        //this will make the player move faster and fall more slowly to the ground when pressing Space while in the air
        if (!isGrounded) //counts time in air so that you don't start hovering immediately from the jump
        {
            airTime++;
        }
        else
        {
            airTime = 0f;
        }
        //got a warning that comparing floats using == and != won't always be accurate, but it doesn't seem to be an issue here because the float gliding will always be set to exact values (either 1f or 3f)
        //gliding must be a float because it is used in multiplication with other floats such as moveSpeed and Time.deltaTime
        if (gliding == 1f && Input.GetButtonDown("Jump")) // when hover starts, halves downward velocity to slow the fall
        {
            velocity.y /= 2f;
        }
        if (Input.GetButtonDown("Jump") && !isGrounded && airTime > 5f) //starts hovering only after player has been in the air for 30 frames
        {
            gliding = glideSpd;
            paraglider.SetActive(true);
        }
        else if (!Input.GetButton("Jump")) // not hovering, reset variables
        {
            gliding = 1f;
            paraglider.SetActive(false);
        }

        //horizontal movement and direction, referenced Brackeys - Third Person Movement in Unity https://www.youtube.com/watch?v=4HpC--2iowE
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= .1f)
        {
            if (gameObject.GetComponent<GeneralFunctions>().able)   //checks if player is able to move
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                if (isGrounded) //movement on the ground
                {
                    controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
                }
                else if (gliding == 1f) //movement while jumping is slightly slower
                {
                    controller.Move(moveDir.normalized * (moveSpeed*.75f) * Time.deltaTime);
                }
                else //movement while gliding is faster
                {
                    controller.Move(moveDir.normalized * moveSpeed * gliding/2f * Time.deltaTime);
                }
            }
        }

        //vertical movement, referenced Brackeys - First Person Movement in Unity - FPS Controller https://youtu.be/_QajrabyTJc?t=898
        if (Input.GetButtonDown("Jump") && isGrounded && gameObject.GetComponent<GeneralFunctions>().able)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity/gliding * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //casting spells
        if (gameObject.GetComponent<GeneralFunctions>().able) //checks whether the player is in endlag
        {
            //shoot fireball
            if (Input.GetKeyDown("f"))
            {
                if (mp >= fireMpCost)
                {
                    //shoots fireball with shootProjectile() from GeneralFunctions class
                    gameObject.GetComponent<GeneralFunctions>().shootProjectile(attack.getValue(), 15f, 60, false); 
                    mp -= fireMpCost; //reduce mp by the mp cost of the spell
                }
                else
                {
                    textToWrite.GetComponent<SplashText>().setText("Not enough MP", 200);
                }
            }

            //cast ice
            if (Input.GetKeyDown("i"))
            {
                if (mp >= iceMpCost)
                {
                    gameObject.GetComponent<GeneralFunctions>().castIce(attack.getValue(), 100);
                    mp -= iceMpCost;
                }
                else
                {
                    textToWrite.GetComponent<SplashText>().setText("Not enough MP", 200);
                }
            }
        }

        //exit game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit(); //this does not exit the game while Unity is open, only if the game is run from outside of Unity
            Debug.Log("Exited Game"); //indicates in Unity that the game would have exited
        }

    } //end of Update() function

    //collision
    void OnTriggerStay(Collider other) //continuous check of collision every frame
    {
        //checkpoint
        if (other.gameObject.CompareTag("checkpoint")) //collision with the checkpoint object
        {
            if (Input.GetKeyDown(KeyCode.Return)) //if enter key is pressed while in the collision area
            {
                Debug.Log("pressed enter"); //still unsure why it doesn't always detect enter input
                if (hp != hpMax || mp != mpMax) //makes sure there is something to heal
                {
                    if (gold >= 10) //player must spend 5 gold to use a checkpoint, which will restore the player's health and mp
                    {
                        gold -= 10;
                        hp = hpMax; // restore all hp
                        mp = mpMax; // restore all mp
                    }
                    else //not enough gold
                    {
                        textToWrite.GetComponent<SplashText>().setText("Not enough Gold", 200);
                    }
                }
                else //hp and mp are both already full
                {
                    textToWrite.GetComponent<SplashText>().setText("HP and MP are already full.", 200);
                }

            }
        }
    }
    void OnTriggerEnter(Collider other) //collision the moment they collide
    {
        //checkpoint
        if (other.gameObject.CompareTag("checkpoint")) //collision with the checkpoint object
        {
            //sets SplashText txt and count to display a message for a duration
            textToWrite.GetComponent<SplashText>().setText("Enter: Heal HP & MP.\nCosts 10 Gold.", 200); 
        }

        //gold
        if (other.gameObject.CompareTag("gold"))
        {
            gold += pickUpGold(5); //because pickUpGold is a generic function, it can take an int
            Destroy(other.gameObject);
        }

        //large gold
        if (other.gameObject.CompareTag("large gold"))
        {
            gold += pickUpGold(50f); //because pickUpGold is a generic function, it can also take a float
            Destroy(other.gameObject);
        }
    }

    // this function is a generic (same thing as a template in C++)
    // it can use any type in place of T
    // generics are good for when different types might be needed for a function, and can be used as an alternative to a bunch of overloaded functions
    // they can also pass classes as a type
    public T pickUpGold<T>(T goldAmount)
    {
        //in the below setText function call, only one parameter is given, the second parameter will automatically be set to the default value
        textToWrite.GetComponent<SplashText>().setText("Found " + goldAmount + " Gold!"); 
        return goldAmount;
    }

    protected override void die()
    {
        playerModel.SetActive(false); //turn off the player model
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //resets the game by loading the scene
    }
}
