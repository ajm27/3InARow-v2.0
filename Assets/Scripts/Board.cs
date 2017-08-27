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

    //GameObject[,] orbs;
    public GameObject[] orbs;
    int orb_count = 0;

    Vector2 mousePosition;
    GameObject selectedOrb;
    GameObject switchOrb;
    Orb selectedOrb_s;
    Orb switchOrb_s;

    public List<int> matchingOrbIndexes_h;
    public List<int> matchingOrbIndexes_v;
    public List<GameObject> orbsToRemove;

    // Use this for initialization
    void Start ()
    {
        gm = GetComponent<GameManager> ();
        //orbs = new GameObject[ BOARD_SIZE_X, BOARD_SIZE_Y ];
        orbs = new GameObject[ORBCOUNT];
        orb_red = (GameObject) Resources.Load("Prefabs/Orbs/Orb_Red");
        orb_blue = (GameObject)Resources.Load("Prefabs/Orbs/Orb_Blue");
        orb_green = (GameObject)Resources.Load("Prefabs/Orbs/Orb_Green");
        orb_yellow = (GameObject)Resources.Load("Prefabs/Orbs/Orb_Yellow");
        //matchingOrbIndexes_h = new List<int> ();
        //matchingOrbIndexes_v = new List<int>();
        orbsToRemove = new List<GameObject>();
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
            findOrbsToRemove();

        if(gm.getProcess() == PROCESS_STATES.DELETE)
        {
            foreach (GameObject orb in orbsToRemove)
            {
                orb.tag = "null";
                Debug.Log(orb + " " + orb.tag);
                Destroy(orb);
            }

            orbsToRemove = new List<GameObject>();

            gm.setProcess(PROCESS_STATES.SHIFT);
        }

        if (gm.getProcess() == PROCESS_STATES.SHIFT)
            shiftBoard();
    }

    //private
    GameObject generateOrb(int index, Vector2 _position)
    {
        int rand = Random.Range(1, 5);
        GameObject tmp;

        switch (rand)
        {
            case 1:
                tmp = Instantiate(orb_red);
                tmp.GetComponent<Orb>().setOrb(index, "red", _position);
                return tmp;
            case 2:
                tmp = Instantiate(orb_blue);
                tmp.GetComponent<Orb>().setOrb(index, "blue", _position);
                return tmp;
            case 3:
                tmp = Instantiate(orb_green);
                tmp.GetComponent<Orb>().setOrb(index, "green", _position);
                return tmp;
            case 4:
                tmp = Instantiate(orb_yellow);
                tmp.GetComponent<Orb>().setOrb(index, "yellow", _position);
                return tmp;
        }

        Debug.Log("There was an issue returning an orb.");
        return null;
    }

    void generateBoard()
    {
        //int i = 0;
        //for (int y = 0; y < BOARD_SIZE_Y; y++)
        //{
        //    for (int x = 0; x < BOARD_SIZE_X; x++)
        //    {
        //        orbs[x, y] = generateOrb(i, new Vector2(x,y));
        //        i++;
        //    }
        //}
        for (int i = 0; i < ORBCOUNT; i++)
            orbs[i] = generateOrb(i, new Vector2(0,0));

        gm.setProcess(PROCESS_STATES.WAIT);
    }

    void switchOrbs(GameObject orb1, GameObject orb2) //orb1 - moving orb | orb2 - stationary orb
    {
        Orb orb1_s = orb1.GetComponent<Orb>(),
            orb2_s = orb2.GetComponent<Orb>();

        int temp_index = orb2_s.getIndex();
        Vector2 temp_pos = orb2_s.getPosition();

        orb2_s.setIndex(orb1_s.getIndex());
        orb2_s.setRefPosition(orb1_s.getPosition());
        orb2_s.setPositionByIndex();

        orb1_s.setIndex(temp_index);
        orb1_s.setRefPosition(temp_pos);

        //orbs[ (int) orb1_s.getPosition().x, (int) orb1_s.getPosition().y] = orb1;
        //orbs[ (int) orb2_s.getPosition().x, (int) orb1_s.getPosition().y] = orb2;
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

    bool checkIfBottom(int index)
    {
        if (index < 6)
            return true;

        return false;
    }

    bool checkIfRight(int index)
    {
        if (index == 5 || index == 11 || index == 17 || index == 23 || index == 29)
            return true;

        return false;
    }

    //void findStrings()
    //{
    //    Debug.Log("findStrings: started: 020..");
    //    //Debug.Log("..");

    //    matchingOrbIndexes_v.Sort();

    //    for (int i = 0; i < (matchingOrbIndexes_h.Count - 1); i++)
    //    {
    //        //Debug.Log("Step: " + i);
    //        if (!checkIfRight(matchingOrbIndexes_h[i]))
    //        {
    //            Debug.Log(string.Format("Comparing {0} to {1}", matchingOrbIndexes_h[i], matchingOrbIndexes_h[i + 1]));
    //            //Debug.Log("Not a right orb");
    //            if (matchingOrbIndexes_h[i].Equals(matchingOrbIndexes_h[i + 1]))
    //            {
    //                Debug.Log("Double found: " + matchingOrbIndexes_h[i]);
    //                if (!orbsToRemove.Contains(orbs[i - 1]))
    //                {
    //                    orbsToRemove.Add(orbs[i - 1]);
    //                    Debug.Log("Adding orb: " + orbs[i - 1] + " index: " + orbs[i - 1].GetComponent<Orb>().getIndex());
    //                }
    //                if (!orbsToRemove.Contains(orbs[i]))
    //                {
    //                    orbsToRemove.Add(orbs[i]);
    //                    Debug.Log("Adding orb: " + orbs[i] + " index: " + orbs[i].GetComponent<Orb>().getIndex());
    //                }
    //                if (!orbsToRemove.Contains(orbs[i + 1]))
    //                {
    //                    orbsToRemove.Add(orbs[i + 1]);
    //                    Debug.Log("Adding orb: " + orbs[i + 1] + " index: " + orbs[i + 1].GetComponent<Orb>().getIndex());
    //                }
    //            }
    //        }
    //    }
    //}

    //void generatePairs()
    //{
    //    //Debug.Log("generatePairs called");

    //    foreach (GameObject orb in orbs)
    //    {

    //        Orb orb1 = orb.GetComponent<Orb>();
    //        Orb orb2;

    //        //check horizontal matches
    //        if (!checkIfRight(orb1.getIndex()))
    //        {
    //            orb2 = orbs[orb.GetComponent<Orb>().getIndex() + 1].GetComponent<Orb>();
    //            if (matchOrbs(orb1, orb2))
    //            {
    //                matchingOrbIndexes_h.Add(orb1.getIndex());
    //                matchingOrbIndexes_h.Add(orb2.getIndex());
    //            }
    //        }
    //        //check vertical matches
    //        if (!checkIfTop(orb1.getIndex()))
    //        {
    //            orb2 = orbs[orb.GetComponent<Orb>().getIndex() + 6].GetComponent<Orb>();
    //            if (matchOrbs(orb1, orb2))
    //            {
    //                matchingOrbIndexes_v.Add(orb1.getIndex());
    //                matchingOrbIndexes_v.Add(orb2.getIndex());
    //            }
    //        }
    //    }

    //    findStrings();

    //    gm.setProcess(PROCESS_STATES.DELETE);

    //}

    //public 

    void findOrbsToRemove()
    {
        foreach(GameObject orb in orbs)
        {
            Orb tmp1_s = orb.GetComponent<Orb>();
            Orb tmp2_s, tmp3_s;
            if (!checkIfRight(tmp1_s.getIndex()))
                tmp2_s = orbs[tmp1_s.getIndex() + 1].GetComponent<Orb>();
            else
                continue;
            if (!checkIfRight(tmp2_s.getIndex()))
                tmp3_s = orbs[tmp2_s.getIndex() + 1].GetComponent<Orb>();
            else
                continue;


            if (tmp1_s.getColor().Equals(tmp2_s.getColor()) && tmp2_s.getColor().Equals(tmp3_s.getColor()))
            {
                if (!orbsToRemove.Contains(tmp1_s.gameObject))
                    orbsToRemove.Add(tmp1_s.gameObject);
                if (!orbsToRemove.Contains(tmp2_s.gameObject))
                    orbsToRemove.Add(tmp2_s.gameObject);
                if (!orbsToRemove.Contains(tmp3_s.gameObject))
                    orbsToRemove.Add(tmp3_s.gameObject);
            }
        }

        foreach (GameObject orb in orbs)
        {
            Orb tmp1_s = orb.GetComponent<Orb>();
            Orb tmp2_s, tmp3_s;
            if (!checkIfTop(tmp1_s.getIndex()))
                tmp2_s = orbs[tmp1_s.getIndex() + 6].GetComponent<Orb>();
            else
                continue;
            if (!checkIfTop(tmp2_s.getIndex()))
                tmp3_s = orbs[tmp2_s.getIndex() + 6].GetComponent<Orb>();
            else
                continue;


            if (tmp1_s.getColor().Equals(tmp2_s.getColor()) && tmp2_s.getColor().Equals(tmp3_s.getColor()))
            {
                if (!orbsToRemove.Contains(tmp1_s.gameObject))
                    orbsToRemove.Add(tmp1_s.gameObject);
                if (!orbsToRemove.Contains(tmp2_s.gameObject))
                    orbsToRemove.Add(tmp2_s.gameObject);
                if (!orbsToRemove.Contains(tmp3_s.gameObject))
                    orbsToRemove.Add(tmp3_s.gameObject);
            }
        }

        gm.setProcess(PROCESS_STATES.DELETE);
    }

    void shiftBoard()
    {
        foreach (GameObject orb in orbs)
        {
            Orb tmp = orb.GetComponent<Orb>();

            if (checkIfBottom(tmp.getIndex()))
                continue;

            GameObject asd = orbs[tmp.getIndex() - 6];
            if (orbs[tmp.getIndex() - 6].tag.Equals("null"))
            {
                Debug.Log("Called 002");
                tmp.setIndex(tmp.getIndex() - 6);
                tmp.setPositionByIndex();
            }
        }
        gm.setProcess(PROCESS_STATES.REGENERATE);
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
