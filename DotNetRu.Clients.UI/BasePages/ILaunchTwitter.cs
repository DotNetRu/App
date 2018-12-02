namespace DotNetRu.Clients.Portable.Interfaces
{
    public interface ILaunchTwitter
    {
        bool OpenUserName(string username);
        bool OpenStatus(string statusId);
    }
}

