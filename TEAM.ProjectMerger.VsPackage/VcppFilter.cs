using Microsoft.VisualStudio.VCProjectEngine;
using System;
using System.IO;

namespace TEAM.TEAM_ProjectMerger
{
   public class VcppFilter : IFolder
   {
      public VcppFilter(VCFilter vcProjectFilter, string physicalDirectoryPath)
      {
         VcProjectFilter = vcProjectFilter;
         PhysicalDirectoryPath = physicalDirectoryPath;
      }

      private readonly VCFilter VcProjectFilter;
      private readonly string PhysicalDirectoryPath;

      public IFolder AddFolder(string directoryName)
      {
         return new VcppFilter(VcProjectFilter.AddFilter(directoryName), Path.Combine(PhysicalDirectoryPath, directoryName));
      }

      public void AddFromFileCopy(string filePath)
      {
         var targetFilePath = Path.Combine(PhysicalDirectoryPath, Path.GetFileName(filePath));
         Directory.CreateDirectory(PhysicalDirectoryPath);
         File.Copy(filePath, targetFilePath, true);
         VcProjectFilter.AddFile(targetFilePath);
      }
   }
}