using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public const float OFFSET = 0.51f;
    public const int   BOARD_WRAP = 6;
    
    Board game_board;

    string color;
    int index;
    Vector4 orbCheck = new Vector4(-1, -1, -1, -1);
    Vector2 position;
    public bool isMoving;

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
            gameObject.AddComponent<Rigidbody2D>();
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
        setPositionByIndex();
        game_board.setSelectedOrb(null);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Orb" && isMoving)
        {
            Debug.Log(coll.name);
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

    public void setPosition(Vector2 _position)
    {
        transform.position = position = _position;
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

    public void setOrb(int _index, string _color)
    {
        index = _index;
        color = _color;

        if (index != 0 && index != 1 && index != 2 && index != 3 && index != 4 && index != 5) //bottom check
            orbCheck.x = index - 6;
        if (index != 0 && index != 6 && index != 12 && index != 18 && index != 24) //left check
            orbCheck.y = index - 1;
        if (index != 24 && index != 25 && index != 26 && index != 27 && index != 28 && index != 29) //top check
            orbCheck.z = index + 6;
        if (index != 5 && index != 11 && index != 17 && index != 23 && index != 29) //right check
            orbCheck.w = index + 1;

        setPositionByIndex();
    }
}
