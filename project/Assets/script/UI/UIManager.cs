using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {
    public GameObject mainCamera;
    public GameObject battleMenu;
    public int battleMenuPosition;
    GameObject gmObj;
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

    public void createBattleMenu(Vector3 position)
    {
        gmObj = GameObject.FindGameObjectWithTag("GM");
        GameManager gm = (GameManager)gmObj.GetComponent("GameManager");
        gm.focusThis = false;

        Vector3 camPos = mainCamera.transform.position;

        if (position.x > camPos.x)
            battleMenuPosition = -battleMenuPosition;
        Vector3 menuPos = new Vector3(camPos.x+battleMenuPosition,camPos.y, -1);

        battleMenu = (GameObject)Instantiate(battleMenu, menuPos, Quaternion.identity);
    }
}
