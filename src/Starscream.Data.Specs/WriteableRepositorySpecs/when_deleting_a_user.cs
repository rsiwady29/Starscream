using System.Collections.Generic;
using System.Linq;
using AcklenAvenue.Data.NHibernate.Testing;
using Starscream.Domain;
using Starscream.Domain.Entities;
using Starscream.Domain.Services;
using Starscream.Domain.ValueObjects;
using Machine.Specifications;
using NHibernate;

namespace Starscream.Data.Specs.WriteableRepositorySpecs
{
    public class when_deleting_a_user
    {
        static IWriteableRepository _writeableRepository;
        static ISession _session;
        static User _userToDelete;

        Establish context =
            () =>
                {
                    _session = InMemorySession.New(new MappingScheme());
                    _writeableRepository = new WriteableRepository(_session);

                    var users = new List<User>
                                    {
                                        new User("test1", "test1@test.com", new EncryptedPassword("password")),
                                        new User("test2-match", "test2@test.com", new EncryptedPassword("password")),
                                        new User("test3", "test2@test.com", new EncryptedPassword("password"))
                                    };
                    users.ForEach(x => _session.Save(x));
                    _session.Flush();

                    _userToDelete = users.First(x => x.Name == "test2-match");
                    _userToDelete.ChangeEmailAddress("another@test.com");
                };

        Because of =
            () => _writeableRepository.Delete<User>(_userToDelete.Id);

        It should_make_the_change_in_the_session =
            () => ShouldExtensionMethods.ShouldBeNull(_session.Get<User>(_userToDelete.Id));
    }
}