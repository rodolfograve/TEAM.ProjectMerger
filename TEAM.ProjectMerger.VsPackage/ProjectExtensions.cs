using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using VSLangProj;

namespace TEAM.TEAM_ProjectMerger
{
   public static class ProjectExtensions
   {

      public static bool IsCSharp(this Project target)
      {
         return target.Kind == PrjKind.prjKindCSharpProject;
      }

      public static bool IsCpp(this Project target)
      {
         return target.Kind == "{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}";
      }

   }
}
