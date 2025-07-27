
using System.Collections.Generic;
using UnityEngine;


public class Centipede : MonoBehaviour
{
    // create and initialse a list of centipede segments
    private List<CentipedeSegment> segments = new List<CentipedeSegment>();


    // reference to centipede segment prefab
    // the variable type 'CentipedeSegment' ensures the game object has the 'CentipedeSegment' script attached to it
    public CentipedeSegment centipedeSegmentPrefab;

    // reference to mushroom prefab
    // the variable type 'Mushroom' ensures the game object has the 'Mushroom' script attached to it
    public Mushroom mushroomPrefab;


    // reference to centipede head sprite
    public Sprite headSprite;

    // reference to centipede body sprite
    public Sprite bodySprite;

    // determines which layers the centipede can collide with
    public LayerMask collisionMask = ~0;

    public Collider2D homeArea;

    // how fast the centipede moves
    public float speed = 20f;

    // how many segments the centipede has
    public int size = 12;



    // when the game / scene starts
    private void OnEnable()
    {
        // loop through all the centipede segments
        foreach (CentipedeSegment segment in segments) 
        {
            // and enable the centipede segments script
            segment.enabled = true;
        }
    }


    // when the game / scene ends
    private void OnDisable()
    {
        /*foreach (CentipedeSegment segment in segments) 
        {
            segment.enabled = true;
        }*/
    }


    // remove the segment of the centipede that has been hit by the player bullet
    // passes in the segment to be removed
    public void Remove(CentipedeSegment segment)
    {
        // get the grid position of the segment that has been hit
        Vector2 position = GridPosition(segment.transform.position);

        // and spawn a mushroom at that position
        Instantiate(mushroomPrefab, position, Quaternion.identity);

        // if there is a centipede segment 'ahead' of the segment that has been hit
        if (segment.ahead != null) 
        {
            // set the centipede segment 'behind' that has been hit for removal
            segment.ahead.behind = null;
        }

        // if there is a centipede segment 'behind' the segment that has been hit
        if (segment.behind != null)
        {
            // set the centipede segment 'ahead' that has been hit for removal
            segment.behind.ahead = null;

            // and set the new 'behind' segment to be the head segment
            segment.behind.UpdateHeadSegment();
        }

        // remove the segment from the centipede's segments list
        segments.Remove(segment);

        // then destroy the segment's game object
        Destroy(segment.gameObject);
    }


    // respawn the centipede
    public void Respawn()
    {
        // first loop through all centipede segments in the segments list
        foreach (CentipedeSegment segment in segments) 
        {
            // and destroy each segment game object
            Destroy(segment.gameObject);
        }

        // then clear the centipede segment list
        segments.Clear();

        // now loop through the number of centipede segments to spawn
        for (int i = 0; i < size; i++)
        {
            // set the position of the current centipede segment one segment to the left of the previous segment
            Vector2 position = GridPosition(transform.position) + (Vector2.left * i);

            // spawn the segment
            CentipedeSegment segment = Instantiate(centipedeSegmentPrefab, position, Quaternion.identity, transform);

            //segment.spriteRenderer.sprite = i == 0 ? headSprite : bodySprite;
            // if the spawned segment is the first segment of the centipede - position 0
            if (i == 0)
            {
                // then set the sprite of the segment to be the centipede's head
                segment.spriteRenderer.sprite = headSprite;
            }

            // otherwise
            else
            {
                // set the sprite of the segment to be the body
                segment.spriteRenderer.sprite = bodySprite;
            }

            // attach the centipede segment script to the spawned segment
            segment.centipede = this;

            // and enable the script
            segment.enabled = enabled;

            // add the centipede segment to the centipede list
            segments.Add(segment);
        }

        // loop through the number of segments in the centipede segments list
        for (int i = 0; i < segments.Count; i++)
        {
            // get a reference to the segment at the current index position
            CentipedeSegment segment = segments[i];

            // set the position of the segment depending on where it is in the list
            segment.ahead = GetSegmentAt(i-1);

            segment.behind = GetSegmentAt(i+1);
        }

        // set the segment enabled flag
        enabled = true;
    }


    // returns the segment at a specific index in the centipede segments list
    private CentipedeSegment GetSegmentAt(int index)
    {
        // if the index of the required segment is greater than zero
        // and
        // the index of the requied segment is less than the number of segments in the list
        if (index >= 0 && index < segments.Count) 
        {
            // return the segment at that index
            return segments[index];
        }
        
        // otherwise
        else 
        {
            // there is nothing to return
            return null;
        }
    }


    // return a grid position of the centipede segments
    private Vector2 GridPosition(Vector2 position)
    {
        position.x = Mathf.Round(position.x);

        position.y = Mathf.Round(position.y);

        return position;
    }


} // end of class
