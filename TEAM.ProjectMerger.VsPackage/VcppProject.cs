using Microsoft.VisualStudio.VCProjectEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM.TEAM_ProjectMerger
{
   public class VcppProject : IFolder
   {

      public VcppProject(VCProject vcProject)
      {
         VcProject = vcProject;
      }

      private readonly VCProject VcProject;

      public async Task<IFolder> AddFolder(string directoryName)
      {
         var result = await VcProject.AddFilter(directoryName);
         return new VcppFilter(result);
      }

      public async void AddFromFileCopy(string filePath)
      {
         await VcProject.AddFile(filePath);
      }
   }
}
