using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotScript : MonoBehaviour
{
    
    /// <summary>
    /// текстовый файл с планом игры бота
    /// </summary>
    public TextAsset textFile;

    /// <summary>
    /// план игры бота
    /// </summary>
    private int[][] gamePlan = new int[0][];

    /// <summary>
    /// текущая стадия выполнения плана
    /// </summary>
    private int currentStage = 0;

    void Start()
    {
        if(GameInfo.GameType.OnePlayerOneDevice == GameInfo.CurrentGameType)
            LoadGamePlan();
    }

    void Update()
    {
        if (currentStage >= gamePlan.Length) return;
        
        if (UnityEngine.Random.value > 1 / 3f) return;
        
        GameObject tmp = null;
        List<Collider> nodes = new List<Collider>();


        switch (gamePlan[currentStage][0])
        {
            case 1:
                tmp = GameInfo.ArcherDull;
                int x = gamePlan[currentStage][1];
                int y = gamePlan[currentStage][2];
                nodes.Add(BlueBaseScript.RedNodes[x, y].GetComponent<Collider>());

                if(BlueBaseScript.PlaceOnList(tmp, nodes, false))
                currentStage++;
                break;
            case 2:
                tmp = GameInfo.SpearmanDull;
                int X = gamePlan[currentStage][1];
                int Y = gamePlan[currentStage][2];
                nodes.Add(BlueBaseScript.RedNodes[X, Y].GetComponent<Collider>());

                if(BlueBaseScript.PlaceOnList(tmp, nodes, false))
                currentStage++;
                break;
            case 3:
                tmp = GameInfo.CatapultDull;
                for (int i = 0; i < 4; i++)
                {
                    int x1 = gamePlan[currentStage][i * 2 + 1];
                    int y1 = gamePlan[currentStage][i * 2 + 2];
                    nodes.Add(BlueBaseScript.RedNodes[x1, y1].GetComponent<Collider>());
                }

                if(BlueBaseScript.PlaceOnList(tmp, nodes, false))
                currentStage++;
                break;
            
        }
    }
    
    
    /// <summary>
    /// Загружает план игры из текстового файла
    /// </summary>
    private void LoadGamePlan()
    {
        string txt = textFile.text;

        string[] Lines = txt.Split(new char[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);

        gamePlan = new int[Lines.Length][];

        for (int i = 0; i < Lines.Length; i++)
        {
            string line = Lines[i];
            gamePlan[i] = Array.ConvertAll(line.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries),
                int.Parse);
        }
    }
}