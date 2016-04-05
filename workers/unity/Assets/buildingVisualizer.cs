using UnityEngine;
using System.Collections;
using Improbable.Entity.Physical;
using Improbable.Unity.Visualizer;
using Improbable.Unity;
using Improbable.Terrainchunk;

public class buildingVisualizer : MonoBehaviour {

    [Require]
    protected BuildingReader building_reader;

	// Use this for initialization
	void Start () {
	
	}
	
    void OnEnable()
    {
        int width = building_reader.Width;
        int height = building_reader.Height;
        int length = building_reader.Length;
        gameObject.transform.localScale = new Vector3(length, height, width);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
