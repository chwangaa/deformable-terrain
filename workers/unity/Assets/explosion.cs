using UnityEngine;
using System.Collections;

public class explosion : MonoBehaviour {

	
    void OnEnable()
    {
        var exp = GetComponent<ParticleSystem>();
        exp.Play();
        Destroy(gameObject, exp.duration);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
