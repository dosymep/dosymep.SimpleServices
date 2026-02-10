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
    public static int Main() => Execute<Build>(x => x.Compile);

    [Solution] public readonly Solution Solution;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter] readonly AbsolutePath Output;
    [Parameter] readonly string DocsOutput = Path.Combine("docs", "_site");
    [Parameter] readonly string DocsConfig = Path.Combine("docs", "docfx.json");
    [Parameter] readonly AbsolutePath DocsCaches = RootDirectory / Path.Combine("docs", "api");

    public Build() {
        AbsolutePath appdataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        Output = appdataFolder / "pyRevit" / "Extensions" / "BIM4Everyone.lib" / "dosymep_libs" / "libs";
    }

    Target Clean => _ => _
        .Executes(() => {
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
            DotNetBuild(s => s
                .EnableForce()
                .DisableNoRestore()
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration));
        });

    Target Publish => _ => _
        .DependsOn(Compile)
        .OnlyWhenStatic(() => IsLocalBuild)
        .Executes(() => {
            Project[] frameworkLibs = Solution.AllProjects
                .Where(item => !item.Name.EndsWith("Tests"))
                .Where(item => item.Name.StartsWith("dosymep"))
                .ToArray();

            DotNetPublish(s => s
                .EnableForce()
                .DisableNoRestore()
                .SetConfiguration(Configuration)
                .CombineWith(frameworkLibs,
                    (s, plugin) => s
                        .SetProject(plugin)
                        .SetProperty("PublishDir", Output)));
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
}