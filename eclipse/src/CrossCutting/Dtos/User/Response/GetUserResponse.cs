using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.UserRole.Response;

namespace CrossCutting.Dtos.User.Response;

public class GetUserResponse : BaseResponse
{
    public long TipoPerfilId { get; set; }

    public string Nome { get; set; }

    public string Email { get; set; }

    public bool Ativo { get; set; }

    public string Senha { get; set; }

    public GetUserRoleResponse TipoPerfil { get; set; }

    public GetUserResponse()
    {

    }

    public GetUserResponse GetUser(
        Domain.Entities.User user)
    {
        if (user == null)
            return null;

        GetUserResponse getUserResponse = new GetUserResponse();
        getUserResponse.Id = user.Id;
        getUserResponse.TipoPerfilId = user.UserRoleId;
        getUserResponse.Nome = user.Name;
        getUserResponse.Email = user.Email;
        getUserResponse.Ativo = user.IsActive;
        getUserResponse.TipoPerfil = new GetUserRoleResponse().GetUserRole(user.UserRole);
        getUserResponse.DataCriacao = user.DateCreated;
        getUserResponse.DataAlteracao = user.DateUpdated;

        return getUserResponse;
    }
}