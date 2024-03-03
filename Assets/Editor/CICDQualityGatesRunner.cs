using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Exception = System.Exception;
using Utils;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;

namespace CICD
{
    public class CICDQualityGatesRunner
    {
        private static int secondsTimeout = 300;

        #region CICD API

        // ReSharper disable once UnusedMember.Global
        [MenuItem("CICD/Run QG locally")]
        public static void RunQualityGates()
        {
            RunQualityGatesAsync().FireAndForgetAsync(delegate(Exception exception)
            {
                EditorApplication.Exit(1);
            });

        }

        #endregion

        private static async Task RunQualityGatesAsync()
        {
            string outputPath = "";
            if (Application.isBatchMode && !BuilderTools.TryGetBuildParameter("outputPath", out outputPath))
            {
                EditorApplication.Exit(1);
                return;
            }

            if (string.IsNullOrWhiteSpace(outputPath))
            {
                outputPath = $"cicdresults/qg/runResult.txt";
            }

            BuilderTools.InitializePath(outputPath);

            List<IQualityGate> gates = new (4)
            {
                new TestsQualityGate(),
                new MissedRefsQualityGate(),
            };

            foreach (IQualityGate gate in gates)
            {
                int waitedSeconds = 0;

                gate.Run();
                while (gate.Status == QualityGateStatus.Running)
                {
                    if (secondsTimeout < waitedSeconds)
                    {
                        gate.ForceStop();
                        break;
                    }
                    await Task.Delay(1000);
                    waitedSeconds++;
                }
            }

            bool qgPassed = gates.All(gate => gate.Status == QualityGateStatus.Passed);

            var toJsonClass = new
            {
                QualityGateResult = qgPassed ? "Passed" : "Failed",
                results = gates.SelectMany(a => a.GetResults()).ToArray(), 
            };
            var json = JsonConvert.SerializeObject(toJsonClass, Formatting.Indented);

            File.WriteAllText(outputPath, json);

            if (Application.isBatchMode)
                EditorApplication.Exit(qgPassed ? 0 : 1);
            else
                EditorUtility.RevealInFinder(outputPath);
        }
    }
}