using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using UnityEngine;

public class LevelGenerate : BaseObject
{
    [Header("0: tutorial | 1: starter | 2: normal | 3: long | 4: hard")]
    [SerializeField] private int folder = 1;
    [SerializeField] private int TARGET_R;
    [SerializeField] private int TARGET_C;
    [SerializeField] private int SPAWN_R;
    [SerializeField] private int SPAWN_C;
    [SerializeField] private GameObject targetClickPrefab;
    [SerializeField] private GameObject shooterClickPrefab;
    [SerializeField] private List<int> targetCount;
    [SerializeField] private List<int> shooterCount;
    [SerializeField] private List<int> deltaCount;
    private bool inTarget = true;
    public static LevelGenerate INSTANCE;
    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.RightShift))
            generate();
        if (Input.GetKeyDown(KeyCode.Tab))
            inTarget = !inTarget;

        if (inTarget)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                setTargetCount(currentR, currentC, 1, currentColor);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                setTargetCount(currentR, currentC, 2, currentColor);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                setTargetCount(currentR, currentC, 3, currentColor);
            if (Input.GetKeyDown(KeyCode.Alpha0))
                setTargetCount(currentR, currentC, 0, currentColor);
            if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Z))
                setTargetColor(currentR, currentC, 0);
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.X))
                setTargetColor(currentR, currentC, 1);
            if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.C))
                setTargetColor(currentR, currentC, 2);
            if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.V))
                setTargetColor(currentR, currentC, 3);
            if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.B))
                setTargetColor(currentR, currentC, 4);
            if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.N))
                setTargetColor(currentR, currentC, 5);
            if (Input.GetKeyDown(KeyCode.Keypad6))
                setTargetColor(currentR, currentC, 6);
            if (Input.GetKeyDown(KeyCode.Keypad7))
                setTargetColor(currentR, currentC, 7);
            if (Input.GetKeyDown(KeyCode.Keypad8))
                setTargetColor(currentR, currentC, 8);
            if (Input.GetKeyDown(KeyCode.Delete))
                Board.INSTANCE.deleteLeftRight(currentSpawnR, currentSpawnC);
            if (Input.GetKeyDown(KeyCode.UpArrow))
                currentR = Mathf.Min(currentR + 1, TARGET_R - 1);
            if (Input.GetKeyDown(KeyCode.DownArrow))
                currentR = Mathf.Max(currentR - 1, 0);
            if (Input.GetKeyDown(KeyCode.RightArrow))
                currentC = Mathf.Min(currentC + 1, TARGET_C - 1);
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                currentC = Mathf.Max(currentC - 1, 0);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
                setShooterCount(currentSpawnR, currentSpawnC, 0, currentSpawnColor);
            if (Input.GetKeyDown(KeyCode.Alpha1))
                setShooterCount(currentSpawnR, currentSpawnC, 10, currentSpawnColor);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                setShooterCount(currentSpawnR, currentSpawnC, 20, currentSpawnColor);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                setShooterCount(currentSpawnR, currentSpawnC, 30, currentSpawnColor);
            if (Input.GetKeyDown(KeyCode.Alpha4))
                setShooterCount(currentSpawnR, currentSpawnC, 40, currentSpawnColor);
            if (Input.GetKeyDown(KeyCode.Alpha5))
                setShooterCount(currentSpawnR, currentSpawnC, 50, currentSpawnColor);
            if (Input.GetKeyDown(KeyCode.Alpha6))
                setShooterCount(currentSpawnR, currentSpawnC, 60, currentSpawnColor);
            if (Input.GetKeyDown(KeyCode.Alpha7))
                setShooterCount(currentSpawnR, currentSpawnC, 70, currentSpawnColor);
            if (Input.GetKeyDown(KeyCode.Alpha8))
                setShooterCount(currentSpawnR, currentSpawnC, 80, currentSpawnColor);
            if (Input.GetKeyDown(KeyCode.Alpha9))
                setShooterCount(currentSpawnR, currentSpawnC, 90, currentSpawnColor);

            if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Z))
                setShooterColor(currentSpawnR, currentSpawnC, 0);
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.X))
                setShooterColor(currentSpawnR, currentSpawnC, 1);
            if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.C))
                setShooterColor(currentSpawnR, currentSpawnC, 2);
            if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.V))
                setShooterColor(currentSpawnR, currentSpawnC, 3);
            if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.B))
                setShooterColor(currentSpawnR, currentSpawnC, 4);
            if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.N))
                setShooterColor(currentSpawnR, currentSpawnC, 5);
            if (Input.GetKeyDown(KeyCode.Keypad6))
                setShooterColor(currentSpawnR, currentSpawnC, 6);
            if (Input.GetKeyDown(KeyCode.Keypad7))
                setShooterColor(currentSpawnR, currentSpawnC, 7);
            if (Input.GetKeyDown(KeyCode.Keypad8))
                setShooterColor(currentSpawnR, currentSpawnC, 8);
            if (Input.GetKeyDown(KeyCode.Keypad9))
                setShooterColor(currentSpawnR, currentSpawnC, 9);

            if (Input.GetKeyDown(KeyCode.F1))
                setShooterType(currentSpawnR, currentSpawnC, Shooter.TYPE_RECT);
            if (Input.GetKeyDown(KeyCode.F2))
                setShooterType(currentSpawnR, currentSpawnC, Shooter.TYPE_TRIANGLE);
            if (Input.GetKeyDown(KeyCode.F3))
                setShooterType(currentSpawnR, currentSpawnC, Shooter.TYPE_CIRCLE);

            if (Input.GetKeyDown(KeyCode.DownArrow))
                currentSpawnR = Mathf.Min(currentSpawnR + 1, SPAWN_R - 1);
            if (Input.GetKeyDown(KeyCode.UpArrow))
                currentSpawnR = Mathf.Max(currentSpawnR - 1, 0);
            if (Input.GetKeyDown(KeyCode.RightArrow))
                currentSpawnC = Mathf.Min(currentSpawnC + 1, SPAWN_C - 1);
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                currentSpawnC = Mathf.Max(currentSpawnC - 1, 0);
        }


        if (Input.GetKey(KeyCode.W))
            Camera.main.transform.position += new Vector3(0, 0, 0.2f);
        if (Input.GetKey(KeyCode.S))
            Camera.main.transform.position += new Vector3(0, 0, -0.2f);
        if (Input.GetKeyDown(KeyCode.Space))
            saveData();
    }
    private void generate()
    {
        deltaCount = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 };
        targetCount = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 };
        shooterCount = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 };
        Board.INSTANCE.initBoard(SPAWN_R, SPAWN_C, TARGET_R, TARGET_C);
        for (int r = 0; r < TARGET_R; r++)
        {
            for (int c = 0; c < TARGET_C; c++)
            {
                LevelTargetClickable click = Instantiate(targetClickPrefab, transform.Find("LevelGen")).GetComponent<LevelTargetClickable>();
                click.r = r;
                click.c = c;
                click.transform.position = Board.INSTANCE.getTargetPos(r, c);
            }
        }
        for (int r = 0; r < SPAWN_R; r++)
        {
            for (int c = 0; c < SPAWN_C; c++)
            {
                LevelShooterClickable click = Instantiate(shooterClickPrefab, transform.Find("LevelGen")).GetComponent<LevelShooterClickable>();
                click.r = r;
                click.c = c;
                click.transform.position = Board.INSTANCE.getSpawnPos(r, c);
            }
        }
    }
    int currentR = 0;
    int currentC = 0;
    int currentColor = 0;
    public void onChooseTarget(int r, int c)
    {
        currentR = r;
        currentC = c;
    }
    private void setTargetCount(int r, int c, int h, int startColor)
    {
        Board.INSTANCE.setTargetCount(r, c, h, startColor);
        targetCount = Board.INSTANCE.computeTargetColor();
        computeDeltaCount();
    }
    private void setTargetColor(int r, int c, int color)
    {
        currentColor = color;
        Board.INSTANCE.setTargetColor(r, c, color);
        targetCount = Board.INSTANCE.computeTargetColor();
        computeDeltaCount();
    }
    private void computeDeltaCount()
    {
        for (int i = 0; i < deltaCount.Count; i++)
        {
            deltaCount[i] = targetCount[i] - shooterCount[i];
        }
    }

    int currentSpawnR = 0;
    int currentSpawnC = 0;
    int currentSpawnColor = 0;
    public void setShooterCount(int r, int c, int count, int startColor = 0)
    {
        Board.INSTANCE.setShooterCount(r, c, count, startColor);
        shooterCount = Board.INSTANCE.computeShooterColor();
        computeDeltaCount();
    }
    public void setShooterType(int r, int c, int type)
    {
        Board.INSTANCE.setShooterType(r, c, type);
        shooterCount = Board.INSTANCE.computeShooterColor();
        computeDeltaCount();
    }
    public void setShooterColor(int r, int c, int color)
    {
        Board.INSTANCE.setShooterColor(r, c, color);
        shooterCount = Board.INSTANCE.computeShooterColor();
        currentSpawnColor = color;
        computeDeltaCount();
    }
    public void onChooseShooter(int r, int c)
    {
        currentSpawnR = r;
        currentSpawnC = c;
    }
    public void saveData()
    {
        computeDeltaCount();
        for (int i = 0; i < deltaCount.Count; i++)
        {
            if (deltaCount[i] != 0)
            {
                Debug.LogError("deltaCount");
                return;
            }
        }

        JSONObject data = new JSONObject();
        data.Add("targetC", TARGET_C);
        data.Add("spawnC", SPAWN_C);
        JSONArray target = new JSONArray();
        data.Add("target", target);
        Target[,] targetMap = Board.INSTANCE.getTargetMap();
        for (int r = 0; r < TARGET_R; r++)
        {
            bool has = false;
            for (int c = 0; c < TARGET_C; c++)
            {
                if (targetMap[r, c])
                    has = true;
            }
            if (!has)
                break;
            for (int c = 0; c < TARGET_C; c++)
            {
                string text = "";
                Target temp = targetMap[r, c];
                while (temp)
                {
                    text += temp.getColor();
                    temp = temp.getTop();
                    if (temp) text += ",";
                }
                target.Add(text);
            }
        }

        JSONArray spawn = new JSONArray();
        data.Add("spawn", spawn);
        Shooter[,] spawnMap = Board.INSTANCE.getShooterMap();
        for (int r = 0; r < SPAWN_R; r++)
        {
            bool has = false;
            for (int c = 0; c < SPAWN_C; c++)
            {
                if (spawnMap[r, c])
                    has = true;
            }
            if (!has) break;
            for (int c = 0; c < SPAWN_C; c++)
            {
                JSONObject shooter = new JSONObject();
                spawn.Add(shooter);
                if (spawnMap[r, c])
                {
                    string textLeft = "";
                    string textRight = "";
                    if (spawnMap[r, c].getLeft()) textLeft = "" + spawnMap[r, c].getLeft().getR() + "," + spawnMap[r, c].getLeft().getC();
                    if (spawnMap[r, c].getRight()) textRight = "" + spawnMap[r, c].getRight().getR() + "," + spawnMap[r, c].getRight().getC();
                    shooter.Add("left", textLeft);
                    shooter.Add("right", textRight);
                    shooter.Add("color", spawnMap[r, c].getColor());
                    shooter.Add("count", spawnMap[r, c].getBulletCount());
                    shooter.Add("type", spawnMap[r, c].getType());
                }
                else
                {
                    shooter.Add("color", 0);
                    shooter.Add("count", 0);
                    shooter.Add("type", 0);
                }
            }
        }
        DateTime dt = DateTime.Now;
        string time = "" + dt.Year + "" + dt.Month + "" + dt.Day + "." + dt.Hour + "" + dt.Minute + "" + dt.Second + "." + dt.Millisecond;
        string path = "Assets/Resources/LevelData/Raw/Group" + folder + "/" + time + ".txt";
        //string path = "Assets/Resources/LevelData/Raw/Group" + folder + "/" + "level1.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(data.ToString());
        writer.Close();

        Debug.Log("saveData " + data.ToString());
    }
}
