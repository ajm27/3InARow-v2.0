using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public const float OFFSET = 0.51f;
    public const int   BOARD_WRAP = 6;
    
    Board game_board;
    
    string color;
    public int index;
    Vector2 position;
    bool isMoving;

    public Rigidbody2D rb = null;

	// Use this for initialization
	void Start ()
    {
        game_board = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Board>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isMoving && rb == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
            rb = gameObject.GetComponent<Rigidbody2D>();
        }
	}

    private void OnMouseDown()
    {
        game_board.setSelectedOrb(this.gameObject);
    }

    private void OnMouseUp()
    {
        isMoving = false;
        gameObject.GetComponent<CircleCollider2D>().radius = 0.255f;
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        rb = null;
        setPositionByIndex();
        game_board.setSelectedOrb(null);

        game_board.getGameManager().setProcess(PROCESS_STATES.MATCH);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Orb" && isMoving)
        {
            game_board.setSwitchOrb(coll.gameObject);
        }
    }

    public string getColor ()
    {
        return color;
    }

    public void setColor (string _color)
    {
        color = _color;
    }

    public int getIndex()
    {
        return index;
    }

    public void setIndex(int _index)
    {
        index = _index;
    }

    public Vector2 getPosition()
    {
        return position;
    }

    public bool getIsMoving()
    {
        return isMoving;
    }

    public void setIsMoving(bool _moving)
    {
        isMoving = _moving;
    }

    public void setRefPosition(Vector2 _position)
    {
        position = _position;
    }

    public void setPosition(Vector2 _position)
    {
        transform.position = _position;
    }

    int getY(int index)
    {
        int tmp = 0;

        while (index > BOARD_WRAP)
        {
            index = index - BOARD_WRAP;
            tmp++;
        }

        return tmp;
    }

    public void setPositionByIndex()
    {
        int x, y;

        x = y = index + 1;
        //get x
        while (x > BOARD_WRAP)
        {
            x = x - 6;
        }
        //get y
        y = getY(y);

        setPosition(new Vector2(x * OFFSET, y * OFFSET));
    }

    public void setOrb(int _index, string _color, Vector2 _position)
    {
        index = _index;
        color = _color;
        position = _position;

        setPositionByIndex();
    }
}
