using UnityEngine;

/// <summary>
/// Visualizes the users gaze point in realtime 3D.
/// </summary>

public class FocusDisplayMod : MonoBehaviour
{
    //Display cursor in scene. Always visibile during calibration
    public bool DisplayCursor = true;
    //Assign to main camera
    public GameObject CameraObject;
    //Determines the original scale of the cursor
    public float Scale = 0.2f;
    //Unsmoothed focus point coordinates, public for recording access
    public Vector3 GazeFocusUnsmoothed;
    //Vectors used in cursor rescaling
    private Vector3 scaleVector;
    private Vector3 distanceVector;
    //Hold the Renderer in a variable for efficient show/hide
    new private Renderer renderer;
    //Raycast to detect focus point in 3D
    private Ray gazeRay;
    private RaycastHit hitInfo;
    private GameObject from;
    private GameObject to;
    //Distance to object
    float distance;
    //Used in visualisation smoothing
    private bool isFirstFrame;
    private Vector3[] gazePositionArray;

    void Start()
    {
        if (!CameraObject)
        {
            CameraObject = StaticHelper.FindCameraObjectInScene();
        }
        from = CameraObject;
        if (!from)
        {
            StaticHelper.DebugMessage("TobiiProVR warning: FocusMod could not find a camera object to focus on! Assign camera object to Camera variable!");
        }

        to = GameObject.Find("TobiiGaze3D");

        scaleVector = new Vector3(Scale, Scale, Scale);
        GazeFocusUnsmoothed = new Vector3();

        renderer = gameObject.GetComponent<Renderer>();
        if (!DisplayCursor)
        {
            renderer.enabled = false;
        }

        gazeRay = new Ray();
        hitInfo = new RaycastHit();

        isFirstFrame = true;
        //array has to be of Length == 5:
        gazePositionArray = new Vector3[] {
            new Vector3(), new Vector3(), new Vector3(), new Vector3(), new Vector3()
        };
    }

    void Update()
    {
        //Check cursor renderer status
        if (DisplayCursor)
        {
            if (!renderer.enabled)
            {
                renderer.enabled = true;
            }
        }
        else
        {
            if (renderer.enabled)
            {
                renderer.enabled = false;
            }
        }

        Debug.DrawLine(from.transform.position, to.transform.position, Color.red);

        //Update gaze ray
        gazeRay.origin = from.transform.position;
        gazeRay.direction = to.transform.position;

        //When gazing at an object
        if (Physics.Linecast(from.transform.position, to.transform.position, out hitInfo))
        {
            //Move focus point to collisioon
            GazeFocusUnsmoothed = hitInfo.point;
            UpdateTransformSmoothed(GazeFocusUnsmoothed);

            //Adjust cursor size depending on distance
            distance = Vector3.Distance(hitInfo.point, from.transform.position);
            if (distance < 2.0f)
            {
                transform.localScale = scaleVector;
            }
            //If the cursor is further removed from the cameras position, start increasing its size.
            else
            {
                distanceVector = new Vector3(distance, distance, distance);
                transform.localScale = scaleVector + distanceVector / 70f;
            }
        }
        //When gazing out into infinity, set TobiiGaze3D direction coordinate as focus point.
        //It is recommended to surround an open space with invisbile hitboxes like in the included example scenes.
        //This allows for continuous gaze visualization and recording.
        else
        {
            GazeFocusUnsmoothed = to.transform.position;
            UpdateTransformSmoothed(GazeFocusUnsmoothed);
        }
    }

    //The FocusMod transform is used only for live visualization and is smoothed for a better experience.
    //When recording, the GazeFocusUnsmoothed Vector3 will be used by the recorder each frame.
    //This means the recorded file will always hold the actual values. These are smoothed again on replay.
    //Smoothing can be adjusted as wanted without damaging the recorded data.
    private void UpdateTransformSmoothed(Vector3 _gazeFocusUnsmoothed)
    {
        var aggregateVectors = new Vector3(0f, 0f, 0f);

        //on the first update, populate whole array with the first realtime Gaze3dPosition
        if (isFirstFrame)
        {
            for (int i = 0; i < gazePositionArray.Length; i++)
            {
                gazePositionArray[i] = _gazeFocusUnsmoothed;
            }
            isFirstFrame = false;
        }

        //use array to render a five point average of the realtime gaze position
        for (int i = 0; i < gazePositionArray.Length; i++)
        {
            //4 values are moved down an index, highest index gets realtime value
            //this means the array always holds the 5 latest gaze pos values
            if (i < 4)
            {
                gazePositionArray[i] = gazePositionArray[i + 1];
            }
            if (i == 4)
            {
                gazePositionArray[i] = _gazeFocusUnsmoothed;
            }

            aggregateVectors += gazePositionArray[i];
        }

        //use static helper method to get 5 point average value from aggregated vectors
        transform.position = StaticHelper.GetAverageVector(aggregateVectors, 5);
    }
}
