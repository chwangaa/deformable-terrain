using UnityEngine;
using System;
using WorldScene.Visualizers;
using Improbable.Debug;

namespace Improbable
{
	class DebugVisualizer : MonoBehaviour, IVisualizer
	{
		[Data]
		private IText Text;

		[Data]
		private IColor Color;

		void OnEnable ()
		{
			gameObject.AddComponent<SpeechBubble> ();
			Color.ValueUpdated += HandleValueUpdated;
			Text.EmitText += HandleEmitText;
		}

		void HandleEmitText (Improbable.Debug.Events.Text.EmitText obj)
		{
			GetComponent<SpeechBubble> ().addNewMessage (obj.Content);
		}

		void OnDisable()
		{
			Destroy (GetComponent<SpeechBubble> ());
		}

		void HandleValueUpdated (Improbable.Math.Vector3f color)
		{
			GetComponent<Renderer> ().material.color = new UnityEngine.Color (color.x, color.y, color.z);
		}

		void OnGUI() {
			var camera = Camera.current;
			if (camera != null) {
				var pos = camera.WorldToScreenPoint (transform.position + Vector3.up * 0.5f);

				if (pos.z > 0.5f && pos.z < 100.0f) {
			
					var centeredStyle = new GUIStyle();
					GUI.contentColor = UnityEngine.Color.black;
					centeredStyle.alignment = TextAnchor.MiddleCenter;

					var depthModifier = 1.0f;
					if (pos.z > 1.0f) {
						depthModifier = pos.z;
					}

					centeredStyle.fontSize = Mathf.RoundToInt (1000.0f / depthModifier);
					centeredStyle.richText = true;
					GUI.Label (new Rect (pos.x - 200, Screen.height - pos.y - 200, 400, 400), Text.Content, centeredStyle);
				}
			}
		}
	}
}