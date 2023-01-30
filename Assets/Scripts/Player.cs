using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Pickup
{

    [SerializeField] private float forceMagnitude;
    public TMP_Text scoreText;
    public int score;
    public Canvas endGameCanvas;
    public Canvas gameOverCanvas;


    bool pickedUp = false;

    public int currentHealth;
    public int maxHealth = 5;

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Pickup"))
        {

            if (!pickedUp) 
            {
                Pickup pickup = col.gameObject.GetComponent<Pickup>();
                pickup.GetPickedUp();
                score += pickup.ScoreValue;
                scoreText.text = "Score: " + score;
                pickedUp= true;
                Destroy(col.gameObject);
            }
        }
        if (col.gameObject.CompareTag("Enemy"))
        {
            currentHealth--;

            if (currentHealth <= 0)
            {
                GameOver();
                
            }
        }

        if (col.gameObject.CompareTag("MovingBox"))
        {
            currentHealth--;

            if (currentHealth <= 0)
            {
                GameOver();

            }
        }


        pickedUp = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
        if (score >= 500)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        Time.timeScale = 0;
        endGameCanvas.gameObject.SetActive(true);
    }

    private void GameOver()
    {

        Time.timeScale = 0;
        gameOverCanvas.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Main");
  
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;
        if (rigidbody != null)
        {
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize();

            rigidbody.AddForceAtPosition(forceDirection * forceMagnitude, transform.position, ForceMode.Impulse);
        }
    }
}
