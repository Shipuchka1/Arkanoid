using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float playerVelocity;
    private Vector3 playerPosition;
    public float boundary;
    private int playerPoints;
    public List<Image> livesImages;
    public int lives;
    public Text scoreText;
    private int counter;
    public Ball ball;
    public AudioClip pointsSound;
    public AudioClip loseSound;
    public RectTransform startPanel;
    public RectTransform gameOverPanel;
    void Start()
    {
        gameOverPanel.gameObject.SetActive(false);

        counter = 2;
        playerPoints = 0;
        playerPosition = gameObject.transform.position;
        startPanel.gameObject.SetActive(true);
        Thread.Sleep(3000);
        startPanel.gameObject.SetActive(false);
      
    }

    // Update вызывается при отрисовке каждого кадра игры
    void Update()
    {
        if ((GameObject.FindGameObjectsWithTag("Block")).Length == 0)
        {
            if (SceneManager.GetActiveScene().name == "Scene1")
                SceneManager.LoadScene("Scene2");
            else if (SceneManager.GetActiveScene().name == "Scene2")
                SceneManager.LoadScene("Scene3");
            else if (SceneManager.GetActiveScene().name == "Scene3")
                GameOver();
        }
            playerPosition.x += Input.GetAxis("Horizontal") * playerVelocity;
        scoreText.text = "Score: " + playerPoints;
        // выход из игры
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // обновим позицию платформы
        transform.position = playerPosition;

        if (playerPosition.x < -boundary)
        {
            transform.position = new Vector3(-boundary, playerPosition.y, playerPosition.z);
            playerPosition.x = -boundary;
        }
        if (playerPosition.x > boundary)
        {
            transform.position = new Vector3(boundary, playerPosition.y, playerPosition.z);
            playerPosition.x = boundary;
        }
    }

    void addPoints(int points)
    {
        GetComponent<AudioSource>().PlayOneShot(pointsSound);

        playerPoints += points;
    }

    private void AddLive()
    {
        if(lives<4)
        livesImages[lives++].enabled = true;
    }

    public void Damage()
    {
        counter = 2;
        GetComponent<AudioSource>().PlayOneShot(loseSound);
        if(lives>0)
            livesImages[--lives].enabled = false;
            if (lives == 0)
                GameOver();
        transform.localScale = new Vector2(2.75f, 1.0f);
        ball.visible = true;
        

    }
    public void GameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="BoostLife")
        {
            AddLive();
        }
        if (collision.tag == "BoostDeath")
        {
            Damage();
        }
        if (collision.tag == "BoostIncrease")
        {
            if(counter < 3)
            {
            transform.localScale = new Vector2(transform.localScale.x*2, transform.localScale.y);
                counter++;
            }
        }
        if (collision.tag == "BoostDecrease")
        {
            if(counter>1)
            {
            transform.localScale = new Vector2(transform.localScale.x/2,transform.localScale.y);
                counter--;
            }
        }

        if(collision.tag=="BoostSlow")
        {
           ball.ResetVelocity();
        }

        if (collision.tag == "BoostInvisible")
        {
            ball.visible = false;
        }
        Destroy(collision.gameObject);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scene1");
    }

    public void Exit()
    {
        Application.Quit();
    }

}