using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using SimpleJSON;
using System;

public class Board : BaseObject
{
    [SerializeField] private List<Slot> slots;
    protected Vector3[,] spawnPos;
    protected Shooter[,] spawnMap;
    protected Vector3[,] targetPos;
    protected Target[,] targetMap;

    protected int SPAWN_R = 1;
    protected int SPAWN_C = 1;
    protected int TARGET_R = 1;
    protected int TARGET_C = 1;

    [SerializeField] private List<string> listLevel;
    // prefabs
    [SerializeField] protected GameObject prefabShooter;
    [SerializeField] protected GameObject prefabBlock;
    //
    // anchors
    private float delayNextShoot = 0;
    [SerializeField] private float shootDelay;
    [SerializeField] private float spawnDistance;
    [SerializeField] private float targetDistance;
    [SerializeField] private Transform anchorTopLeft;
    [SerializeField] private Transform anchorIpadTop;
    [SerializeField] private Transform anchorIpadBottom;
    [SerializeField] private Transform anchorSpawn;
    [SerializeField] private Transform anchorSlot;
    [SerializeField] private Transform anchorTarget;
    [SerializeField] private Transform anchorExitLeft;
    [SerializeField] private Transform anchorExitRight;
    //

    public static Board INSTANCE;
    private int currentLevel = 0;
    public bool isFinished = false;
    private void Start()
    {
        Application.targetFrameRate = 120;
        INSTANCE = this;
        if (!GetComponent<LevelGenerate>())
        {
            initCamera();
            //testData();

            doSmthAfter(0.5f, () => {
                currentLevel = PlayerPrefs.GetInt("current_level", 0);
                loadLevel(currentLevel);
            });
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            PopupManager.INSTANCE.showLose(slots[0].gameObject);
        if (delayNextShoot > 0)
        {
            delayNextShoot -= Time.deltaTime;
        }
        if (delayNextShoot <= 0)
        {
            delayNextShoot = shootDelay;
            // check shoot
            checkShoot();
        }
    }
    public void initCamera()
    {
        if ((float)Screen.height / (float)Screen.width > 1.8f)
        {
            if (Camera.main.WorldToScreenPoint(anchorTopLeft.transform.position).x > 0)
            {
                while (Camera.main.WorldToScreenPoint(anchorTopLeft.transform.position).x > 0)
                {
                    Camera.main.orthographicSize -= 0.05f;
                }
            }
            else if (Camera.main.WorldToScreenPoint(anchorTopLeft.transform.position).x < 0)
            {
                while (Camera.main.WorldToScreenPoint(anchorTopLeft.transform.position).x < 0)
                {
                    Camera.main.orthographicSize += 0.05f;
                }
            }
            if (Camera.main.WorldToScreenPoint(anchorTopLeft.transform.position).y < Screen.height)
            {
                while (Camera.main.WorldToScreenPoint(anchorTopLeft.transform.position).y < Screen.height)
                {
                    Camera.main.transform.position += new Vector3(0, 0, -0.02f);
                }
            }
            else if (Camera.main.WorldToScreenPoint(anchorTopLeft.transform.position).y > Screen.height)
            {
                while (Camera.main.WorldToScreenPoint(anchorTopLeft.transform.position).y > Screen.height)
                {
                    Camera.main.transform.position += new Vector3(0, 0, 0.02f);
                }
            }
        }
        else
        {
            if (Camera.main.WorldToScreenPoint(anchorIpadTop.transform.position).y - Camera.main.WorldToScreenPoint(anchorIpadBottom.transform.position).y < Screen.height)
            {
                while (Camera.main.WorldToScreenPoint(anchorIpadTop.transform.position).y - Camera.main.WorldToScreenPoint(anchorIpadBottom.transform.position).y < Screen.height)
                {
                    Camera.main.orthographicSize -= 0.05f;
                }
            }
            else if (Camera.main.WorldToScreenPoint(anchorIpadTop.transform.position).y - Camera.main.WorldToScreenPoint(anchorIpadBottom.transform.position).y > Screen.height)
            {
                while (Camera.main.WorldToScreenPoint(anchorIpadTop.transform.position).y - Camera.main.WorldToScreenPoint(anchorIpadBottom.transform.position).y > Screen.height)
                {
                    Camera.main.orthographicSize += 0.05f;
                }
            }
            if (Camera.main.WorldToScreenPoint(anchorIpadTop.transform.position).y < Screen.height)
            {
                while (Camera.main.WorldToScreenPoint(anchorIpadTop.transform.position).y < Screen.height)
                {
                    Camera.main.transform.position += new Vector3(0, 0, -0.02f);
                }
            }
            else if (Camera.main.WorldToScreenPoint(anchorIpadTop.transform.position).y > Screen.height)
            {
                while (Camera.main.WorldToScreenPoint(anchorIpadTop.transform.position).y > Screen.height)
                {
                    Camera.main.transform.position += new Vector3(0, 0, 0.02f);
                }
            }
        }
    }
    public void clearBoard()
    {
        clearAllChildren(transform.Find("Targets"));
        clearAllChildren(transform.Find("Shooters"));
        clearAllChildren(transform.Find("Bullets"));
    }
    public void initBoard(int spawnR, int spawnC, int targetR, int targetC)
    {
        clearBoard();
        SPAWN_R = spawnR;
        SPAWN_C = spawnC;
        TARGET_R = targetR;
        TARGET_C = targetC;
        spawnPos = new Vector3[SPAWN_R, SPAWN_C];
        spawnMap = new Shooter[SPAWN_R, SPAWN_C];
        targetPos = new Vector3[TARGET_R, TARGET_C];
        targetMap = new Target[TARGET_R, TARGET_C];

        Vector3 startSpawn = anchorSpawn.transform.position - new Vector3((SPAWN_C - 1) * spawnDistance / 2, 0, 0);
        for (int r = 0; r < SPAWN_R; r++)
        {
            for (int c = 0; c < SPAWN_C; c++)
            {
                spawnPos[r, c] = startSpawn + new Vector3(c * spawnDistance, 0, -r * spawnDistance);
            }
        }
        Vector3 startTarget = anchorTarget.transform.position - new Vector3((TARGET_C - 1) * targetDistance / 2, 0, 0);
        for (int r = 0; r < TARGET_R; r++)
        {
            for (int c = 0; c < TARGET_C; c++)
            {
                targetPos[r, c] = startTarget + new Vector3(c * targetDistance, 0, r * targetDistance);
            }
        }
    }
    public void testData()
    {
        slots[slots.Count - 1].setState(Slot.STATE_ADS);

        SPAWN_R = 5;
        SPAWN_C = 4;
        TARGET_R = 20;
        TARGET_C = 10;
        spawnPos = new Vector3[SPAWN_R, SPAWN_C];
        spawnMap = new Shooter[SPAWN_R, SPAWN_C];
        targetPos = new Vector3[TARGET_R, TARGET_C];
        targetMap = new Target[TARGET_R, TARGET_C];

        Vector3 startSpawn = anchorSpawn.transform.position - new Vector3((SPAWN_C - 1) * spawnDistance / 2, 0, 0);
        for (int r = 0; r < SPAWN_R; r++)
        {
            for (int c = 0; c < SPAWN_C; c++)
            {
                spawnPos[r, c] = startSpawn + new Vector3(c * spawnDistance, 0, -r * spawnDistance);
            }
        }
        Vector3 startTarget = anchorTarget.transform.position - new Vector3((TARGET_C - 1) * targetDistance / 2, 0, 0);
        for (int r = 0; r < TARGET_R; r++)
        {
            for (int c = 0; c < TARGET_C; c++)
            {
                targetPos[r, c] = startTarget + new Vector3(c * targetDistance, 0, r * targetDistance);
            }
        }

        // test
        for (int r = 0; r < SPAWN_R; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                Shooter shooter = generateShooter(c, 10, c == 2 || c == 0 || c == 1 ? Shooter.TYPE_CIRCLE : Shooter.TYPE_TRIANGLE);
                shooter.transform.position = spawnPos[r, c];
                shooter.setRC(r, c);
                spawnMap[r, c] = shooter;
            }
            Shooter shooter1 = generateShooter(0, 10, Shooter.TYPE_RECT);
            shooter1.transform.position = spawnPos[r, SPAWN_C - 1];
            shooter1.setRC(r, SPAWN_C - 1);
            spawnMap[r, SPAWN_C - 1] = shooter1;
        }
        for (int r = 0; r < TARGET_R; r++)
        {
            for (int c = 0; c < TARGET_C; c++)
            {
                Target target = generateTarget(2);
                target.transform.position = targetPos[r, c];
                targetMap[r, c] = target;
                target.setTop(generateTarget(3));
            }
        }
        spawnMap[0, 0].setRight(spawnMap[0, 1]); spawnMap[0, 1].setRight(spawnMap[0, 2]);
        spawnMap[1, 0].setRight(spawnMap[1, 1]); spawnMap[1, 0].doHide();
        spawnMap[2, 0].setRight(spawnMap[2, 1]);
    }
    public void loadLevel(int index)
    {
        if (index >= listLevel.Count)
            return;
        PlayerPrefs.SetInt("current_level", index);
        PlayerPrefs.Save();
        UIManager.INSTANCE.onLevelChange(index);
        currentLevel = index;
        string json = getJsonString(listLevel[index]);
        loadLevelData(json);
    }
    private int totalTarget;
    public void loadLevelData(string jsonString)
    {
        isFinished = false;
        slots[slots.Count - 1].setState(Slot.STATE_ADS);
        JSONNode json = JSONNode.Parse(jsonString);
        TARGET_C = json["targetC"];
        SPAWN_C = json["spawnC"];
        JSONArray targetArray = (JSONArray)json["target"];
        TARGET_R = Mathf.CeilToInt((float)targetArray.Count / (float) TARGET_C);
        JSONArray spawnArray = (JSONArray)json["spawn"];
        SPAWN_R = Mathf.CeilToInt((float) spawnArray.Count / (float) SPAWN_C);
        initBoard(SPAWN_R, SPAWN_C, TARGET_R, TARGET_C);

        totalTarget = 0;
        for (int r = 0; r < TARGET_R; r++)
        {
            for (int c = 0; c < TARGET_C; c++)
            {
                string targetString = targetArray[r * TARGET_C + c];
                string[] temp = targetString.Split(',');
                if (temp.Length > 0)
                {
                    Target target = generateTarget(int.Parse(temp[0]));
                    targetMap[r, c] = target;
                    target.transform.position = targetPos[r, c];
                    totalTarget++;
                    for (int i = 1; i < temp.Length; i++)
                    {
                        Target next = generateTarget(int.Parse(temp[i]));
                        target.setTop(next);
                        target = next;
                        totalTarget++;
                    }
                }
            }
        }
        UIManager.INSTANCE.onLevelProgress(0, totalTarget);
        for (int r = 0; r < SPAWN_R; r++)
        {
            for (int c = 0; c < SPAWN_C; c++)
            {
                JSONNode js = spawnArray[r * SPAWN_C + c];
                int color = js["color"];
                int count = js["count"];
                int type = js["type"];
                string textLeft = js["left"];
                string textRight = js["right"];
                Debug.Log("createShooter " + r + " " + c + " " + color + " " + count + " " + type);
                if (count > 0)
                {
                    Shooter shooter = generateShooter(color, count, type);
                    spawnMap[r, c] = shooter;
                    shooter.setRC(r, c);
                    shooter.transform.position = spawnPos[r, c];
                    if (textLeft != null && textLeft.Length > 0)
                    {
                        string[] temp = textLeft.Split(',');
                        shooter.leftR = int.Parse(temp[0]);
                        shooter.leftC = int.Parse(temp[1]);
                    }
                    if (textRight != null && textRight.Length > 0)
                    {
                        string[] temp = textRight.Split(',');
                        shooter.rightR = int.Parse(temp[0]);
                        shooter.rightC = int.Parse(temp[1]);
                    }
                }
            }
        }
        for (int r = 0; r < SPAWN_R; r++)
        {
            for (int c = 0; c < SPAWN_C; c++)
            {
                if (spawnMap[r, c])
                {
                    if (spawnMap[r, c].leftR != -1)
                    {
                        spawnMap[r, c].setLeft(spawnMap[spawnMap[r, c].leftR, spawnMap[r, c].leftC]);
                    }
                    if (spawnMap[r, c].rightR != -1)
                    {
                        spawnMap[r, c].setRight(spawnMap[spawnMap[r, c].rightR, spawnMap[r, c].rightC]);
                    }
                }
            }
        }
    }
    public string getJsonString(string name)
    {
        string path = "LevelData/Raw/Group";

        for (int i = 0; i <= 4; i++)
        {
            TextAsset txt = (TextAsset)Resources.Load(path + i + "/" + name, typeof(TextAsset));
            if (txt != null && txt.text != null)
                return txt.text;
        }
        return "";
    }
    private void onGameOver()
    {
        PopupManager.INSTANCE.showLose(slots[0].gameObject);
    }
    private void onGameFinish()
    {
        isFinished = true;
        doSmthAfter(1.5f, () => {
            PopupManager.INSTANCE.showWin(currentLevel * 4);
        });
    }
    public Shooter generateShooter(int color, int count, int type = 0)
    {
        Shooter shooter = Instantiate(prefabShooter, transform.Find("Shooters")).GetComponent<Shooter>();
        shooter.setColor(color);
        shooter.setBulletCount(count);
        shooter.setType(type);
        return shooter;
    }
    public Target generateTarget(int color)
    {
        //Debug.Log("generateTarget");
        Target target = Instantiate(prefabBlock, transform.Find("Targets")).GetComponent<Target>();
        target.setColor(color);
        return target;
    }
    public void checkShoot()
    {
        bool hasOneShoot = false;
        for (int i = 0; i < slots.Count; i++)
        {
            Shooter shooter = slots[i].getShooter();
            if (shooter && shooter.canShoot())
            {
                int lastC = shooter.getLastShotC();
                bool shooted = false;
                for (int c = lastC + 1; c < TARGET_C; c++)
                {
                    Target target = targetMap[0, c];
                    if (target && target.canShoot()
                        && target.getColor() == shooter.getColor())
                    {
                        shooter.doShoot(target);
                        shooter.setLastShotC(c);
                        shooted = true;
                        hasOneShoot = true;
                        break;
                    }
                }
                if (!shooted)
                {
                    for (int c = 0; c <= lastC; c++)
                    {
                        Target target = targetMap[0, c];
                        if (target && target.canShoot()
                            && target.getColor() == shooter.getColor())
                        {
                            shooter.doShoot(target);
                            shooter.setLastShotC(c);
                            shooted = true;
                            break;
                        }
                    }
                }
            }
            if (shooter && shooter.getBulletCount() <= 0)
            {
                if (shooter.isWaitingMerge())
                {
                    // do nothing
                }
                else if (shooter.getLeft() && shooter.getLeft().getBulletCount() > 0)
                {
                    // do nothing
                }
                else if (shooter.getRight() && shooter.getRight().getBulletCount() > 0)
                {
                    // do nothing
                }
                else
                {
                    //Debug.Log(shooter.name + " isWaitingMerge " + shooter.isWaitingMerge());
                    Vector3 exitPosition = shooter.transform.position.x < 0 ? anchorExitLeft.transform.position : anchorExitRight.transform.position;
                    Debug.Log("runtoExit " + exitPosition);
                    shooter.runto(exitPosition, () => {
                        Destroy(shooter.gameObject);
                    });
                    if (shooter.getLeft())
                        shooter.getLeft().runto(exitPosition);
                    if (shooter.getRight())
                        shooter.getRight().runto(exitPosition);
                    for (int k = 0; k < slots.Count; k++)
                    {
                        if (slots[k].getShooter() == shooter || slots[k].getShooter() == shooter.getLeft() 
                            || slots[k].getShooter() == shooter.getRight())
                            slots[k].add(null);
                    }
                }
                    
            }
        }
        if (!hasOneShoot)
        {
            int freeSlotCount = 0;
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].canAdd())
                {
                    freeSlotCount++;
                }
            }
            if (freeSlotCount == 0)
            {
                onGameOver();
            }
        }

        if (targetMap != null && !isFinished)
        {
            int targetCount = 0;
            for (int r = 0; r < TARGET_R; r++)
            {
                for (int c = 0; c < TARGET_C; c++)
                {
                    if (targetMap[r, c] != null && targetMap[r, c].getHealth() > 0)
                    {
                        targetCount++;
                    }
                }
            }
            if (targetCount == 0)
            {
                onGameFinish();
            }
            else
            {
                UIManager.INSTANCE.onLevelProgress(totalTarget - targetCount, totalTarget);
            }
        }
        
    }
    public void onTouchShooter(Shooter shooter)
    {
        if (PopupManager.INSTANCE.isShowingPopup())
            return;
        if (shooter.getR() > 0) {
            // show animation
            return;
        }
        if (shooter.getLeft() && shooter.getLeft().getR() > 0)
        {
            return;
        }
        if (shooter.getRight() && shooter.getRight().getR() > 0)
        {
            return;
        }
        UnityAction<Shooter> upSpawnI = (Shooter shooter) => {
            spawnMap[shooter.getR(), shooter.getC()] = null;
            int c = shooter.getC();
            for (int r = 0; r < SPAWN_R - 1; r++)
            {
                spawnMap[r, c] = spawnMap[r + 1, c];
                spawnMap[r + 1, c] = null;
                if (spawnMap[r, c])
                {
                    spawnMap[r, c].runto(spawnPos[r, c]);
                    spawnMap[r, c].setRC(r, c);
                }
            }
            if (spawnMap[0, c])
                spawnMap[0, c].doShow();
        };
        List<Shooter> waitMerge = new List<Shooter>();
        List<Shooter> list = getListLeftRight(shooter);
        bool canAddMulti = false;
        if (list.Count > 1)
        {
            // check mergeGroup
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].getShooter())
                {
                    List<Shooter> groupTo = getListLeftRight(slots[i].getShooter());
                    if (canMergeGroup(list, groupTo))
                    {
                        doMergeGroup(list, groupTo);
                        for (int j = 0; j < list.Count; j++)
                        {
                            upSpawnI(list[j]);
                        }
                        return;
                    }
                }
            }
            //

            int canAddCount = 0;
            for (int i = 0; i < slots.Count - 1; i++)
            {
                if (slots[i].canAdd())
                    canAddCount++;
                else
                {
                    Shooter s = slots[i].getShooter();
                    if (s)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            if (list[j].canMerge(s) && s.getLeft() == null && s.getRight() == null)
                            {
                                canAddCount++;
                            }
                        }
                    }
                }
            }
            if (canAddCount >= list.Count)
            {
                canAddMulti = true;
                // sort slot
                Debug.Log("check wait merge");
                for (int i = 0; i < slots.Count - 1; i++)
                {
                    if (slots[i].getShooter())
                    {
                        Shooter s = slots[i].getShooter();
                        for (int j = 0; j < list.Count; j++)
                        {
                            if (list[j].canMerge(s) && s.getLeft() == null && s.getRight() == null)
                            {
                                slots[i].add(null);
                                waitMerge.Add(s);
                            }
                        }
                    }
                    if (!slots[i].getShooter())
                    {
                        for (int j = i; j < slots.Count - 1; j++)
                        {
                            slots[j].add(slots[j + 1].getShooter());
                            slots[j + 1].add(null);
                        }
                    }
                }

                for (int i = 0; i < slots.Count - 1; i++)
                {
                    if (slots[i].getShooter())
                    {
                        slots[i].getShooter().runto(slots[i].transform.position);
                    }
                }
            }
        }
        if (list.Count == 1)
        {
            bool checkAdded = false;
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].getShooter() && slots[i].getShooter().canMerge(shooter))
                {
                    Shooter shooterI = slots[i].getShooter();
                    shooterI.waitMerge();
                    shooter.runto(slots[i].transform.position, () => {
                        shooterI.doMerge(shooter);
                    });

                    upSpawnI(shooter);
                    checkAdded = true;
                    break;
                }
            }
            if (!checkAdded)
            {
                for (int i = 0; i < slots.Count; i++)
                {
                    if (slots[i].canAdd())
                    {
                        slots[i].add(shooter);
                        shooter.name = "shooter " + i;
                        shooter.runto(slots[i].transform.position);

                        upSpawnI(shooter);
                        break;
                    }
                }
            }
        }
        else if (canAddMulti)
        {
            Debug.Log("do add multi " + waitMerge.Count);
            int startPos = -1;
            for (int i = 0; i < slots.Count; i++)
            {
                if (!slots[i].getShooter())
                {
                    startPos = i;
                    break;
                }
            }
            if (startPos >= 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    slots[i + startPos].add(list[i]);
                    list[i].name = "shooter " + i;
                    list[i].runto(slots[i + startPos].transform.position);

                    upSpawnI(list[i]);
                }
            }
            for (int i = 0; i < waitMerge.Count; i++)
            {
                onTouchShooter(waitMerge[i]);
            }
        }
    }
    private List<Shooter> getListLeftRight(Shooter shooter)
    {
        List<Shooter> list = new List<Shooter>();

        list.Add(shooter);
        while (list[0].getLeft())
        {
            list.Insert(0, list[0].getLeft());
        }
        while (list[list.Count - 1].getRight())
        {
            list.Add(list[list.Count - 1].getRight());
        }

        return list;
    }
    private bool canMergeGroup(List<Shooter> from, List<Shooter> to)
    {
        if (from.Count > to.Count)
        {
            return false;
        }
        for (int i = 0; i < from.Count; i++)
            if (!to[i].canMerge(from[i]))
            {
                return false;
            }
        return true;
    }
    private void doMergeGroup(List<Shooter> from, List<Shooter> to)
    {
        if (!canMergeGroup(from, to))
            return;
        for (int i = 0; i < from.Count; i++)
        {
            int I = i;
            to[i].waitMerge();
            from[i].runto(to[i].transform.position, () => {
                to[I].doMerge(from[I]);
            });
        }
    }
    public void onTargetShooted(Target target)
    {
        for (int r = 0; r < TARGET_R; r++)
        {
            for (int c = 0; c < TARGET_C; c++)
            {
                if (targetMap[r, c] == target)
                {
                    targetMap[r, c] = null;
                    target.doDisappear();
                    if (!target.getTop())
                    {
                        for (int R = r; R < TARGET_R - 1; R++)
                        {
                            targetMap[R, c] = targetMap[R + 1, c];
                            targetMap[R + 1, c] = null;
                            Target t = targetMap[R, c];
                            if (t)
                            {
                                t.onStartMove();
                                t.GetComponent<MoveComponent>().stopAll().wait(0.3f).thenMoveTo(targetPos[R, c], 0.15f).setEndAction(() =>
                                {
                                    t.onStopMove();
                                });
                            }
                        }
                    }
                    else
                    {
                        Target top = target.getTop();
                        target.setTop(null);
                        targetMap[r, c] = top;
                        top.onStartMove();
                        top.GetComponent<MoveComponent>().stopAll().wait(0.3f).thenMoveTo(targetPos[r, c], 0.15f).setEndAction(() =>
                        {
                            top.onStopMove();
                        });
                    }
                    break;
                }
            }
        }
    }
    public void doReset()
    {
        clearBoard();
        loadLevel(currentLevel);
    }
    public void doNextLevel()
    {
        loadLevel(currentLevel + 1);
    }
    public void doAddSlot()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].getState() == Slot.STATE_ADS)
            {
                slots[i].setState(Slot.STATE_UNLOCKED);
                return;
            }
        }
    }
    // for level gen
    public void destroyTops(int r, int c)
    {
        targetMap[r, c].destroyAllTop();
    }
    public void setTargetCount(int r, int c, int h, int startColor = 0)
    {
        if (targetMap[r, c] == null)
        {
            targetMap[r, c] = generateTarget(startColor);
            targetMap[r, c].transform.position = targetPos[r, c];
        }
        targetMap[r, c].destroyAllTop();
        if (h > 0)
        {
            List<Target> tops = new List<Target>();
            for (int i = 1; i < h; i++)
            {
                tops.Add(generateTarget(targetMap[r, c].getColor()));
            }
            Target temp = targetMap[r, c];
            while (tops.Count > 0)
            {
                temp.setTop(tops[0]);
                temp = temp.getTop();
                tops.RemoveAt(0);
            }
        }
        else
        {
            Destroy(targetMap[r, c].gameObject);
            targetMap[r, c] = null;
        }
    }
    public void setTargetColor(int r, int c, int color)
    {
        if (targetMap[r, c] == null)
            return;
        Target temp = targetMap[r, c];
        while (temp != null)
        {
            temp.setColor(color);
            temp = temp.getTop();
        }
    }
    public Vector3 getTargetPos(int r, int c)
    {
        return targetPos[r, c];
    }
    public Vector3 getSpawnPos(int r, int c)
    {
        return spawnPos[r, c];
    }
    public List<int> computeTargetColor()
    {
        List<int> result = new List<int>();
        result.Add(0); result.Add(0); result.Add(0); result.Add(0); result.Add(0); result.Add(0); result.Add(0); result.Add(0); result.Add(0); result.Add(0); result.Add(0);
        for (int r= 0; r < TARGET_R; r++)
        {
            for (int c = 0; c < TARGET_C; c++)
            {
                if (targetMap[r, c])
                {
                    List<Target> list = new List<Target>();
                    Target temp = targetMap[r, c];
                    while (temp != null)
                    {
                        list.Add(temp);
                        temp = temp.getTop();
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        result[list[i].getColor()] = result[list[i].getColor()] + 1;
                        result[10] = result[10] + 1;
                    }
                }
            }
        }
        return result;
    }

    public void setShooterCount(int r, int c, int count, int startColor = 0)
    {
        if (count == 0)
        {
            Destroy(spawnMap[r, c].gameObject);
            spawnMap[r, c] = null;
            return;
        }
        if (spawnMap[r, c] == null)
        {
            Shooter shooter = generateShooter(startColor, count);
            shooter.transform.position = spawnPos[r, c];
            spawnMap[r, c] = shooter;
            shooter.setRC(r, c);
            shooter.setType(Shooter.TYPE_RECT);
        }
        spawnMap[r, c].setBulletCount(count);
    }
    public void setShooterType(int r, int c, int type)
    {
        if (spawnMap[r, c])
            spawnMap[r, c].setType(type);
    }
    public void setShooterColor(int r, int c, int color)
    {
        if (spawnMap[r, c])
            spawnMap[r, c].setColor(color);
    }
    public void setShooterLeftRight(int leftR, int leftC, int rightR, int rightC)
    {
        if (spawnMap[leftR, leftC] && spawnMap[rightR, rightC])
        {
            spawnMap[leftR, leftC].setRight(spawnMap[rightR, rightC]);
        }
    }
    public void deleteLeftRight(int r, int c)
    {
        if (spawnMap[r, c])
        {
            spawnMap[r, c].setLeft(null);
            spawnMap[r, c].setRight(null);
        }
    }
    public List<int> computeShooterColor()
    {
        List<int> result = new List<int>();
        result.Add(0); result.Add(0); result.Add(0); result.Add(0); result.Add(0); result.Add(0); result.Add(0); result.Add(0); result.Add(0); result.Add(0); result.Add(0);
        for (int r = 0; r < SPAWN_R; r++)
        {
            for (int c = 0; c < SPAWN_C; c++)
            {
                if (spawnMap[r, c])
                {

                    result[spawnMap[r, c].getColor()] = result[spawnMap[r, c].getColor()] + spawnMap[r, c].getBulletCount();
                    result[10] = result[10] + spawnMap[r, c].getBulletCount();
                }
            }
        }
        return result;
    }

    public Target[,] getTargetMap()
    {
        return targetMap;
    }
    public Shooter[,] getShooterMap()
    {
        return spawnMap;
    }
    //
}
