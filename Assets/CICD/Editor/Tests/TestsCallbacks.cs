using System;
using UnityEditor.TestTools.TestRunner.Api;

namespace CICD
{
    /// <summary>
    /// Implements test callbacks for Unity's Test Runner.
    /// </summary>
    public class TestsCallbacks : ICallbacks
    {
        /// <summary>
        /// Event triggered when test run is finished.
        /// </summary>
        public event Action<ITestResultAdaptor> ResultCallback;

        public TestsCallbacks() { }

        public TestsCallbacks(Action<ITestResultAdaptor> callback)
        {
            ResultCallback += callback;
        }

        /// <summary>
        /// Called when all tests have finished running.
        /// </summary>
        public void RunFinished(ITestResultAdaptor result)
        {
            ResultCallback?.Invoke(result);
        }

        public void RunStarted(ITestAdaptor testsToRun) { }

        public void TestFinished(ITestResultAdaptor result) { }

        public void TestStarted(ITestAdaptor test) { }
    }
}
