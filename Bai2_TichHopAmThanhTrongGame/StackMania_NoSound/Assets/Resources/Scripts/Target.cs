using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Target : BaseObject
{
    [SerializeField] private GameObject renderer;
    [SerializeField] private int color;
    [SerializeField] private Animator animator;
    private Target top;
    private int health = 1;
    private bool isMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        setColor(color);
    }

    // Update is called once per frame
    void Update()
    {
        if (top)
        {
            top.transform.position = transform.position + new Vector3(0, 1, 0);
        }

    }
    public void setColor(int c)
    {
        color = c;
        UnityAction a = () => {
            renderer.GetComponent<SkinnedMeshRenderer>().material = ColorManager.INSTANCE.getTargetMat(c);
        };
        if (ColorManager.INSTANCE)
            a();
        else
            doSmthAfter(0.01f, a);
    }
    public int getHealth()
    {
        return health;
    }
    public int getColor()
    {
        return color;
    }
    public bool canShoot()
    {
        return health > 0;
    }
    public void onTargeted()
    {
        health--;
    }
    public void onStartMove()
    {
        isMoving = true;
    }
    public void onStopMove()
    {
        isMoving = false;
    }
    public void playIdle()
    {
        animator.Play("idle", 0);
    }
    public void playAppear()
    {
        animator.Play("appear", 0);
    }
    public void onHit(bool left)
    {
        if (health > 0)
            animator.Play(left ? "hit_L" : "hit_R", 0);
    }
    public void doDisappear()
    {
        animator.SetBool("isShooted", true);
        Destroy(gameObject, 2);
    }
    public void setTop(Target t)
    {
        top = t;
    }
    public Target getTop()
    {
        return top;
    }
    public void destroyAllTop()
    {
        List<Target> tops = new List<Target>();
        Target temp = top;
        while (temp != null)
        {
            tops.Add(temp);
            temp = temp.top;
        }
        for (int i = 0; i < tops.Count; i++)
        {
            Destroy(tops[i].gameObject);
        }
        top = null;
    }
}
