﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace ShaderTools.CodeAnalysis.Completion
{
    /// <summary>
    /// Determines whether the enter key is passed through to the editor after it has been used to commit a completion item.
    /// </summary>
    public enum EnterKeyRule
    {
        Default = 0,

        /// <summary>
        /// The enter key is never passed through to the editor after it has been used to commit the completion item.
        /// </summary>
        Never,

        /// <summary>
        /// The enter key is always passed through to the editor after it has been used to commit the completion item.
        /// </summary>
        Always,

        /// <summary>
        /// The enter is key only passed through to the editor if the completion item has been fully typed out.
        /// </summary>
        AfterFullyTypedWord
    }
}