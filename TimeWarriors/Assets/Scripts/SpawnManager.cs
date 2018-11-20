using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject gm;
    public GameManager gameManager;
    //Range for X for Spawning PowerUps
    public float xMin;
    public float xMax;

    // Y is always the Same
    public float ystat;

    // Countdown
    private float countdown = 0.0f;
    public float countdownReset = 10.0f;
    
    //Number of active PowerUps and maximum Powerup without ending health, X = explosive mines
    public int numPU = 0;
    public int maxPU = 3;
    
    // Use this for initialization

    //Prefab HealthPU
    public GameObject powerUp;
    


    void Start() {
        gameManager = gm.GetComponent<GameManager>();
    }

    // Update is called once per frame
    public void Update()
    {
       //Check if Game is running
        if (gameManager.GetGameState()==GameState.GAME)
        { 
         // Countdown for Spawning PowerUps Health
            countdown -= Time.deltaTime;
            if (countdown <= 0 && (numPU < maxPU || maxPU == 0))
                {
                SpawnPU();
                
                //Reset Countdown
                countdown = countdownReset;
            } 
        } 
    }

   public void CountdownReset() {
        //Reset Countdown
        countdown = countdownReset;
    }


    //Spawnfunction HealthPU
    public void SpawnPU()
    {
        // Define the borders of Random Spawn area
        Vector2 pos = new Vector2(Random.Range(xMin, xMax), ystat);

        //Spawn Prefab
        Instantiate(powerUp, pos, Quaternion.identity, transform);

        //Increase Number of Spawned PowerUp
        numPU ++;

    }

    

    //Clean Up PowerUps
    public void CleanUp()
    {
       //Destroy Power Ups
       foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
       }

        //Reset Num PU
        numPU = 0;

        CountdownReset();
    }


}