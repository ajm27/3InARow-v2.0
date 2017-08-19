using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PROCESS_STATES
{
    GENERATE = 0,
    WAIT,
    MATCH,
    DELETE,
    SHIFT,
    REGENERATE
};

public class GameManager : MonoBehaviour
{    
    PROCESS_STATES current_process;

	// Use this for initialization
	void Start ()
    {
        gameObject.AddComponent<Board>();
        current_process = PROCESS_STATES.GENERATE;
	}
	
	// Update is called once per frame
	//void Update (){}

    public PROCESS_STATES getProcess()
    {
        return current_process;
    }

    public void setProcess (PROCESS_STATES _process)
    {
        current_process = _process;
    }
}
