using System.Threading.Tasks;
using XamarinEvolve.DataObjects;

namespace XamarinEvolve.DataStore.Abstractions
{
	public interface IConferenceFeedbackStore : IBaseStore<ConferenceFeedback>
	{
		Task<bool> LeftFeedback();
		Task DropConferenceFeedback();
	}
}
