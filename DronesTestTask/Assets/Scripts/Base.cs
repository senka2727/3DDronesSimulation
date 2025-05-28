using UnityEngine;
using TMPro;

public class Base : MonoBehaviour
{
    [SerializeField] private int MyResourceCount = 0;

    [SerializeField] private TMP_Text ResourceCountText;

    [SerializeField] private ParticleSystem ResourceDropParticles;

    public void AddResource(){
        MyResourceCount += 1;

        ResourceCountText.text = MyResourceCount.ToString();

        ResourceDropParticles.Play();
    }
}
