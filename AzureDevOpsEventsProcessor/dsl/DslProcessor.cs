//------------------------------------------------------------------------------------------------- 
// <copyright file="DslProcessor.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using NLog;

namespace AzureDevOpsEventsProcessor.Dsl
{
    using System.Collections;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;
    using Helpers;
    using Interfaces;
    using NLog.Targets;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Contains the DSL API
    /// </summary>
    public class DslProcessor
    {
        /// <summary>
        /// Instance of nLog interface
        /// </summary>
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The variable to bind the MEF loaded DSL object
        /// </summary>
        [ImportMany(typeof(IDslLibrary))]
#pragma warning disable S2933 // Fields that are only assigned in the constructor should be "readonly"
        private IEnumerable<IDslLibrary> dslLibraries = null;
#pragma warning restore S2933 // Fields that are only assigned in the constructor should be "readonly"


        private readonly bool redirectScriptEngineOutputtoLogging = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public DslProcessor()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="redirectScriptEngineOutput">Flag to allow redirect or output</param>
        public DslProcessor(bool redirectScriptEngineOutput)
        {
            this.redirectScriptEngineOutputtoLogging = redirectScriptEngineOutput;
        }

        /// <summary>
        /// Runs a named Pyphon script that uses the DSL
        /// </summary>
        /// <param name="scriptname">The python script file</param>
        /// <param name="IAzureDevOpsProvider">The TFS provider</param>
        /// <param name="iEmailProvider">The email provider</param>
        /// <param name="iEventdataProvider">The raw data provider</param>
        public void RunScript(
            string scriptname,
            IAzureDevOpsProvider iAzureDevOpsProvider,
            IEmailProvider iEmailProvider,
            IEventDataProvider iEventdataProvider)
        {

            this.RunScript(
                FolderHelper.GetRootedPath(@"dsl"),
                FolderHelper.GetRootedPath("~/"),
                scriptname,
                null,
                iAzureDevOpsProvider,
                iEmailProvider,
                iEventdataProvider);
        }

        /// <summary>
        /// Runs a named Pyphon script that uses the DSL
        /// </summary>
        /// <param name="scriptname">The python script file</param>
        /// <param name="args">The parameters to pass to the script</param>
        /// <param name="IAzureDevOpsProvider">The TFS provider</param>
        /// <param name="iEmailProvider">The email provider</param>
        /// <param name="iEventdataProvider">The raw data provider</param>
        public void RunScript(
            string scriptname,
            Dictionary<string, object> args,
            IAzureDevOpsProvider iAzureDevOpsProvider,
            IEmailProvider iEmailProvider,
            IEventDataProvider iEventdataProvider)
        {
            this.RunScript(
                FolderHelper.GetRootedPath(@"dsl"),
                FolderHelper.GetRootedPath("~/"),
                scriptname,
                args,
                iAzureDevOpsProvider,
                iEmailProvider,
                iEventdataProvider);
        }

