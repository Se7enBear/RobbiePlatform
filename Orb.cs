using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    int player;
    public GameObject ex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==player)
        {
            Instantiate(ex, transform.position, transform.rotation);
            gameObject.SetActive(false);
            AudioManager.PlayOrbAudio();
            GameManager.PlayerGrabbedOrb(this);
        }
    }
    private void Start()
    {
        player = LayerMask.NameToLayer("Player");
        GameManager.RegisterOrb(this);

    }
}