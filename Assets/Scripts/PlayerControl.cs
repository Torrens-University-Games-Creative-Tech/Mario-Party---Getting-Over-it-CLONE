using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Important Settings")]
    [SerializeField]
    private bool isAPlayer = true;
    [SerializeField]
    private bool isInvincible = true;

    [Header("Attributes")]
    public float jumpPower = 1;
    public RigidbodyConstraints myDeathConstraints;

    //Make private later
    public bool isAlive = true;
    public bool isJumping = false;
    public bool canJump = true;

    private GameManager gameMan;

    public Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponentInChildren<Animator>();
        gameMan = FindObjectOfType<GameManager>();

        if(isAPlayer)
        {
            //GetComponentInChildren<CharacterSkinController>().ChangeMaterialSettings(3);
        }
    }

    public void OnJump()
    {
        if(gameMan.currentGameState == GameManager.GameState.Active)
        {
            HandleJump();
        }        
    }

    void HandleJump()
    {
        if(canJump)
        {
            //Call animation
            myAnimator.SetTrigger("Jump");
            //Do the jump
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            //Prevent double jump
            canJump = false;
        }       
    }

    void Victory()
    {
        myAnimator.SetBool("Victory", true);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Ground")
        {
            canJump = true;
        }
    }
    public void Die()
    {
        GetComponent<PlayerControl>().isAlive = false;
        GetComponent<Rigidbody>().constraints = myDeathConstraints;
        GetComponent<ConstantForce>().enabled = false;
        GetComponent<Rigidbody>().AddForce(Vector3.forward * 2, ForceMode.Impulse);
    }

}
