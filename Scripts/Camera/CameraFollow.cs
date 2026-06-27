using UnityEngine;

public class CameraFollow : Singleton<CameraFollow>
{
    
    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private float followSpeed = 5f;

    private Transform targetTransform;
    private float fixedXPosition;

   
    private void Start()
    {
        // Cache player transform and initial camera offset
        targetTransform = PlayerController.Instance.transform;
        offset = transform.position - targetTransform.position;
        fixedXPosition = transform.position.x;
    }

    

    private void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(
            fixedXPosition,
            transform.position.y,
            targetTransform.position.z + offset.z);

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime);
    }
}