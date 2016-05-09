using UnityEngine;
using System.Collections;

public class BlockCreater : MonoBehaviour {
    public GameObject singleBlock;
    public GameObject background;

    BoxCollider backGround;
    float bkWidth;
    float bkLength;

    BoxCollider block;
    float blockWidth;
    float blockLength;

    public static float length;
    public static float width;

    public Block[,] blocklist { get; set; }
    helperMethods helper = new helperMethods();
    // Use this for initialization
    void Start () {
        getInfo();
        blocklist = createMap();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void createRange()
    {

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

    Block[,] createMap()
    {
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
                    }
                    else
                    {
                        Sprite pathBlock = Sprite.Create(path, spr.sprite.textureRect, new Vector2(0.5f, 0.5f));//注意居中显示采用0.5f值 
                        spr.sprite = pathBlock;
                        block.isPath = true;
                    }
                }
                else
                {
                    Sprite pathBlock = Sprite.Create(path, spr.sprite.textureRect, new Vector2(0.5f, 0.5f));//注意居中显示采用0.5f值 
                    spr.sprite = pathBlock;
                    block.isPath = true;
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
