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
    GameObject selectedOrb;
    GameObject switchOrb;
    Orb selectedOrb_s;
    Orb switchOrb_s;

    public List<int> matchingOrbIndexes_h;
    public List<int> matchingOrbIndexes_v;
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
        matchingOrbIndexes_h = new List<int> ();
        matchingOrbIndexes_v = new List<int>();
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
            switchOrbs(selectedOrb, switchOrb);
        }

        if (gm.getProcess() == PROCESS_STATES.MATCH)
            generatePairs();

        //if(gm.getProcess() == PROCESS_STATES.DELETE)

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

    void switchOrbs(GameObject orb1, GameObject orb2) //orb1 - moving orb | orb2 - stationary orb
    {
        Orb orb1_s = orb1.GetComponent<Orb>(),
            orb2_s = orb2.GetComponent<Orb>();

        int temp_index = orb2_s.getIndex();

        orb2_s.setIndex(orb1_s.getIndex());
        orb2_s.setPositionByIndex();

        orb1_s.setIndex(temp_index);

        orbs[orb1_s.getIndex()] = orb1;
        orbs[orb2_s.getIndex()] = orb2;

        switchOrb = null;
        switchOrb_s = null;
    }

    bool matchOrbs(Orb orb1, Orb orb2)
    {
        if (orb1.getColor().Equals(orb2.getColor()))
            return true;

        return false;
    }

    bool checkIfTop(int index)
    {
        if (index > 23)
            return true;

        return false;
    }

    bool checkIfRight(int index)
    {
        if (index == 5 || index == 11 || index == 17 || index == 23 || index == 29)
            return true;

        return false;
    }

    void findStrings()
    {

    }

    void generatePairs()
    {
        foreach(GameObject orb in orbs)
        {
            Orb orb1 = orb.GetComponent<Orb>();
            Orb orb2;

            //check horizontal matches
            if(!checkIfRight(orb1.getIndex()))
            {
                orb2 = orbs[orb.GetComponent<Orb>().getIndex() + 1].GetComponent<Orb>();
                if(matchOrbs(orb1, orb2))
                {
                    matchingOrbIndexes_h.Add(orb1.getIndex());
                    matchingOrbIndexes_h.Add(orb2.getIndex());
                }
            }
            //check vertical matches
            if (!checkIfTop(orb1.getIndex()))
            {
                orb2 = orbs[orb.GetComponent<Orb>().getIndex() + 6].GetComponent<Orb>();
                if (matchOrbs(orb1, orb2))
                {
                    matchingOrbIndexes_v.Add(orb1.getIndex());
                    matchingOrbIndexes_v.Add(orb2.getIndex());
                }
            }
        }



        gm.setProcess(PROCESS_STATES.DELETE);
    }

    //public 
    public GameManager getGameManager()
    {
        return gm;
    }

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
