using Improbable.Unity.Visualizer;
using UnityEngine;
using Improbable.Debug;

namespace Improbable
{
	class DebugVisualizer : MonoBehaviour
	{
		[Require] public TextReader Text;

        [Require] public ColorReader Color;

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
	}
}