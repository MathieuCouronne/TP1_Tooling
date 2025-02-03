# TP1_Tooling
Tool for unreal engine
to launch the project go to the file MyTool and then enter this line:

For Windows
```bash
dotnet publish -c Release -r win-x64 --self-contained -o ./publish
```
For MacOS
```bash
dotnet publish -c Release -r osx-x64 --self-contained -o ./publish
```

You will have executable in the publish folder
