using UnityEngine;
using Improbable.Unity.Visualizer;
using Improbable.Physical;

public class RaycastVisualizer : MonoBehaviour
{
    [Require]
    private RaycastRequestReader raycastRequest;
    [Require]
    private RaycastResponseWriter raycastResponse;
    void OnEnable()
    {
        raycastRequest.RaycastRequested += HandleRaycastRequested;
    }

    void HandleRaycastRequested(RaycastRequestedEvent obj)
    {
        /* Raycast magik! */
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        RaycastHit hitResult;

        // Cast a ray from the player's position, pointing forward and write the result
        // to the `hitResult` variable.
        if (Physics.Raycast(transform.position, forward, out hitResult))
        {
            Debug.Log("ray casted hit an object");

            // The ray hit something!
            var entityId = hitResult.transform.gameObject.EntityId();
            raycastResponse.Update.TriggerRaycastResponded(entityId).FinishAndSend();


        }
        else
        {
            Debug.Log("ray casted did not hit any object");
        }
    }
}