        /// <summary>
        /// Runs a named Pyphon script that uses the DSL
        /// </summary>
        /// <param name="dslFolder">The folder to scan for MEF DSL files instead of the current folder</param>
        /// <param name="scriptFolder">The base folder to load scripts file</param>
        /// <param name="scriptname">The python script file, this can be a full path or a file in the base folder</param>
        /// <param name="args">The parameters to pass to the script</param>
        /// <param name="IAzureDevOpsProvider">The TFS provider</param>
        /// <param name="iEmailProvider">The email provider</param>
        /// <param name="iEventData">The raw XML or Json data from the event</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Allowing complexity as this method pulling in the whole DSL")]
        public void RunScript(
            string dslFolder,
            string scriptFolder,
            string scriptname,
            Dictionary<string, object> args,
            IAzureDevOpsProvider iAzureDevOpsProvider,
            IEmailProvider iEmailProvider,
            IEventDataProvider iEventData)
        {
            if (scriptname == null)
            {
                throw new ArgumentNullException("scriptname");
            }
            else
            {
                if (string.IsNullOrEmpty(scriptFolder))
                {
                    scriptFolder = FolderHelper.GetRootedPath("~/");
                }
                else
                {
                    scriptFolder = FolderHelper.GetRootedPath(scriptFolder);
                }

                if (File.Exists(scriptname) == false)
                {
                    // we have not been given a resolvable file name, so try appending the base path
                    scriptname = Path.Combine(scriptFolder, scriptname);
                    if (File.Exists(scriptname) == false)
                    {
                        this.logger.Error(string.Format("DslProcessor: DslProcessor cannot find script:{0}", scriptname));
                        return;
                    }
                }
            }

            if (iEventData == null)
            {
                throw new ArgumentNullException("iEventData");
            }

            if (iAzureDevOpsProvider == null)
            {
                throw new ArgumentNullException("iAzureDevOpsProvider");
            }

            if (iEmailProvider == null)
            {
                throw new ArgumentNullException("iEmailProvider");
            }

            if (string.IsNullOrEmpty(dslFolder))
            {
                dslFolder = FolderHelper.GetRootedPath("~/");
            }
            else
            {
                dslFolder = FolderHelper.GetRootedPath(dslFolder);
            }

            // load the DSL wrapper class via MEF
            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();
            //Adds all the parts found in the same assembly as the Program class
            if (Directory.Exists(dslFolder))
            {
                this.logger.Info(
                    string.Format("DslProcessor: DslProcessor loading DSL from {0}", Path.GetFullPath(dslFolder)));
                catalog.Catalogs.Add(new DirectoryCatalog(dslFolder));
            }
            else
            {
                this.logger.Error(
                    string.Format("DslProcessor: DslProcessor cannot find DSL folder {0}", Path.GetFullPath(dslFolder)));
                return;
            }

            try
            {
                //Create the CompositionContainer with the parts in the catalog
                var container = new CompositionContainer(catalog);

                container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                this.logger.Error(string.Format("DsLProcessor: {0}", compositionException.Message));
                return;
            }
            catch (ReflectionTypeLoadException ex)
            {
                this.logger.Error(string.Format("DsLProcessor: {0}", ex.Message));
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null)
                    {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                this.logger.Error(string.Format("DslProcessor: {0}",sb.ToString()));
                return;
            }

            this.logger.Info(
            string.Format("DslProcessor: DslProcessor found  {0} DSL libraries", this.dslLibraries.Count()));

            if (this.dslLibraries.Any())
            {
                // create the engine
                var engine = IronPython.Hosting.Python.CreateEngine(args);
                var objOps = engine.Operations;
                var scope = engine.CreateScope();

                // inject the providers
                foreach (var item in this.dslLibraries)
                {
                    item.EmailProvider = iEmailProvider;
                    item.TfsProvider = iAzureDevOpsProvider;
                    item.EventData = iEventData;
                    item.ScriptFolder = scriptFolder;

                    // Read in the methods
                    foreach (string memberName in objOps.GetMemberNames(item))
                    {
                        scope.SetVariable(memberName, objOps.GetMember(item, memberName));
                    }
                }

                if (this.redirectScriptEngineOutputtoLogging == true)
                {
                    // redirect the console out opf the script to the nLog
                    engine.Runtime.IO.SetOutput(
                        new ConsoleRedirectStreamToNlog(LogLevel.Info, "PythonScriptConsole"),
                        System.Text.Encoding.ASCII);
                    engine.Runtime.IO.SetErrorOutput(
                        new ConsoleRedirectStreamToNlog(LogLevel.Error, "PythonScriptError"),
                        System.Text.Encoding.ASCII);
                }
                else
                {
                    // this is only used in tests where the console is visible or being cpatured in another manner
                    engine.Runtime.IO.RedirectToConsole();
                }

                // run the script
                this.logger.Info(string.Format(
                      "DslProcessor: DslProcessor running script:{0}",
                      scriptname));
                var script = engine.CreateScriptSourceFromFile(scriptname);
                script.Execute(scope);
            }
            else
            {
                this.logger.Error(
                    string.Format("DslProcessor: DslProcessor cannot find DSL libraries in folder {0}", Path.GetFullPath(dslFolder)));
                return;
            }

        }

        /// <summary>
        /// Returns the base executing path
        /// </summary>
        public string BasePath
        {
            get
            {
                return FolderHelper.GetRootedPath("~/");
            }
        }
    }
}
