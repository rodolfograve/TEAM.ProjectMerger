using EnvDTE;
using Microsoft.VisualStudio.VCProjectEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using VSLangProj;

namespace TEAM.TEAM_ProjectMerger
{
   public static class ProjectStrategyFactory
   {

      public static IProject Create(Project project, OutputWindow outputWindow)
      {
         if (project.IsCSharp())
         {
            return new CSharpProject((VSProject)project.Object, outputWindow);
         }
         else if (project.IsCpp())
         {
            return new VcppProject((VCProject)project.Object, outputWindow);
         }
         throw new ArgumentOutOfRangeException("Project kind '" + project.Kind + "' not supported. Only C# (csproj) and C++ (vcxproj) are supported.");
      }
   }

}
