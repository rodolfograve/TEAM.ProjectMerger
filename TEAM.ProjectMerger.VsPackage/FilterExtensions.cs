using Microsoft.VisualStudio.VCProjectEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TEAM.TEAM_ProjectMerger
{
   public static class FilterExtensions
   {

      public static VCFilter FindFilter(dynamic filters, string name)
      {
         VCFilter result = null;
         for (int i = 1; i <= filters.Count && result == null; i++)
         {
            if (filters.Item(i).Name == name)
            {
               result = filters.Item(i);
            }
         }
         return result;
      }

   }
}
