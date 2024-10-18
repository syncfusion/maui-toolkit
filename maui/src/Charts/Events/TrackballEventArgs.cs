using System;
using System.Collections.Generic;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Provides event data for the <see cref="SfCartesianChart.TrackballCreated"/> event, which is triggered when the trackball is created in the chart.
	/// </summary>
	public class TrackballEventArgs : EventArgs
    {
        #region Public Property 

        /// <summary>
        /// Gets the collection of trackball information that is created for each series in the chart when the trackball is active.
        /// This collection is updated whenever the trackball moves across the chart, reflecting the current data points at the trackball's position.
        /// </summary>
        public List<TrackballPointInfo> TrackballPointsInfo { get; internal set; }

		#endregion

		#region Constructor

		/// <summary>
		///  Initializes a new instance of the <see cref="TrackballEventArgs"/> class.
		/// </summary>
		/// <exclude/>
		public TrackballEventArgs(List<TrackballPointInfo> chartPointsInfo)
        {
            TrackballPointsInfo = chartPointsInfo;
        }

        #endregion
    }
}