using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Starscream.Domain.Entities;

namespace Starscream.Data
{
    public class UserAutoMappingOverride : IAutoMappingOverride<User>
    {
        public void Override(AutoMapping<User> mapping)
        {                      
        }
    }
}