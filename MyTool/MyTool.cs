using System;
using System.IO;
using System.Text.Json;

class Tooling
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: MyTool [PATH OF THE PROJECT] build" +
                              "MyTool [PATH OF THE PROJECT] show-infos" +
                              "MyTool [PATH OF THE PROJECT] package [PACKAGE PATH]");
            return;
        }

        string command = args[1];
        string uprojectPath = args[0];

        if (!File.Exists(uprojectPath))
        {
            Console.WriteLine("Error : the file .uproject is missing.");
            return;
        }

        if (command == "show-infos")
        {
            ShowInfos(uprojectPath);
        }
        else if (command == "build")
        {
            BuildProject(uprojectPath);
        }
        else if (command == "package" && args.Length >= 3)
        {
            string packagePath = args[2];
            PackageProject(uprojectPath, packagePath);
        }
        else
        {
            Console.WriteLine("Invalid commande.");
        }
    }

    static void ShowInfos(string uprojectPath)
    {
        string jsonContent = File.ReadAllText(uprojectPath);
        var uprojectData = JsonSerializer.Deserialize<UProject>(jsonContent);

        if (uprojectData == null)
        {
            Console.WriteLine("Error : the file .uproject is missing.");
            return;
        }
        if (uprojectData.Modules == null)
        {
            Console.WriteLine("Error : no Name in the .uproject file.");
            
        }
        else
        {
            foreach (var module in uprojectData.Modules)
            {
                Console.WriteLine($"Game name : {module.Name}");
            }
        }

        if (uprojectData.EngineAssociation == null)
        {
            Console.WriteLine("Unreal version : From Source");
        }
        else
        {
            Console.WriteLine($"Unreal version : {uprojectData.EngineAssociation}");
        }
        Console.WriteLine("Used Plugins :");
        foreach (var plugin in uprojectData.Plugins)
        {
            Console.WriteLine($"- {plugin.Name} ({(plugin.Enabled ? "Active" : "Inactive")})");
        }
    }

    static void BuildProject(string uprojectPath)
    {
        string command = $"./Engine/Build/BatchFiles/Build.bat BuildCookRun -project=\"{uprojectPath}\" -build";
        ExecuteCommand(command);
    }

    static void PackageProject(string uprojectPath, string packagePath)
    {
        string command = $"./Engine/Build/BatchFiles/RunUAT.sh BuildCookRun -project=\"{uprojectPath}\" -package -output=\"{packagePath}\"";
        ExecuteCommand(command);
    }

    static void ExecuteCommand(string command)
    {
        System.Diagnostics.Process.Start("bash", $"-c \"{command}\"");
    }
}

class UProject
{
    public string EngineAssociation { get; set; }
    public Plugin[] Plugins { get; set; }
    
    public Module[] Modules { get; set; }
}

class Plugin
{
    public string Name { get; set; }
    public bool Enabled { get; set; }
}

class Module
{
    public string Name { get; set; }
    public string Type { get; set; }
}