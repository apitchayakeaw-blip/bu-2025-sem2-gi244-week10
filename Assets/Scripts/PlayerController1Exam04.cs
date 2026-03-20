using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController1Exam04 : MonoBehaviour
{

    public int hp = 0;
    public float jumpForce;
    public float gravityModifier;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    public AudioClip jumpSfx;
    public AudioClip crashSfx;

    private Rigidbody rb;
    private InputAction jumpAction;
    private bool isOnGround = true;
    public bool doubleJump = false;


    private Animator playerAnim;
    private AudioSource playerAudio;

    public bool gameOver = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics.gravity *= gravityModifier;

        jumpAction = InputSystem.actions.FindAction("Jump");

        gameOver = false;

        hp = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpAction.triggered && !gameOver )
        {
            if (isOnGround)
            {
                rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
                isOnGround = false;
                playerAnim.SetTrigger("Jump_trig");
                dirtParticle.Stop();
                playerAudio.PlayOneShot(jumpSfx);

                doubleJump = true;
            }
            else if (doubleJump)
            {
                rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
                
                playerAnim.SetTrigger("Jump_trig");
               
                playerAudio.PlayOneShot(jumpSfx);

                doubleJump = false;
            }
            

       
            
        }
        
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            hp--;

            if (hp <= 0)
            {
            
                Debug.Log("Game Over!");
                gameOver = true;
                playerAnim.SetBool("Death_b", true);
                playerAnim.SetInteger("DeathType_int", 1);

                Destroy(this.gameObject);

            }
            else
            {
                  explosionParticle.Play();
                  dirtParticle.Stop();
                  playerAudio.PlayOneShot(crashSfx);
            }
            
        }
    }

}