using BetfairMetadataService.Domain.BetfairDtos;
using System.Threading.Tasks;

namespace BetfairMetadataService.WebRequests.Interfaces
{
    public interface IAuthenticationClientAsync
    {
        Task<LoginResponse> Login();
    }
}
