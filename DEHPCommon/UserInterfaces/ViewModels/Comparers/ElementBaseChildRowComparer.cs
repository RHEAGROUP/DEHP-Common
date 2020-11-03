﻿// ------------------------------------------------------------------------------------------------
// <copyright file="ElementBaseChildRowComparer.cs" company="RHEA System S.A.">
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

namespace DEHPCommon.UserInterfaces.ViewModels.Comparers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CDP4Common.CommonData;
    using CDP4Common.Comparers;
    using CDP4Common.EngineeringModelData;

    using DEHPCommon.UserInterfaces.ViewModels.Interfaces;
    using DEHPCommon.UserInterfaces.ViewModels.Rows.ElementDefinitionTreeRows;

    /// <summary>
    /// The <see cref="IComparer{T}"/> used to sort the child rows of the <see cref="ElementBaseRowViewModel{T}"/>
    /// </summary>
    public class ElementBaseChildRowComparer : IComparer<IRowViewModelBase<Thing>>
    {
        /// <summary>
        /// The Permissible Kind of child <see cref="IRowViewModelBase{T}"/>
        /// </summary>
        private static readonly List<Type> PermissibleRowTypes = new List<Type>
        {
            typeof(ParameterOrOverrideBaseRowViewModel),
            typeof(ParameterSubscriptionRowViewModel),
            typeof(ParameterGroupRowViewModel),
            typeof(ElementUsageRowViewModel)
        };

        /// <summary>
        /// Compares two <see cref="IRowViewModelBase{Thing}"/>
        /// </summary>
        /// <param name="x">The first <see cref="IRowViewModelBase{Thing}"/> to compare</param>
        /// <param name="y">The second <see cref="IRowViewModelBase{Thing}"/> to compare</param>
        /// <returns>
        /// Less than zero : x is "lower" than y 
        /// Zero: x "equals" y. 
        /// Greater than zero: x is "greater" than y.
        /// </returns>
        public int Compare(IRowViewModelBase<Thing> x, IRowViewModelBase<Thing> y)
        {
            if (x == null || y == null)
            {
                throw new ArgumentNullException();
            }

            var xType = x.GetType();
            var yType = y.GetType();

            if (!PermissibleRowTypes.Any(type => type.IsAssignableFrom(xType)) || !PermissibleRowTypes.Any(type => type.IsAssignableFrom(yType)))
            {
                throw new NotSupportedException("The list contains other types of row than the specified ones.");
            }

            if (xType == yType)
            {
                return this.CompareSameType(x, y, yType);
            }

            if ((typeof(ParameterOrOverrideBaseRowViewModel).IsAssignableFrom(xType) || typeof(ParameterSubscriptionRowViewModel).IsAssignableFrom(xType)) &&
                (typeof(ParameterOrOverrideBaseRowViewModel).IsAssignableFrom(yType) || typeof(ParameterSubscriptionRowViewModel).IsAssignableFrom(yType)))
            {
                return this.CompareSameType(x, y, yType);
            }

            if (typeof(ParameterOrOverrideBaseRowViewModel).IsAssignableFrom(xType) ||
                typeof(ParameterSubscriptionRowViewModel).IsAssignableFrom(xType))
            {
                return -1;
            }

            if (typeof(ElementUsageRowViewModel).IsAssignableFrom(xType))
            {
                return 1;
            }

            // x is a ParameterGroupRow
            if(typeof(ParameterOrOverrideBaseRowViewModel).IsAssignableFrom(yType) ||
                typeof(ParameterSubscriptionRowViewModel).IsAssignableFrom(yType))
            {
                return 1;
            }

            // x is ParameterGroupRow, y is ElementUsageRow
            return -1;
        }

        /// <summary>
        /// Compares two <see cref="IRowViewModelBase{Thing}"/> of the same type
        /// </summary>
        /// <param name="x">The First <see cref="IRowViewModelBase{Thing}"/></param>
        /// <param name="y">The second <see cref="IRowViewModelBase{Thing}"/></param>
        /// <param name="type">The actual Type</param>
        /// <returns>
        /// Less than zero : x is "lower" than y 
        /// Zero: x "equals" y. 
        /// Greater than zero: x is "greater" than y.
        /// </returns>
        private int CompareSameType(IRowViewModelBase<Thing> x, IRowViewModelBase<Thing> y, Type type)
        {
            if (typeof(ParameterOrOverrideBaseRowViewModel).IsAssignableFrom(type) || typeof(ParameterSubscriptionRowViewModel).IsAssignableFrom(type))
            {
                var comparer = new ParameterBaseComparer();
                return comparer.Compare((ParameterBase)x.Thing, (ParameterBase)y.Thing);
            }

            if (typeof(ParameterGroupRowViewModel).IsAssignableFrom(type))
            {
                var comparer = new ParameterGroupComparer();
                return comparer.Compare((ParameterGroup)x.Thing, (ParameterGroup)y.Thing);
            }

            var usageComparer = new ElementUsageComparer();
            return usageComparer.Compare((ElementUsage)x.Thing, (ElementUsage)y.Thing);
        }
    }
}