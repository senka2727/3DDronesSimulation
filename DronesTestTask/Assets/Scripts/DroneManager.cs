using UnityEngine;

public class DroneManager : MonoBehaviour
{
    [SerializeField] private GameObject[] RedDronesObjects;
    [SerializeField] private GameObject[] BlueDronesObjects;

    [SerializeField] private Drone[] RedDronesScripts;
    [SerializeField] private Drone[] BlueDronesScripts;

    public void ChangeRedDronesSpeed(float newSpeed){
        for(int i = 0; i < RedDronesScripts.Length; i++) RedDronesScripts[i].ChangeSpeed((int) newSpeed);
    }

    public void ChangeRedDronesAmount(float Amount){
        for(int i = 0; i < RedDronesObjects.Length; i++){
            if(i < Amount) RedDronesObjects[i].SetActive(true);
            else RedDronesObjects[i].SetActive(false);
        }
    }

    public void ChangeBlueDronesSpeed(float newSpeed){
        for(int i = 0; i < BlueDronesScripts.Length; i++) BlueDronesScripts[i].ChangeSpeed((int) newSpeed);
    }

    public void ChangeBlueDronesAmount(float Amount){
        for(int i = 0; i < BlueDronesObjects.Length; i++){
            if(i < Amount) BlueDronesObjects[i].SetActive(true);
            else BlueDronesObjects[i].SetActive(false);
        }
    }

    public void SetDronesDrawPath(bool isOn){
        for(int i = 0; i < RedDronesScripts.Length; i++) RedDronesScripts[i].SetDrawPath(isOn);
        for(int i = 0; i < BlueDronesScripts.Length; i++) BlueDronesScripts[i].SetDrawPath(isOn);
    }
}
