using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    private SceneFader fader;
    List<Orb> orbs;
    private Door lockDoor;
    public int deathNum;
    float gameTime;
    bool gameIsOver;
    private void Update()
    {   if (gameIsOver)
        {
            return;
        }
        gameTime += Time.deltaTime;
        UIManager.UpdateTimeUI(gameTime);
        Exit();
    }
    public static void RegisterDoor(Door door)
    {
        instance.lockDoor = door;
    }
    private void Awake()
    {if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        orbs = new List<Orb>();
        DontDestroyOnLoad(this);
       
    }
    public static bool GameOver()
    {
        return instance.gameIsOver;
    }
   public static void PlayWin()
    {
        instance.gameIsOver = true;
        AudioManager.PlayerWonAudio();
        UIManager.Win();
    }
    public static void PlayerDied()
    {
        instance.fader.Fadeout();
        instance.deathNum++;
        UIManager.UpdateDeathUI(instance.deathNum);
        instance.Invoke("RestartScene", 1.5f);
      
    }
    public void Exit()
    {
       if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public static void RegisterOrb(Orb orb)
    {
        if (instance == null) return;
        if(!instance.orbs.Contains(orb))
        {
            instance.orbs.Add(orb);
        }
        UIManager.UpdateOrbUI(instance.orbs.Count);
    }
    public static void PlayerGrabbedOrb(Orb orb)
    {
        
        if (!instance.orbs.Contains(orb)) return;
        instance.orbs.Remove(orb);
        if (instance.orbs.Count == 0)
            instance.lockDoor.Open();
        UIManager.UpdateOrbUI(instance.orbs.Count);
    }
    public static void RegisterSceneFader(SceneFader obj)
    {

        instance.fader = obj;
    }

    void RestartScene()
    {
        instance.orbs.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
