using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM.TEAM_ProjectMerger
{
   public class CSharpProject : IFolder
   {

      public CSharpProject(Project project)
      {
         Project = project;
      }

      private readonly Project Project;

      public Task<IFolder> AddFolder(string directoryName)
      {
         var result = Project.ProjectItems.AddFolder(directoryName);
         return Task.FromResult((IFolder)new CSharpProjectFolder(result));
      }

      public void AddFromFileCopy(string filePath)
      {
         Project.ProjectItems.AddFromFileCopy(filePath);
      }
   }
}
