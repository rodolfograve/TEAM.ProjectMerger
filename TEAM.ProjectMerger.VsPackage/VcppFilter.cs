using Microsoft.VisualStudio.VCProjectEngine;
using System;
using System.IO;

namespace TEAM.TEAM_ProjectMerger
{
   public class VcppFilter : IFolder
   {
      public VcppFilter(VCFilter vcProjectFilter)
      {
         VcProjectFilter = vcProjectFilter;
         PhysicalDirectoryPath = vcProjectFilter.project.ProjectDirectory;
      }

      private readonly VCFilter VcProjectFilter;
      private readonly string PhysicalDirectoryPath;

      public IFolder AddFolder(string directoryName)
      {
         return VcProjectFilter.AddFilter(directoryName);
      }

      public void AddFromFileCopy(string filePath)
      {
         var targetFilePath = Path.Combine(PhysicalDirectoryPath, Path.GetFileName(filePath));
         File.Copy(filePath, targetFilePath, true);
         VcProjectFilter.AddFile(targetFilePath);
      }
   }
}