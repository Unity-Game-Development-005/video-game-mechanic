
using UnityEngine;


public class Blaster : MonoBehaviour
{
    // reference to the player's rigidbody component
    private Rigidbody2D rb;

    // the movement direction of the player
    private Vector2 direction;

    // where the player is to be spawned
    private Vector2 spawnPosition;

    // the speed of the player
    public float speed = 20f;



    private void Awake()
    {
        // get the player;s rigidbody component
        rb = GetComponent<Rigidbody2D>();

        // set the spawn position for the player
        spawnPosition = transform.position;
    }


    private void Update()
    {
        GetPlayerInput();
    }


    private void GetPlayerInput()
    {
        direction.x = Input.GetAxis("Horizontal");

        direction.y = Input.GetAxis("Vertical");
    }


    // move the player
    private void FixedUpdate()
    {
        // get the player's position
        Vector2 position = rb.position;

        // and move the player based on their input
        position += speed * Time.fixedDeltaTime * direction.normalized;

        rb.MovePosition(position);
    }


    public void Respawn()
    {
        // set the player's position to the player's spawn position
        transform.position = spawnPosition;

        // enable the player
        gameObject.SetActive(true);
    }


} // end of class
