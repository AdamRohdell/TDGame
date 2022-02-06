using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionComponent : MonoBehaviour
{
    // Start is called before the first frame update

    public ParticleSystem selectionSystem;
    void Start()
    {
        selectionSystem = GameObject.Find("SelectionRing").GetComponent<ParticleSystem>();
        selectionSystem.Play();
    }

}
