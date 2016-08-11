using UnityEngine;
using System.Collections;

public class PathFinder : MonoBehaviour
{
    public GameObject player;
    public GameObject MainCamera;
    Block[,] blocklist;
    Vector3[] movingPath = null;
    Vector3 targetPos = Vector3.back;

    // Use this for initialization
    void Start()
    {
        getBlockList();
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    void move()
    {
        if (movingPath != null && movingPath.Length > 0)
        {
            //print(movingPath[movingPath.Length - 1].x + " " + movingPath[movingPath.Length - 1].y + " " + movingPath[movingPath.Length - 1].z);
            //保存最终目标坐标，在计算新路径时强制更新坐标
            targetPos = movingPath[movingPath.Length - 1];
            //按路径移动，有0.00125*格数的固定误差，原因不明
            iTween.MoveTo(player, iTween.Hash("position", new Vector3(movingPath[movingPath.Length - 1].x, movingPath[movingPath.Length - 1].y, 0), "path", movingPath, "speed", 10f));
            //清空移动路径
            movingPath = null;
        }
        else
            //获得新路径
            setNextPath();
    }

    //find path，A*算法
    public ArrayList findPath(Point start, Point end)
    {
        print("Start Point: (" + start.X + ", " + start.Y + ") || End Point: ("+ end.X + ", " +end.Y+")");
        //坐标实际表示[Y轴,X轴]
        ArrayList myPath = new ArrayList();

        int[,] target = convertBlockInfoToIntArray(blocklist);

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

    //给实际的二维坐标，得到矩阵坐标
    Point findPointInMap(float x, float y)
    {
        Point result = null;
        //遍历二维坐标矩阵
        for (int i = 0; i < blocklist.GetLength(1); i++)//x
        {
            for (int j = 0; j < blocklist.GetLength(0); j++)//y
            {
                if (blocklist[j, i].coord.X == x && blocklist[j, i].coord.Y == y)
                    //通行路径矩阵在外围加了一圈1配合寻路算法，所以坐标矩阵对应时需要+1
                    result = new Point(j + 1, i + 1);
            }
        }
        return result;
    }

    void getBlockList()
    {
        BlockCreater bc = (BlockCreater)this.gameObject.GetComponent("BlockCreater");
        blocklist = bc.blocklist;
    }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
    //转化通行路径为实际坐标路径
    void setNextPath()
    {
        Vector3[] next = null;

        if (Input.GetMouseButtonDown(0))
        {
            //update block list
            getBlockList();
            printMapArray();

            //因为有不明原因误差，所以强制修改最终目标坐标适应寻路
            if (targetPos.z != -1)
                player.transform.position = targetPos;

            Ray ray = MainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //从屏幕点击处向内发射射线，若射到可通行的path，则开始寻路
            if (Physics.Raycast(ray, out hit))
            {

                print("Hit Object: " + hit.collider.name);

                Block block = (Block)hit.collider.GetComponent("Block");
                if (block != null && block.isPath)
                {
                    //寻路，从人物坐标到点击处坐标的可用路径
                    ArrayList paths = findPath(findPointInMap(player.transform.position.x, player.transform.position.y), findPointInMap(hit.collider.transform.position.x, hit.collider.transform.position.y));
                    //从通行矩阵坐标转换成实际矩阵坐标并保存为三维坐标array
                    Block currBlock;
                    next = new Vector3[paths.Count];
                    Point posInfo;
                    int pos = 0;
                    for (int i = paths.Count - 1; i > -1; i--)
                    {
                        posInfo = (Point)paths[i];
                        currBlock = blocklist[posInfo.X - 1, posInfo.Y - 1];
                        next[pos] = new Vector3(currBlock.coord.X, currBlock.coord.Y, 0);
                        pos++;
                    }
                }
            }
            movingPath = next;
        }
    }

    //将实际三维坐标矩阵转化成可通过路径矩阵，得到的矩阵比实际坐标矩阵XY值大1，因为加了一圈空气墙
    int[,] convertBlockInfoToIntArray(Block[,] blocklist)
    {
        int col = blocklist.GetLength(1);
        int row = blocklist.GetLength(0);

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
                if (blocklist[i - 1, j - 1].isPath)
                    result[i, j] = 0;
                else
                    result[i, j] = 1;
            }
        }

        return result;
    }

    //打出可通过路径矩阵
    void printMapArray()
    {
        int[,] target = convertBlockInfoToIntArray(blocklist);
        string output1 = "Print Integer Map(Click to see the whole map):\n";
        for (int i = 0; i < target.GetLength(0); i++)
        {
            for (int j = 0; j < target.GetLength(1); j++)
            {
                output1 += target[i, j] + " ";
            }
            output1 += "\n";
        }
        print("Integer Map size:\tWidth: " + target.GetLength(0) + "\tLength: " + target.GetLength(1));
        print(output1);
    }
}
