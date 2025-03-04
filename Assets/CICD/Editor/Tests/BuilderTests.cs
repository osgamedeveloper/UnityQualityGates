using NUnit.Framework;
using UnityEditor;

namespace CICD.Tests
{
    public class CICDTests
    {
        [Test]
        public void PlayerSettings_Version_IsNotEmpty()
        {
            string bundleVersion = PlayerSettings.bundleVersion;
            Assert.IsFalse(string.IsNullOrEmpty(bundleVersion), "PlayerSettings isn't valid. bundleVersion is empty");
        }

        [TestCase("true", true)]
        [TestCase("false", false)]
        [TestCase("\"false\"", false)]
        [TestCase("\"true\"", true)]
        public void ArgsBuilderTools_ConvertStringToValue_CorrectBoolean(string value, bool expectedResult)
        {
            bool parsedValue = BuilderTools.ConvertStringToValue<bool>(value);
            Assert.AreEqual(expectedResult, parsedValue);
        }

        [TestCase("1", 1)]
        [TestCase("-1", -1)]
        public void ArgsBuilderTools_ConvertStringToValue_CorrectInt(string value, int expectedResult)
        {
            int parsedValue = BuilderTools.ConvertStringToValue<int>(value);
            Assert.AreEqual(expectedResult, parsedValue);
        }

        [TestCase("\"Android\"", BuildTarget.Android)]
        [TestCase("\"iOS\"", BuildTarget.iOS)]
        [TestCase("Android", BuildTarget.Android)]
        [TestCase("iOS", BuildTarget.iOS)]
        public void ArgsBuilderTools_ConvertStringToValue_CorrectBuildTarget(string value, BuildTarget expectedResult)
        {
            BuildTarget parsedValue = BuilderTools.ConvertStringToValue<BuildTarget>(value);
            Assert.AreEqual(expectedResult, parsedValue);
        }

        [TestCase("")]
        [TestCase("qq")]
        public void ArgsBuilderTools_TryGetBuildParameter_NoError(string value)
        {
            bool result = BuilderTools.TryGetBuildParameter(value, out string par);
            Assert.IsFalse(result);
            Assert.IsTrue(string.IsNullOrEmpty(par));
        }

        [TestCase("")]
        [TestCase("qq")]
        public void ArgsBuilderTools_HaveBuildParameter_NoError(string value)
        {
            bool result = BuilderTools.HasBuildParameter(value);
            Assert.IsFalse(result);
        }

        [TestCase("")]
        [TestCase("qq")]
        public void ArgsBuilderTools_GetBuildParameter_NoError(string value)
        {
            string result = BuilderTools.GetBuildParameter(value, string.Empty);
            Assert.IsTrue(string.IsNullOrEmpty(result));
        }
    }
}
