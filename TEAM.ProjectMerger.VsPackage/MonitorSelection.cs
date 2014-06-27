using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace TEAM.TEAM_ProjectMerger
{
   public class MonitorSelection
   {

      public MonitorSelection(IVsMonitorSelection monitorSelectionService)
      {
         MonitorSelectionService = monitorSelectionService;
      }

      private readonly IVsMonitorSelection MonitorSelectionService;

      public Project[] GetSelectedProjects()
      {
         IntPtr hierarchyPointer, selectionContainerPointer;
         IVsMultiItemSelect multiItemSelect;
         uint projectItemId;

         MonitorSelectionService.GetCurrentSelection(out hierarchyPointer, out projectItemId, out multiItemSelect, out selectionContainerPointer);

         if (projectItemId == (uint)VSConstants.VSITEMID.Selection)
         {
            // Multiple projects are selected
            uint numberOfSelectedItems;
            int isSingleHieracrchy;
            multiItemSelect.GetSelectionInfo(out numberOfSelectedItems, out isSingleHieracrchy);

            var selectedItems = new VSITEMSELECTION[numberOfSelectedItems];

            multiItemSelect.GetSelectedItems(0, numberOfSelectedItems, selectedItems);

            var result = new Project[numberOfSelectedItems];
            for (int i = 0; i < numberOfSelectedItems; i++)
            {
               object selectedObject = null;
               ErrorHandler.ThrowOnFailure(selectedItems[i].pHier.GetProperty(selectedItems[i].itemid, (int)__VSHPROPID.VSHPROPID_ExtObject, out selectedObject));

               result[i] = selectedObject as Project;
            }
            return result;
         }
         else
         {
            // Only one project is selected
            object selectedObject = null;
            IVsHierarchy selectedHierarchy = null;
            try
            {
               selectedHierarchy = Marshal.GetTypedObjectForIUnknown(hierarchyPointer, typeof(IVsHierarchy)) as IVsHierarchy;
            }
            catch (Exception)
            {
               return null;
            }

            if (selectedHierarchy != null)
            {
               ErrorHandler.ThrowOnFailure(selectedHierarchy.GetProperty(projectItemId, (int)__VSHPROPID.VSHPROPID_ExtObject, out selectedObject));
            }

            Project selectedProject = selectedObject as Project;

            return new Project[] { selectedProject };
         }
      }
   }
}
