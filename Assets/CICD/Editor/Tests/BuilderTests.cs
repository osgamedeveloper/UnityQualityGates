using NUnit.Framework;
using UnityEditor;

namespace CICD.Tests
{
    public class CICDTests
    {
        [TestCase]
        public void PlayerSettings_Version_IsNotEmpty()
        {
            string bundleVersion = PlayerSettings.bundleVersion;

            Assert.AreEqual(true, !string.IsNullOrEmpty(bundleVersion),$"PlayerSettings isn't valid. bundleVersion is empty");
        }

        [TestCase("true", true)]
        [TestCase("false", false)]
        [TestCase("\"false\"", false)]
        [TestCase("\"true\"", true)]
        public void ArgsBuilderTools_ConvertStringToValue_CorrectBoolean(string value, bool result)
        {
            bool parsedValue = BuilderTools.ConvertStringToValue<bool>(value);

            Assert.AreEqual(result, parsedValue);
        }

        [TestCase("1", 1)]
        [TestCase("-1", -1)]
        public void ArgsBuilderTools_ConvertStringToValue_CorrectInt(string value, int result)
        {
            int parsedValue = BuilderTools.ConvertStringToValue<int>(value);

            Assert.AreEqual(result, parsedValue);
        }

        [TestCase("\"Android\"", BuildTarget.Android)]
        [TestCase("\"iOS\"", BuildTarget.iOS)]
        [TestCase("Android", BuildTarget.Android)]
        [TestCase("iOS", BuildTarget.iOS)]
        public void ArgsBuilderTools_ConvertStringToValue_CorrectBuildTarget(string value, BuildTarget result)
        {
            BuildTarget parsedValue = BuilderTools.ConvertStringToValue<BuildTarget>(value);

            Assert.AreEqual(result, parsedValue);
        }

        [TestCase("")]
        [TestCase("qq")]
        public void ArgsBuilderTools_TryGetBuildParameter_NoError(string value)
        {
            bool result = BuilderTools.TryGetBuildParameter(value, out string par);

            Assert.AreEqual(false, result);
            Assert.AreEqual(true, string.IsNullOrEmpty(par));
        }

        [TestCase("")]
        [TestCase("qq")]
        public void ArgsBuilderTools_HaveBuildParameter_NoError(string value)
        {
            bool result = BuilderTools.HasBuildParameter(value);

            Assert.AreEqual(false, result);
        }

        [TestCase("")]
        [TestCase("qq")]
        public void ArgsBuilderTools_GetBuildParameter_NoError(string value)
        {
            string result = BuilderTools.GetBuildParameter(value, "");

            Assert.AreEqual(true, string.IsNullOrEmpty(result));
        }

        //Example with errors
        [TestCase("\"\"", BuildTarget.Android)]
        [TestCase("\"\"", BuildTarget.iOS)]
        [TestCase("", BuildTarget.Android)]
        [TestCase("c", BuildTarget.iOS)]
        public void ArgsBuilderTools_ConvertStringToValue_CorrectBuildTargetErrorsForATest(string value, BuildTarget result)
        {
            BuildTarget parsedValue = BuilderTools.ConvertStringToValue<BuildTarget>(value);

            Assert.AreEqual(result, parsedValue);
        }
    }
}