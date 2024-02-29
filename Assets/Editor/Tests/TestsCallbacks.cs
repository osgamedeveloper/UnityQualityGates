using System;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

namespace CICD
{
    public class TestsCallbacks : ICallbacks
    {
        public Action<ITestResultAdaptor> ResultCallback;
        public void RunFinished(ITestResultAdaptor result)
        {
            ResultCallback?.Invoke(result);
        }

        public void RunStarted(ITestAdaptor testsToRun)
        {
        }

        public void TestFinished(ITestResultAdaptor result)
        {
        }

        public void TestStarted(ITestAdaptor test)
        {
        }
    }
}
