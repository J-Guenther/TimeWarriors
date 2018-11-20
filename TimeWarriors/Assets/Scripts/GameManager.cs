using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Age {SPACE, INFO, WARS, IMPERIAL, MEDIEVAL, ANCIENT, STONE };
public enum PlayerID { P1, P2 , EMPTY};
public enum GameState { LOADING, STARTING, PAUSE, GAME, TIMETRAVEL, RESPAWN, END};

public class GameManager : MonoBehaviour {

    public TimeWarrior player1;
    public TimeWarrior player2;
    
    [SerializeField] private GameObject spaceCharacter;
    [SerializeField] private GameObject infoCharacter;
    [SerializeField] private GameObject warsCharacter;
    [SerializeField] private GameObject imperialCharacter;
    [SerializeField] private GameObject medievalCharacter;
    [SerializeField] private GameObject ancientCharacter;
    [SerializeField] private GameObject stoneCharacter;

    private UImanager uiManager;
    private GameState gameState;
    public UnityEngine.UI.Text startCountdown;
    public Countdown countdown;
    public float roundLength = 120.0f;
    public float showdownStart = 30.0f;
    public bool showdown = false;
    private PlayerID winnerId;

    public UnityEngine.UI.Text winnerTxt;
    // For Development reasons
    public UnityEngine.UI.Text stateText;

    private GameObject[] gameObjects;

    //find Spawnmanager 
    public GameObject sm;
    private SpawnManager spawnManager;
    

    //find Spawnmanager 
    public GameObject sm2;
    private SpawnManager spawnManager2;

    
    



    void Awake(){
        uiManager = transform.GetComponent<UImanager>();
        countdown = transform.GetComponent<Countdown>();
        
        //Spawnmanager
        spawnManager = sm.GetComponent<SpawnManager>();
        spawnManager2 = sm2.GetComponent<SpawnManager>();
    }

    // Use this for initialization
    void Start () {
        StartGame();
    }
	
	// Update is called once per frame
	void Update () {
        stateText.text = gameState.ToString();
	}
    
    public void SpawnPlayer(Age theAge, TimeWarrior w ) {
        GameObject spawnPoint;
        w.currentAge = theAge;
        spawnPoint = w.spawnPoint;
            
        switch (theAge) {
            case Age.SPACE:
                w.characterModel = Instantiate(spaceCharacter, spawnPoint.transform.position, Quaternion.identity);
                break;
            case Age.INFO:
                w.characterModel = Instantiate(infoCharacter, spawnPoint.transform.position, Quaternion.identity);
                break;
            case Age.WARS:
                w.characterModel = Instantiate(warsCharacter, spawnPoint.transform.position, Quaternion.identity);
                break;
            case Age.IMPERIAL:
                w.characterModel = Instantiate(imperialCharacter, spawnPoint.transform.position, Quaternion.identity);
                break;
            case Age.MEDIEVAL:
                w.characterModel = Instantiate(medievalCharacter, spawnPoint.transform.position, Quaternion.identity);
                break;
            case Age.ANCIENT:
                w.characterModel = Instantiate(ancientCharacter, spawnPoint.transform.position, Quaternion.identity);
                break;
            case Age.STONE:
                w.characterModel = Instantiate(stoneCharacter, spawnPoint.transform.position, Quaternion.identity);
                break;
        }

        w.characterModel.GetComponent<PlayerCharacter.UserControl>().SetPlayerID(w.playerID);
        SetColor(w);
    }
    
    public void SpawnOnClick() {
        SpawnPlayer(Age.SPACE, player1);
        SpawnPlayer(Age.SPACE, player2);
        player2.characterModel.GetComponent<PlayerCharacter.PlayerController>().Flip();
    }
  
