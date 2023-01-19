using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public TMP_Text scoreText;
    public int score;
    public Canvas endGameCanvas;

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Pickup"))
        {
            Debug.Log("picked up");
            Pickup pickup = col.gameObject.GetComponent<Pickup>();
            pickup.GetPickedUp();
            score += pickup.ScoreValue;
            scoreText.text = "Score: " + score;
            Destroy(col.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
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
        Time.timeScale = 0f;
        endGameCanvas.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Main");
  
    }
}
