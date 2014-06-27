using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TEAM.TEAM_ProjectMerger
{
   public class Solution
   {

      public Solution(IVsSolution solutionService)
      {
         SolutionService = solutionService;
      }

      private readonly IVsSolution SolutionService;

      public string GetProjectTypeGuids(Project proj)
      {
         string projectTypeGuids = "";
         IVsHierarchy hierarchy = null;
         IVsAggregatableProject aggregatableProject = null;
         int result = 0;

         result = SolutionService.GetProjectOfUniqueName(proj.UniqueName, out hierarchy);

         if (result == 0)
         {
            aggregatableProject = (IVsAggregatableProject)hierarchy;
            result = aggregatableProject.GetAggregateProjectTypeGuids(out projectTypeGuids);
         }

         return projectTypeGuids;
      }
   }
}
