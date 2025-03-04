using System;
using UnityEngine;
using UnityEditor.TestTools.TestRunner.Api;

namespace CICD
{
    public class TestTools
    {
        public static void RunTests(Action<ITestResultAdaptor> resultCallback)
        {
            TestsCallbacks testsCallbacks = new TestsCallbacks();
            testsCallbacks.ResultCallback += resultCallback;

            var testRunnerApi = ScriptableObject.CreateInstance<TestRunnerApi>();
            testRunnerApi.RegisterCallbacks(testsCallbacks);
            testRunnerApi.Execute(new ExecutionSettings()
            {
                filters = new Filter[] { new Filter() { testMode = TestMode.EditMode, } }
            });
        }
    }
}
