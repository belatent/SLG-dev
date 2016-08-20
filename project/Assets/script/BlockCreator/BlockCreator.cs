using UnityEngine;
using System.Collections;

public class BlockCreator : MonoBehaviour {
    public GameObject singleBlock;
    public GameObject movingBlock;
    public GameObject background;

    BoxCollider backGround;
    float bkWidth;
    float bkLength;

    BoxCollider block;
    float blockWidth;
    float blockLength;

    public static float length;
    public static float width;

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
            if (block.coord.X == position.x && block.coord.Y == position.y)
                result = block;
        }
        return result;
    }

    bool isBlockAccessable(Block playerBlock, Block targetBlock)
    {

        Point start = new Point(playerBlock.coord.y + 1, playerBlock.coord.x + 1);
        Point end = new Point(targetBlock.coord.y + 1, targetBlock.coord.x + 1);
        PathFinder pf = (PathFinder)this.gameObject.GetComponent("PathFinder");
       
        if (pf.findPath(new Point(start.X,start.Y), new Point(end.X,end.Y)).Count > 0)
        {
            print(true+"\n");
            return true;
        }
        else
        {
            print(false + "\n");
            return false;
        }
    }
            

    GameObject createRangeBlock(GameObject player, Vector3 position)
    {
        float x;
        float y;
        Block movingBlockAtrb;

        if (isBlockAccessable(searchBlockByPostion(player.transform.position), searchBlockByPostion(position)))
        {
            movingBlock = (GameObject)Instantiate(movingBlock, position, Quaternion.identity);
            x = ((float)position.x - (float)player.transform.position.x) / (float)1.28;
            y = ((float)position.y - (float)player.transform.position.y) / (float)1.28;
            movingBlock.name = "moving block（" + y + ", " + x + " )";
            movingBlockAtrb = (Block)movingBlock.GetComponent("Block");
            movingBlockAtrb.blockType = MOVING_RANGE;
            return movingBlock;
        }
        else
            return null;
    }

    public void createRange(GameObject player)
    {
        Block blockCreated;
        Block playerBlock = searchBlockByPostion(player.transform.position);
        Vector3 position;
        Character info =  (Character)player.GetComponent("Character");
        int x;
        int y;

        if (info.attr.movingRange > 0) {
            for (x = 1; x <= info.attr.movingRange; x++)
            {
                for (y = 1; y <= info.attr.movingRange; y++)
                {
                    if (x + y <= info.attr.movingRange)
                    {
                        //第一象限
                        position = new Vector3(player.transform.position.x + x * blockLength, player.transform.position.y + y * blockWidth, 0);
                        createRangeBlock(player, position);
                        //第二象限
                        position = new Vector3(player.transform.position.x - x * blockLength, player.transform.position.y + y * blockWidth, 0);
                        createRangeBlock(player, position);
                        //第三象限
                        position = new Vector3(player.transform.position.x - x * blockLength, player.transform.position.y - y * blockWidth, 0);
                        createRangeBlock(player, position);
                        //第四象限
                        position = new Vector3(player.transform.position.x + x * blockLength, player.transform.position.y - y * blockWidth, 0);
                        createRangeBlock(player, position);
                    }
                    if (x + y <= info.attr.movingRange+2)
                    {
                        //上下左右
                        if (x == 1)
                        {
                            //上
                            position = new Vector3(player.transform.position.x, player.transform.position.y + y * blockWidth, 0);
                            createRangeBlock(player, position);
                            //下
                            position = new Vector3(player.transform.position.x, player.transform.position.y - y * blockWidth, 0);
                            createRangeBlock(player, position);
                        }
                        if (y == 1)
                        {
                            //左
                            position = new Vector3(player.transform.position.x - x * blockLength, player.transform.position.y, 0);
                            createRangeBlock(player, position);
                            //右
                            position = new Vector3(player.transform.position.x + x * blockLength, player.transform.position.y, 0);
                            createRangeBlock(player, position);
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
        bkLength = backGround.size.x;
        bkWidth = backGround.size.y;
        //get block size
        block = singleBlock.GetComponent<BoxCollider>();
        blockLength = block.size.x;
        blockWidth = block.size.y;

        length = bkLength / blockLength;
        width = bkWidth / blockWidth;
        print("Block Map size:\tWidth: "+helper.convertFloattoInt(width) + "\tLength: " + helper.convertFloattoInt(length));
    }

    public Block[,] createMap()
    {
        getInfo();

        Block[,] blocklist = new Block[helper.convertFloattoInt(width),helper.convertFloattoInt(length)];

        Block block;
        Vector3 position;
        int x = 0;
        int y = 0;
        for (int i = (int)-length / 2 - 1; i < length / 2; i++)
        {
            for (int j = (int)width / 2 ; j > -width / 2 - 1; j--)
            {
                //set coordinate
                position = new Vector3(i * blockLength + (float)blockLength / 2, j * blockWidth + (float)blockWidth / 2, 0);
                
                //create block
                singleBlock = (GameObject)Instantiate(singleBlock, position, Quaternion.identity);
                singleBlock.name = "block（" + y + ", " + x + " )";
                block = (Block)singleBlock.GetComponent("Block");
                block.setCoord(x, y, position.x, position.y);

                Texture2D notPath = (Texture2D)Resources.Load("pict/charA");//更换图片
                Texture2D path = (Texture2D)Resources.Load("pict/charB");//更换图片  
                SpriteRenderer spr = singleBlock.GetComponent<SpriteRenderer>();


                //check whether passable by ray
                RaycastHit hit;
                if (Physics.Raycast(position, Vector3.forward, out hit, 1))
                {
                    if (hit.collider.name.Equals("Objects"))
                    {
                        Sprite notPathBlock = Sprite.Create(notPath, spr.sprite.textureRect, new Vector2(0.5f, 0.5f));//注意居中显示采用0.5f值 
                        spr.sprite = notPathBlock;
                        block.isPath = false;
                        block.blockType = IMPASSABLE;
                    }
                    else
                    {
                        Sprite pathBlock = Sprite.Create(path, spr.sprite.textureRect, new Vector2(0.5f, 0.5f));//注意居中显示采用0.5f值 
                        spr.sprite = pathBlock;
                        block.isPath = true;
                        block.blockType = PASSABLE;
                    }
                }
                else
                {
                    Sprite pathBlock = Sprite.Create(path, spr.sprite.textureRect, new Vector2(0.5f, 0.5f));//注意居中显示采用0.5f值 
                    spr.sprite = pathBlock;
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
