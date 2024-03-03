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
{
  "QualityGateResult": "Failed",
  "results": [
    {
      "Passed": true,
      "Name": "TestsQualityGate",
      "Classname": "CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBoolean",
      "Time": 0,
      "SystemOut": null,
      "FailureMessage": ""
    },
    {
      "Passed": true,
      "Name": "TestsQualityGate",
      "Classname": "CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBoolean",
      "Time": 0,
      "SystemOut": null,
      "FailureMessage": ""
    },
    {
      "Passed": true,
      "Name": "TestsQualityGate",
      "Classname": "CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBoolean",
      "Time": 0,
      "SystemOut": null,
      "FailureMessage": ""
    },
    {
      "Passed": true,
      "Name": "TestsQualityGate",
      "Classname": "CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBoolean",
      "Time": 0,
      "SystemOut": null,
      "FailureMessage": ""
    },
    {
      "Passed": true,
      "Name": "TestsQualityGate",
      "Classname": "CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBuildTarget",
      "Time": 0,
      "SystemOut": null,
      "FailureMessage": ""
    },
    {
      "Passed": true,
      "Name": "TestsQualityGate",
      "Classname": "CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBuildTarget",
      "Time": 0,
      "SystemOut": null,
      "FailureMessage": ""
    },
    {
      "Passed": true,
      "Name": "TestsQualityGate",
      "Classname": "CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBuildTarget",
      "Time": 0,
      "SystemOut": null,
      "FailureMessage": ""
    },
    {
      "Passed": true,
      "Name": "TestsQualityGate",
      "Classname": "CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBuildTarget",
      "Time": 0,
      "SystemOut": null,
      "FailureMessage": ""
    },
    {
      "Passed": false,
      "Name": "TestsQualityGate",
      "Classname": "CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBuildTargetErrorsForATest",
      "Time": 0,
      "SystemOut": "System.ArgumentException : Must specify valid information for parsing in the string.",
      "FailureMessage": "System.ArgumentException : Must specify valid information for parsing in the string."
    },
    {
      "Passed": false,
      "Name": "TestsQualityGate",
      "Classname": "CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBuildTargetErrorsForATest",
      "Time": 0,
      "SystemOut": "System.ArgumentException : Must specify valid information for parsing in the string.",
      "FailureMessage": "System.ArgumentException : Must specify valid information for parsing in the string."
    },
    {
      "Passed": false,
      "Name": "TestsQualityGate",
      "Classname": "CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBuildTargetErrorsForATest",
      "Time": 0,
      "SystemOut": "System.ArgumentException : Must specify valid information for parsing in the string.",
      "FailureMessage": "System.ArgumentException : Must specify valid information for parsing in the string."
    },
    {
      "Passed": false,
      "Name": "TestsQualityGate",
      "Classname": "CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectBuildTargetErrorsForATest",
      "Time": 0,
      "SystemOut": "System.ArgumentException : Requested value 'c' was not found.",
      "FailureMessage": "System.ArgumentException : Requested value 'c' was not found."
    },
    {
      "Passed": true,
      "Name": "TestsQualityGate",
      "Classname": "CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectInt",
      "Time": 0,
      "SystemOut": null,
      "FailureMessage": ""
    },
    {
      "Passed": true,
      "Name": "TestsQualityGate",
      "Classname": "CICD.Tests.CICDTests.ArgsBuilderTools_ConvertStringToValue_CorrectInt",
      "Time": 0,
      "SystemOut": null,
      "FailureMessage": ""
    },
    {
      "Passed": true,
      "Name": "TestsQualityGate",
      "Classname": "CICD.Tests.CICDTests.ArgsBuilderTools_GetBuildParameter_NoError",
      "Time": 0,
      "SystemOut": null,
      "FailureMessage": ""
    },
    {
      "Passed": true,
      "Name": "TestsQualityGate",
      "Classname": "CICD.Tests.CICDTests.ArgsBuilderTools_GetBuildParameter_NoError",
      "Time": 0,
      "SystemOut": null,
      "FailureMessage": ""
    },
    {
      "Passed": true,
      "Name": "TestsQualityGate",
      "Classname": "CICD.Tests.CICDTests.ArgsBuilderTools_HaveBuildParameter_NoError",
      "Time": 0,
      "SystemOut": null,
      "FailureMessage": ""
    },
    {
      "Passed": true,
      "Name": "TestsQualityGate",
      "Classname": "CICD.Tests.CICDTests.ArgsBuilderTools_HaveBuildParameter_NoError",
      "Time": 0,
      "SystemOut": null,
      "FailureMessage": ""
    },
    {
      "Passed": true,
      "Name": "TestsQualityGate",
      "Classname": "CICD.Tests.CICDTests.ArgsBuilderTools_TryGetBuildParameter_NoError",
      "Time": 0,
      "SystemOut": null,
      "FailureMessage": ""
    },
    {
      "Passed": true,
      "Name": "TestsQualityGate",
      "Classname": "CICD.Tests.CICDTests.ArgsBuilderTools_TryGetBuildParameter_NoError",
      "Time": 0,
      "SystemOut": null,
      "FailureMessage": ""
    },
    {
      "Passed": true,
      "Name": "TestsQualityGate",
      "Classname": "CICD.Tests.CICDTests.PlayerSettings_Version_IsNotEmpty",
      "Time": 0,
      "SystemOut": null,
      "FailureMessage": ""
    },
    {
      "Passed": false,
      "Name": null,
      "Classname": "MissedRefsQualityGate",
      "Time": 0,
      "SystemOut": "Failed",
      "FailureMessage": "Missed scrpits in the prefab : PrefabWithMissingScript"
    },
    {
      "Passed": false,
      "Name": null,
      "Classname": "MissedRefsQualityGate",
      "Time": 0,
      "SystemOut": "Failed",
      "FailureMessage": "Missed scripts on the scene:  in Main Camera"
    },
    {
      "Passed": false,
      "Name": null,
      "Classname": "MissedRefsQualityGate",
      "Time": 0,
      "SystemOut": "Failed",
      "FailureMessage": "Missed scripts on the scene:  in InstanceOfThePrefab"
    },
    {
      "Passed": false,
      "Name": null,
      "Classname": "MissedRefsQualityGate",
      "Time": 0,
      "SystemOut": "Failed",
      "FailureMessage": "The scene  has 2 missed refs."
    }
  ]
}
```

## Usage

   ```sh
   Unity.exe -batchmode -projectPath %ProjectPath% -executeMethod CICD.CICDQualityGatesRunner.RunQualityGates -outputPath result.txt
   ```

or in editor you can use the context menu

## Contact

Linkedin - [@linkedin](https://www.linkedin.com/in/godofcoding/)
