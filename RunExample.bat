@echo off
set ProjectPath=%CD%
set UnityPath="C:\Program Files\Unity\Hub\Editor\6000.0.39f1\Editor\Unity.exe"

%UnityPath% -batchmode -projectPath "%ProjectPath%" -executeMethod CICD.CICDQualityGatesRunner.RunQualityGates -outputPath result.txt

echo Quality gates check completed. Results saved in result.txt.
pause