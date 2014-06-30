using System;
using System.Threading.Tasks;

namespace TEAM.TEAM_ProjectMerger
{
   public class VcppFilter : IFolder
   {
      public VcppFilter(dynamic vcProjectFilter)
      {
         VcProjectFilter = vcProjectFilter;
      }

      private readonly dynamic VcProjectFilter;

      public async Task<IFolder> AddFolder(string directoryName)
      {
         return await VcProjectFilter.AddFilter(directoryName);
      }

      public async void AddFromFileCopy(string filePath)
      {
         await VcProjectFilter.AddFile(filePath);
      }
   }
}