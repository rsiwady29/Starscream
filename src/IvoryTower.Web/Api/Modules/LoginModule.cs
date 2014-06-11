﻿using System;
using IvoryTower.Data;
using IvoryTower.Domain;
using IvoryTower.Web.Api.Infrastructure.Exceptions;
using IvoryTower.Web.Api.Requests;
using IvoryTower.Web.Api.Responses;
using Nancy;
using Nancy.ModelBinding;

namespace IvoryTower.Web.Api.Modules
{
    public class LoginModule : NancyModule
    {
        public LoginModule(IPasswordEncryptor passwordEncryptor, IReadOnlyRepository readOnlyRepository, IUserSessionFactory userSessionFactory)
        {
            Post["/login"] =
                _ =>
                    {
                        var loginInfo = this.Bind<LoginRequest>();
                        if (loginInfo.Email == null) throw new UserInputPropertyMissingException("Email");
                        if (loginInfo.Password == null) throw new UserInputPropertyMissingException("Password");

                        EncryptedPassword encryptedPassword = passwordEncryptor.Encrypt(loginInfo.Password);

                        try
                        {
                            var user =
                                readOnlyRepository.First<User>(
                                    x => x.Email == loginInfo.Email && x.EncryptedPassword == encryptedPassword.Password);

                            var userSession = userSessionFactory.Create(user);

                            return new SuccessfulLoginResponse<Guid>(userSession.Id, user.Name, userSession.Expires);
                        }
                        catch (ItemNotFoundException<User>)
                        {
                            throw new UnauthorizedAccessException();
                        }
                    };
        }
    }
}