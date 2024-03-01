using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAnimation : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private float second;

    [SerializeField]
    private string animName;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("impact", true);
    }

    void Update()
    {
        AnimatorClipInfo[] currentAnim = animator.GetCurrentAnimatorClipInfo(0);

        if (currentAnim[0].clip.name == animName)
        {
            second -= Time.deltaTime;
            if (second <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
