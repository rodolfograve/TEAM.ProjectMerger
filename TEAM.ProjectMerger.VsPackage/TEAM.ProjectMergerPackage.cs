using System;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using System.Collections.Generic;
using EnvDTE;
using System.IO;

namespace TEAM.TEAM_ProjectMerger
{
   /// <summary>
   /// This is the class that implements the package exposed by this assembly.
   ///
   /// The minimum requirement for a class to be considered a valid package for Visual Studio is to implement the IVsPackage interface and register itself with the shell.
   /// This package uses the helper classes defined inside the Managed Package Framework (MPF) to do it: it derives from the Package class that provides the implementation of the 
   /// IVsPackage interface and uses the registration attributes defined in the framework to register itself and its components with the shell.
   /// </summary>
   // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is a package.
   [PackageRegistration(UseManagedResourcesOnly = true)]
   // This attribute is used to register the information needed to show this package in the Help/About dialog of Visual Studio.
   [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
   // This attribute is needed to let the shell know that this package exposes some menus.
   [ProvideMenuResource("Menus.ctmenu", 1)]
   [Guid(GuidList.guidTEAM_ProjectMergerPkgString)]
   public sealed class TEAM_ProjectMergerPackage : Package
   {
      /// <summary>
      /// Default constructor of the package.
      /// Inside this method you can place any initialization code that does not require any Visual Studio service because at this point the package object is created but 
      /// not sited yet inside Visual Studio environment. The place to do all the other initialization is the Initialize method.
      /// </summary>
      public TEAM_ProjectMergerPackage()
      {
         Debug.WriteLine("Entering constructor for: " + ToString());
      }

      private OutputWindow OutputWindow;
      private Shell Shell;
      private MonitorSelection MonitorSelection;
      private Solution Solution;
      private DTE Dte;

      /// <summary>
      /// Initialization of the package; this method is called right after the package is sited, so this is the place where you can put all the initialization code that rely on services provided by VisualStudio.
      /// </summary>
      protected override void Initialize()
      {
         Debug.WriteLine("Entering Initialize() of: " + ToString());
         base.Initialize();

         // Add our command handlers for menu (commands must exist in the .vsct file)
         var menuCommandService = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
         if (null != menuCommandService)
         {
            // Create the command for the menu item.
            CommandID menuCommandID = new CommandID(GuidList.guidTEAM_ProjectMergerCmdSet, (int)PkgCmdIDList.teamMergeProjects);
            MenuCommand menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
            menuCommandService.AddCommand(menuItem);
         }

         OutputWindow = OutputWindow.Create(GetService(typeof(SVsOutputWindow)) as IVsOutputWindow);
         Shell = new Shell((IVsUIShell)GetService(typeof(SVsUIShell)));
         MonitorSelection = new MonitorSelection(((IVsMonitorSelection)Package.GetGlobalService(typeof(SVsShellMonitorSelection))));
         Solution = new Solution((IVsSolution)GetService(typeof(IVsSolution)));
         Dte = (DTE)GetService(typeof(DTE));
      }

      /// <summary>
      /// This function is the callback used to execute a command when the a menu item is clicked. See the Initialize method to see how the menu item is associated to this function using
      /// the OleMenuCommandService service and the MenuCommand class.
      /// </summary>
      private void MenuItemCallback(object sender, EventArgs e)
      {
         OutputWindow.WriteLine("TEAM.MergeProjects invoked...");

         try
         {
            var selectedProjects = MonitorSelection.GetSelectedProjects();

            if (selectedProjects.Length <= 1)
            {
               MessageBox("You must select more than 1 project in order to merge them.");
            }
            else
            {
               var targetProject = selectedProjects[0];
               OutputWindow.WriteLine("Joining all selected projects [" + ToStringList(selectedProjects) + "] into " + targetProject.Name);

               var invalidProjectKinds = selectedProjects.Where(x => x.Kind != targetProject.Kind).ToArray();
               if (invalidProjectKinds.Any())
               {
                  MessageBox("The following projects have different Kinds to project " + targetProject.Name + " and cannot be safely merged. Change your selection: " + ToStringList(invalidProjectKinds));
               }

               var targetProjectFlavour = Solution.GetProjectTypeGuids(targetProject);
               var invalidProjectFlavours = selectedProjects.Where(x => Solution.GetProjectTypeGuids(x) != targetProjectFlavour).ToArray();
               if (invalidProjectFlavours.Any())
               {
                  MessageBox("The following projects have different 'flavours' to project " + targetProject.Name + " and cannot be safely merged. Change your selection: " + ToStringList(invalidProjectFlavours));
               }

               if (!invalidProjectKinds.Any() && !invalidProjectFlavours.Any())
               {
                  foreach (var project in selectedProjects.Where(x => x != targetProject))
                  {
                     MergeProjects(project, targetProject);
                  }
               }
            }
            MessageBox("TEAM.MergeProjects finished OK.");
         }
         catch (Exception ex)
         {
            MessageBox("There was an error while trying to merge the projects: " + ex);
         }
      }

      public void MergeProjects(Project sourceProject, Project targetProject)
      {
         var sourceProjectName = sourceProject.Name;
         OutputWindow.WriteLine("Merging project " + sourceProjectName + " into project " + targetProject.Name);
         sourceProject.Save();

         var targetProjectStrategy = FolderFactory.Create(targetProject);

         // Create a directory in the targetProject in order to keep a similar structure
         var targetDirectory = targetProjectStrategy.AddFolder(sourceProject.Name);

         MoveProjectItems(sourceProject.ProjectItems, targetDirectory, sourceProject.Name, targetProject.Name + "/" + sourceProjectName);

         targetProject.Save();
         sourceProject.Save();

         Dte.Solution.Remove(sourceProject);

         MessageBox("Merged project " + sourceProjectName + " into project " + targetProject.Name);
      }

      private void MoveProjectItems(ProjectItems sourceProjectItems, IFolder targetFolder, string sourceName, string targetName)
      {
         // Keep a list of items that have been moved so that we don't modify the sourceProjectItems enumerable.
         var itemsToDeleteFromSource = new List<ProjectItem>();

         foreach (ProjectItem item in sourceProjectItems)
         {
            string itemName = item.Name;
            if (item.IsOpen && item.IsDirty)
            {
               item.Save();
            }

            if (CanBeMigrated(item))
            {
               OutputWindow.WriteLine("Copying item " + itemName + " from project " + sourceName + " into project " + targetName + "...");
               for (short i = 0; i < item.FileCount; i++)
               {
                  if (item.IsPhysicalDirectory())
                  {
                     var directoryName = item.Name;
                     OutputWindow.WriteLine("Copying directory " + directoryName + " from project " + sourceName + " into project " + targetName + "...");
                     var newDirectory = targetFolder.AddFolder(directoryName);
                     MoveProjectItems(item.ProjectItems, newDirectory, sourceName + "/" + directoryName, targetName + "/" + directoryName);
                     OutputWindow.WriteLine("Copied directory " + directoryName + " from project " + sourceName + " into project " + targetName + ".");
                  }
                  else if (item.IsPhysicalFile())
                  {
                     var fileName = item.FileNames[i];
                     OutputWindow.WriteLine("Copying file " + fileName + " from project " + sourceName + " into project " + targetName + "...");
                     targetFolder.AddFromFileCopy(fileName);
                     OutputWindow.WriteLine("Copied file " + fileName + " from project " + sourceName + " into project " + targetName + ".");
                  }
                  else
                  {
                     OutputWindow.WriteLine("Unsupported item Kind " + item.Kind + ". This item won't be moved to the project " + targetName + ".");
                  }
               }
               itemsToDeleteFromSource.Add(item);
               OutputWindow.WriteLine("Copied item " + itemName + " from project " + sourceName + " into project " + targetName);
            }
            else
            {
               OutputWindow.WriteLine("Ignoring special item '" + item.Name + "'. This item cannot be migrated.");
            }
         }

         foreach (var item in itemsToDeleteFromSource)
         {
            OutputWindow.WriteLine("Deleting item '" + item.Name + "' from source project...");
            item.Delete();
            OutputWindow.WriteLine("Deleted item '" + item.Name + "' from source project.");
         }
      }

      private bool CanBeMigrated(ProjectItem item)
      {
         return
            !(item.ContainingProject.IsCSharp() && item.IsPhysicalDirectory() && item.Name == "Properties") &&
            !(item.ContainingProject.IsCpp() && item.IsPhysicalFile() && Path.GetExtension(item.Name) == ".filters");// &&
            //!(item.ContainingProject.IsCpp() && item.IsPhysicalFile() && (Path.GetFileName(item.Name) == "stdafx.cpp" || Path.GetFileName(item.Name) == "stdafx.h"));
      }

      private void MessageBox(string message)
      {
         OutputWindow.WriteLine(message);
         Shell.MessageBox(message);
      }

      private string ToStringList(IEnumerable<Project> projects, Func<Project, string> selector = null)
      {
         if (selector == null)
         {
            selector = x => x.Name + "(Kind=" + x.Kind + ", Flavours=" + Solution.GetProjectTypeGuids(x) + ")";
         }
         return string.Join(Environment.NewLine + "- ", projects.Select(selector).ToArray());
      }

   }
}
