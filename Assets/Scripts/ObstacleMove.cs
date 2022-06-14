using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    [SerializeField]
    bool canMove = true;
    [SerializeField]
    float speed = 1; //direction

    public bool startOnLeft = true;

    private GameManager gameMan;
    private GameObject mySpawn;
    private Animator myAnimator;
    // Start is called before the first frame update
    void Start()
    {
        gameMan = FindObjectOfType<GameManager>();
    }

    public void SetupObstacle(float mySpeed, bool startLeft, GameObject spawner)
    {
        mySpawn = spawner;
        myAnimator = GetComponentInChildren<Animator>();
        myAnimator.SetBool("RotateLeft", !startLeft);

        speed = mySpeed * 4.71f;
        startOnLeft = startLeft;
        if (startLeft == false)
        {
            //print("Start on right");
            speed = speed * -1;
           
        }
        //Circumference = 4.71
        //1 rotation per second (360)
      
    }
    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            transform.Translate(Vector3.right * speed * Time.smoothDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(gameMan.currentGameState == GameManager.GameState.Active)
        {
            if (other.transform.tag == "Player")
            {
                gameMan.LoseGame();
                other.GetComponent<PlayerControl>().Die();
            }
            if (other.transform.tag == "AIPlayer")
            {
                other.GetComponent<PlayerControl>().Die();
                StartCoroutine(other.GetComponent<AICharacter>().RagdollMe());
            }
        }
        
        if (other.transform.tag == "Spawn" && other.gameObject != mySpawn)
        {
            GameManager.Instance.DespawnObstacle(this.gameObject);
        }
    }
}
