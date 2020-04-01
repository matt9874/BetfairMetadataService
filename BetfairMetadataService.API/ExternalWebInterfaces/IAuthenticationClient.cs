using BetfairMetadataService.API.BetfairApi.Dtos;

namespace BetfairMetadataService.API.ExternalWebInterfaces
{
    public interface IAuthenticationClient
    {
        LoginResponse Login();
    }
}
