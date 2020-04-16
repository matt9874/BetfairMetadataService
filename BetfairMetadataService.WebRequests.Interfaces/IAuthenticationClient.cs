using BetfairMetadataService.Domain.BetfairDtos;

namespace BetfairMetadataService.WebRequests.Interfaces
{
    public interface IAuthenticationClient
    {
        LoginResponse Login();
    }
}
