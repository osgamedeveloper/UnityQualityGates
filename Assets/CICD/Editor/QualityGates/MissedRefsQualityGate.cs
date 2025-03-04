using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CICD
{
    /// <summary>
    /// Quality gate that checks all existing scenes and prefabs for missing references.
    /// </summary>
    public class MissedRefsQualityGate : IQualityGate
    {
        private const string PrefabFilter = "t:Prefab";

        private readonly List<QualityGateResult> _results = new();
        public QualityGateStatus Status { get; private set; }
        public string Name => nameof(MissedRefsQualityGate);
        public string Info { get; private set; }

        public void Run()
        {
            Status = QualityGateStatus.Running;

            try
            {
                var prefabsWithMissing = AssetDatabase.FindAssets(PrefabFilter)
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .Select(AssetDatabase.LoadAssetAtPath<GameObject>)
                    .Where(prefab => prefab != null && HasMissingReferences(prefab, true));

                foreach (var prefab in prefabsWithMissing)
                {
                    _results.Add(new QualityGateResult(
                        false, Info, Name,
                        QualityGateStatus.Failed.ToString(),
                        $"Missing scripts in prefab: {prefab.name}"));
                }

                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    var scene = SceneManager.GetSceneAt(i);

                    if (string.IsNullOrEmpty(scene.path) || !scene.path.Contains("Assets"))
                        continue;

                    var currentScene = EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Single);
                    var missedObjects = currentScene.GetRootGameObjects()
                        .Where(obj => HasMissingReferences(obj, false))
                        .Distinct()
                        .ToList();

                    int missingRefCount = missedObjects.Count;

                    foreach (var obj in missedObjects)
                    {
                        _results.Add(new QualityGateResult(
                            false, Info, Name,
                            QualityGateStatus.Failed.ToString(),
                            $"Missing scripts in scene '{scene.name}' on object '{obj.name}'"));
                    }

                    _results.Add(new QualityGateResult(
                        missingRefCount == 0, Info, Name,
                        missingRefCount == 0 ? QualityGateStatus.Passed.ToString() : QualityGateStatus.Failed.ToString(),
                        missingRefCount == 0
                            ? $"Scene '{scene.name}' has no missing references."
                            : $"Scene '{scene.name}' has {missingRefCount} missing references."));
                }

                if (_results.All(result => result.Passed))
                {
                    _results.Add(new QualityGateResult(
                        true, Name, Name, QualityGateStatus.Passed.ToString(), Info));
                    Status = QualityGateStatus.Passed;
                    return;
                }
            }
            catch (Exception e)
            {
                _results.Add(new QualityGateResult(
                    false, e.GetType().ToString(), Name, QualityGateStatus.Failed.ToString(), e.Message));
                Status = QualityGateStatus.Failed;
                return;
            }

            Status = QualityGateStatus.Failed;
        }

        public IReadOnlyList<QualityGateResult> GetResults() => _results;

        public void ForceStop()
        {
            _results.Add(new QualityGateResult(
                false, QualityGateStatus.ForceStopped.ToString(), Name, QualityGateStatus.Failed.ToString(),
                QualityGateStatus.ForceStopped.ToString()));
            Status = QualityGateStatus.ForceStopped;
        }

        private static bool HasMissingReferences(GameObject go, bool includeChildren)
        {
            var components = includeChildren ? go.GetComponentsInChildren<Component>() : go.GetComponents<Component>();
            return components.Any(component => component == null);
        }
    }
}
