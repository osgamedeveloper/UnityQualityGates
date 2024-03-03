using System;
using System.Linq;
using System.IO;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

namespace CICD
{
    public static class BuilderTools
    {
        private static string[] Args => Environment.GetCommandLineArgs();

        public static string InitializePath(string buildPath)
        {
            string directoryPath = Path.GetDirectoryName(buildPath);
            if (!Directory.Exists(directoryPath))
            {
                if (directoryPath != null)
                {
                    Directory.CreateDirectory(directoryPath);
                }
            }

            return buildPath;
        }

        public static bool HasBuildParameter(string key)
        {
            string searchedKey = $"-{key}";
            return Args.Any(t => t.Equals(searchedKey, StringComparison.OrdinalIgnoreCase));
        }

        public static T GetBuildParameter<T>(string key, T defaultValue)
        {
            string searchedKey = $"-{key}";

            for (int i = 0; i < Args.Length; i++)
            {
                if (Args[i].Equals(searchedKey, StringComparison.OrdinalIgnoreCase))
                {
                    if (Args.Length < i + 1)
                    {
                        return (T)Convert.ChangeType(Args[i + 1], typeof(T));
                    }
                }
            }

            return defaultValue;
        }

        public static bool TryGetBuildParameter<T>(string key, out T parameter)
        {
            if (string.IsNullOrEmpty(key))
            {
                parameter = default;
                return false;
            }

            if (key[0] == '-')
                key = key.Substring(1);

            string searchedKey = $"-{key}";
            parameter = default;

            for (int i = 0; i < Args.Length; i++)
            {
                if (Args[i].Equals(searchedKey, StringComparison.OrdinalIgnoreCase))
                {
                    if (Args.Length > i + 1)
                    {
                        if (Args[i + 1].Contains("{") && Args[i + 1].Contains("}"))
                        {
                            return false;
                        }
                        parameter = ConvertStringToValue<T>(Args[i + 1]);
                    }

                    return true;
                }
            }

            return false;
        }

        public static void RunTests(Action<ITestResultAdaptor> resultCallback)
        {
            var testsCallbacks = new TestsCallbacks();
            testsCallbacks.ResultCallback += resultCallback;

            var testRunnerApi = ScriptableObject.CreateInstance<TestRunnerApi>();
            testRunnerApi.RegisterCallbacks(testsCallbacks);
            testRunnerApi.Execute(new ExecutionSettings()
            {
                filters = new Filter[] { new Filter() { testMode = TestMode.EditMode, } }
            });
        }

        /// <typeparam name="T">value types, enums</typeparam>
        public static T ConvertStringToValue<T>(string arg)
        {
            if (typeof(T) != typeof(string))
                arg = arg.Replace("\"", "");

            if (typeof(T).IsEnum)
            {
                return (T)Enum.Parse(typeof(T), arg);
            }

            return (T)Convert.ChangeType(arg, typeof(T));
        }
    }
}