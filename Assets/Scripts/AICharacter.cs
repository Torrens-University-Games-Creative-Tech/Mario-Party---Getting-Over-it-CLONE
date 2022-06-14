using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacter : MonoBehaviour
{
    public List<GameObject> incomingObstacles;
    public float thresholdForJump = 2.0f;

    public float brainPower = 0.2f; //Default smartness
    [Header("Ragdoll Settings")]
    public GameObject standardModel;
    public GameObject ragDollObj;
   
    // Start is called before the first frame update
    void Awake()
    {
        FindObjectOfType<GameManager>().AIplayers.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        //Add dumbness here. maybe remove coroutine.
        if(GameManager.Instance.currentGameState == GameManager.GameState.Active)
        {
            if(GetComponent<PlayerControl>().isAlive)
            {
                if (GetComponent<PlayerControl>().canJump)
                {
                    CheckDistanceFromObstacles();
                }
            }
        } 
    }

    void CheckDistanceFromObstacles()
    {
        foreach(GameObject obstacle in  incomingObstacles)
        {
            float dist = Vector3.Distance(obstacle.transform.position, transform.position);

            //check if close enough to trigger a jump, add some randomness for being bad.
            if(dist < thresholdForJump)
            {
                //check what side its on, has it gone past us????? 
                if(obstacle.transform.position.x > transform.position.x && obstacle.GetComponent<ObstacleMove>().startOnLeft)
                {
                    //Moved pasted us.
                    print("Moved pasted us, now on our right");
                }
                else if (obstacle.transform.position.x < transform.position.x && !obstacle.GetComponent<ObstacleMove>().startOnLeft)
                {
                    print("Moved pasted us, now on our left");
                }
                else
                {
                    //Have AI jump...but maybe we need some dumbness?
                    StartCoroutine(BrainTicker()); 
                    
                }
                
            }

        }
    }

    //Ai brain delayer
    IEnumerator BrainTicker()
    {
        float ranNum = Random.Range(0f, brainPower);
        yield return new WaitForSeconds(ranNum);
        Jump();
    }

    //Turn on a ragdoll
    public IEnumerator RagdollMe()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<Collider>().enabled = false;
        
        standardModel.SetActive(false);
        ragDollObj.SetActive(true);
    }
   
    //My Jump call
    void Jump()
    {
        GetComponent<PlayerControl>().OnJump();
    }
}
