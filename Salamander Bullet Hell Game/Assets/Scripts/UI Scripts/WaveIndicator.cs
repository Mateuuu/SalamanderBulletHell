using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveIndicator : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        animator.SetBool("NewWave", true);
    }
    public void NewWave()
    {
        animator.SetBool("NewWave", true);
    }
    public void ExecuteWave(float duration)
    {
        animator.SetBool("NewWave", false);
        animator.SetFloat("WaveCountdownSpeed", 1/duration);
    }
}
