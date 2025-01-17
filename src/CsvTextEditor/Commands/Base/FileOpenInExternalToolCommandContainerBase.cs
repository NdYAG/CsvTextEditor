﻿namespace CsvTextEditor
{
    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.FileSystem;
    using Orc.ProjectManagement;
    using Services;

    public class FileOpenInExternalToolCommandContainerBase : ProjectCommandContainerBase
    {
        private readonly IFileService _fileService;
        private readonly IProcessService _processService;
        private readonly string _externalToolPath;

        public FileOpenInExternalToolCommandContainerBase(string commandName, string fileExtension, ICommandManager commandManager, IProjectManager projectManager, 
            IFileExtensionService fileExtensionService, IFileService fileService, IProcessService processService) 
            : base(commandName, commandManager, projectManager)
        {
            Argument.IsNotNullOrEmpty(() => fileExtension);
            Argument.IsNotNull(() => fileExtensionService);
            Argument.IsNotNull(() => fileService);
            Argument.IsNotNull(() => processService);

            _fileService = fileService;
            _processService = processService;

            _externalToolPath = fileExtensionService.GetRegisteredTool(fileExtension);
        }

        protected override bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_externalToolPath) && _fileService.Exists(_externalToolPath)
                   && !string.IsNullOrEmpty(_projectManager.ActiveProject?.Location);
        }

        protected override void Execute(object parameter)
        {
            _processService.StartProcess(_externalToolPath, _projectManager.ActiveProject.Location);
        }
    }
}
