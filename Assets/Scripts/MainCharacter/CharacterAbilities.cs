using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class CharacterAbilities : MonoBehaviour
{

    public bool ShouldDebugFaceColor;

    // ================ DEFAULT UNITY METHODS ===================




    // ================ SCRIPT SPECIFIC METHODS ===================

    /// <summary>
    /// Get the color of the face on the ground.
    /// </summary>
    /// <returns> Returns 0 if the face is blue or 1 if the face is red. </returns>
    public int GetDownFaceColor()
    {
        // The method will compare the 3 transform vectors
        // (aligned with the Normals of the 3 red faces
        // Get which one is the most vertical
        // And then determine if it's red or blue:


        // Default values will say "the closest is RIGHT"
        // It won't be that in the end but it's for start
        string colinearVectorName = "right";
        float colinearVectorDiffToUp = GetDifferenceWithWorldUp(transform.right);

        float currentlyTesting = 0;



        currentlyTesting = GetDifferenceWithWorldUp(transform.up);
        // Compare the 3 transform vectors to the world up
        if (currentlyTesting < colinearVectorDiffToUp)
        {
            colinearVectorDiffToUp = currentlyTesting;
            colinearVectorName = "up";
        }

        currentlyTesting = GetDifferenceWithWorldUp(transform.forward);
        if (currentlyTesting < colinearVectorDiffToUp)
        {
            colinearVectorDiffToUp = currentlyTesting;
            colinearVectorName = "forward";
        }


        Vector3 checkingVector = Vector3.zero;

        // Check which one it is
        switch (colinearVectorName)
        {
            case "right":
                checkingVector = transform.right;
                break;

            case "up":
                checkingVector = transform.up;
                break;

            case "forward":
                checkingVector = transform.forward;
                break;
        }


        // Know if it's blue (y>0) or red (y<0) on top
        // and return the other one

        if (checkingVector.y > 0)
        {
            // Face on the ground is Red

            if (ShouldDebugFaceColor)
            {
                Debug.Log("The face on the ground is Red. Should Dash");
            }
            return 1;
        }
        else
        {
            // Face on the ground is Blue

            if (ShouldDebugFaceColor)
            {
                Debug.Log("The face on the ground is Blue. Should Jump");
            }
            return 0;
        }
    }

    float GetDifferenceWithWorldUp(Vector3 _vector)
    {
        return (MathPlus.AbsVector(_vector) - Vector3.up).sqrMagnitude;
    }

    




}
