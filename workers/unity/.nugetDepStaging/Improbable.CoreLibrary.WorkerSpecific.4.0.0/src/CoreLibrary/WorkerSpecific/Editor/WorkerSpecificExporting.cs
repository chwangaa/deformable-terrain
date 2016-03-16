using UnityEditor;

namespace Improbable.CoreLibrary.WorkerSpecific.Editor
{
    internal class WorkerSpecificBuildMenuItem
    {
        [MenuItem("Improbable/Worker Specific/Package Development")]
        public static void DevelopmentBuild()
        {
            WorkerSpecificBuild.DevelopmentBuild();
        }

        [MenuItem("Improbable/Worker Specific/Package Deployment")]
        public static void DeploymentBuild()
        {
            WorkerSpecificBuild.DeploymentBuild();
        }
    }
}