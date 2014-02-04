﻿// -----------------------------------------------------------------------
// <copyright file="ITemplatedControl.cs" company="Steven Kirk">
// Copyright 2014 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls
{
    using System.Collections.Generic;

    public interface ITemplatedControl
    {
        IEnumerable<IVisual> VisualChildren { get; }
    }
}
