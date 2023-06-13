using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    //Script for SinglePlayer mode -> target practice

    //bikin event buat ketika score
    public delegate void ScoreEvent();
    public static event ScoreEvent OnScore;

    [SerializeField] private Goal[] _goalAreas;

    private void OnEnable()
    {
        OnScore += GenerateNewTarget;
    }
    private void OnDisable()
    {
        OnScore -= GenerateNewTarget;
    }

    private void Start()
    {
        GenerateNewTarget();
    }

    public void TriggerScoreEvent()
    {
        OnScore?.Invoke();
    }

    private void GenerateNewTarget()
    {
        //dari pool of target pick one to choose
        int target = Random.Range(0, _goalAreas.Length);

        //foreach, kalo target set jadi target, kalo bukan ya set jadi nontarget
        for (int i = 0; i < _goalAreas.Length; i++)
        {
            if(i == target)
            {
                //_goalAreas[i].SetTarget(true);
            }else
            {
                //_goalAreas[i].SetTarget(false);
            }

        }
    }
}
