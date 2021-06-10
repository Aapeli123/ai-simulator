
using System;
using UnityEngine;

public class EnergyCoreController : MonoBehaviour
{
    public enum CoreShape
    {
        Block_0 = 0,
        Ball_1 = 1
    }

    public enum CoreType
    {
        Negative = 0,
        Positive = 1
    }

    public float distanceToNearestRed = float.PositiveInfinity;
    public float distanceToNearestBlue = float.PositiveInfinity;
    
    [HideInInspector]
    public GameArena arena;
    public CoreType m_CoreType;

    public string redGoalTag; //will be used to check if collided with red goal
    public string blueGoalTag; //will be used to check if collided with blue goal

    void Awake()
    {
        arena = transform.parent.GetComponent<GameArena>();
    }

    void Update()
    {
        // Check for occasional energy core getting out of arena
        // and dropping down.
        if (transform.position.y < -0.5f)
        {
            transform.position = arena.GetRandomSpawnPosInArena();
        }
    }

    private void FixedUpdate()
    {
        var redGoals = GameObject.FindGameObjectsWithTag(redGoalTag);
        var blueGoals = GameObject.FindGameObjectsWithTag(blueGoalTag);

        float min = float.PositiveInfinity;
        foreach (var redGoal in redGoals)
        {
            Vector3 curPos = transform.position;
            Vector3 pos = redGoal.transform.position;
            var distance = Vector3.Distance(curPos, pos);
            if (min > distance)
            {
                min = distance;
            }
        }

        distanceToNearestRed = min;
        min = float.PositiveInfinity;
        foreach (var blueGoal in blueGoals)
        {
            Vector3 curPos = transform.position;
            Vector3 pos = blueGoal.transform.position;
            var distance = Vector3.Distance(curPos, pos);
            if (min > distance)
            {
                min = distance;
            }
        }

        distanceToNearestBlue = min;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(redGoalTag)) //ball touched red goal
        {
            gameObject.SetActive(false);
            arena.GoalTouched(AIRobotAgent.Team.Red, m_CoreType);
        }
        if (col.gameObject.CompareTag(blueGoalTag)) //ball touched blue goal
        {
            gameObject.SetActive(false);
            arena.GoalTouched(AIRobotAgent.Team.Blue, m_CoreType);
        }
    }
}
