using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCursor : MonoBehaviour
{
    public GameObject CursorOnHolograms;
    public GameObject CursorOffHolograms;
    
    void Awake () {
        if (CursorOnHolograms == null || CursorOffHolograms == null)
        {
            return;
        }

        CursorOnHolograms.SetActive(false);
        CursorOffHolograms.SetActive(false);
    }
	
	void Update () {

        if (GazeManager.Instance == null || CursorOnHolograms == null || CursorOffHolograms == null)
        {
            return;
        }

        if (GazeManager.Instance.Hit)
        {
            CursorOnHolograms.SetActive(true);
            CursorOffHolograms.SetActive(false);

        }
        else
        {
            CursorOffHolograms.SetActive(true);
            CursorOnHolograms.SetActive(false);
        }

        gameObject.transform.position = GazeManager.Instance.Position;

        gameObject.transform.forward = GazeManager.Instance.Normal;
    }
}
