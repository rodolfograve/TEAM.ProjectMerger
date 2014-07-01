using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAM.TEAM_ProjectMerger
{
   public interface IProject : IFolder
   {

      void MergeReferences(IProject otherProject);

   }
}
