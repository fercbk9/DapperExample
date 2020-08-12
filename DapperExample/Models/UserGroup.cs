using System;
using System.Collections.Generic;
using System.Text;

namespace DapperExample.Models
{
    public class UserGroup : BaseModel
    {
        [AttributeModel(IsPrimaryKey = true)]
        public string IDUserGroup { get; set; }
        public string CodUserGroup { get; set; }
        public string Description { get; set; }
        [AttributeModel(IsComplexProperty = true,IsUpdateable = false)]
        public List<User> Users { get; set; }

        public UserGroup()
        {
            Users = new List<User>();
        }
        public override Dictionary<string, Type> GetRelations()
        {
            if (base.RelationsDictionary.Count > 0)
            {
                RelationsDictionary.Add("User", typeof(User));
            }
            return base.GetRelations();
        }

    }


}
