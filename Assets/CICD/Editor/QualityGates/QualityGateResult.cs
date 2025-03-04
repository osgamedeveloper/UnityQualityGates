using System;

namespace CICD
{
    /// <summary>
    /// General tests serialization format for GitLab.
    /// </summary>
    [Serializable]
    public class QualityGateResult
    {
        // Fields should have private setters, but standard JsonUtility only works with public fields.
        // In case of Newtonsoft.Json, we can use private setters.
        public bool Passed { get; set; }
        public string Name { get; set; }
        public string Classname { get; set; }
        public string SystemOut { get; set; }
        public string FailureMessage { get; set; }

        public QualityGateResult(bool passed, string name, string classname, string systemOut, string failureMessage)
        {
            Passed = passed;
            Name = name;
            Classname = classname;
            SystemOut = systemOut;
            FailureMessage = failureMessage;
        }
    }
}