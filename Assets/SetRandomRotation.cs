using UnityEngine;

public class SetRandomRotation : MonoBehaviour
{
    public Transform rotatorTransform;
    
    void Awake()
    {
        var eulerAngles = rotatorTransform.eulerAngles;
        eulerAngles = new Vector3(
            eulerAngles.x,
            eulerAngles.y,
            eulerAngles.z + Random.Range(0, 350f)
        );

        rotatorTransform.eulerAngles = eulerAngles;
    }
}
