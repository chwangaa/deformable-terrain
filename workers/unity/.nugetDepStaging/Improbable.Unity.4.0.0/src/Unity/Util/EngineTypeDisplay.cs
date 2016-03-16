using Improbable.Unity.Core;
using UnityEngine;

namespace Improbable.Unity.Util
{
    public class EngineTypeDisplay : MonoBehaviour
    {
        private static readonly Rect NAME_LABEL_POSITION = new Rect(10, 5, 300, 30);
        private string engineType = "";

        private void Start()
        {
            engineType = EngineConfiguration.Instance.EngineType.ToString();
        }

        private void OnGUI()
        {
            GUI.Label(NAME_LABEL_POSITION, engineType);
        }
    }
}
