using UnityEngine;
using System.Collections;

public class BlockCreator : MonoBehaviour {
    public GameObject impassableBlock;
    public GameObject passableBlock;
    public GameObject movingBlock;
    public GameObject background;

    BoxCollider backGround;
    int bkWidth;
    int bkLength;

    int blockWidth;
    int blockLength;

    const string IMPASSABLE = "impassable";
    const string PASSABLE = "passable";
    const string MOVING_RANGE = "moving range";
    const string ATK_RANGE = "attack range";
    const string HEALING_RANGE = "healing range";
    const string SELECT_RANGE = "select range";

    public Block[,] blocklist { get; set; }
    HelperMethods helper = new HelperMethods();
    // Use this for initialization
    void Start () {
        
        //blocklist = createMap();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    Block searchBlockByPostion(Vector3 position)
    {

        Block result = null;
        foreach (Block block in blocklist)
        {
            if (block.transform.position == position)
            {
                result = block;
            }
                
        }
        return result;
    }

    bool isBlockAccessable(int range, Block playerBlock, Block targetBlock)
    {
        print(playerBlock.transform.position+" "+targetBlock.transform.position);
        Point start = new Point(playerBlock.coord.y+1, playerBlock.coord.x+1);
        Point end = new Point(targetBlock.coord.y+1, targetBlock.coord.x+1);
        PathFinder pf = (PathFinder)this.gameObject.GetComponent("PathFinder");
        ArrayList path = pf.findPath(new Point(start.X, start.Y), new Point(end.X, end.Y));

        if (path.Count > 0 && path.Count <= range+1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
            

    GameObject createRangeBlock(int range, GameObject player, Vector3 position)
    {
        float x;
        float y;
        Block movingBlockAtrb;
        print("crb" + position);
        Block playerBlock;
        Block targetBlock;

        playerBlock = searchBlockByPostion(player.transform.position);
        targetBlock = searchBlockByPostion(position);

        if(targetBlock != null)
        {
            if (isBlockAccessable(range, playerBlock, targetBlock))
            {
                movingBlock = (GameObject)Instantiate(movingBlock, position, Quaternion.identity);
                x = position.x - player.transform.position.x;
                y = position.y - player.transform.position.y;
                movingBlock.name = "moving block（" + y + ", " + x + " )";
                movingBlockAtrb = (Block)movingBlock.GetComponent("Block");
                movingBlockAtrb.blockType = MOVING_RANGE;
                return movingBlock;
            }
            else
                return null;
        }
        else
            return null;
    }

    public void createRange(GameObject player)
    {
        Block playerBlock = searchBlockByPostion(player.transform.position);
        Vector3 position;
        Character info =  (Character)player.GetComponent("Character");
        int movRange = info.attr.movingRange;
        int x;
        int y;

        if (movRange > 0) {
            for (x = 1; x <= info.attr.movingRange; x++)
            {
                for (y = 1; y <= info.attr.movingRange; y++)
                {
                    if (x + y <= info.attr.movingRange)
                    {
                        //第一象限
                        position = new Vector3(player.transform.position.x + x * blockLength, player.transform.position.y + y * blockWidth, 0);
                        createRangeBlock(movRange, player, position);
                        //第二象限
                        position = new Vector3(player.transform.position.x - x * blockLength, player.transform.position.y + y * blockWidth, 0);
                        createRangeBlock(movRange, player, position);
                        //第三象限
                        position = new Vector3(player.transform.position.x - x * blockLength, player.transform.position.y - y * blockWidth, 0);
                        createRangeBlock(movRange, player, position);
                        //第四象限
                        position = new Vector3(player.transform.position.x + x * blockLength, player.transform.position.y - y * blockWidth, 0);
                        createRangeBlock(movRange, player, position);
                    }
                    if (x + y <= info.attr.movingRange+2)
                    {
                        //上下左右
                        if (x == 1)
                        {
                            //上
                            position = new Vector3(player.transform.position.x, player.transform.position.y + y * blockWidth, 0);
                            createRangeBlock(movRange, player, position);
                            //下
                            position = new Vector3(player.transform.position.x, player.transform.position.y - y * blockWidth, 0);
                            createRangeBlock(movRange, player, position);
                        }
                        if (y == 1)
                        {
                            //左
                            position = new Vector3(player.transform.position.x - x * blockLength, player.transform.position.y, 0);
                            createRangeBlock(movRange, player, position);
                            //右
                            position = new Vector3(player.transform.position.x + x * blockLength, player.transform.position.y, 0);
                            createRangeBlock(movRange, player, position);
                        }
                    }
                }
            }
            
        }
    }

    void getInfo()
    {
        //get background size
        backGround = background.GetComponent<BoxCollider>();
        bkLength = (int)backGround.size.x;
        bkWidth = (int)backGround.size.y;
        //get block size
        blockLength = 1;
        blockWidth = 1;
        print("Block Map size:\tWidth: "+ bkWidth + "\tLength: " + bkLength);
    }

    public Block[,] createMap()
    {
        getInfo();

        Block[,] blocklist = new Block[bkWidth,bkLength];
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
    
    public void destoryBlockByTag(string tag)
    {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag(tag))
        {
            DestroyObject(obj);
            if (tag == "moving range")
                movingBlock = (GameObject)Resources.Load("perfab/move");
        }
    }
}
