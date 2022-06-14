using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { PreGame, Active, Win, Lose};
    public GameState currentGameState = GameState.PreGame;

    public enum SpawnState { Idle, Spawn}
    public SpawnState currentSpawnState = SpawnState.Idle;

    public float roundCount = 0;
    public float roundTimeLength = 5;
    [SerializeField]
    private float currentRoundTime = 0;

    public bool canFireObstacle = false;

    public GameObject obstaclePrefab;
    public GameObject leftObstacleSpawn;
    public GameObject rightObstacleSpawn;
    public GameObject loseTextObject;

    public GameObject playerObject;
    public List<AICharacter> AIplayers = new List<AICharacter>();
    public int aiPlayersAlive = 0;
    public CameraManager cameraManager;

    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }

    // Start is called before the first frame update

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else Destroy(this);

    }
    // Start is called before the first frame update
    void Start()
    {
        currentRoundTime = roundTimeLength;
        currentGameState = GameState.Active;
        cameraManager = FindObjectOfType<CameraManager>();
        aiPlayersAlive = AIplayers.Count;
    }

    // Update is called once per frame
    void Update()
    {  
        if(currentGameState == GameState.Active)
        {
            //Win check
            aiPlayersAlive = AIplayers.Count; //reset

            if(AIplayers.Count > 0)
            {
                for (int i = 0; i < AIplayers.Count;i++)
                {
                    if (!AIplayers[i].GetComponent<PlayerControl>().isAlive)
                    {
                        aiPlayersAlive--;
                    }
                }
                
                if (aiPlayersAlive == 0)
                {
                    currentGameState = GameState.Win;
                    WinGame();
                } 
            }
            
            //Check round timer, if its ready spawn some things
            if (currentRoundTime <= 0)
            {
                roundCount++;
                currentSpawnState = SpawnState.Spawn;
                currentRoundTime = roundTimeLength;
            }

            //Spawner Control
            switch (currentSpawnState)
            {
                case SpawnState.Idle: //Sit and wait
                    if (currentGameState == GameState.Active)
                    {
                        currentRoundTime -= Time.deltaTime;
                    }

                    break;
                case SpawnState.Spawn:

                    currentSpawnState = SpawnState.Idle;//So we dont spawn anything until the state switchs back here.
                    bool randBoolean = (Random.value > 0.5f);
                    //test
                    StartCoroutine(SpawnObstacle(1, randBoolean, Random.Range(1, 4)));

                    break;
                default:
                    break;
            }
        }
    }

    //Public functions
    public void LoseGame()
    {
        if(currentGameState != GameState.Lose)
        {
            currentGameState = GameState.Lose;
            currentSpawnState = SpawnState.Idle;
            loseTextObject.SetActive(true);

            //Update UI
            UIManager.Instance.LosePanel.SetActive(true);

            //Handle AI
            foreach (AICharacter aic in AIplayers)
            {
                aic.GetComponent<PlayerControl>().myAnimator.SetBool("Victory", true);
            }

            //Handle Camera
            cameraManager.CameraAnimationTransition("Lose");
        }
    }

    void WinGame()
    {
        playerObject.GetComponent<PlayerControl>().myAnimator.SetBool("Victory", true);
        
        //Handle Camera
        cameraManager.CameraAnimationTransition("Win");
    }

    //Private functions
    IEnumerator SpawnObstacle(float speed, bool onLeft, int amount)
    {
        canFireObstacle = false;

        //delay amount, maybe need to move this to global later
        float delay = 1.5f;

        for(int i = 0; i < amount; i++)
        {
            GameObject obstacle;
            
            if (onLeft)
            {
                obstacle = Instantiate(obstaclePrefab, leftObstacleSpawn.transform.position, leftObstacleSpawn.transform.rotation) as GameObject;
                obstacle.GetComponent<ObstacleMove>().SetupObstacle(speed, onLeft, leftObstacleSpawn);

            }
            else
            {
                obstacle = Instantiate(obstaclePrefab, rightObstacleSpawn.transform.position, rightObstacleSpawn.transform.rotation) as GameObject;
                obstacle.GetComponent<ObstacleMove>().SetupObstacle(speed, onLeft, rightObstacleSpawn);
            }

            //Add each new ostacle to each of the ai players to track.
            foreach(AICharacter aic in  AIplayers)
            {
                aic.incomingObstacles.Add(obstacle);
            }

            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForEndOfFrame();
    }

    //Safley despawn the obstacles/cleanup
    public void DespawnObstacle(GameObject obj)
    {
        foreach (AICharacter aic in AIplayers)
        {
            aic.incomingObstacles.Remove(obj);
        }

        Destroy(obj);
    }
}
