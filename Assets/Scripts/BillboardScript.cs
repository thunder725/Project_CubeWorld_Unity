using UnityEngine;

public class BillboardScript : MonoBehaviour
{
    public enum PotentialAxes { RedAxis, GreenAxis, BlueAxis };
    public PotentialAxes billboardAxis;


    public bool isBillboardActive;
    void Update()
    {
        if (isBillboardActive)
        {

            switch(billboardAxis)
            {
                case PotentialAxes.RedAxis:
                    transform.right = Camera.main.transform.forward;
                    break;

                case PotentialAxes.GreenAxis:
                    transform.up = Camera.main.transform.forward;
                    break;

                case PotentialAxes.BlueAxis:
                    transform.forward = Camera.main.transform.forward;
                    break;
            }
            
        }
    }
}
