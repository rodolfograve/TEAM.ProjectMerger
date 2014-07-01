using Microsoft.VisualStudio.VCProjectEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TEAM.TEAM_ProjectMerger
{
   public class VcppProject : IProject
   {

      public VcppProject(VCProject vcProject, OutputWindow outputWindow)
      {
         VcProject = vcProject;
         OutputWindow = outputWindow;
      }

      private readonly VCProject VcProject;
      private readonly OutputWindow OutputWindow;

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

      public void MergeReferences(IProject otherProject)
      {
         var other = otherProject as VcppProject;
         if (other == null) throw new ArgumentException("VC++ projects can only be merged with other VC++ projects. Merging with " + otherProject.GetType() + " is not supported.");

         OutputWindow.WriteLine("Merging references is not yet supported for VC++ projects.");

         //var references = other.VcProject.VCReferences;
         //for (int i = 1; i <= references.Count; i++)
         //{
         //   VCReference reference = references.Item(i);
         //   OutputWindow.WriteLine("Copying reference " + reference.Name + "...");
         //   VcProject.VCReferences.Add(reference);
         //   OutputWindow.WriteLine("Copied reference " + reference.Name + ".");
         //}
      }

   }
}
