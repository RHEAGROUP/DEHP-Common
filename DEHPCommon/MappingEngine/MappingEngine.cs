﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MappingEngine.cs"company="RHEA System S.A.">
//    Copyright(c) 2020 RHEA System S.A.
//    Author: Sam Gerené, Alex Vorobiev, Alexander van Delft, Kamil Wojnowski, Nathanael Smiechowski.
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

namespace DEHPCommon.MappingEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using DEHPCommon.Exceptions;
    using DEHPCommon.MappingRules.Core;
    
    using NLog;

    /// <summary>
    /// The <see cref="MappingEngine"/> allows to map dst tool models to ECSS-E-TM-10-25A model or the other way arround
    /// </summary>
    public class MappingEngine : IMappingEngine
    {
        /// <summary>
        /// Gets the constructor parameter name of this <see cref="MappingEngine"/>
        /// </summary>
        public static readonly string ParameterName = "ruleAssembly";

        /// <summary>
        /// The <see cref="NLog"/> logger
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets a <see cref="Dictionary{TKey,TValue}"/> that contains all the available <see cref="IMappingRule"/> based on the provided assembly
        /// where the Key is the Input type of the Value of a corresponding <see cref="IMappingRule{TInput,TOutput}"/>
        /// </summary>
        public Dictionary<Type, IMappingRule> Rules { get; private set; } = new Dictionary<Type, IMappingRule>();

        /// <summary>
        /// Initializes a new <see cref="MappingEngine"/>
        /// </summary>
        /// <param name="ruleAssembly">The assembly that contains the rules</param>
        public MappingEngine(Assembly ruleAssembly)
        {
            this.PopulateRules(ruleAssembly);
        }

        /// <summary>
        /// Maps the provided <see cref="object"/> to another type if a rule is found
        /// </summary>
        /// <param name="input">The object to map</param>
        /// <returns>The transformed <paramref name="input"/></returns>
        public object Map(object input)
        {
            if (!this.Rules.Any())
            {
                return default;
            }

            if (this.Rules.TryGetValue(input.GetType(), out var foundRule))
            {
                try
                {
                    return foundRule.GetType().GetMethod("Transform")?.Invoke(foundRule, new[] { input });
                }
                catch (TargetInvocationException exception)
                {
                    throw new MappingException($"Could not map {input} to a {this.GetBaseTypeGenericArgument(foundRule.GetType(), 1)}", exception.InnerException);
                }
            }

            Logger.Warn($"Could not map {input}, no corresponding mapping rule has been found");

            return default;
        }

        /// <summary>
        /// Populates the rules that have been found. A rule shall implement <see cref="MappingRule{TInput,TOutput}"/>
        /// </summary>
        /// <param name="ruleAssembly">The assembly that contains the rules</param>
        private void PopulateRules(Assembly ruleAssembly)
        {
            this.Rules = ruleAssembly.GetTypes()
                .Where(x => x.GetInterface(nameof(IMappingRule)) != null && x.BaseType != null && x.BaseType.IsAbstract && !x.IsAbstract)
                .ToDictionary(type => this.GetBaseTypeGenericArgument(type, 0), type => (IMappingRule) Activator.CreateInstance(type));
        }

        /// <summary>
        /// Gets the generic argument from <see cref="Type.BaseType"/>
        /// </summary>
        /// <param name="type">The <see cref="Type"/></param>
        /// <param name="index">Indicates which argument to get</param>
        /// <returns>The generic argument <see cref="Type"/></returns>
        public Type GetBaseTypeGenericArgument(Type type, int index = 0)
        {
            if (type.BaseType == null)
            {
                throw new ArgumentException($"The provided type {type.Name} does not have a ${nameof(Type.BaseType)}", nameof(type));
            }
            
            return type.BaseType.GetGenericArguments()[index];
        }
    }
}
