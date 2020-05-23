using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public EnemyDummySignalsInput edsi;

    void Start()
    {
        edsi = this.GetComponent<EnemyDummySignalsInput>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
