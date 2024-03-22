using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Enemy : MonoBehaviour
{
    #region Movement_variables
    public float movespeed;
    #endregion

    #region Physics_components
    Rigidbody2D EnemyRB;
    #endregion

    #region Targeting_variables
    public Transform player;
    #endregion

    #region Attack_variables
    public float explosionDamage;
    public float explosionRadius;
    public GameObject explosionObj;
    #endregion

    #region Health_variables
    public float maxHealth;
    float currHealth;
    #endregion


    //Runs once on creation
    #region Unity_functions
    private void Awake() {
        EnemyRB = GetComponent<Rigidbody2D>();

        currHealth = maxHealth;
        transform.position = new Vector2(Random.Range(-24.0f, 8.0f), Random.Range(-12.0f, 5.0f));

        Vector2 startingPosition = transform.position;
    }

    //Runs every frame
    private void Update() {
        //Check to see if we know where the player is
        if (player == null) {
            return;
        }

        Move();
    }
    #endregion

    #region Movement_functions
    //Move directly at player
    private void Move() {
        //Calculate movement vector player position - enemy postion = direction of player relative to enemy
        Vector2 direction = player.position - transform.position;

        EnemyRB.velocity = direction.normalized * movespeed;
    }
    #endregion

    #region Attack_functions
    //Raycasts box for player, causes damage, spawns explosions prefab
    private void Explode() {
        //Call audioManager for explosion sound
        FindObjectOfType<AudioManager>().Play("Explosion");

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, explosionRadius, Vector2.zero);

        foreach (RaycastHit2D hit in hits) {
            if (hit.transform.CompareTag("Player")) {
                //Cause damage
                Debug.Log("Hit Player with explosion");
                //spawn explosion prefab
                Instantiate(explosionObj, transform.position, transform.rotation);
                hit.transform.GetComponent<PlayerController>().TakeDamage(explosionDamage);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.CompareTag("Player")) {
            Explode();
        }
    }
    #endregion

    #region Health_functions
    //Enemy takes damage based on value param
    public void TakeDamage(float value) {
        FindObjectOfType<AudioManager>().Play("BatHurt");
        //Decrement health
        currHealth -= value;
        Debug.Log("Health is now " + currHealth.ToString());

        //Check for death
        if (currHealth <= 0) {
            Die();
            Score.Singleton.AddScore(1);
        }
    }

    //Destroys enemy object
    private void Die() {
        //Destroys enemy object
        Destroy(this.gameObject);
    }
    #endregion
}
