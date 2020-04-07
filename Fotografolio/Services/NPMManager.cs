using Fotografolio.Data.Interfaces;
using Fotografolio.Data.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fotografolio.Services
{
    public class NpmManager
    {
        public IHostEnvironment env { get; private set; }
        public IUnitOfWork unitOfWork { get; private set; }

        public NpmManager(IHostEnvironment env,
            IUnitOfWork unitOfWork)
        {
            this.env = env;
            this.unitOfWork = unitOfWork;
        }

        private string RunCommand(string commandToRun, string workingDirectory = null)
        {
            //var proc = Process.Start("npm.cmd", commandToRun);
            //proc.WaitForExit();
            if (string.IsNullOrEmpty(workingDirectory))
            {
                workingDirectory = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory());
            }

            var processStartInfo = new ProcessStartInfo()
            {
                FileName = "cmd",
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                WorkingDirectory = workingDirectory
            };

            var process = Process.Start(processStartInfo);

            if (process == null)
            {
                throw new Exception("Process should not be null.");
            }

            process.StandardInput.WriteLine($"{commandToRun} & exit");
            process.WaitForExit();

            var output = process.StandardOutput.ReadToEnd();
            return output;
        }
        public void NpmBuild()
        {
            RunCommand("npm install", Path.Combine(env.ContentRootPath, "clientapp"));
            RunCommand("npm run build", Path.Combine(env.ContentRootPath, "clientapp"));
        }
        public static void CopyBuildFilesToPublicRoot(IWebHostEnvironment env)
        {
            var contentPath = env.ContentRootPath;
            if (Data.FotografolioDbContext.HerokuPostgreSqlConnectionString != null)
                contentPath = Path.Combine(System.IO.Directory.GetParent(env.ContentRootPath).ToString(), typeof(Fotografolio.Program).Namespace);
                //contentPath = Path.Combine(env.ContentRootPath,"..",typeof(Fotografolio.Program).Namespace);

            NpmManager.DirectoryCopy(
                Path.Combine(contentPath, "clientapp", "dist", "js"),
                Path.Combine(contentPath, "wwwroot", "js"),
                copySubDirs:true
                );
            NpmManager.DirectoryCopy(
                Path.Combine(contentPath, "clientapp", "dist", "css"),
                Path.Combine(contentPath, "wwwroot", "css"),
                copySubDirs: true
                );
        }
        public VueFilesViewModel CopyBuildFilesToModel()
        {
            var model = new VueFilesViewModel();
            var appCssRegexObject = new Regex(@"app.+css");
            var appJsRegexObject = new Regex(@"app.+js");
            var chunkJsRegexObject = new Regex(@"chunk-vendors.+js");

            var contentPath = env.ContentRootPath;
            if (Data.FotografolioDbContext.HerokuPostgreSqlConnectionString != null)
                contentPath = Path.Combine(System.IO.Directory.GetParent(env.ContentRootPath).ToString(), typeof(Fotografolio.Program).Namespace);

            using (StreamReader streamReader = new StreamReader(Path.Combine(contentPath, "clientapp", "dist", "index.html")))
            {
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.Load(streamReader);
                var allLinkTags = doc.DocumentNode.SelectNodes("//html/head/link");
                foreach (var linkTag in allLinkTags)
                {
                    if (linkTag.GetAttributeValue("href", null) == null) continue;
                    bool appCssRegex = appCssRegexObject.IsMatch(linkTag.GetAttributeValue("href", null));
                    bool appJsRegex = appJsRegexObject.IsMatch(linkTag.GetAttributeValue("href", null));
                    bool chunkJsRegex = chunkJsRegexObject.IsMatch(linkTag.GetAttributeValue("href", null));
                    if (appCssRegex) model.AppCss = linkTag.GetAttributeValue("href", null);
                    if (appJsRegex) model.AppJs = linkTag.GetAttributeValue("href", null);
                    if (chunkJsRegex) model.ChunkJs = linkTag.GetAttributeValue("href", null);
                }
                //[matches(@href, 'chunk-vendors.+js')][1]
                //starts - with(@href, 'app') and ends-with(@href, 'css')]

                return model;
            }
        }
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
