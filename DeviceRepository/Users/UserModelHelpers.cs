using DeviceRepository.Common.Models;
using DeviceRepository.DataAccess.Entities;
using DeviceRepository.Users.Interfaces;

namespace DeviceRepository.Users;

internal static class UserModelsHelper
{
    public static IUserModel GetModel(this User user)
    {
        var userModel = new UserModel(user.Login)
        {
            Name = user.Name
        };

        userModel.AppendAdditionalInfo(user);
        return userModel;
    }
}