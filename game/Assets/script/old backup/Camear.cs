using UnityEngine;
using System.Collections;

public class Camear : MonoBehaviour
{
    public GameObject player;
    public GameObject background;
    public GameObject singleBlock;
    Vector3[] movingPath = null;
    Vector3 targetPos = Vector3.back;



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        move();

    }



    int getMovingRange()
    {
        character info = (character)player.GetComponent("character");

        return info.attr.movingRange;
    }

    //延时用的
    IEnumerator Example()
    {
        print(Time.time);
        yield return new WaitForSeconds(30);
        print(Time.time);
    }

    void move()
    {
        if (movingPath != null)
        {
            print(movingPath[movingPath.Length - 1].x + " " + movingPath[movingPath.Length - 1].y + " " + movingPath[movingPath.Length - 1].z);
            //保存最终目标坐标，在计算新路径时强制更新坐标
            targetPos = movingPath[movingPath.Length - 1];
            //按路径移动，有0.00125*格数的固定误差，原因不明
            iTween.MoveTo(player, iTween.Hash("position", new Vector3(movingPath[movingPath.Length - 1].x, movingPath[movingPath.Length - 1].y, 0), "path", movingPath, "speed", 10f));
            //清空移动路径
            movingPath = null;
        }
        else
            //获得新路径
            coordinate();
    }

    //输入实际的三维坐标，返回可移动路径的实际二维坐标，不可移动路径返回（0，0）
    Vector2 getTargetPosition(Vector3 input)
    {
        Ray ray = GetComponent<Camera>().ScreenPointToRay(input);
        RaycastHit hit;
        //从屏幕向内发射射线，如果命中了path，则返回碰撞器X,Y坐标
        if (Physics.Raycast(ray, out hit))
        {
            Block block = (Block)hit.collider.GetComponent("Block");
            if (block != null && block.isPath)

            {
                return new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
            }
            //否则返回（0，0）
            else
                return new Vector2(0, 0);
        }
        else
            return new Vector2(0, 0);
    }
    //给实际的二维坐标，得到矩阵坐标
    Point findPointInMap(float x, float y)
    {
        Point result = null;
        Vector3[,] mapCoords = findPassableBlocks();

        printMapArray();
        //遍历二维坐标矩阵
        for (int i = 0; i < mapCoords.GetLength(1); i++)//x
        {
            for (int j = 0; j < mapCoords.GetLength(0); j++)//y
            {
                if (mapCoords[j, i].x == x && mapCoords[j, i].y == y)
                    //通行路径矩阵在外围加了一圈1配合寻路算法，所以坐标矩阵对应时需要+1
                    result = new Point(j + 1, i + 1);
            }
        }
        return result;
    }

    //转化通行路径为实际坐标路径
    void coordinate()
    {
        Vector3[] next = null;
        if (Input.GetMouseButtonDown(0))
        {
            //因为有不明原因误差，所以强制修改最终目标坐标适应寻路
            if (targetPos.z != -1)
                player.transform.position = targetPos;

            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //从屏幕点击处向内发射射线，若射到可通行的path，则开始寻路
            if (Physics.Raycast(ray, out hit))
            {

                print(hit.collider.name);

                Block block = (Block)hit.collider.GetComponent("Block");
                if (block != null && block.isPath)
                {
                    //寻路，从人物坐标到点击处坐标的可用路径
                    ArrayList paths = findPath(findPointInMap(player.transform.position.x, player.transform.position.y), findPointInMap(hit.collider.transform.position.x, hit.collider.transform.position.y));
                    //从通行矩阵坐标转换成实际矩阵坐标并保存为三维坐标array
                    Vector3[,] mapCoords = findPassableBlocks();
                    next = new Vector3[paths.Count];
                    Point posInfo;
                    int pos = 0;
                    for (int i = paths.Count - 1; i > -1; i--)
                    {
                        posInfo = (Point)paths[i];
                        next[pos] = mapCoords[posInfo.X - 1, posInfo.Y - 1];
                        pos++;
                    }
                    print(player.transform.position);
                }
            }

        }
        movingPath = next;
    }

    //绘制实际坐标矩阵，可通行记录三维坐标，不可通行记录（0，0，0）
    Vector3[,] findPassableBlocks()
    {
        //get block info
        float col = BlockCreater.length;
        float row = BlockCreater.width;

        float blockLength = singleBlock.GetComponent<BoxCollider>().size.x;
        float blockWidth = singleBlock.GetComponent<BoxCollider>().size.y;

        Vector3[,] paths = new Vector3[(int)row + 1, (int)col + 1];

        int currCol = 0;
        int currRow = convertCarry(row) - 1;

        Vector3 position;

        //traverse all blocks
        for (int i = (int)-col / 2 - 1; i < col / 2; i++)
        {
            for (int j = (int)-row / 2 - 1; j < row / 2; j++)
            {
                //强制计算每块格子的中心坐标
                position = new Vector3(i * blockLength + (float)blockLength / 2, j * blockWidth + (float)blockWidth / 2, 0);

                RaycastHit hit;
                if (Physics.Raycast(position, Vector3.back, out hit, 1))
                {
                    Block block = (Block)hit.collider.GetComponent("Block");
                    //create block array with each passable block's center position, (0,0,0) is unpassable block
                    if (block.isPath)
                        paths[currRow, currCol] = hit.collider.transform.position;
                    else
                        paths[currRow, currCol] = new Vector3(0, 0, 0);
                }
                currRow--;
            }
            currCol++;
            currRow = convertCarry(row) - 1;
        }

        return paths;
    }

    //将实际三维坐标矩阵转化成可通过路径矩阵，得到的矩阵比实际坐标矩阵XY值大1，因为加了一圈空气墙
    int[,] convertV3ToInt(Vector3[,] paths)
    {
        int col = paths.GetLength(1);
        int row = paths.GetLength(0);

        int[,] result = new int[row + 2, col + 2];

        //add air wall
        for (int m = 0; m < row + 2; m++)
        {
            result[m, 0] = 1;
            result[m, col + 1] = 1;
        }
        for (int n = 0; n < col + 2; n++)
        {
            result[0, n] = 1;
            result[row + 1, n] = 1;
        }

        //convert
        for (int i = 1; i < row + 1; i++)
        {
            for (int j = 1; j < col + 1; j++)
            {
                if (isPassable(paths[i - 1, j - 1]))
                    result[i, j] = 0;
                else
                    result[i, j] = 1;
            }
        }

        return result;
    }

    //判断是否是可通过路径
    bool isPassable(Vector3 blockPos)
    {
        return !(blockPos.x == 0 && blockPos.y == 0 && blockPos.z == 0);
    }

    //强制进位算法
    int convertCarry(float num)
    {
        if (num % (int)num > 0)
            return (int)num + 1;
        else
            return (int)num;
    }

    //find path，A*算法
    ArrayList findPath(Point start, Point end)
    {
        //坐标实际表示[Y轴,X轴]
        ArrayList myPath = new ArrayList();

        int[,] target = convertV3ToInt(findPassableBlocks());

        AStar maze = new AStar(target);

        var parent = maze.FindPath(start, end, false);

        string output = "Print path: ";
        while (parent != null)
        {
            myPath.Add(new Point(parent.X, parent.Y));
            output += "(" + parent.X + ", " + parent.Y + ")\t";
            parent = parent.ParentPoint;

        }

        print(output);

        return myPath;
    }


    //打出可通过路径矩阵
    void printMapArray()
    {
        int[,] target = convertV3ToInt(findPassableBlocks());
        string output1 = "";
        for (int i = 0; i < target.GetLength(0); i++)
        {
            for (int j = 0; j < target.GetLength(1); j++)
            {
                output1 += target[i, j] + " ";
            }
            output1 += "\n";
        }

        print(output1);
    }


}
