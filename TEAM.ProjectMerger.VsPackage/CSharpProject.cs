using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSLangProj;

namespace TEAM.TEAM_ProjectMerger
{
   public class CSharpProject : IFolder
   {

      public CSharpProject(VSProject vsProject)
      {
         VsProject = vsProject;
      }

      private readonly VSProject VsProject;

      public IFolder AddFolder(string directoryName)
      {
         var result = VsProject.Project.ProjectItems.AddFolder(directoryName);
         return new CSharpProjectFolder(result);
      }

      public void AddFromFileCopy(string filePath)
      {
         VsProject.Project.ProjectItems.AddFromFileCopy(filePath);
      }
   }
}
