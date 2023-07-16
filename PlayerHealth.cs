using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private int trapsLauer;
    public GameObject deathVFXPrefab;
    public GameObject deathVFXPrefab2;
    void Start()
    {
        trapsLauer = LayerMask.NameToLayer("Traps");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isdied=false;
        if (collision.gameObject.layer == trapsLauer)
        {
           isdied = true;
        }
        if (isdied == true)
        {
            Instantiate(deathVFXPrefab2, transform.position, transform.rotation);
            Instantiate(deathVFXPrefab, transform.position, Quaternion.Euler(0,0,Random.Range(-45,45)));
            gameObject.SetActive(false);
            AudioManager.PlayDeathAudio();
            GameManager.PlayerDied();
        }
    }
}
