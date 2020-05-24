using BetfairMetadataService.Domain.BetfairDtos;
using System.Threading.Tasks;

namespace BetfairMetadataService.DataAccess.Interfaces.WebRequests
{
    public interface IAuthenticationClientAsync
    {
        Task<LoginResponse> Login();
    }
}
