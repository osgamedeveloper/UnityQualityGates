using System;

namespace CICD
{
    /// <summary>
    /// General tests serialization format for gitlab
    /// </summary>
    [Serializable]
    public class QualityGateResult
    {
        //Fields should be private set but standart JsonUtility only works with this.
        //In case of newtonsoft we can use private setters.
        public bool Passed;
        public string Name;
        public string Classname;
        public string SystemOut;
        public string FailureMessage;

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
