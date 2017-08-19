using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public const int BOARD_SIZE_X = 6;
    public const int BOARD_SIZE_Y = 5;
    public const int ORBCOUNT = 30;

    GameManager gm;

    GameObject orb_red, orb_blue, orb_green, orb_yellow;

    GameObject[] orbs;
    int orb_count = 0;

    Vector2 mousePosition;
    public GameObject selectedOrb;
    public GameObject switchOrb;
    public Orb selectedOrb_s;
    public Orb switchOrb_s;

    List<int> matchingOrbIndexes;
    List<GameObject> orbsToRemove;
    

    // Use this for initialization
    void Start ()
    {
        gm = GetComponent<GameManager> ();
        orbs = new GameObject[ORBCOUNT];
        orb_red = (GameObject) Resources.Load("Prefabs/Orbs/Orb_Red");
        orb_blue = (GameObject)Resources.Load("Prefabs/Orbs/Orb_Blue");
        orb_green = (GameObject)Resources.Load("Prefabs/Orbs/Orb_Green");
        orb_yellow = (GameObject)Resources.Load("Prefabs/Orbs/Orb_Yellow");
        matchingOrbIndexes = new List<int> ();
	}
	
	// Update is called once per frame
	void Update ()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (gm.getProcess() == PROCESS_STATES.GENERATE)
            generateBoard();

        if (selectedOrb != null)
        {
            selectedOrb_s.setPosition(mousePosition);
            selectedOrb_s.setIsMoving(true);
            selectedOrb.GetComponent<CircleCollider2D>().radius = 0.01f;
        }
        if (switchOrb != null)
        {
            switchOrbs(selectedOrb_s, switchOrb_s);
        }
    }

    //private

    GameObject generateOrb(int index)
    {
        int rand = Random.Range(1, 5);
        GameObject tmp;

        switch (rand)
        {
            case 1:
                tmp = Instantiate(orb_red);
                tmp.GetComponent<Orb>().setOrb(index, "red");
                return tmp;
            case 2:
                tmp = Instantiate(orb_blue);
                tmp.GetComponent<Orb>().setOrb(index, "blue");
                return tmp;
            case 3:
                tmp = Instantiate(orb_green);
                tmp.GetComponent<Orb>().setOrb(index, "green");
                return tmp;
            case 4:
                tmp = Instantiate(orb_yellow);
                tmp.GetComponent<Orb>().setOrb(index, "yellow");
                return tmp;
        }

        Debug.Log("There was an issue returning an orb.");
        return null;
    }

    void generateBoard()
    {
        for (int i = 0; i < (BOARD_SIZE_X * BOARD_SIZE_Y); i++)
        {
            orbs[i] = generateOrb(i);
        }

        gm.setProcess(PROCESS_STATES.WAIT);
    }

    void switchOrbs(Orb orb1, Orb orb2) //orb1 - moving orb | orb2 - stationary orb
    {
        int temp_index = orb2.getIndex();

        orb2.setIndex(orb1.getIndex());
        orb2.setPositionByIndex();

        orb1.setIndex(temp_index);

        switchOrb = null;
        switchOrb_s = null;
    }

    //public 
    public GameObject getSelectedOrb()
    {
        return selectedOrb;
    }

    public void setSelectedOrb(GameObject _selectedOrb)
    {
        selectedOrb = _selectedOrb;
        if (_selectedOrb != null)
        {
            selectedOrb_s = _selectedOrb.GetComponent<Orb>();
        }
        else
            selectedOrb_s = null;
    }

    public GameObject getSwitchOrb()
    {
        return switchOrb;
    }

    public void setSwitchOrb(GameObject _switchOrb)
    {
        switchOrb = _switchOrb;
        if (_switchOrb != null)
            switchOrb_s = _switchOrb.GetComponent<Orb>();
        else
            switchOrb_s = null;
    }
}
