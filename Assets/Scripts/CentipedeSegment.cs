
using UnityEngine;


public class CentipedeSegment : MonoBehaviour
{
    // reference to the centipede segment's sprite renderer component
    public SpriteRenderer spriteRenderer;

    // reference to the centipede segment's box collider component
    public BoxCollider2D boxCollider;

    // reference to the centipede script
    public Centipede centipede;

    // reference to the centipede segment ahead of the current segment
    public CentipedeSegment ahead;

    // reference to the centipede segment behind the current segment
    public CentipedeSegment behind;


    // flag to check if the centipede's head segment is ahead of the current segment
    public bool isHead => ahead == null;


    // the initial direction the centipede segment is moving
    private Vector2 direction = Vector2.right + Vector2.down;

    // what the centipede segment is moving toward
    private Vector2 targetPosition;




    private void Awake()
    {
        // get the centipede segment's sprite renderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // get the centipede segment's box collider component
        boxCollider = GetComponent<BoxCollider2D>();

        // set the target position of the current centipede segment to the segment's current position
        targetPosition = transform.position;
    }


    // when the game / scene starts
    private void OnEnable()
    {
        // enable the centipede's segment collider
        boxCollider.enabled = true;
    }


    // when the game / scene ends
    private void OnDisable()
    {
        // disable the centipede's segment collider
        boxCollider.enabled = false;
    }


    private void Update()
    {
        // if the head segment of the centipede has reached its target position
        if (isHead && Vector2.Distance(transform.position, targetPosition) < 0.1f) 
        {
            // get the next target position
            UpdateHeadSegment();
        }

        // get the current position of the centipede's head segment
        Vector2 currentPosition = transform.position;

        // get the speed of the centipede
        float speed = centipede.speed * Time.deltaTime;

        // move the centipede's head segment towards the target position 
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, speed);

        // get the centipede's head segment movement direction
        Vector2 movementDirection = (targetPosition - currentPosition).normalized;

        // get the angle the centipede's head segment is facing
        float angle = Mathf.Atan2(movementDirection.y, movementDirection.x);

        // set the rotation of the centipede's head segment depending on which direction it is moving
        // convert the value from 'radians' to 'degrees'
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);

        // if the centipede segment is the head
        if (spriteRenderer.sprite == isHead)
        {
            // set the sprite renderer to the head sprite
            spriteRenderer.sprite = centipede.headSprite;
        }

        // otherwise
        // if the centipede segment is the body
        else
        {
            // set the sprite renderer to the body sprite
            spriteRenderer.sprite = centipede.bodySprite;
        }
    }


    // get the next target position for the centipede's head segment
    public void UpdateHeadSegment()
    {
        // get the current grid position of the head segment
        Vector2 gridPosition = GridPosition(transform.position);

        // set the target position
        targetPosition = gridPosition;

        // get the next grid position
        targetPosition.x += direction.x;


        // if the centipede head segment collides with an object
        if (Physics2D.OverlapBox(targetPosition, Vector2.zero, 0f, centipede.collisionMask))
        {
            // reverse the horizontal direction of the centipede head segment
            direction.x = -direction.x;

            // and advance to the next row
            targetPosition.x = gridPosition.x;

            targetPosition.y = gridPosition.y + direction.y;

            // get the home bounds of the centipede's home area
            Bounds homeBounds = centipede.homeArea.bounds;

            // if the centipede is moving up and its 'y' position is greater than the home area's maximum 'y' position
            // or
            // the centipede is down up and its 'y' position is less than the home area's minimum 'y' position
            if ((direction.y == 1f && targetPosition.y > homeBounds.max.y) || (direction.y == -1f && targetPosition.y < homeBounds.min.y))
            {
                // reverse the vertical direction of the centipede
                direction.y = -direction.y;

                targetPosition.y = gridPosition.y + direction.y;
            }
        }

        // if there are body segments behind the head segment
        if (behind != null) 
        {
            // update the body segments
            behind.UpdateBodySegment();
        }
    }


    private void UpdateBodySegment()
    {
        // set the current body segment's target movement direction the segment ahead of it
        targetPosition = GridPosition(ahead.transform.position);

        // set the segment's movement direction
        direction = ahead.direction;

        // if there are body segments behind the current body segment
        if (behind != null) 
        {
            // update the body segments
            behind.UpdateBodySegment();
        }
    }


    // return a vector two grid position for a centipede segment
    private Vector2 GridPosition(Vector2 position)
    {
        position.x = Mathf.Round(position.x);

        position.y = Mathf.Round(position.y);

        return position;
    }


    // checks for collisions on the centipede
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if the centipede has collided with the player
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && centipede.enabled)
        {
            // disable the centipede script
            centipede.enabled = false;

            // show the game over screen
            GameManager.Instance.GameOver();
        }

        // if the centipede has been hit by the player's bullet
        if (collision.gameObject.layer == LayerMask.NameToLayer("Dart") && collision.collider.enabled)
        {
            // disable the collider on the segment that has been hit
            collision.collider.enabled = false;

            // then remove the segment that has been hit
            centipede.Remove(this);
        }
    }


} // end of class
