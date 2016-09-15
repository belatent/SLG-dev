using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {
    public GameObject mainCamera;
    public GameObject battleMenu;
    public int battleMenuPosition;
    GameObject gmObj;
    HelperMethods helper = new HelperMethods();
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        setResource();
    }

    void setResource()
    {
        if(battleMenu == null)
            battleMenu = (GameObject)Resources.Load("perfab/battleMenu");
    }

    bool checkRangeExist()
    {
        foreach (GameObject gameObj in FindObjectsOfType<GameObject>())
        {
            if (gameObj.tag.Equals("moving range"))
                return true;
        }
        return false;
    }

    public void createBattleMenu(Vector3 position)
    {
        CameraMove camMove = (CameraMove)mainCamera.GetComponent("CameraMove");
        gmObj = GameObject.FindGameObjectWithTag("GM");
        GameManager gm = (GameManager)gmObj.GetComponent("GameManager");
        gm.focusThis = false;

        Vector3 camPos = mainCamera.transform.position;

        if (position.x > camPos.x)
            battleMenuPosition = -battleMenuPosition;
        Vector3 menuPos = new Vector3(camPos.x + battleMenuPosition, camPos.y, 0);

        battleMenu = (GameObject)Instantiate(battleMenu, menuPos, Quaternion.identity);
        camMove.lockCam();
    }
}
