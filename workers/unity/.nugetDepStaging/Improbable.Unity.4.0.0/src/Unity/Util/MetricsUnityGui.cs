using System.Collections.Generic;
using System.Linq;
using Improbable.Util.Collections;
using Improbable.Util.Metrics;
using UnityEngine;

namespace Improbable.Unity.Util
{
    public class MetricsUnityGui : MonoBehaviour
    {
        private Vector2 scrollPosition;
        private bool showUnityMetrics;
        private bool showObjectPools;
        private Texture2D backgroundTexture;
        private GUIStyle backgroundStyle;
        private List<ObjectPoolBase> pools = new List<ObjectPoolBase>();

        private void Awake()
        {
            backgroundTexture = new Texture2D(1,1);
            backgroundTexture.SetPixel(0,0, new Color(0,0,0,0.7f));
            backgroundTexture.Apply();

            backgroundStyle = new GUIStyle { normal = { background = backgroundTexture } };
        }

        private void OnDestroy()
        {
            Destroy(backgroundTexture);
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 50, Screen.width * 0.2f, Screen.height * 0.5f));
            GUILayout.BeginVertical();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false);

            showUnityMetrics = GUILayout.Toggle(showUnityMetrics, "Metrics");

            if (showUnityMetrics)
            {
                GUILayout.BeginVertical(backgroundStyle);

                var keys = MetricsUpdatersManager.AllMetrics.Keys.ToList();
                keys.Sort();

                for (var i = 0; i < keys.Count; i++)
                {
                    GUILayout.BeginHorizontal();

                    var key = keys[i];
                    GUILayout.Label(key);
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(MetricsUpdatersManager.AllMetrics[key].Value.ToString());

                    GUILayout.EndHorizontal();                    
                }

                GUILayout.EndVertical();
            }

            showObjectPools = GUILayout.Toggle(showObjectPools, "Object Pools");
            if (showObjectPools)
            {
                GUILayout.BeginVertical(backgroundStyle);
                ObjectPoolBase.GetPools(pools);

                GUILayout.BeginHorizontal();

                GUILayout.Label("Name");
                GUILayout.FlexibleSpace();
                
                GUILayout.Label("Active");
                GUILayout.Space(8);
                GUILayout.Label("Free");

                GUILayout.EndHorizontal();     
                
                for (var i = 0; i < pools.Count; i++)
                {
                    var pool = pools[i];
                    GUILayout.BeginHorizontal();

                    GUILayout.Label(pool.Name);
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(pool.ActiveObjectCount.ToString());
                    GUILayout.Space(8);
                    GUILayout.Label(pool.FreeObjectCount.ToString());

                    GUILayout.EndHorizontal();     
                    
                }
                GUILayout.EndVertical();
            }

            GUILayout.EndScrollView();

            GUILayout.EndVertical();
            GUILayout.EndArea();                
        }
    }
}