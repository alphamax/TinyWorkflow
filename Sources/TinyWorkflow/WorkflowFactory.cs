namespace TinyWorkflow
{
	public class WorkflowFactory : IWorkflowFactory
	{
		public IWorkflow<T> Create<T>()
		{
			return new Workflow<T>();
		}

		public static IWorkflow<T> New<T>()
		{
			return new Workflow<T>();
		}
	}
}