    public void SetGameState(GameState state) {
        gameState = state;
        switch (state) {
            case GameState.STARTING:
            case GameState.LOADING:
            case GameState.RESPAWN:
            case GameState.END:
            case GameState.PAUSE:
                try {
                    player1.characterModel.GetComponent<PlayerCharacter.UserControl>().DisableControl();
                    player2.characterModel.GetComponent<PlayerCharacter.UserControl>().DisableControl();
                }
                catch {
                    break;
                }
                break;
            case GameState.TIMETRAVEL:
            case GameState.GAME:
                try {
                    player1.characterModel.GetComponent<PlayerCharacter.UserControl>().EnableControl();
                    player2.characterModel.GetComponent<PlayerCharacter.UserControl>().EnableControl();
                }
                catch {
                    break;
                }
                break;
        }
    }

    public void TimeTravel(PlayerID p){
        if (gameState == GameState.RESPAWN)
            return;
        SetGameState(GameState.TIMETRAVEL);
        TimeWarrior winner;
        TimeWarrior looser;
        Age newAgeWinner;
        Age newAgeLooser;

        if (p == PlayerID.P1){
            winner = player2;
            looser = player1;
        }
        else{
            winner = player1;
            looser = player2;
        }

        // Note: Counting of ages is in reversed order
        newAgeWinner = winner.currentAge + 1;
        if (newAgeWinner > Age.STONE){
            winnerId = winner.playerID;
            EndGame();
            return;
        }

        newAgeLooser = looser.currentAge - 1;
        if (newAgeLooser < Age.SPACE)
            newAgeLooser = Age.SPACE;

        // Clean Up Flying Bullets
        DestroyAllBullets();

        // Respawn
        SetGameState(GameState.RESPAWN);
        Destroy(winner.characterModel);
        SpawnPlayer(newAgeWinner, winner);
        SpawnPlayer(newAgeLooser, looser);

        // Update UI
        uiManager.UpdateStatusbar();

        StartCoroutine(RespawnCooldown());

    }

    IEnumerator StartRound()
    {
        winnerId = PlayerID.EMPTY;
        SpawnOnClick();
        SetGameState(GameState.STARTING);
        // Update UI
        uiManager.UpdateStatusbar();
        yield return new WaitForSeconds(2);
        countdown.ResetCountdown();
        startCountdown.text = "3";
        yield return new WaitForSeconds(1);
        startCountdown.text = "2";
        yield return new WaitForSeconds(1);
        startCountdown.text = "1";
        yield return new WaitForSeconds(1);
        startCountdown.text = "Fight!";
        SetGameState(GameState.GAME);
        yield return new WaitForSeconds(1);
        startCountdown.text = "";
    }

    public GameState GetGameState() { return gameState; }

    public void StartGame(){
        Destroy(player1.characterModel);
        Destroy(player2.characterModel);
        uiManager.CloseEndDialog();
        
        StartCoroutine(StartRound());
    }

    public void EndGame(){
        SetGameState(GameState.END);
        if(winnerId == PlayerID.EMPTY){
            winnerTxt.text = "Draw!";
        } else {
            winnerTxt.text = "" + winnerId.ToString() + " wins!";
        }
        uiManager.OpenEndDialog();

        spawnManager.CleanUp();
        spawnManager2.CleanUp();
    }

    void DestroyAllBullets() {
        gameObjects = GameObject.FindGameObjectsWithTag("Bullet");

        for (var i = 0; i < gameObjects.Length; i++) {
            Destroy(gameObjects[i]);
        }
    }

    IEnumerator RespawnCooldown() {
        yield return new WaitForSeconds(1.0f);
        SetGameState(GameState.GAME);
    }

    private void SetColor(TimeWarrior w){
        if(w.playerID == PlayerID.P1){
            SpriteRenderer spriteRenderer = w.characterModel.transform.Find("Graphics").GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(240.0f / 255.0f, 125.0f / 255.0f, 125.0f / 255.0f, 255.0f / 255.0f);
        } else {
            SpriteRenderer spriteRenderer = w.characterModel.transform.Find("Graphics").GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(125.0f / 255.0f, 148.0f / 255.0f, 240.0f / 255.0f, 255.0f / 255.0f);
        }
    }
}

