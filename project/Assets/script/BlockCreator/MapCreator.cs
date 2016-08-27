using UnityEngine;
using System.Collections;

public class MapCreator : MonoBehaviour {
    public GameObject impassableBlock;
    public GameObject passableBlock;
    public GameObject background;
    public int blockWidth;
    public int blockLength;

    BoxCollider backGround;
    int bkWidth;
    int bkLength;

    const string IMPASSABLE = "impassable";
    const string PASSABLE = "passable";

    public Block[,] blocklist { get; set; }
    HelperMethods helper = new HelperMethods();
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void init()
    {
        //get background size
        backGround = background.GetComponent<BoxCollider>();
        bkLength = (int)backGround.size.x;
        bkWidth = (int)backGround.size.y;
        print("Map Creator initialized.");
        print("Block Map size:\tWidth: " + bkWidth + "\tLength: " + bkLength);
        blocklist = createMap();
    }

    public Block[,] createMap()
    {
        Block[,] blocklist = new Block[bkWidth, bkLength];
        GameObject blockObj;
        Block block;
        Vector3 position;
        int x = 0;
        int y = 0;

        for (int i = -bkLength / 2; i < bkLength / 2; i++)
        {
            for (int j = -bkWidth / 2; j < bkWidth / 2; j++)
            {
                //set coordinate
                position = new Vector3(i * blockLength + blockLength / 2, j * blockWidth + blockWidth / 2, 0);

                //create block
                //check whether passable by ray
                RaycastHit hit;
                if (Physics.Raycast(position, Vector3.forward, out hit, 1))
                {
                    if (hit.collider.name.Equals("Objects"))
                    {
                        blockObj = (GameObject)Instantiate(impassableBlock, position, Quaternion.identity);

                        blockObj.name = "block（" + y + ", " + x + " )";
                        block = (Block)blockObj.GetComponent("Block");
                        block.setCoord(x, y, position.x, position.y);
                        block.isPath = false;
                        block.blockType = IMPASSABLE;
                    }
                    else
                    {
                        blockObj = (GameObject)Instantiate(passableBlock, position, Quaternion.identity);
                        blockObj.name = "block（" + y + ", " + x + " )";
                        block = (Block)blockObj.GetComponent("Block");
                        block.setCoord(x, y, position.x, position.y);
                        block.isPath = true;
                        block.blockType = PASSABLE;
                    }
                }
                else
                {
                    blockObj = (GameObject)Instantiate(passableBlock, position, Quaternion.identity);
                    blockObj.name = "block（" + y + ", " + x + " )";
                    block = (Block)blockObj.GetComponent("Block");
                    block.setCoord(x, y, position.x, position.y);
                    block.isPath = true;
                    block.blockType = PASSABLE;
                }

                //add to block list
                blocklist[y, x] = block;

                //modify signs
                y++;
            }
            y = 0;
            x++;
        }
        return blocklist;
    }
}
