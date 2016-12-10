﻿using System;
using Microsoft.CodeAnalysis;
using static System.AttributeTargets;

namespace Traction {

    /// <summary>
    /// Base class for contract attributes.
    /// </summary>
    [AttributeUsage(Parameter | ReturnValue | Property, AllowMultiple = false, Inherited = true)]
    public abstract class ContractAttribute : Attribute {

        protected ContractAttribute() {
        }
        
        public abstract bool IsValidType(TypeInfo typeInfo);
    }
}
