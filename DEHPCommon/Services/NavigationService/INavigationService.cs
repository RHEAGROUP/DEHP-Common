﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INavigationService.cs" company="RHEA System S.A.">
//    Copyright (c) 2020-2020 RHEA System S.A.
// 
//    Author: Sam Gerené, Alex Vorobiev, Alexander van Delft, Nathanael Smiechowski.
// 
//    This file is part of DEHP Common Library
// 
//    The DEHPCommon is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 3 of the License, or (at your option) any later version.
// 
//    The DEHPCommon is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
// 
//    You should have received a copy of the GNU Lesser General Public License
//    along with this program; if not, write to the Free Software Foundation,
//    Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DEHPCommon.Services.NavigationService
{
    using System.Windows;

    using DevExpress.Xpf.Core;

    /// <summary>
    /// Interface definition for the <see cref="NavigationService"/>
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Brings a view to the user sight resolving its datacontext based on its name
        /// </summary>
        /// <typeparam name="TView">The view <see cref="System.Type"/> to show</typeparam>
        void Show<TView>() where TView : Window, new();

        /// <summary>
        /// Brings a view to the user sight with it's associated view model of the provided type <typeparamref name="TViewModel"></typeparamref>
        /// </summary>
        /// <typeparam name="TView">The view <see cref="System.Type"/> to show</typeparam>
        /// <typeparam name="TViewModel">The View Model <see cref="System.Type"/> to associate with the view</typeparam>
        /// <param name="viewModel">The View Model instance</param>
        void Show<TView, TViewModel>(TViewModel viewModel = null) where TView : Window, new() where TViewModel : class;

        /// <summary>
        /// Brings a view to the user sight resolving its datacontext based on its name
        /// </summary>
        /// <typeparam name="TView">The view <see cref="System.Type"/> to show</typeparam>
        /// <returns>A value indicating the dialog result</returns>
        bool? ShowDialog<TView>() where TView : Window, new();

        /// <summary>
        /// Brings a view to the user sight as a modal with it's associated view model of the provided type <typeparamref name="TViewModel"/>
        /// </summary>
        /// <typeparam name="TView">The view <see cref="System.Type"/> to show</typeparam>
        /// <typeparam name="TViewModel">The View Model <see cref="System.Type"/> to associate with the view</typeparam>
        /// <param name="viewModel">The View Model instance</param>
        /// <returns>A value indicating the dialog result</returns>
        bool? ShowDialog<TView, TViewModel>(TViewModel viewModel = null) where TView : Window, new() where TViewModel : class;

        /// <summary>
        /// Brings a <see cref="DXDialogWindow"/> to the user sight as a modal with it's associated view model of the provided type <typeparamref name="TViewModel"/>
        /// </summary>
        /// <typeparam name="TView">The view <see cref="System.Type"/> to show</typeparam>
        /// <typeparam name="TViewModel">The View Model <see cref="System.Type"/> to associate with the view</typeparam>
        /// <param name="viewModel">The View Model instance</param>
        /// <returns>A value indicating the dialog result</returns>
        bool? ShowDxDialog<TView, TViewModel>(TViewModel viewModel = null) where TView : DXDialogWindow, new() where TViewModel : class;

        /// <summary>
        /// Brings a <see cref="DXDialogWindow"/> to the user sight as a modal without any view model
        /// </summary>
        /// <typeparam name="TView">The view <see cref="System.Type"/> to show</typeparam>
        /// <returns>A value indicating the dialog result</returns>
        bool? ShowDxDialog<TView>() where TView : DXDialogWindow, new();

        /// <summary>
        /// Builds up the view instance with it's associated view model resolved based on its name
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <returns>A <typeparamref name="TView"/> instance</returns>
        TView BuildView<TView>() where TView : Window, new();

        /// <summary>
        /// Builds up the view instance with it's associated view model of the provided type <typeparamref name="TViewModel"/>
        /// </summary>
        /// <typeparam name="TView">The view <see cref="System.Type"/> to show</typeparam>
        /// <typeparam name="TViewModel">The View Model <see cref="System.Type"/> to associate with the view</typeparam>
        /// <param name="viewModel">The View Model instance</param>
        TView BuildView<TView, TViewModel>(TViewModel viewModel) where TView : Window, new() where TViewModel : class;
    }
}
