using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBaseScript : MonoBehaviour
{
    /// <summary>
    /// Клетки синего игрока
    /// </summary>
    public static GameObject[,] BlueNodes;
    /// <summary>
    /// Клетки красного игрока
    /// </summary>
    public static GameObject[,] RedNodes;


    /// <summary>
    /// количество клеток по OX
    /// </summary>
    public static int X = 10;
    /// <summary>
    /// количество клеток по OZ
    /// </summary>
    public static int Z = 10;
    /// <summary>
    /// расстояние между клетками
    /// </summary>
    public static float distance = 0.2f;
    /// <summary>
    /// размеры клетки
    /// </summary>
    public static float CubeSize;

    /// <summary>
    /// цвет синей клетки
    /// </summary>
    public static Color StandartBlueColor;
    /// <summary>
    /// цвет красной клетки
    /// </summary>
    public static Color StandartRedColor;
    
    /// <summary>
    /// текущая выделенная клетка
    /// </summary>
    private static GameObject selectedNode;

    /// <summary>
    /// расставляемые клетки
    /// </summary>
    public GameObject NodeGameObject;

    /// <summary>
    /// Свойство для выделенной клетки 
    /// </summary>
    public static GameObject SelectedNode
    {
        get { return selectedNode; }
        set
        {
            if (selectedNode != null)
            {
                if(selectedNode.tag.Contains("Blue"))
                selectedNode.GetComponent<Renderer>().material.color = StandartBlueColor;
                else
                    selectedNode.GetComponent<Renderer>().material.color = StandartRedColor;
            }

            value.GetComponent<Renderer>().material.color = Color.yellow;
            selectedNode = value;
            
        }
    }


    void Start()
    {
        if (NodeGameObject.tag.Contains("Blue"))
            StandartBlueColor = this.GetComponent<Renderer>().material.color;
        else
            StandartRedColor = this.GetComponent<Renderer>().material.color;
        SelectedNode = gameObject;

        CubeSize = gameObject.transform.localScale.x;

        
        
        DrawGrid();
    }


    /// <summary>
    /// Рисует сетку для юнитов
    /// </summary>
    protected void DrawGrid()
    {
        if (NodeGameObject.tag.Contains("Blue"))
        {
            
            BlueNodes = new GameObject[X, Z];
            BlueNodes[0, 0] = this.gameObject;
            
        }
        else
        {
            RedNodes = new GameObject[X, Z];
            
            RedNodes[0, 0] = this.gameObject;
        }


        for (int i = 0; i < X; i++)
        for (int j = 0; j < Z; j++)
        {
            if (i == 0 && j == 0) continue;

            Vector3 target = gameObject.transform.position;
            target.x += (CubeSize + distance) * (i);
            target.z += (CubeSize + distance) * (j);

            if (NodeGameObject.tag.Contains("Blue"))
                BlueNodes[i, j] = Instantiate(NodeGameObject, target, new Quaternion(0, 0, 0, 0));
            else
                RedNodes[i, j] = Instantiate(NodeGameObject, target, new Quaternion(0, 0, 0, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
    }


    /// <summary>
    /// Помещает в выделенную клетку юнита
    /// </summary>
    /// <param name="tmp">Помещаемый юнит</param>
    public static void PlaceOnTargetNode(GameObject tmp)
    {
        Vector3 target = SelectedNode.transform.position;
        target.y += SelectedNode.transform.localScale.y / 2;

        BlueNodeScript node = (SelectedNode.GetComponent("BlueNodeScript") as BlueNodeScript);
        if (!node.IsPressed)
        {
            if (node.tag.Contains("Blue"))
            {
                Instantiate(tmp, target, new Quaternion(0, 0, 0, 0)).tag = "BlueDull";

                GameInfo.BluePlayer.Resources.AmountOfGold -=
                    tmp.GetComponent<DullScript>().gObject.GetComponent<UnitScript>().UnitCost;
            }
            else
            {
                if (GameInfo.CurrentGameType != GameInfo.GameType.TwoPlayerOneDevice) return;
                Instantiate(tmp, target, new Quaternion(0, -1, 0, 0)).tag = "RedDull";
                GameInfo.RedPlayer.Resources.AmountOfGold -=
                    tmp.GetComponent<DullScript>().gObject.GetComponent<UnitScript>().UnitCost;
            }

            node.IsPressed = true;
        }
    }

    /// <summary>
    /// Разместить юнита по списку клеток
    /// </summary>
    /// <param name="dull">размещаемый юнит</param>
    /// <param name="nodes">занятые клетки</param>
    public static bool PlaceOnList(GameObject dull, List<Collider> nodes,  bool PlacedByPlayer = true)
    {
        bool isFree = true;


        Vector3 destVector = new Vector3(0, 0, 0);

        Color defaultColor = StandartRedColor;
        if (nodes[0].tag.Contains("Blue"))
             defaultColor = StandartBlueColor;
        
        
        foreach (var node in nodes)
        {
            node.gameObject.GetComponent<Renderer>().material.color = defaultColor;
            if (node.GetComponent<BlueNodeScript>().IsPressed)
            {
                isFree = false;
            }

            destVector.x += node.transform.position.x;
            destVector.y += node.transform.position.y;
            destVector.z += node.transform.position.z;
        }

        destVector.x /= nodes.Count;
        destVector.y /= nodes.Count;
        destVector.z /= nodes.Count;

        destVector.y += SelectedNode.transform.localScale.y / 2;

        if (isFree)
        {
            if (nodes[0].tag.Contains("Red") && GameInfo.RedPlayer.Resources.AmountOfGold >=
                dull.GetComponent<DullScript>().gObject.GetComponent<UnitScript>().UnitCost)
            {
                if (GameInfo.CurrentGameType != GameInfo.GameType.TwoPlayerOneDevice && PlacedByPlayer) return false;


                GameInfo.RedPlayer.Resources.AmountOfGold -=
                    dull.GetComponent<DullScript>().gObject.GetComponent<UnitScript>().UnitCost;
                GameObject tmp = Instantiate(dull, destVector, Quaternion.identity);
                tmp.tag = "RedDull";
                tmp.transform.Rotate(0, 180, 0, Space.Self);


                foreach (var node in nodes)
                    node.GetComponent<BlueNodeScript>().IsPressed = true;
                return true;
            }

            if (nodes[0].tag.Contains("Blue") && GameInfo.BluePlayer.Resources.AmountOfGold >=
                dull.GetComponent<DullScript>().gObject.GetComponent<UnitScript>().UnitCost)
            {
                GameInfo.BluePlayer.Resources.AmountOfGold -=
                    dull.GetComponent<DullScript>().gObject.GetComponent<UnitScript>().UnitCost;
                GameObject tmp = Instantiate(dull, destVector, Quaternion.identity);
                tmp.tag = "BlueDull";

                foreach (var node in nodes)
                    node.GetComponent<BlueNodeScript>().IsPressed = true;
                return true;
            }


            return false;
        }

        return false;
    }


    /// <summary>
    /// Помещает в выделенную клетку юнита
    /// </summary>
    /// <param name="tmp"></param>
    public void PlaceUnitButtonClick(GameObject tmp)
    {
        PlaceOnTargetNode(tmp);
    }
}