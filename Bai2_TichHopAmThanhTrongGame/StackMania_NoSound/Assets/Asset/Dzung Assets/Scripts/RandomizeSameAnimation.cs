using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeSameAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            animator.Play(state.fullPathHash, 0, Random.Range(0f, 1f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
