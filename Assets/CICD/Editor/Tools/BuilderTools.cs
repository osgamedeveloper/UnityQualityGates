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

        /// <summary>
        /// Creates the directory for the build path if it does not exist.
        /// </summary>
        public static string InitializePath(string buildPath)
        {
            string directoryPath = Path.GetDirectoryName(buildPath);
            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            return buildPath;
        }

        /// <summary>
        /// Checks if a specific parameter exists in the command-line arguments.
        /// </summary>
        public static bool HasBuildParameter(string key)
        {
            string searchedKey = $"-{key}";
            return Args.Any(arg => arg.Equals(searchedKey, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Retrieves the value of a command-line parameter or returns a default value if not found.
        /// </summary>
        public static T GetBuildParameter<T>(string key, T defaultValue)
        {
            string searchedKey = $"-{key}";

            for (int i = 0; i < Args.Length - 1; i++) // Fixed condition
            {
                if (Args[i].Equals(searchedKey, StringComparison.OrdinalIgnoreCase))
                {
                    return ConvertStringToValue<T>(Args[i + 1]);
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Attempts to retrieve a command-line parameter.
        /// </summary>
        public static bool TryGetBuildParameter<T>(string key, out T parameter)
        {
            parameter = default;
            if (string.IsNullOrEmpty(key)) return false;

            string searchedKey = key.StartsWith("-") ? key : $"-{key}";

            for (int i = 0; i < Args.Length - 1; i++) // Reduced range
            {
                if (Args[i].Equals(searchedKey, StringComparison.OrdinalIgnoreCase))
                {
                    string value = Args[i + 1];

                    if (value.Contains("{") && value.Contains("}"))
                    {
                        return false; // Invalid argument format
                    }

                    parameter = ConvertStringToValue<T>(value);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Executes unit tests and provides a callback for the results.
        /// </summary>
        public static void RunTests(Action<ITestResultAdaptor> resultCallback)
        {
            var testsCallbacks = new TestsCallbacks(resultCallback);

            var testRunnerApi = ScriptableObject.CreateInstance<TestRunnerApi>();
            testRunnerApi.RegisterCallbacks(testsCallbacks);
            testRunnerApi.Execute(new ExecutionSettings
            {
                filters = new[] { new Filter { testMode = TestMode.EditMode } }
            });
        }

        /// <summary>
        /// Converts a string to the specified data type.
        /// </summary>
        public static T ConvertStringToValue<T>(string arg)
        {
            if (typeof(T) != typeof(string))
                arg = arg.Replace("\"", "");

            try
            {
                if (typeof(T).IsEnum)
                {
                    if (Enum.IsDefined(typeof(T), arg))
                    {
                        return (T)Enum.Parse(typeof(T), arg);
                    }
                    throw new ArgumentException($"Value '{arg}' is not a valid enum for {typeof(T).Name}");
                }

                return (T)Convert.ChangeType(arg, typeof(T));
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error converting '{arg}' to {typeof(T).Name}: {ex.Message}");
                return default;
            }
        }
    }
}