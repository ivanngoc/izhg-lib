using System;
using System.Threading.Tasks;

namespace IziHardGames
{
    public interface IHttpClient
    {
        ValueTask<ReadOnlyMemory<byte>> SendRequest(EHttpMethod method, string uri, ReadOnlyMemory<byte> body);
    }
}
