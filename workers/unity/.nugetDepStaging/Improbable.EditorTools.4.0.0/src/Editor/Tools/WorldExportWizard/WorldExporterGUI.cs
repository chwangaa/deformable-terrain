using System.Collections.Generic;
using System.Linq;
using Improbable.Unity.EditorTools.Util;
using Improbable.Unity.EditorTools.Util.Validators;
using Improbable.Unity.EditorTools.WorldExportWizard.ObjExporter.TerrainExporter;
using UnityEditor;
using UnityEngine;

namespace Improbable.Unity.EditorTools.WorldExportWizard
{
    public class WorldExporterGUI : EditorWindow
    {
        private const string WORLD_NAME_LABEL = "World Name";
        private static string worldName = "WorldName";

        private static TerrainSaveResolution terrainResolution = TerrainSaveResolution.Eighth;

        private static bool exportWorldAssetBundle = true;
        private static bool exportNavmesh = true;
        private static bool exportJson = true;

        private static readonly List<Validator<string>> WORLD_NAME_VALIDATORS = new List<Validator<string>>
        {
            new NotEmpty("You must enter a world name."),
            new OnlyAlphanumeric("The name of a world should contain only letters and numbers."),
            new FirstLetterUppercase("The first letter of a world's name should be uppercase.")
        };

        [MenuItem("Improbable/Export World... %&#W")]
        public static void OnUpdateWorld()
        {
            GetWindow<WorldExporterGUI>().Show();
        }

        public void OnGUI()
        {
            DisplayTerrainOptions();
            DisplayExportOptions();

            var violations = Validation<string>.GetViolations(WORLD_NAME_VALIDATORS, worldName).ToList();

            EditorGUI.BeginDisabledGroup(violations.Any());
            DisplayExportButton();
            EditorGUI.EndDisabledGroup();

            foreach (var violation in violations)
            {
                EditorGUILayout.HelpBox(violation, MessageType.Warning, true);
            }
        }

        private static void DisplayTerrainOptions()
        {
            EditorGUILayout.LabelField(WORLD_NAME_LABEL, EditorStyles.boldLabel);
            worldName = EditorGUILayout.TextField(WORLD_NAME_LABEL, worldName);
            terrainResolution = (TerrainSaveResolution) EditorGUILayout.EnumPopup("Terrain Resolution", terrainResolution);
        }

        private static void DisplayExportOptions()
        {
            exportWorldAssetBundle = EditorGUILayout.Toggle("Export Asset Bundle", exportWorldAssetBundle);
            exportNavmesh = EditorGUILayout.Toggle("Export Navmesh", exportNavmesh);
            exportJson = EditorGUILayout.Toggle("Export JSON", exportJson);
        }

        private static void DisplayExportButton()
        {
            if (GUILayout.Button("Export"))
            {
                WorldExporter.Export(worldName, exportWorldAssetBundle, exportNavmesh, exportJson, terrainResolution);
            }
        }
    }
}