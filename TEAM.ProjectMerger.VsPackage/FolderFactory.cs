using EnvDTE;
using Microsoft.VisualStudio.VCProjectEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using VSLangProj;

namespace TEAM.TEAM_ProjectMerger
{
   public static class FolderFactory
   {

      public static IFolder Create(Project project)
      {
         if (project.IsCSharp())
         {
            return new CSharpProject((VSProject)project.Object);
         }
         else if (project.IsCpp())
         {
            return new VcppProject((VCProject)project.Object);
         }
         throw new ArgumentOutOfRangeException("Project kind '" + project.Kind + "' not supported. Only C# (csproj) and C++ (vcxproj) are supported.");
      }
   }

}
