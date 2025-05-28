using UnityEngine;

public class StateIndicatorRotater : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.Euler(30f, 0, 0);       
    }
}
