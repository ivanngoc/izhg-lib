using System.Threading;
using System.Threading.Tasks;

namespace IziHardGames
{
    public interface IHttpClientSimple
    {
        ValueTask<T> RequestAsync<T>(string method, string uri, CancellationToken ct = default);
        ValueTask<T> RequestAsync<T>(string method, string uri, string content, CancellationToken ct = default);
    }
}
