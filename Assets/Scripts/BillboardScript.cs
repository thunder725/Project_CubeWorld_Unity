using UnityEngine;

public class BillboardScript : MonoBehaviour
{
    public bool isBillboardActive;
    void Update()
    {
        if (isBillboardActive)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
