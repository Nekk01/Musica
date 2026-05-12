using System;

namespace Musica;

public interface IProfileService
{
    Task<UserModel> FindByID(int id);
    Task<bool> UpdateProfile(int userId, UserModel model);
}
