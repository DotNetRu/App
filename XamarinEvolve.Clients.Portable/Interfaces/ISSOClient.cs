using System;
using System.Threading.Tasks;

namespace XamarinEvolve.Clients.Portable
{
    public interface ISSOClient
    {
        Task<AccountResponse> LoginAsync(string username, string password);
		Task<AccountResponse> LoginAnonymouslyAsync(string impersonateUserId);

		Task LogoutAsync();
    }
}

