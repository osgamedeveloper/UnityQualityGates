using System;

namespace CICD
{
    /// <summary>
    /// General tests serialization format for gitlab
    /// </summary>
    [Serializable]
    public class QualityGateResult
    {
        public bool Passed { get; private set; }
        public string Name { get; private set; }
        public string Classname { get; private set; }
        public long Time { get; private set; }
        public string SystemOut { get; private set; }
        public string FailureMessage { get; private set; }

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
