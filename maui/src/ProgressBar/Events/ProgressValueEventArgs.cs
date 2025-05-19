namespace Syncfusion.Maui.Toolkit.ProgressBar
{
    /// <summary>
    /// Provides event data for the <see cref="ProgressBarBase.ProgressChanged"/> and <see cref="ProgressBarBase.ProgressCompleted"/> event.
    /// </summary>
    /// <example>
    /// # [XAML](#tab/tabid-1)
    /// This snippet shows how to hook ProgressCompleted event for <see cref="SfLinearProgressBar"/>
    /// <code><![CDATA[
    /// <progressBar:SfLinearProgressBar ProgressCompleted="progressBar_ProgressCompleted"
    ///                                  ProgressChanged="progressBar_ProgressChanged" 
    ///                                  Progress = "100"/>
    /// ]]></code>
    /// This snippet shows how to hook ProgressCompleted event for <see cref="SfCircularProgressBar"/>
    /// <code><![CDATA[
    /// <progressBar:SfCircularProgressBar ProgressCompleted="progressBar_ProgressCompleted" 
    ///                                    ProgressChanged="progressBar_ProgressChanged" 
    ///                                    Progress = "100"/>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// Snippet for <see cref="SfLinearProgressBar"/>
    /// <code><![CDATA[
    /// private void progressBar_ProgressCompleted(object sender, ProgressValueEventArgs e)
    /// {
    ///    var value = e.Progress;
    /// }
    /// 
    /// private void progressBar_ProgressChanged(object sender, ProgressValueEventArgs e)
    /// {
    ///    var value = e.Progress;
    /// }
    /// ]]></code>
    /// Snippet for <see cref="SfCircularProgressBar"/>
    /// <code><![CDATA[
    /// private void progressBar_ProgressCompleted(object sender, ProgressValueEventArgs e)
    /// {
    ///    var value = e.Progress;
    /// }
    /// 
    /// private void progressBar_ProgressChanged(object sender, ProgressValueEventArgs e)
    /// {
    ///    var value = e.Progress;
    /// }
    /// ]]></code>
    /// ***
    /// </example>
    public class ProgressValueEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the current progress.
        /// </summary>
        /// <value>
        /// The current progress value.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// This snippet shows how to hook ProgressCompleted event for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar ProgressCompleted="progressBar_ProgressCompleted"
        ///                                  ProgressChanged="progressBar_ProgressChanged" 
        ///                                  Progress = "100"/>
        /// ]]></code>
        /// This snippet shows how to hook ProgressCompleted event for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar ProgressCompleted="progressBar_ProgressCompleted" 
        ///                                    ProgressChanged="progressBar_ProgressChanged" 
        ///                                    Progress = "100"/>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// private void progressBar_ProgressCompleted(object sender, ProgressValueEventArgs e)
        /// {
        ///    var value = e.Progress;
        /// }
        /// 
        /// private void progressBar_ProgressChanged(object sender, ProgressValueEventArgs e)
        /// {
        ///    var value = e.Progress;
        /// }
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// private void progressBar_ProgressCompleted(object sender, ProgressValueEventArgs e)
        /// {
        ///    var value = e.Progress;
        /// }
        /// 
        /// private void progressBar_ProgressChanged(object sender, ProgressValueEventArgs e)
        /// {
        ///    var value = e.Progress;
        /// }
        /// ]]></code>
        /// ***
        /// </example>
        public double Progress
        {
            get
            {
                return CurrentProgress;
            }
        }

        /// <summary>
        /// Gets or sets the current progress value.
        /// </summary>
        internal double CurrentProgress { get; set; }
    }
}