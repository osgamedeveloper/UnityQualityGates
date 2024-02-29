using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Exception = System.Exception;
using Utils;
using System.IO;

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
                outputPath = $"ci/qg/runResult.txt";
            }

            BuilderTools.InitializePath(outputPath);

            List<IQualityGate> gates = new (4)
            {
                new TestsQualityGate(),
                new MissedRefsQualityGate(),
            };

            if (Application.isBatchMode)
            {
                if (BuilderTools.TryGetBuildParameter("gates", out string gateNames))
                {
                    string[] gateNamesArray = gateNames.Replace(" ","").Split(",").ToArray();

                    List<IQualityGate> newGates = new List<IQualityGate>();
                    foreach (var gate in gates)
                    {
                        if (gateNamesArray.Any(gateName=>gateName.Equals(gate.Name, StringComparison.OrdinalIgnoreCase)))
                            newGates.Add(gate);
                    }

                    gates = newGates;
                }
            }

            string result = "";

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
                //Here should be an xml file compilation (as for a gitlab for example) but for the test work we can stay with it ^^
                result += $"{gate.Name}|{gate.Status}{Environment.NewLine}";
                foreach(var qgResult in gate.GetResults())
                    result += $"{qgResult.Classname}|{(qgResult.Passed?"Passed": qgResult.FailureMessage)}{Environment.NewLine}";
            }

            bool qgPassed = gates.All(gate => gate.Status == QualityGateStatus.Passed);

            result += qgPassed ? "QG Passed":"QG Failed";

            File.WriteAllText(outputPath, result);

            if (Application.isBatchMode)
                EditorApplication.Exit(1);
            else
                EditorUtility.RevealInFinder(outputPath);
        }
    }
}