using System;
using System.IO;
using System.Linq;

using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild {
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.DocsCompile);

    [Solution] public readonly Solution Solution;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter] readonly AbsolutePath ArtifactPath;
    [Parameter] readonly AbsolutePath PublishOutput;
    [Parameter] readonly string DocsOutput = Path.Combine("docs", "_site");
    [Parameter] readonly string DocsConfig = Path.Combine("docs", "docfx.json");
    [Parameter] readonly AbsolutePath DocsCaches = RootDirectory / Path.Combine("docs", "api");

    public Build() {
        ArtifactPath = RootDirectory / "bin";

        AbsolutePath appdataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        PublishOutput = appdataFolder / "pyRevit" / "Extensions" / "BIM4Everyone.lib" / "dosymep_libs" / "libs";
    }

    Target Clean => _ => _
        .Executes(() => {
            ArtifactPath.CreateOrCleanDirectory();
            (RootDirectory / DocsOutput).CreateOrCleanDirectory();
            DocsCaches.GlobFiles("**/*.yml").DeleteFiles();
            RootDirectory.GlobDirectories("**/bin", "**/obj")
                .Where(item => item != (RootDirectory / "build" / "bin"))
                .Where(item => item != (RootDirectory / "build" / "obj"))
                .DeleteDirectories();
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() => {
            DotNetRestore(s => s.SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() => {
            Project[] frameworkLibs = GetFrameworkLibs();

            DotNetBuild(s => s
                .EnableForce()
                .DisableNoRestore()
                .SetConfiguration(Configuration)
                .CombineWith(frameworkLibs,
                    (s, frameworkLib) => s
                        .SetProjectFile(frameworkLib)
                        .SetProperty("OutputPath", ArtifactPath)));

        });

    Target Publish => _ => _
        .DependsOn(Compile)
        .Executes(() => {
            Project[] frameworkLibs = GetFrameworkLibs();

            DotNetPublish(s => s
                .EnableForce()
                .DisableNoRestore()
                .SetConfiguration(Configuration)
                .CombineWith(frameworkLibs,
                    (s, frameworkLib) => s
                        .SetProject(frameworkLib)
                        .SetProperty("PublishDir", PublishOutput)));
        });

    Target DocsCompile => _ => _
        .DependsOn(Compile)
        .Executes(() => {
            ProcessTasks.StartProcess(
                "docfx",
                DocsConfig
                + (IsLocalBuild
                    ? " --serve"
                    : string.Empty),
                RootDirectory).WaitForExit();

            // DocFXBuild(s => s
            //     .EnableForceRebuild()
            //     .SetServe(IsLocalBuild)
            //     .SetOutputFolder(DocsOutput)
            //     .SetProcessWorkingDirectory(RootDirectory)
            // );
        });

    Project[] GetFrameworkLibs() {
        Project[] frameworkLibs = Solution.AllProjects
            .Where(item => item.Parent?.ToString()?.Equals("src") == true)
            .ToArray();
        return frameworkLibs;
    }
}