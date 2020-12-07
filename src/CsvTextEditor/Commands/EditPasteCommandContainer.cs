﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditPasteCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using System.Windows;
    using Catel.IoC;
    using Catel.MVVM;
    using Orc.ProjectManagement;

    public class EditPasteCommandContainer : EditProjectCommandContainerBase
    {
        #region Constructors
        public EditPasteCommandContainer(ICommandManager commandManager, IProjectManager projectManager, ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider)
            : base(Commands.Edit.Paste, commandManager, projectManager, csvTextEditorInstanceProvider)
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


            return true;
        }

        protected override void Execute(object parameter)
        {
            if (Clipboard.ContainsText())
            {
                CsvTextEditorInstance.Paste();
            }

            base.Execute(parameter);
        }
        #endregion
    }
}
