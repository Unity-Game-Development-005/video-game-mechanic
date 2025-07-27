
using UnityEngine;


public class Dart : MonoBehaviour
{
    // reference to the bullet's rigidbody component
    private Rigidbody2D rb;

    // reference to the bullet's box collider component
    private BoxCollider2D boxCollider;

    // reference to the player
    private Transform parent;

    // speed of bullet
    public float speed = 50f;




    private void Awake()
    {
        // get the bullet's rigidbody component
        rb = GetComponent<Rigidbody2D>();

        // prevent the bullet from moving
        rb.bodyType = RigidbodyType2D.Kinematic;

        // get the bullet's box collider component
        boxCollider = GetComponent<BoxCollider2D>();

        // disable the box collider
        boxCollider.enabled = false;

        // set the bullet as a child of the player
        parent = transform.parent;
    }


    private void Update()
    {
        FireBullet();
    }


    private void FireBullet()
    {
        // if the bullet is not moving and the player presses the 'fire' button
        //if (rb.isKinematic && Input.GetButton("Fire1"))
        if (rb.bodyType == RigidbodyType2D.Kinematic && Input.GetButton("Fire1"))
        {
            // detach the bullet from the player so it can move
            transform.SetParent(null);

            // move the bullet
            rb.bodyType = RigidbodyType2D.Dynamic;

            // enable the bullet's box collider
            boxCollider.enabled = true;
        }
    }


    // move the bullet
    private void FixedUpdate()
    {
        // if the bullet is not moving
        //if (rb.isKinematic) return;
        if (rb.bodyType == RigidbodyType2D.Kinematic)
        {
            // then return
            return;
        }

        // otherwise
        // get the bullet's position
        Vector2 position = rb.position;

        // and move the bullet
        position += speed * Time.fixedDeltaTime * Vector2.up;

        rb.MovePosition(position);
    }


    // if the bullet collides with an object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // re-attach the bullet to the player
        transform.SetParent(parent);

        // and set it's position
        transform.localPosition = new Vector3(0f, 0.5f, 0f);

        // stop the bullet from moving
        rb.bodyType = RigidbodyType2D.Kinematic;

        // disable the bullet's collider
        boxCollider.enabled = false;
    }


} // end of class
