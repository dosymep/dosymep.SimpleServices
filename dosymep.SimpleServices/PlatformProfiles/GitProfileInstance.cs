using System;
using System.IO;

using LibGit2Sharp;

namespace dosymep.SimpleServices.PlatformProfiles {
    internal class GitProfileInstance : ProfileInstance {
        public GitProfileInstance(ProfileInfo profileInfo, string profileUri, ProfileSpace profileSpace)
            : base(profileUri, profileInfo, profileSpace) {
        }

        public string Branch { get; set; }

        protected override void CopyProfileImpl(string directory) {
            using(Repository repository = GitClone(directory)) {
                Commands.Checkout(repository, repository.Branches[Branch]);
                GitPull(repository);
            }
        }

        protected override void CommitProfileImpl(string pluginConfigPath) {
            throw new NotImplementedException();
        }

        public Repository GitClone(string directory) {
            CloneOptions cloneOptions = CreateCloneOptions(Branch, Credentials.Username, Credentials.Password);
            if(Directory.Exists(directory)) {
                return new Repository(directory);
            }

            string clonedPath = Repository.Clone(ProfileUri, directory, cloneOptions);
            return new Repository(clonedPath);
        }

        public void GitPull(Repository repository) {
            PullOptions options = CreatePullOptions(Credentials.Username, Credentials.Password);
            Signature signature = new Signature(
                Credentials.Username ?? Environment.UserName,
                Credentials.Username ?? Environment.UserDomainName, DateTimeOffset.Now);

            Commands.Pull(repository, signature, options);
        }

        public static CloneOptions CreateCloneOptions(string branch = default, string username = default,
            string password = default) {
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) {
                return new CloneOptions {BranchName = branch};
            }

            return new CloneOptions {
                BranchName = branch,
                CredentialsProvider = (url, uname, types)
                    => CreateUsernamePasswordCredentials(username, password)
            };
        }

        public static PullOptions CreatePullOptions(string username = default, string password = default) {
            return new PullOptions {FetchOptions = CreateFetchOptions(username, password)};
        }

        public static FetchOptions CreateFetchOptions(string username = default, string password = default) {
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) {
                return new FetchOptions();
            }

            return new FetchOptions {
                CredentialsProvider = (url, uname, types)
                    => CreateUsernamePasswordCredentials(username, password)
            };
        }

        public static UsernamePasswordCredentials CreateUsernamePasswordCredentials(string username = default,
            string password = default) {
            return new UsernamePasswordCredentials {Username = username, Password = password};
        }
    }
}