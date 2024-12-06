// <copyright file="Enum.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Syncfusion.Maui.Toolkit.TextInputLayout
{
	/// <summary>
	/// Defines the background appearance options for the <see cref="SfTextInputLayout"/>.
	/// </summary>
	public enum ContainerType
	{
		/// <summary>
		/// Displays a border around the input layout.
		/// </summary>
		Outlined,

		/// <summary>
		/// Shows a filled background with a baseline.
		/// </summary>
		Filled,

		/// <summary>
		/// Applies no background and uses compact spacing.
		/// </summary>
		None,
	}

	/// <summary>
	/// Specifies the positioning of leading and trailing views within the <see cref="SfTextInputLayout"/>.
	/// </summary>
	public enum ViewPosition
	{
		/// <summary>
		/// Places the view within the layout's boundaries.
		/// </summary>
		Inside,

		/// <summary>
		/// Positions the view outside the layout's boundaries.
		/// </summary>
		Outside,
	}
}
