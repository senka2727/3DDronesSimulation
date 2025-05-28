using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Drone : MonoBehaviour
{   
    [SerializeField] private DroneState MyCurrentState = DroneState.Idle;

    [Header("Base")]
    [SerializeField] private int MySpeed;
    private GameObject MyCurrentTarget;
    [SerializeField] private Transform MyBase;
    [SerializeField] private Base MyBaseScript;
    [Space]
    [SerializeField] private NavMeshAgent MyNavAgent;
    [SerializeField] private TrailRenderer MyTrail;

    [Header("Loading resource")]
    private float CurrentLoadingTimer;
    [SerializeField] private float TargetLoadingTime = 2f;

    [Header("Path rendering")]
    [SerializeField] private bool drawPathLine = false;
    [SerializeField] private LineRenderer PathLine;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer MyStateIndicator;
    
    //Можно заплатить за плагин или написать самому скрипт на кастом инспектор и сразу напрямую заполнить dictionary там, но мне быстрее будет использовать классический обходной путь :)
    private Dictionary<DroneState, Sprite> StateIndicators = new Dictionary<DroneState, Sprite>();
    [SerializeField] private Sprite[] StateSprites;

    void Awake(){
        StateIndicators.Add(DroneState.Idle, StateSprites[0]);
        StateIndicators.Add(DroneState.LoadResource, StateSprites[1]);
        StateIndicators.Add(DroneState.UnloadResource, StateSprites[2]);
    }

    void OnDisable(){
        MyCurrentTarget = null;
        CurrentLoadingTimer = 0;
    }

    void OnEnable(){
        ChangeState(DroneState.LoadResource);
    }

    void Update(){
        switch(MyCurrentState){
            case DroneState.Idle:
                Idle();
                break;
            case DroneState.LoadResource:
                LoadResource();
                break;
            case DroneState.UnloadResource:
                UnloadResource();
                break;
        }

        if(MyNavAgent.hasPath && drawPathLine) DrawPath();
    }

    public void ChangeState(DroneState newState){
        MyCurrentState = newState;
        
        MyStateIndicator.sprite = StateIndicators[MyCurrentState];
    }

    void Idle(){
        MyTrail.enabled = false;
        MyNavAgent.isStopped = true;
    }

    void LoadResource(){
        MyTrail.enabled = false;
        MyNavAgent.isStopped = false;

        if(MyCurrentTarget == null) MyCurrentTarget = ResourceManager.Instance.FindNearestResource(transform.position); //Если нет цели, найти новый ближайший ресурс
        if(MyCurrentTarget == null){ //Если нет свободных ресурсов подождать две секунды и проверить снова
            StartCoroutine(SetIdle(2f, DroneState.LoadResource));
            return;
        } 

        if(Vector3.Distance(transform.position, MyCurrentTarget.transform.position) >= 1){ //Лететь к ресурсу
            MyNavAgent.SetDestination(MyCurrentTarget.transform.position);
        } else{ //Собирать ресурс
            CurrentLoadingTimer += 1 * Time.deltaTime;

            if(CurrentLoadingTimer >= TargetLoadingTime){
                CurrentLoadingTimer = 0;

                Destroy(MyCurrentTarget);
                MyCurrentTarget = null;

                ChangeState(DroneState.UnloadResource);
            }
        }
    }

    void UnloadResource(){
        MyTrail.enabled = false;
        MyNavAgent.isStopped = false;

        if(Vector3.Distance(transform.position, MyBase.position) >= 2){ //Лететь на базу
            MyNavAgent.SetDestination(MyBase.transform.position);
        } else{ //Доставить ресурс
            MyBaseScript.AddResource();

            ChangeState(DroneState.LoadResource);
        }  
    }


    IEnumerator SetIdle(float SecondsToIdle, DroneState TargetStateAfterIdle){
        if(MyCurrentState == DroneState.Idle) yield break;

        ChangeState(DroneState.Idle);
        yield return new WaitForSeconds(SecondsToIdle);
        ChangeState(TargetStateAfterIdle);
    }


    public void ChangeSpeed(int newSpeed){
        MySpeed = newSpeed;
        MyNavAgent.speed = newSpeed;
    }

    public void SetDrawPath(bool isOn){
        drawPathLine = isOn;
        PathLine.gameObject.SetActive(isOn);
    }

    void DrawPath(){
        PathLine.positionCount = MyNavAgent.path.corners.Length;
        PathLine.SetPositions(MyNavAgent.path.corners);


        Debug.Log("Path corners: " + MyNavAgent.path.corners.Length);
        // PathLine.SetPositions(MyNavAgent.path.corners);
    }
}

public enum DroneState{
    Idle,
    LoadResource,
    UnloadResource
}
