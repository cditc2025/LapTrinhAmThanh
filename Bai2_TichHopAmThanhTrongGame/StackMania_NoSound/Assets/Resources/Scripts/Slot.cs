using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public const int STATE_UNLOCKED = 1;
    public const int STATE_FUTURE = 2;
    public const int STATE_ADS = 3;
    [SerializeField] private TextMeshPro textFuture;
    [SerializeField] private GameObject adIcon;
    [SerializeField] private int state = 1;
    [SerializeField] private Shooter shooter;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void setUnlockAt(int level)
    {
        textFuture.text = "Level\n" + level;
        setState(STATE_FUTURE);
    }
    public void setState(int s)
    {
        state = s;
        if (state == STATE_UNLOCKED)
        {
            textFuture.gameObject.SetActive(false);
            adIcon.SetActive(false);
        }
        else if (state == STATE_FUTURE)
        {
            textFuture.gameObject.SetActive(true);
            adIcon.SetActive(false);
        }
        else if (state == STATE_ADS)
        {
            textFuture.gameObject.SetActive(false);
            adIcon.SetActive(true);
        }
    }
    public int getState()
    {
        return state;
    }
    public bool isUnlocked()
    {
        return state == STATE_UNLOCKED;
    }
    public bool canAdd()
    {
        return isUnlocked() && shooter == null;
    }
    public void add(Shooter shooter)
    {
        this.shooter = shooter;
    }
    public Shooter getShooter()
    {
        return shooter;
    }
}
