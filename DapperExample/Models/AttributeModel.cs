using System;
using System.Collections.Generic;
using System.Text;

namespace DapperExample.Models
{
    public class AttributeModel : Attribute
    {
        public AttributeModel() { }
        public bool IsPrimaryKey { get; set; }
        public bool IsUpdateable { get; set; }
        public bool IsComplexProperty { get; set; }
    }
}
