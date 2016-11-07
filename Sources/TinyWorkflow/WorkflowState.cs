namespace TinyWorkflow
{
	/// <summary>
	/// State of workflow
	/// </summary>
	public enum WorkflowState
	{
		/// <summary>
		/// Workflow is running
		/// </summary>
		Running,

		/// <summary>
		/// Workflow is blocked, wwaiting for being unblocked
		/// </summary>
		Blocked,

		/// <summary>
		/// Workflow has not been started
		/// </summary>
		NotRunning,

		/// <summary>
		/// Workflow has ended
		/// </summary>
		End,
	}
}