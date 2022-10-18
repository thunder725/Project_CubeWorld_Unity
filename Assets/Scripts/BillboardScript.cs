using UnityEngine;

public class BillboardScript : MonoBehaviour
{
    public enum PotentialAxes { RedAxis, GreenAxis, BlueAxis };
    public PotentialAxes billboardAxis;
    public bool invertAxis;


    public bool isBillboardActive;
    void Update()
    {
        if (isBillboardActive)
        {

            switch(billboardAxis)
            {
                case PotentialAxes.RedAxis:
                    transform.right = Camera.main.transform.forward * (invertAxis? -1 : 1);
                    break;

                case PotentialAxes.GreenAxis:
                    transform.up = Camera.main.transform.forward * (invertAxis ? -1 : 1);
                    break;

                case PotentialAxes.BlueAxis:
                    transform.forward = Camera.main.transform.forward * (invertAxis ? -1 : 1);
                    break;
            }
            
        }
    }
}
