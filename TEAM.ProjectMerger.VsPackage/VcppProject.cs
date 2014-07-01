using Microsoft.VisualStudio.VCProjectEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TEAM.TEAM_ProjectMerger
{
   public class VcppProject : IFolder
   {

      public VcppProject(VCProject vcProject)
      {
         VcProject = vcProject;
      }

      private readonly VCProject VcProject;

      public IFolder AddFolder(string directoryName)
      {
         var result = FilterExtensions.FindFilter(VcProject.Filters, directoryName);
         if (result == null)
         {
            result = VcProject.AddFilter(directoryName);
         }
         return new VcppFilter(result, Path.Combine(VcProject.ProjectDirectory, directoryName));
      }

      public void AddFromFileCopy(string filePath)
      {
         var targetFilePath = Path.Combine(VcProject.ProjectDirectory, Path.GetFileName(filePath));
         File.Copy(filePath, targetFilePath, true);
         VcProject.AddFile(targetFilePath);
      }
   }
}
