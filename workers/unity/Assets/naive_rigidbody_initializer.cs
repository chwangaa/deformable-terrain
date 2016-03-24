using UnityEngine;
using Improbable.Entity.Physical;
using Improbable.Unity.Visualizer;
using Improbable.Unity;

public class naive_rigidbody_initializer : MonoBehaviour {
    [Require]
    protected PositionReader Position;
    // Use this for initialization
    void Start () {
	
	}

    void onEnable()
    {
        Debug.Log("naive rigidbody set the position");
        transform.position = Position.Value.ToUnityVector();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
