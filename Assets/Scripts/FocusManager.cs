using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusManager : MonoBehaviour {

    public static FocusManager Instance = null;

    public GameObject FocusedGameObject { get; private set; }

    void Awake () {
            Instance = this;
    }

    void Start()
    {
        FocusedGameObject = null;
    }

    void Update()
    {
        if (GazeManager.Instance.Hit)
        {
            RaycastHit hitInfo = GazeManager.Instance.HitInfo;
            if (hitInfo.collider != null)
            {
                FocusedGameObject = hitInfo.collider.gameObject;
            }
            else
            {
                FocusedGameObject = null;
            }
        }
        else
        {
            FocusedGameObject = null;
        }
    }
}
