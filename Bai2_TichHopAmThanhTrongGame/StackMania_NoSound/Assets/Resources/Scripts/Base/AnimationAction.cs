using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAction : MonoBehaviour
{
    private Dictionary<string, GameObject> map = new Dictionary<string, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        List<Transform> temp = new List<Transform>();
        temp.Add(transform);
        while (temp.Count > 0)
        {
            Transform t = temp[0];
            temp.RemoveAt(0);
            if (!map.ContainsKey(t.gameObject.name))
                map.Add(t.gameObject.name, t.gameObject);
            for (int i = 0; i < t.childCount; i++)
                temp.Add(t.GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void showChildren(string names)
    {
        string[] children = names.Split(',');
        for (int i = 0; i < children.Length; i++)
        {
            showChild(children[i]);
        }
    }
    public void hideChildren(string names)
    {
        string[] children = names.Split(',');
        for (int i = 0; i < children.Length; i++)
        {
            hideChild(children[i]);
        }
    }
    private void showChild(string name)
    {
        map[name].SetActive(true);
    }
    private void hideChild(string name)
    {
        map[name].SetActive(false);
    }
    public void specialAction(string name)
    {

    }
}
