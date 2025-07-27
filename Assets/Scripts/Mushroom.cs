
using UnityEngine;


public class Mushroom : MonoBehaviour
{
    /*
        Script added to mushroom game object 
    */


    // when a mushroom is hit
    public void Damage()
    {
        // destroy the mushroom
        Destroy(gameObject);
    }


    // checks for a collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if the player's bullet has collided with the mushroom
        if (collision.gameObject.layer == LayerMask.NameToLayer("Dart")) 
        {
            // destroy the mushroom
            Damage();
        }
    }


} // end of class
