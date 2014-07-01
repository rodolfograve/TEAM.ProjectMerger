using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAM.TEAM_ProjectMerger
{
   public class CSharpProjectFolder : IFolder
   {

      public CSharpProjectFolder(ProjectItem projectFolder)
      {
         ProjectFolder = projectFolder;
      }

      private readonly ProjectItem ProjectFolder;

      public IFolder AddFolder(string directoryName)
      {
         var result = ProjectFolder.ProjectItems.AddFolder(directoryName);
         return new CSharpProjectFolder(result);
      }

      public void AddFromFileCopy(string filePath)
      {
         ProjectFolder.ProjectItems.AddFromFileCopy(filePath);
      }
   }
}
