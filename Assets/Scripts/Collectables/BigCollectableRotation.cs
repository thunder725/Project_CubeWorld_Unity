using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCollectableRotation : MonoBehaviour
{
    Transform child1, child2, child3;

    [SerializeField] float rotationSpeed;

    Vector3[] randomRotations;

    [SerializeField] float scaleModifiyerIntensity, scaleModifyerFrequency;

    float defaultScale;


    private void Awake()
    {
        // Get references to the children objects
        // Can't be children of children, so they have unique rotations
        child1 = transform.GetChild(0);
        child2 = transform.GetChild(1);
        child3 = transform.GetChild(2);

        GenerateQuaternions();

        defaultScale = transform.localScale.x;
    }

    void GenerateQuaternions()
    {
        randomRotations = new Vector3[4];

        randomRotations[0] = new Vector3(0, 180, 0);

        randomRotations[1] = new Vector3(-90, 0, 90);

        randomRotations[2] = new Vector3(Random.Range(0, 180f), Random.Range(0, 180f), Random.Range(0, 180f));

        randomRotations[3] = new Vector3(0, -90, -180);
    }
    
    void Update()
    {
        transform.localScale = Vector3.one * (defaultScale + (Mathf.Sin(Time.time * scaleModifyerFrequency) * scaleModifiyerIntensity));

        transform.Rotate(Time.deltaTime * randomRotations[0] * rotationSpeed / 100);

        child1.Rotate(Time.deltaTime * randomRotations[1] * rotationSpeed / 100);

        child2.Rotate(Time.deltaTime * randomRotations[2] * rotationSpeed / 100);

        child3.Rotate(Time.deltaTime * randomRotations[3] * rotationSpeed / 100);
    }


}
