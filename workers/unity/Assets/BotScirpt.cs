using UnityEngine;
using Improbable.Unity.Visualizer;
using Improbable.Physical;

public class BotScirpt : MonoBehaviour {

    [Require]
    private BlaReader blaReader;

    // Use this for initialization
    void Start () {
	
	}

    void OnEnable()
    {
        blaReader.BlaDyingRequested += HandleBlaDyingRequest;
    }

    void HandleBlaDyingRequest(BlaDyingRequestedEvent obj)
    {
        var center = gameObject.transform.position;
        Instantiate(Resources.Load("EntityPrefabs/Explosion02"), center, Quaternion.identity);
    }

    // Update is called once per frame
    void Update () {
	    
	}
}
