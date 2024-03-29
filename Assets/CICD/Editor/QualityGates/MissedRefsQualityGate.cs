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
    /// Quality gate checks all existed scenes and prefabs for missing refs
    /// </summary>
    public class MissedRefsQualityGate : IQualityGate
    {
        private const string filterPrefabs = "t:Prefab";

        private readonly List<QualityGateResult> _results = new();
        public QualityGateStatus Status { get; private set; }
        public string Name => $"{nameof(MissedRefsQualityGate)}";
        public string Info { get; private set; }

        public void Run()
        {
            Status = QualityGateStatus.Running;

            try
            {
                var prefabsWithMissing = AssetDatabase.FindAssets(filterPrefabs)
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .Select(AssetDatabase.LoadAssetAtPath<GameObject>);

                foreach (var prefabWithMissed in prefabsWithMissing)
                {
                    bool hasMissed = IsMissing(prefabWithMissed.gameObject, true);
                    if (hasMissed)
                    {
                        _results.Add(
                            new QualityGateResult(
                            false, Info, Name,
                            QualityGateStatus.Failed.ToString(), 
                            $"Missed scrpits in the prefab : {prefabWithMissed.name}"));
                    }
                }

                for (int sceneId = 0; sceneId < SceneManager.sceneCount; sceneId++)
                {
                    var scene = SceneManager.GetSceneAt(sceneId);

                    if (string.IsNullOrEmpty(scene.path) || !scene.path.Contains("Assets"))
                        continue;

                    var currentScene = EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Single);

                    var missedScriptsOnScene = currentScene.GetRootGameObjects()
                        .Where(x => IsMissing(x, false))
                        .Distinct()
                        .ToList();
                    int amountOfMissedRefs = 0;
                    foreach (var missedScriptOnScene in missedScriptsOnScene)
                    {
                        _results.Add(new QualityGateResult(
                            false, Info, Name,
                            QualityGateStatus.Failed.ToString(), 
                            $"Missed scripts on the scene: {scene.name} in {missedScriptOnScene.name}"));
                        amountOfMissedRefs++;
                    }

                    if (amountOfMissedRefs > 0)
                    {
                        _results.Add(new QualityGateResult(
                            false, Info, Name,
                            QualityGateStatus.Failed.ToString(), 
                            $"The scene {scene.name} has {amountOfMissedRefs} missed refs."));
                    }
                    else
                    {
                        _results.Add(new QualityGateResult(
                            true, Info, Name,
                            systemOut: QualityGateStatus.Passed.ToString(), 
                            $"The scene {scene.name} has no missing refs."));
                    }

                }

                if (_results.All(result => result.Passed))
                {
                    _results.Add(new QualityGateResult(
                        true, 
                        Name, 
                        Name, 
                        QualityGateStatus.Passed.ToString(),
                        Info));
                    Status = QualityGateStatus.Passed;
                    return;
                }
            }
            catch (Exception e)
            {
                _results.Add(new QualityGateResult(
                    false, e.GetType().ToString(), Name, QualityGateStatus.Failed.ToString(),
                    e.Message));

                Status = QualityGateStatus.Failed;
                return;
            }

            Status = QualityGateStatus.Failed;
        }

        public List<QualityGateResult> GetResults()
        {
            return _results;
        }

        public void ForceStop()
        {
            _results.Add(new QualityGateResult(
                false, QualityGateStatus.ForceStopped.ToString(), Name, QualityGateStatus.Failed.ToString(),
                QualityGateStatus.ForceStopped.ToString()));
            Status = QualityGateStatus.ForceStopped;
        }

        private static bool IsHasCanvasOverlay(GameObject go)
        {
            if (go.GetComponent<Canvas>() != null)
            {
                if (go.GetComponent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay)
                {
                    return go.GetComponent<Canvas>();
                }
            }

            return false;
        }

        private static bool IsMissing(GameObject go, bool includeChildren)
        {
            var components = includeChildren
                ? go.GetComponentsInChildren<Component>()
                : go.GetComponents<Component>();

            return components.Any(x => x == null);
        }
    }
}
