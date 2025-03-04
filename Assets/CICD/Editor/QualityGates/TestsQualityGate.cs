using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.TestTools.TestRunner.Api;

namespace CICD
{
    /// <summary>
    /// Quality Gate runs all existing editor mode tests and provides the quality gate result.
    /// </summary>
    public class TestsQualityGate : IQualityGate
    {
        private const string PassedKey = "Passed";
        private readonly List<QualityGateResult> _qualityGateResults = new();
        public QualityGateStatus Status { get; private set; }
        public long Time { get; private set; } = 0;
        public string Name => nameof(TestsQualityGate);
        public string Info { get; private set; } = string.Empty;

        public void Run()
        {
            Info = string.Empty;
            Status = QualityGateStatus.Running;
            BuilderTools.RunTests(ResultCallback);
        }

        private void ResultCallback(ITestResultAdaptor result)
        {
            PassTestResult(result);
            if (result.FailCount > 0)
                Info = $"{nameof(TestsQualityGate)} failed.";
            else
                _qualityGateResults.Add(new QualityGateResult(true, Name, Name, Status.ToString(), Info));

            Status = result.FailCount == 0 ? QualityGateStatus.Passed : QualityGateStatus.Failed;
        }

        public IReadOnlyList<QualityGateResult> GetResults() => _qualityGateResults;

        public void ForceStop()
        {
            _qualityGateResults.Add(new QualityGateResult(
                false,
                QualityGateStatus.ForceStopped.ToString(),
                Name,
                QualityGateStatus.Failed.ToString(),
                QualityGateStatus.ForceStopped.ToString()));
            Status = QualityGateStatus.ForceStopped;
        }

        private void PassTestResult(ITestResultAdaptor resultAdaptor)
        {
            if (resultAdaptor.HasChildren)
            {
                foreach (var child in resultAdaptor.Children)
                    PassTestResult(child);
            }
            else
            {
                bool isPassed = resultAdaptor.ResultState.Equals(PassedKey, StringComparison.OrdinalIgnoreCase);
                string testName = resultAdaptor.Test.FullName.Split('(', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "UnknownTest";

                _qualityGateResults.Add(new QualityGateResult(
                    isPassed,
                    Name,
                    testName,
                    resultAdaptor.Message ?? string.Empty,
                    isPassed ? string.Empty : resultAdaptor.Message ?? string.Empty));
            }
        }
    }
}