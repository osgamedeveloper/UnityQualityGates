using System.Collections.Generic;

namespace CICD
{
    public interface IQualityGate
    {
        QualityGateStatus Status { get; }
        string Name { get; }
        string Info { get; }

        void Run();
        void ForceStop();
        IReadOnlyList<QualityGateResult> GetResults();
    }
}
