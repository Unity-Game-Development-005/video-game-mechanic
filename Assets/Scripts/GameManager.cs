
using UnityEngine;
using UnityEngine.UI;


[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    // make the script accessible to other scripts
    public static GameManager Instance;


    // reference to the player script
    private Blaster blaster;

    // reference to the centipede script
    private Centipede centipede;

    // reference to the mushroom field script
    private MushroomField mushroomField;


    // reference to game over screen
    public GameObject gameOver;

    // if the game is in play flag
    private bool gameInPlay;




    private void Awake()
    {
        // if an instance of this script already exists
        if (Instance != null) 
        {
            // then destroy it
            DestroyImmediate(gameObject);
        } 
        
        // otherwise
        else 
        {
            // create an instance of the script
            Instance = this;
        }
    }


    // when the game / scene ends
    private void OnDestroy()
    {
        // if an instance of the script exists
        if (Instance == this) 
        {
            // remove the instance of the script
            Instance = null;
        }
    }


    private void Start()
    {
        // find the player game object
        //blaster = FindObjectOfType<Blaster>();
        blaster = FindFirstObjectByType<Blaster>();

        // find the centipede game object
        //centipede = FindObjectOfType<Centipede>();
        centipede = FindFirstObjectByType<Centipede>();

        // find the mushrrom field game object
        //mushroomField = FindObjectOfType<MushroomField>();
        mushroomField = FindFirstObjectByType<MushroomField>();


        // start a new game
        NewGame();
    }


    private void Update()
    {
        WaitForKeyPress();
    }


    private void WaitForKeyPress()
    {
        // if the game is not in play and the player presses any key
        if (!gameInPlay && Input.anyKeyDown)
        {
            // start a new game
            NewGame();
        }
    }


    private void NewGame()
    {
        // show the player
        blaster.Respawn();

        // show the centipede
        centipede.Respawn();

        // clear and generate the mushroom field
        mushroomField.Clear();

        mushroomField.Generate();

        // hide the game over screen
        gameOver.SetActive(false);

        // set the game in play flag
        gameInPlay = true;

        // enable gameplay
        Time.timeScale = 1;
    }


    public void GameOver()
    {
        gameInPlay = false;

        // freeze the game
        Time.timeScale = 0;

        // show the game over screen
        gameOver.SetActive(true);

        // disable the player
        blaster.gameObject.SetActive(false);
    }


} // end of class
