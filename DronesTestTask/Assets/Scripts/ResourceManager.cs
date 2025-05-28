using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [Header("Time")]
    private float CurrentSpawnTimer;
    [SerializeField] private float TargetSpawnTimer = 3;

    [Header("Prefab and Gameobject management")]
    [SerializeField] private List<GameObject> AvailableResources;
    [SerializeField] private GameObject ResourcePrefab;
    [SerializeField] private Transform ResourceSpawnArea;

    void Start(){
        if(Instance == null) Instance = this;
        else Destroy(this);
    }

    void Update(){
        CurrentSpawnTimer += 1 * Time.deltaTime;

        if(CurrentSpawnTimer >= TargetSpawnTimer){
            CurrentSpawnTimer = 0;
            AvailableResources.Add(Instantiate(ResourcePrefab, GetRandomSpawnPosition(), ResourcePrefab.transform.rotation));
        }
    }

    Vector3 GetRandomSpawnPosition(){
        float SpawnPosX = UnityEngine.Random.Range(ResourceSpawnArea.localScale.x / 2 * -1, ResourceSpawnArea.localScale.x / 2);
        float SpawnPosZ = UnityEngine.Random.Range(ResourceSpawnArea.localScale.z / 2 * -1, ResourceSpawnArea.localScale.z / 2);
        
        Vector3 SpawnPos = new Vector3(SpawnPosX, ResourceSpawnArea.position.y, SpawnPosZ);
        return SpawnPos;
    }

    public void RemoveResourceFromList(GameObject ResourceToRemove){
        AvailableResources.Remove(ResourceToRemove);
    }

    public void DeleteResource(int ID){
        Destroy(AvailableResources[ID]);
        AvailableResources.RemoveAt(ID);
    }

    public void DeleteResource(GameObject ResourceGameObject){
        for(int i = 0; i < AvailableResources.Count; i++){
            if(ResourceGameObject == AvailableResources[i]){
                Destroy(AvailableResources[i]);
                AvailableResources.RemoveAt(i);
            }
        }
    }

    public GameObject FindNearestResource(Vector3 startPos){
        if(AvailableResources.Count == 0) return null; //Нет не занятых ресурсов

        //Найти дистанции
        float[] Distances = new float[AvailableResources.Count];

        for(int i = 0; i < Distances.Length; i++){
            Distances[i] = Vector3.Distance(startPos, AvailableResources[i].transform.position); 
        }   

        //Найти индекс ближайшего ресурса
        int MinIndex = 0;
        float MinValue = Distances[0];

        for(int i = 1; i < Distances.Length; i++){
            if(Distances[i] < MinValue){ 
                MinValue = Distances[i];
                MinIndex = i;
            }
        }

        GameObject TargetResource = AvailableResources[MinIndex];
        RemoveResourceFromList(AvailableResources[MinIndex]);

        return TargetResource; //Отдать индекс ближайшего ресурса
    }

    #region Changable Settings
    public void ChangeResourcesSpawnRate(string SpawnPerSeconds){
        int ConvertedString = int.Parse(SpawnPerSeconds);
        if(ConvertedString >= 0) TargetSpawnTimer = ConvertedString;
    }

    public void DestroyAllResources(){
        for(int i = 0; i < AvailableResources.Count; i++){
            Destroy(AvailableResources[0]);
            AvailableResources.RemoveAt(0);
        }
    }
    #endregion
}
