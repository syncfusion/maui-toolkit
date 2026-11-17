// <copyright file="DrawerEventArgs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Syncfusion.Maui.Toolkit.NavigationDrawer
{
    using System;

    /// <summary>
    /// Provides data for drawer open and close events.
    /// </summary>
    public sealed class DrawerEventArgs : EventArgs
    {
        /// <summary>
        /// Gets a value indicating whether the event originated from the primary drawer.
        /// </summary>
        public bool IsPrimary { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawerEventArgs"/> class.
        /// </summary>
        /// <param name="isPrimary">True if the event is for the primary drawer, otherwise false.</param>
        public DrawerEventArgs(bool isPrimary)
        {
            this.IsPrimary = isPrimary;
        }
    }
}