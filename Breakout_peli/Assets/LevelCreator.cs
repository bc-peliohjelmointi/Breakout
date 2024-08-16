using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class LevelCreator : MonoBehaviour
{
    public GameObject blockPrefab; // Mit‰ luodaan
    public GameObject blockStartPlace;  // Miss‰ on aloituskohta

    public GameObject ballPrefab;

    public int columns;
    public int rows;

    public float scoreSpeed;

    public Text scoreText;
    public Text livesText;
    public Text comboText;

    bool addScore = false;

    bool firstUpdate = false;
    int amountLives;
    int amountBlocks;
    float score;
    float scoreTarget;

    private DynamicBallController ballScript;

    // T‰m‰ funktio on kytketty delegateen
    public void OnBlockDestroyed() {
        if (amountBlocks == 0 ) {
            // Player won!
        }
        if (addScore)
        {
            scoreTarget += 100.0f;
        }
    }

    public void OnBallDestroyed()
    {
        amountLives -= 1;
        livesText.text = "Lives: " + amountLives;
        if (amountLives > 0)
        {
            ballScript.Launch();
        }
        else
        {
            // Game over
        }
    }

    void Start()
    {
        // Hae pistelaskuri
        score = 0;
        GameObject scoreObject = GameObject.FindGameObjectWithTag("ScoreDisplay");
        if (scoreObject == null)
        {
            Debug.LogError("Score not found");
        }
        scoreText = scoreObject.GetComponent<Text>();
        addScore = true;

        amountLives = 10;
        GameObject livesObject = GameObject.FindGameObjectWithTag("LiveDisplay");
        if (livesObject == null)
        {
            Debug.LogError("Lives not found");
        }
        livesText = livesObject.GetComponent<Text>();
        livesText.text = "Lives: " + amountLives;

        // Luo pallo ja huomaa kun se tuhoutuu
        
        GameObject game = GameObject.Find("BallStartPlace");
        Vector3 startpos = game.transform.position;
        GameObject ball = Instantiate(ballPrefab, startpos, Quaternion.identity, this.transform);
        ballScript = ball.GetComponent<DynamicBallController>();
        ballScript.BallDestroyedEvent += OnBallDestroyed;
        firstUpdate = true;

		// Pid‰ kirjaa palojen m‰‰r‰st‰ ja huomaa kun ne
		// kaikki on tuhottu
		// Alusta laskuri
		amountBlocks = 0;
       
        for (int rivi = 0; rivi < rows; rivi++) {
			// Hae aloituspaikka toisesta GameObjectista
			Vector3 blockPlace = blockStartPlace.transform.position;
            blockPlace.y -= rivi * 1.0f;
            // Luo yksi rivi
            for (int i = 0; i < columns; i++) {
                // Luo kohtaan blockPlace, laita oma transform vanhemmaksi
                GameObject createdBlock = Instantiate(blockPrefab, blockPlace, Quaternion.identity, transform);

                // Kuuntele palikan l‰hett‰m‰‰ tapahtumaa
                createdBlock.GetComponent<BlockController>().BlockDestroyEvent += OnBlockDestroyed;

                blockPlace.x += 0.7f; // Siirr‰ luomiskohtaa
                amountBlocks += 1; // Lis‰‰ laskuriin 1
            }
        }
    }

	private void OnDestroy()
	{
        // Disable scoring when game ends and all blocks are destroyed?
        addScore = false;
	}

    // Update is called once per frame
    void Update()
    {
        if (firstUpdate)
        {
            ballScript.Launch();
            firstUpdate = false;
        }
        if (score < scoreTarget)
        {
            score += scoreSpeed * Time.deltaTime;
            scoreText.text = "Score: " + (int)score;
        }
    }
}
