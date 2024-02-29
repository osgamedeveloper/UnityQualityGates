## Getting Started

This is an example of how you may create a QG setup for your project.
Just copy the existing Editor folder to your project and it's ready to go.

### Prerequisites

Just copy the existing Editor folder to your project and it's ready to go.

### Installation

1. Copy Editor folder to your project
2. Use CICD context menu button for additional actions
3. Develop your own QG by implementing IQualityGate.cs interface
   ```cs
   public class MyQualityGate:IQualityGate
   ```
4. Run your method in batchmode by 
   ```sh
   Unity.exe -batchmode -projectPath %ProjectPath% -executeMethod CICD.CICDQualityGatesRunner.RunQualityGates -outputPath result.txt
   ```

## Usage

   ```sh
   Unity.exe -batchmode -projectPath %ProjectPath% -executeMethod CICD.CICDQualityGatesRunner.RunQualityGates -outputPath result.txt
   ```

	or in editor you can use the context menu

## Contact

Linkedin - [@linkedin](https://www.linkedin.com/in/godofcoding/)

