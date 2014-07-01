using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAM.TEAM_ProjectMerger
{
   public static class ProjectItemExtensions
   {

      public static bool IsPhysicalFile(this ProjectItem target)
      {
         return target.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFile;
      }

      public static bool IsPhysicalDirectory(this ProjectItem target)
      {
         return target.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFolder;
      }

   }
}
