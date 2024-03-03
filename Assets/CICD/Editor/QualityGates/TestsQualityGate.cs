using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.TestTools.TestRunner.Api;

namespace CICD
{
    /// <summary>
    /// Quality Gate runs all existed editor mode tests and provide the quality gate result
    /// </summary>
    public class TestsQualityGate : IQualityGate
    {
        private readonly List<QualityGateResult> _qualityGateResults = new List<QualityGateResult>();
        public QualityGateStatus Status { get; private set; }
        public long Time { get; private set; } = 0;
        public string Name => $"{nameof(TestsQualityGate)}";
        public string Info { get; private set; }

        public void Run()
        {
            Info = "";
            Status = QualityGateStatus.Running;
            BuilderTools.RunTests(ResultCallback);
        }

        private void ResultCallback(ITestResultAdaptor result)
        {
            PassTestResult(result);
            if (result.FailCount > 0)
            {
                Info = $"{nameof(TestsQualityGate)} is failed.";
            }
            else
            {
                _qualityGateResults.Add(new QualityGateResult(
                    false, Name, Name, Status.ToString(), Info));
            }
            Status = result.FailCount <= 0 ? QualityGateStatus.Passed : QualityGateStatus.Failed;
        }

        public List<QualityGateResult> GetResults()
        {
            return _qualityGateResults;
        }

        public void ForceStop()
        {
            _qualityGateResults.Add(new QualityGateResult(
                false, QualityGateStatus.ForceStopped.ToString(), Name, QualityGateStatus.Failed.ToString(),
                QualityGateStatus.ForceStopped.ToString()));
            Status = QualityGateStatus.ForceStopped;
        }

        private void PassTestResult(ITestResultAdaptor resultAdaptor)
        {
            if (resultAdaptor.HasChildren)
            {
                foreach (var child in resultAdaptor.Children)
                {
                    PassTestResult(child);
                }
            }

            if (!resultAdaptor.HasChildren)
            {
                bool isPassed = resultAdaptor.ResultState.Equals("Passed", StringComparison.OrdinalIgnoreCase);
                string testName =
                    $"{resultAdaptor.Test.FullName.Split('(', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()}";
                //We leave resultAdaptor.Duration what we obviously should use in a real project.
                _qualityGateResults.Add(
                    new QualityGateResult(
                        isPassed,
                        Name,
                        testName,
                        resultAdaptor.Message,
                        isPassed ? "" : $"{resultAdaptor.Message}"));
            }
        }
    }
}
