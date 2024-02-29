using System.Collections.Generic;

namespace CICD
{
    public interface IQualityGate
    {
        public QualityGateStatus Status { get; }
        public string Name{ get; }
        public string Info{ get; }
        public void Run();
        public void ForceStop();
        public List<QualityGateResult> GetResults();
    }
}
