using UnityEngine;

public class Oazo_Spawner : MonoBehaviour
{

    [SerializeField] GameObject OazoPrefab;
    [SerializeField] Vector2 flockHeightRange;

    Vector3 leadingOazoPosition;

    [SerializeField] float distanceBetweenTwoOazo;

    [SerializeField] float timerBetweenFlocks;
    float currentTimeBetweenFlocks;



    // ======================== [DEFAULT UNITY METHODS] ========================

    void Start()
    {
        currentTimeBetweenFlocks = 0;
    }

    void Update()
    {
        currentTimeBetweenFlocks -= Time.deltaTime;

        if (currentTimeBetweenFlocks <= 0)
        {
            currentTimeBetweenFlocks = timerBetweenFlocks;
            SpawnOazoFlock(Random.Range(5, 10));
        }
    }


    // ======================== [SCRIPT METHODS] ========================

    void SpawnOazoFlock(int numberOfOazo)
    {
        // Only have Odd Number of birds
        if (numberOfOazo % 2 == 0)
        { numberOfOazo++; }


        // Select the position of the Leading Oazo

        /*
            ALGO TIME : 

            It should spawn BEHIND the camera
            so :
                - Access Camera.forward, remove the Y component
                - Take its negative value (so straight behing the camera)
                - Offset by a random value
                - Then put it above witht he FlockHeightRange

        */

        // Camera.forward & remove Y
        leadingOazoPosition = Camera.main.transform.forward;
        leadingOazoPosition.y = 0;

        // Invert it
        leadingOazoPosition = -leadingOazoPosition;

        // Multiply by uhhh... idk?
        leadingOazoPosition *= Random.Range(20, 50);

        // Put it above
        leadingOazoPosition.y = Random.Range(flockHeightRange.x, flockHeightRange.y);

        SpawnOneOazo(leadingOazoPosition, 0);


        // The first one is spawned, so we must have only an Even Number
        numberOfOazo--;


        /*
            From the lead :
                - Create a vector which is +X-Z  or -X-Z
                - Multiply it by the row (Mathf.FloorToInt(i/2) + 1) and the distanceBetweenTwoOazo
        */

        Vector3 nextSpawnPos = leadingOazoPosition;
        Vector3 offset = Vector3.zero;

        for (int i = 0; i < numberOfOazo; i++)
        {
            offset = (Vector3.back + Vector3.right + Vector3.back).normalized;

            // if == 0 => Right otherwise Left
            offset.x = (i % 2 == 0) ? offset.x : -offset.x;

            offset *= distanceBetweenTwoOazo * (1 + Mathf.FloorToInt(i/2));

            nextSpawnPos = leadingOazoPosition + offset;

            SpawnOneOazo(nextSpawnPos, (1 + Mathf.FloorToInt(i/2)));
        }

    }

    void SpawnOneOazo(Vector3 position, int rowNumber)
    {
        Instantiate(OazoPrefab, position, Quaternion.identity);
    }

}
