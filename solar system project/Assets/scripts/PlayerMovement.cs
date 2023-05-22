using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMovement : MonoBehaviour {

    private enum movementState {idle, run, jump, fall}
    public GameObject player;
    private Animator anim;
    private SpriteRenderer sprite;
    private float toX = 0f;
    [SerializeField] private float moveSpeed = 8f;
     public float movingSpeed;
    public float jumpForce;
    private float moveInput;

    private bool facingRight = false;
    [HideInInspector]
    public bool deathState = false;

    [SerializeField] public LayerMask groundLayer;

    private Rigidbody2D rigidbody;
    private CapsuleCollider2D coll;
    private Animator animator;
    public TextMeshProUGUI text;
    public static int cubes = 0;
    public bool thing;
    
    
    void Start()
    {
        
        rigidbody = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        PlayerPrefs.SetInt("cubes", 0);
        cubes = 0;
        text.text = "Cubes Collected: " + PlayerPrefs.GetInt("cubes");
    }

    // Update is called once per frame
    void Update()
    {
        // if (DialogueManager.GetInstance().dialogueIsPlaying) return;
        rigidbody.gravityScale = 1f;
        toX = Input.GetAxis("Horizontal");
        rigidbody.velocity = new Vector2(toX * moveSpeed, rigidbody.velocity.y);

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)){
            rigidbody.gravityScale = 20f;
        }

        if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) || Input.GetKeyDown(KeyCode.W)) {
                if (CheckGround()) {
                    jumpForce += 5;
                    rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
                    jumpForce -= 5;
                }
        }
        updateAnimation();
        if (Input.GetKeyDown(KeyCode.L)) {
            PlayerPrefs.SetInt("cubes", 0);
        }
    }
    private bool CheckGround() {
        return  Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
    }


    void updateAnimation() {
        movementState state;
        
        if (toX > 0f) {
            state = movementState.run;
            sprite.flipX = false;
        }
        else if(toX < 0f) {
            state = movementState.run;
            sprite.flipX = true;
        }
        else {
            state = movementState.idle;
        }
        if (rigidbody.velocity.y < -0.1f) {
            state = movementState.jump;
        }
        if(rigidbody.velocity.y > 0.1f) {
            state = movementState.jump;
        }
        
        


        anim.SetInteger("state", (int)state);

    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "spike") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            PlayerPrefs.SetInt("cubes", 0);
            cubes = 0;
        }
        if (other.gameObject.tag == "portal") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("cubes", 0);
            cubes = 0;
        }

    
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "ice") {
            cubes++;
            PlayerPrefs.SetInt("cubes", cubes);
            PlayerPrefs.Save();
            text.text = "Cubes Collected: " + PlayerPrefs.GetInt("cubes");
            Destroy(other.gameObject);
            Debug.Log("destroyed");
        }
        if (other.gameObject.tag == "NPC") {
            IInteractable interactable = other.gameObject.GetComponent<NPCInteractable>();
            interactable.Interact();

        }
    }
    
}
