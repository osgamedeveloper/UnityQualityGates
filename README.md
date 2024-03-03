# UnityQualityGates
Quality Gates for Unity
=======
## Getting Started

This is an example of how you may create a QG setup for your project.

### Prerequisites

Just copy the existing Assets/Editor folder to your project and it's ready to go.

### Installation

1. Copy Assets/Editor folder to your project
2. Use CICD context menu button for additional actions
3. Develop your own QG by implementing IQualityGate.cs interface
   ```cs
   public class MyQualityGate:IQualityGate
   ```
4. Run your method in batchmode by 
   ```sh
   Unity.exe -batchmode -projectPath %ProjectPath% -executeMethod CICD.CICDQualityGatesRunner.RunQualityGates -outputPath result.txt
   ```
5. Example of output:
```result.txt
TestsQualityGate:Failed
CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBoolean:Passed
CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBoolean:Passed
CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBoolean:Passed
CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBoolean:Passed
CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBuildTarget:Passed
CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBuildTarget:Passed
CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBuildTarget:Passed
CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBuildTarget:Passed
CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBuildTargetErrorsForATest:System.ArgumentException : Must specify valid information for parsing in the string.
CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBuildTargetErrorsForATest:System.ArgumentException : Must specify valid information for parsing in the string.
CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBuildTargetErrorsForATest:System.ArgumentException : Must specify valid information for parsing in the string.
CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBuildTargetErrorsForATest:System.ArgumentException : Requested value 'c' was not found.
CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectInt:Passed
CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectInt:Passed
CICD.Tests.CICDTests.ArgsBuilderTools_GetBuildParameter_NoError:Passed
CICD.Tests.CICDTests.ArgsBuilderTools_GetBuildParameter_NoError:Passed
CICD.Tests.CICDTests.ArgsBuilderTools_HaveBuildParameter_NoError:Passed
CICD.Tests.CICDTests.ArgsBuilderTools_HaveBuildParameter_NoError:Passed
CICD.Tests.CICDTests.ArgsBuilderTools_TryGetBuildParameter_NoError:Passed
CICD.Tests.CICDTests.ArgsBuilderTools_TryGetBuildParameter_NoError:Passed
CICD.Tests.CICDTests.PlayerSettings_Version_IsNotEmpty:Passed
MissedRefsQualityGate:Failed
MissedRefsQualityGate:Missed scrpits in the prefab : PrefabWithMissingScript
MissedRefsQualityGate:Missed scripts on the scene:  in Main Camera
MissedRefsQualityGate:Missed scripts on the scene:  in InstanceOfThePrefab
MissedRefsQualityGate:The scene  has 2 missed refs.
QG Failed
```

## Usage

   ```sh
   Unity.exe -batchmode -projectPath %ProjectPath% -executeMethod CICD.CICDQualityGatesRunner.RunQualityGates -outputPath result.txt
   ```

or in editor you can use the context menu

## Contact

Linkedin - [@linkedin](https://www.linkedin.com/in/godofcoding/)
