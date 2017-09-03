using System.Threading.Tasks;
using XamarinEvolve.DataObjects;

namespace XamarinEvolve.DataStore.Azure.Abstractions
{
	public interface IConferenceFeedbackStore : IBaseStore<ConferenceFeedback>
	{
		Task<bool> LeftFeedback();
		Task DropConferenceFeedback();
	}
}
