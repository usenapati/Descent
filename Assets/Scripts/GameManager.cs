using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject GOMenu;
    public PlayerMovementAdvanced pm;
    public NextGenWallRunning wr;
    public SlidingDone sliding;
    public AILocomotion aiLoco;
    private float healthplaceholder;
    public void CompleteLevel()
    {
        Debug.Log("reached the end!");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void gameOverMenu()
    {
        pm.enabled = false;
        wr.enabled = false;
        sliding.enabled = false;
        GameObject.Find("EnemyAI").SetActive(false);
        GOMenu.SetActive(true);
        
    }
}
