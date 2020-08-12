using System;
using System.Collections.Generic;
using System.Text;

namespace DapperExample.Models
{
    public class User : BaseModel
    {
        [AttributeModel(IsPrimaryKey = true)]
        public string IDUser { get; set; }
        public string CodUser { get; set; }
        public string Description { get; set; }
        public string IDUserGroup { get; set; }
        [AttributeModel(IsComplexProperty = true,IsUpdateable = false)]
        public UserGroup UserGroup { get; set; }
    }
}
