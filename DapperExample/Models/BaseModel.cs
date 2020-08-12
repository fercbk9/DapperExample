using System;
using System.Collections.Generic;
using System.Text;

namespace DapperExample.Models
{
    public class BaseModel
    {
            public Dictionary<string, Type> RelationsDictionary = new Dictionary<string, Type>();
            public virtual Dictionary<string, Type> GetRelations()
            {
                return RelationsDictionary;
            }
    }
}
