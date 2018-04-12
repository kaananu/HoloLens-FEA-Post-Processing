using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class GestureManager : MonoBehaviour {

    public static GestureManager Instance = null;

    GestureRecognizer gestureRecognizer;

    public GameObject FocusedObject { get; private set; }

    void Start ()
    {
        Instance = this;

        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);

        gestureRecognizer.Tapped += (args) =>
        {
            GameObject focusedObject = FocusManager.Instance.FocusedGameObject;

            if (focusedObject != null)
            {
                focusedObject.SendMessageUpwards("OnSelect", SendMessageOptions.DontRequireReceiver);
            }
        };

        gestureRecognizer.StartCapturingGestures();
    }
	
	void Update () {
		

	}
}
