using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ObstaclePusher : MonoBehaviour
{

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 10f;
    float cycles;

    float movementFactor = 0; //0 for not moved, 1 for fully moved

    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(period < Mathf.Epsilon)
        { 
            return;
        }

        float cycles = Time.time / period;

        float rawSinWave = Mathf.Sin(cycles * 2 * Mathf.PI);

        movementFactor = rawSinWave / 2f + 0.5f;  //should occilate between 0 and 1;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
