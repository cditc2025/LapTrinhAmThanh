using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Shooter : BaseObject
{
    public const int TYPE_RECT = 1;
    public const int TYPE_CIRCLE = 2;
    public const int TYPE_TRIANGLE = 3;
    [SerializeField] private List<GameObject> renderer;
    [SerializeField] private int color;
    [SerializeField] private int bulletCount = 20;
    [SerializeField] private int type;
    [SerializeField] private GameObject bPrefab;
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshPro bulletCountText;
    [SerializeField] private GameObject lineLeft;
    [SerializeField] private GameObject lineRight;
    [SerializeField] private Target testTarget1;
    [SerializeField] private Target testTarget2;
    [SerializeField] private Target testTarget3;
    [SerializeField] private GameObject testSlot;
    [SerializeField] private GameObject lookat;
    [SerializeField] private GameObject lookStart;
    [SerializeField] private float runningV;

    [SerializeField] private Shooter left;
    [SerializeField] private Shooter right;

    [SerializeField] private List<GameObject> rectangleObjects;
    [SerializeField] private List<GameObject> circleObjects;
    [SerializeField] private List<GameObject> triangleObjects;

    public int leftR = -1;
    public int leftC = -1;
    public int rightR = -1;
    public int rightC = -1;

    private bool isHiding = false;

    private int r;
    private int c;
    // Start is called before the first frame update
    void Start()
    {

    }
    float time2ThangDaulai = 0;
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    doShoot(testTarget1);
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    doShoot(testTarget2);
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    doShoot(testTarget3);
        //}
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    runto(testSlot.transform.position);
        //}
        if (time2ThangDaulai > 0)
        {
            if (!runtolookat)
            {
                time2ThangDaulai -= Time.deltaTime;
                if (time2ThangDaulai <= 0)
                {
                    //transform.eulerAngles= new Vector3(0, 0, 0);
                    Debug.Log("back to straight");
                    lookat.GetComponent<MoveComponent>().stopAll().moveTo(transform.position + new Vector3(0, 0, 1), 0.3f);
                }
            }
        }
        updateLookatRotate();

        if (runtolookat)
        {
            float distance = Vector3.Distance(transform.position, lookat.transform.position);
            float canMoveDistance = runningV * Time.deltaTime;
            if (distance < canMoveDistance)
            {
                transform.position = lookat.transform.position;
                runtolookat = false;
                playIdle();
                lookStart.transform.localPosition = new Vector3(0, 0, 1);
                lookat.transform.position = lookStart.transform.position;
                lookat.GetComponent<MoveComponent>().stopAll().moveTo(transform.position + new Vector3(0, 0, 1), 0.15f);
                // run finish
                if (runToEndAction != null)
                {
                    runToEndAction();
                    runToEndAction = null;
                }
            }
            else
            {
                float x = canMoveDistance / distance;
                transform.position = transform.position + (lookat.transform.position - transform.position) * x;
            }
        }
        if (left && left.gameObject.activeSelf)
        {
            lineLeft.SetActive(true);

            var lookPos = lineLeft.transform.position - left.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            lineLeft.transform.rotation = Quaternion.Slerp(lineLeft.transform.rotation, rotation, 1.0f);
            lineLeft.transform.localScale = new Vector3(lineLeft.transform.localScale.x, lineLeft.transform.localScale.y, Vector3.Distance(transform.position, left.transform.position) / 2);
        }
        else
        {
            lineLeft.SetActive(false);
        }
        if (right && right.gameObject.activeSelf)
        {
            lineRight.SetActive(true);

            var lookPos = lineRight.transform.position - right.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            lineRight.transform.rotation = Quaternion.Slerp(lineRight.transform.rotation, rotation, 1.0f);
            lineRight.transform.localScale = new Vector3(lineRight.transform.localScale.x, lineRight.transform.localScale.y, Vector3.Distance(transform.position, right.transform.position) / 2);
        }
        else
        {
            lineRight.SetActive(false);
        }
    }
    public void setBulletCount(int count)
    {
        bulletCount = count;
        bulletCountText.text = "" + count;
    }
    public void setType(int type)
    {
        this.type = type;

        foreach (var t in circleObjects)
            t.SetActive(type == TYPE_CIRCLE);
        foreach (var t in rectangleObjects)
            t.SetActive(type == TYPE_RECT);
        foreach (var t in triangleObjects)
            t.SetActive(type == TYPE_TRIANGLE);
    }
    public int getType()
    {
        return type;
    }
    public void setRC(int r, int c)
    {
        this.r = r;
        this.c = c;
    }
    public int getR()
    {
        return r;
    }
    public int getC()
    {
        return c;
    }
    public void setColor(int c)
    {
        color = c;
        UnityAction a = () => {
            setMat(ColorManager.INSTANCE.getShooterMat(c));
        };
        if (ColorManager.INSTANCE)
            a();
        else
            doSmthAfter(0.01f, a);
    }
    public void setMat(Material mat)
    {
        foreach (GameObject r in renderer)
            r.GetComponent<SkinnedMeshRenderer>().material = mat;
        lineLeft.transform.GetChild(0).GetComponent<MeshRenderer>().material = mat;
        lineRight.transform.GetChild(0).GetComponent<MeshRenderer>().material = mat;
    }
    public int getColor()
    {
        return color;
    }
    public void setLeft(Shooter shooter)
    {
        //if (left)
        //{
        //    Shooter temp = left;
        //    left = null;
        //    temp.setRight(null);
        //}
        if (left != shooter)
        {
            left = shooter;
            shooter.setRight(this);
        }
    }
    public void setRight(Shooter shooter)
    {
        //if (right)
        //{
        //    Shooter temp = right;
        //    right = null;
        //    temp.setLeft(null);
        //}
        if (right != shooter)
        {
            right = shooter;
            shooter.setLeft(this);
        }
    }
    public Shooter getLeft()
    {
        return left;
    }
    public Shooter getRight()
    {
        return right;
    }
    public void doShoot(Target target)
    {
        if (target && !target.canShoot())
        {
            return;
        }
        if (bulletCount <= 0)
            return;
        setBulletCount(bulletCount - 1);
        target.onTargeted();

        Bullet b = Instantiate(bPrefab).GetComponent<Bullet>();
        b.transform.position = transform.position;
        b.shoot(target, () =>
        {
            Board.INSTANCE.onTargetShooted(target);
        });
        lookat.transform.SetParent(transform.parent);
        lookStart.transform.localPosition = new Vector3(0, 0, Vector3.Distance(transform.position, target.transform.position));
        lookat.transform.position = lookStart.transform.position;
        lookat.GetComponent<MoveComponent>().stopAll().moveTo(target.transform.position, 0.1f);
        time2ThangDaulai = 0.11f;
        playShooting();
    }
    private void updateLookatRotate()
    {
        var lookPos = lookat.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1.0f);
    }
    private bool runtolookat = false;
    private UnityAction runToEndAction;
    public void runto(Vector3 position, UnityAction endAction = null)
    {
        Debug.Log("runTo " + position);
        time2ThangDaulai = -1;
        float duration = Vector3.Distance(transform.position, position) / runningV;
        lookat.transform.SetParent(transform.parent);
        lookStart.transform.localPosition = new Vector3(0, 0, Vector3.Distance(transform.position, position));
        lookat.transform.position = lookStart.transform.position;
        lookat.GetComponent<MoveComponent>().stopAll().moveTo(position, duration * 0.75f);
        lookat.transform.position = position;
        runtolookat = true;
        runToEndAction = endAction;
        playRun();
    }
    public bool canShoot()
    {
        return !runtolookat && bulletCount > 0;
    }
    private void OnMouseDown()
    {
        Board.INSTANCE.onTouchShooter(this);
    }
    private int lastShotC = -1;
    public void setLastShotC(int c)
    {
        lastShotC = c;
    }
    public int getLastShotC()
    {
        return lastShotC;
    }
    public int getBulletCount()
    {
        return bulletCount;
    }
    private void playIdle()
    {
        animator.SetBool("isRunning", false);
    }
    private void playRun()
    {
        animator.SetBool("isRunning", true);
    }
    private void playShooting()
    {
        animator.Play("shoot");
    }
    private bool isMerging = false;
    public bool isWaitingMerge()
    {
        return isMerging;
    }
    public void waitMerge()
    {
        isMerging = true;
    }
    public bool canMerge(Shooter shooter)
    {
        if (type == TYPE_CIRCLE)
        {
            if (shooter.getType() == TYPE_CIRCLE && shooter.getColor() == getColor())
                return true;
        }
        return false;
    }
    public void doMerge(Shooter shooter)
    {
        if (!canMerge(shooter))
            return;
        Debug.Log("doMerge " + shooter);
        setBulletCount(bulletCount + shooter.getBulletCount());
        Destroy(shooter.gameObject);
        isMerging = false;
    } 
    public void doHide()
    {
        isHiding = true;
        setMat(ColorManager.INSTANCE.getHideMat());
    }
    public void doShow()
    {
        isHiding = false;
        setMat(ColorManager.INSTANCE.getShooterMat(color));
    }
}
