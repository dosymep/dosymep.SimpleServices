using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DocFX;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using Nuke.Components;

using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.DocFX.DocFXTasks;

class Build : NukeBuild, IHazSolution {
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter] readonly AbsolutePath Output = RootDirectory / "bin";
    [Parameter] readonly string DocsOutput = Path.Combine("docs", "_site");
    [Parameter] readonly string DocsConfig = Path.Combine("docs", "docfx.json");
    [Parameter] readonly AbsolutePath DocsCaches = RootDirectory / Path.Combine("docs", "api");
    [Parameter] readonly AbsolutePath PublishOutput;

    public Build() {
        AbsolutePath appdataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        PublishOutput = appdataFolder / "pyRevit" / "Extensions" / "BIM4Everyone.lib" / "dosymep_libs" / "libs";
    }

    Target Clean => _ => _
        .Executes(() => {
            Output.CreateOrCleanDirectory();
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
            DotNetRestore(s => s
                .SetProjectFile(((IHazSolution) this).Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() => {
            DotNetBuild(s => s
                .EnableForce()
                .DisableNoRestore()
                .SetConfiguration(Configuration)
                .SetProjectFile(((IHazSolution) this).Solution)
                .When(settings => IsServerBuild,
                    _ => _
                        .EnableContinuousIntegrationBuild())
                .SetProperty("OutputPath", Output));
        });

    Target Publish => _ => _
        .DependsOn(Restore)
        .OnlyWhenStatic(() => IsLocalBuild)
        .Executes(() => {
            DotNetPublish(s => s
                .EnableForce()
                .DisableNoRestore()
                .SetConfiguration(Configuration)
                .SetProperty("PublishDir", PublishOutput));
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