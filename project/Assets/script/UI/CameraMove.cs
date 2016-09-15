using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
    public GameObject background;
    public float margin;
    public float moveSensitivity;
    public float minSize;
    public float maxSize;
    public float zoomSensitivity;
    Camera camera;
    public Vector2 minXY;
    public Vector2 maxXY;
    bool cameraLock = false;

    // Use this for initialization
    void Start () {
        camera = (Camera)this.gameObject.GetComponent("Camera");
        //setMANandMin();
    }
	
	// Update is called once per frame
	void Update () {
        if (!cameraLock)
        {
            zoomCam();
            moveCam();
        }
    }

    void moveCam()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print(Input.mousePosition);
        }
        //move right
        if (Input.mousePosition.x >= Screen.width - margin)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x + moveSensitivity,minXY.x,maxXY.x), transform.position.y,0);
        }
        //move left
        if (Input.mousePosition.x <= margin)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x - moveSensitivity, minXY.x, maxXY.x), transform.position.y, 0);
        }
        //move up
        if (Input.mousePosition.y >= Screen.height - margin)
        {
           
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y + moveSensitivity, minXY.y, maxXY.y), 0);
        }
        //move down
        if (Input.mousePosition.y <= margin)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y - moveSensitivity, minXY.y, maxXY.y), 0);
        }
    }

    void zoomCam()
    {
        float size = camera.orthographicSize;
        size += Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
        size = Mathf.Clamp(size, minSize, maxSize);
        camera.orthographicSize = size;
        //setMANandMin();
    }

    public void lockCam()
    {
        if (!cameraLock)
            cameraLock = true;
    }

    public void unlockCam()
    {
        if (cameraLock)
            cameraLock = false;
    }

    void checkKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("您按下了A键");
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("您抬起了A键");
        }
    }
}
