﻿namespace CsvTextEditor
{
    using Catel.MVVM;
    using Orc.ProjectManagement;

    public class EditCutCommandContainer : EditProjectCommandContainerBase
    {
        #region Constructors
        public EditCutCommandContainer(ICommandManager commandManager, IProjectManager projectManager, ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider)
            : base(Commands.Edit.Cut, commandManager, projectManager, csvTextEditorInstanceProvider)
        {
        }
        #endregion

        #region Methods
        protected override bool CanExecute(object parameter)
        {
            if (!base.CanExecute(parameter))
            {
                return false;
            }

            return CsvTextEditorInstance?.HasSelection ?? false;
        }

        protected override void Execute(object parameter)
        {
            CsvTextEditorInstance.Cut();

            base.Execute(parameter);
        }
        #endregion
    }
}
