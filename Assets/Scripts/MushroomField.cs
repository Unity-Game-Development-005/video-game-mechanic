
using UnityEngine;


public class MushroomField : MonoBehaviour
{
    // reference to box collidor component that acts as the spawning area for the mushrooms
    private BoxCollider2D mushroomSpawnArea;

    // reference to mushroom prefab
    // the variable type 'Mushroom' ensures the game object has the 'Mushroom' script attached to it
    public Mushroom mushroomPrefab;

    // number of mushrooms to spawn
    public int amount = 50;



    private void Awake()
    {
        // get the box collidor component
        // bounding area of the mushroom spawn area
        mushroomSpawnArea = GetComponent<BoxCollider2D>();
    }


    // generate the mushroom field
    public void Generate()
    {
        // get the size of the bounding area of the box collidor
        Bounds bounds = mushroomSpawnArea.bounds;

        // loop through the number of mushrooms to spawn
        for (int i = 0; i < amount; i++)
        {
            // initialise a starting position for the mushrooms
            Vector2 position = Vector2.zero;

            // get a random 'x' and 'y' position for the mushroom
            // within the bounds of the mushroom spawn area
            // 'Math.f.Round' rounds the number to the nearest whole number
            position.x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));

            position.y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));


            // place a mushroom at the random position
            Instantiate(mushroomPrefab, position, Quaternion.identity, transform);
        }
    }


    // clear any remaining mushrooms
    public void Clear()
    {
        // find any mushrooms in the play field
        // and store them in an array
        Mushroom[] mushrooms = FindObjectsOfType<Mushroom>();

        // loop through the mushroom array
        for (int i = 0; i < mushrooms.Length; i++) 
        {
            // and destroy the mushroon game object
            Destroy(mushrooms[i].gameObject);
        }
    }


} // end of class
