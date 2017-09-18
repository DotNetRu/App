namespace XamarinEvolve.DataStore.Mock.Abstractions
{
    using System.Threading.Tasks;

    using XamarinEvolve.DataObjects;

    public interface IConferenceFeedbackStore : IBaseStore<ConferenceFeedback>
	{
		Task<bool> LeftFeedback();
		Task DropConferenceFeedback();
	}
}
