using UnityEngine;
using UnityEngine.UI;


public class Countdown : MonoBehaviour {

    public Text textClock;
    public GameManager gameManager;
    private float timeLeft;
    private AudioManager audioManager;
    private bool tick = true;

    // Use this for initialization
    void Start()
    {
        gameManager = transform.GetComponent<GameManager>();
        timeLeft = gameManager.roundLength;
        audioManager = GameObject.Find("_AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GetGameState() == GameState.GAME)
        {
            string timerMessage = "Fight!";
            timeLeft -= Time.deltaTime;
            
            if (timeLeft > 0.0f && timeLeft < gameManager.showdownStart){
                timerMessage = Mathf.Round(timeLeft).ToString().PadLeft(2, '0');
                gameManager.showdown = true;
            } else if(timeLeft <= 0.0f){
                timerMessage = "0";
                if(gameManager.GetGameState() != GameState.END)
                    gameManager.EndGame();
            }
            textClock.text = timerMessage;
        }
    }

    public void ResetCountdown(){
        timeLeft = gameManager.roundLength;
    }

    private void TickTack() {
        if (tick) {
            audioManager.Play("Tick");
            tick = false;
        }
        else {
            audioManager.Play("Tack");
            tick = true;
        }
    }
}
