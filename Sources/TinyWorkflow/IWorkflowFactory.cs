namespace TinyWorkflow
{
	public interface IWorkflowFactory
	{
		IWorkflow<T> Create<T>();
	}
}