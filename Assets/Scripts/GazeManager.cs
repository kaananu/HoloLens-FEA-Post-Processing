using UnityEngine;

public class GazeManager : MonoBehaviour {

    public static GazeManager Instance = null;

    [Tooltip("Maximum gaze distance for calculating a hit.")]
    public float MaxGazeDistance = 5.0f;

    [Tooltip("Select the layers raycast should target.")]
    public LayerMask RaycastLayerMask = Physics.DefaultRaycastLayers;

    public bool Hit { get; private set; }

    public RaycastHit HitInfo { get; private set; }

    public Vector3 Position { get; private set; }

    public Vector3 Normal { get; private set; }

    private GazeStabilizer gazeStabilizer;
    private Vector3 gazeOrigin;
    private Vector3 gazeDirection;

    void Awake()
    {
        Instance = this;
        // GetComponent GazeStabilizer and assign it to gazeStabilizer.
        gazeStabilizer = GetComponent<GazeStabilizer>();
    }

    void Update () {
        //  Assign Camera's main transform position to gazeOrigin.
        gazeOrigin = Camera.main.transform.position;

        // Assign Camera's main transform forward to gazeDirection.
        gazeDirection = Camera.main.transform.forward;

        // Using gazeStabilizer, call function UpdateHeadStability.
        // Pass in gazeOrigin and Camera's main transform rotation.
        gazeStabilizer.UpdateHeadStability(gazeOrigin, Camera.main.transform.rotation);

        // Using gazeStabilizer, get the StableHeadPosition and
        // assign it to gazeOrigin.
        gazeOrigin = gazeStabilizer.StableHeadPosition;

        UpdateRaycast();
    }

    private void UpdateRaycast()
    {
        RaycastHit hitInfo;

        Hit = Physics.Raycast(gazeOrigin, gazeDirection, out hitInfo, MaxGazeDistance, RaycastLayerMask);

        HitInfo = hitInfo;

        if (Hit)
        {
            Position = hitInfo.point;
            Normal = hitInfo.normal;
        }
        else
        {
            Position = gazeOrigin + (MaxGazeDistance * gazeDirection);
            Normal = gazeDirection;
        }
    }
}
