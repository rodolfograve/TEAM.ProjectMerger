using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSLangProj;

namespace TEAM.TEAM_ProjectMerger
{
   public class CSharpProject : IProject
   {

      public CSharpProject(VSProject vsProject, OutputWindow outputWindow)
      {
         VsProject = vsProject;
         OutputWindow = outputWindow;
      }

      private readonly VSProject VsProject;
      private readonly OutputWindow OutputWindow;

      public IFolder AddFolder(string directoryName)
      {
         var result = VsProject.Project.ProjectItems.AddFolder(directoryName);
         return new CSharpProjectFolder(result);
      }

      public void AddFromFileCopy(string filePath)
      {
         VsProject.Project.ProjectItems.AddFromFileCopy(filePath);
      }

      public void MergeReferences(IProject otherProject)
      {
         var other = otherProject as CSharpProject;
         if (other == null) throw new ArgumentException("C# projects can only be merged with other C# projects. Merging with " + otherProject.GetType() + " is not supported.");

         foreach (Reference reference in other.VsProject.References)
         {
            try
            {
               Reference newReference;
               if (reference.SourceProject == null)
               {
                  if (reference.Type == prjReferenceType.prjReferenceTypeActiveX)
                  {
                     OutputWindow.WriteLine("ActiveX references cannot be migrated.");
                  }
                  else if (reference.Type == prjReferenceType.prjReferenceTypeAssembly)
                  {
                     OutputWindow.WriteLine("Copying assembly reference " + reference.Name + "...");
                     var path = reference.Path;
                     newReference = VsProject.References.Add(path);
                     newReference.CopyLocal = reference.CopyLocal;
                     OutputWindow.WriteLine("Copied assembly reference " + newReference.Name + ".");
                  }
               }
               else
               {
                  OutputWindow.WriteLine("Copying reference to project " + reference.SourceProject.Name + "...");
                  newReference = VsProject.References.AddProject(reference.SourceProject);
                  OutputWindow.WriteLine("Copied reference to project " + reference.SourceProject.Name + ".");
               }
            }
            catch (Exception ex)
            {
               OutputWindow.WriteLine("Failed to merge reference " + reference.Name + ". " + ex);
            }
         }
      }
   }
}
