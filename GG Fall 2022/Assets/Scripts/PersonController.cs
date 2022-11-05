using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;

public class PersonController : MonoBehaviour
{
    public PersonAIValues AIValues;
    public NavMeshAgent Agent;
    public PersonAIState CurrentAIState, 
        RandomMoveState = new PersonRandomRunState(), 
        RunAwayState = new PersonRunFromWarningState(), 
        IdleState = new PersonIdleState(),
        SavedState = new SavedPersonState();
    public string StateName;
    public Vector3 CurrentDestination;
    Renderer visual;
    public Color myColor;
    private void Update()
    {
        CurrentAIState?.UpdateState();
        StateName = CurrentAIState.ToString();
    }
    public void ToggleRunningState(bool active)
    {
        SwitchState(active ? RunAwayState : RandomMoveState);
    }
    public void SwitchState(PersonAIState newState)
    {
        if (CurrentAIState == SavedState) return;
        CurrentAIState?.ExitState();
        CurrentAIState = newState;
        CurrentAIState?.EnterState(this);
    }
    public void SetDestination(Vector3 destination)
    {
        CurrentDestination = destination;
        Agent.SetDestination(destination);
    }
    public void Init(Color myColor)
    {
        visual.material.color = this.myColor = myColor;
    }
    public void SavePerson(Vector3 SavedLocation)
    {
        SwitchState(SavedState);
        SetDestination(SavedLocation);
    }
    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        visual = GetComponent<Renderer>();
        SwitchState(RandomMoveState);
    }
    public class SavedPersonState : PersonAIState
    {
        public override void EnterState(PersonController Controller)
        {
            base.EnterState(Controller);
        }
        public override void UpdateState()
        {
            if(controller.Agent.remainingDistance < 0.5f)
            {
                GameObject.Destroy(controller.gameObject);
            }
        }
        public override void ExitState()
        {
            
        }
    }
    public class PersonRunFromWarningState : PersonAIState
    {
        WarningArea warningArea;
        public override void EnterState(PersonController Controller)
        {
            base.EnterState(Controller);
            warningArea = WarningArea.Instance;
            controller.Agent.speed = controller.AIValues.AIRunSpeed;
        }
        public override void UpdateState()
        {
            Vector3 runAwayDirection = controller.transform.position - warningArea.transform.position;
            runAwayDirection.Normalize();
            controller.SetDestination(runAwayDirection * controller.AIValues.ChangeDirectionDistance + controller.transform.position);
        }
        public override void ExitState()
        {
            controller.Agent.speed = controller.AIValues.AIWalkSpeed;
        }
    }
    public class PersonRandomRunState : PersonAIState
    {
        float timeBeforeIdle;
        public override void EnterState(PersonController Controller)
        {
            base.EnterState(Controller);
            timeBeforeIdle = controller.AIValues.TimeBeforeIdle * (Random.value + 0.5f);
            SetNewRandomPositionOnNavMesh(3);
            Controller.Agent.speed = Controller.AIValues.AIWalkSpeed;
        }
        public override void UpdateState()
        {
            if (controller.RunAwayState.CheckCanSwitch(controller))
            {
                controller.SwitchState(controller.RunAwayState);
                return;
            }
            if (controller.Agent.remainingDistance < controller.AIValues.ChangeDirectionDistance)
            {
                SetNewRandomPositionOnNavMesh(3);
                return;
            }
            timeBeforeIdle -= Time.deltaTime;
            if(timeBeforeIdle < 0)
                controller.SwitchState(controller.IdleState);
        }
        public void SetNewRandomPositionOnNavMesh(int triesLeft)
        {
            if (triesLeft == 0)
            {
                controller.SwitchState(controller.IdleState);
                return;
            }
            controller.SetDestination(GetRandomPoint() + controller.transform.position);
            if (controller.CurrentDestination != controller.Agent.pathEndPosition)
                SetNewRandomPositionOnNavMesh(--triesLeft);
        }
        public override void ExitState()
        {
            
        }
        Vector3 GetRandomPoint()
        {
            float x = Random.value * 2 - 1;
            float z = Random.value * 2 - 1;
            var pos = new Vector3(x, 0, z).normalized * controller.AIValues.ChangeDirectionDistance;
            if (SpawnArea.Instance != null)
            {
                pos += SpawnArea.Instance.GetRandomPositionInBounds();
                pos /= 2;
            }
            return pos;
        }
    }
    public class PersonIdleState : PersonAIState
    {
        float idleTime;
        public override void EnterState(PersonController Controller)
        {
            base.EnterState(Controller);
            idleTime = controller.AIValues.IdleTime * (Random.value + 0.5f);
        }
        public override void UpdateState()
        {
            idleTime -= Time.deltaTime;
            if (idleTime <= 0)
                controller.SwitchState(controller.RandomMoveState);
        }
        public override void ExitState()
        {
            
        }
    }
    public abstract class PersonAIState
    {
        protected PersonController controller;
        public virtual void EnterState(PersonController Controller)
        {
            controller = Controller;
        }
        public abstract void UpdateState();
        public abstract void ExitState();
        public virtual bool CheckCanSwitch(PersonController Controller)
        {
            controller = Controller;
            return true;
        }
    }
}
