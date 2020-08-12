using Dapper;
using DapperExample.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace DapperExample
{
    class Program
    {
        static string connectionstring;
        static void Main(string[] args)
        {
            
            Console.WriteLine("Hello World!");
            connectionstring = ConfigurationManager.ConnectionStrings[1].ConnectionString;
            Console.WriteLine(connectionstring);
            using (var db = new SqlConnection(connectionstring))
            {
                string sql_insert = @"Insert into [User](IDUserGroup,IDUser,CodUser,Description) VALUES (@IDUserGroup,@IDUser,@CodUser,@Description)";
                var userGroup = new User() { CodUser = "1", IDUser = Guid.NewGuid().ToString(), Description = "Usuario 1",IDUserGroup = "740fff7e-8892-4a46-ad07-7dde515a48bb" };
                var affected_rows = db.Execute(sql_insert, userGroup);
                //Console.WriteLine("Filas Afectadas: " + affected_rows);

                var userGroupList = Select2<UserGroup>("SELECT * FROM UserGroup");
                var listOfPrimaryKeys = GetPrimaryKeyProperties(userGroup);
            }
        }

        public static IEnumerable<EntityT> Select<EntityT>(string sql, Type type, object param = null)
        {
            try
            {
                using (var db = new SqlConnection(connectionstring))
                {
                    return db.Query<EntityT>(sql).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static IEnumerable<EntityT> Select2<EntityT>(string sql, object param = null) where EntityT : BaseModel, new()
        {
            try
            {
                var model = new EntityT();
                if (model.RelationsDictionary.Count > 0)
                {

                }
                using (var db = new SqlConnection(connectionstring))
                {
                    var list = GetComplexProperties<EntityT>();
                    var result = db.Query<EntityT>(sql).ToList();
                    if (list.Count() > 0)
                    {
                        foreach (var item in result)
                        {
                            foreach (var item2 in list)
                            {
                                var property = item.GetType().GetProperty(item2.Key);
                                Type type = property.PropertyType.DeclaringType;
                                property.SetValue(item, Select2<User>("select * from [User]"));
                                var aux = Activator.CreateInstance(type);
                            }
                        }
                    }
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        private static List<string> GetPrimaryKeyProperties<EntityT>(EntityT model)
        {
            var list = new List<string>();
            model.GetType().GetProperties().ToList().ForEach(x =>
            {
                if (x.GetCustomAttributes<Models.AttributeModel>().Count() > 0)
                    if (x.GetCustomAttribute<Models.AttributeModel>().IsPrimaryKey)
                    {
                        list.Add(x.Name);
                    }
            }
            );
            return list;
        }
        private static Dictionary<string,Type> GetComplexProperties<EntityT>()
        {
            var list = new Dictionary<string, Type>();
            typeof(EntityT).GetProperties().ToList().ForEach(x =>
            {
                if (x.GetCustomAttributes<Models.AttributeModel>().Count() > 0)
                    if (x.GetCustomAttribute<Models.AttributeModel>().IsComplexProperty)
                    {
                        list.Add(x.Name,x.GetType());
                    }
            }
            );
            return list;
        }

        private static void GetComplexResults<EntityT>(Dictionary<string,Type> complexPropertiesList,ref EntityT data)
        {
            foreach (var item in complexPropertiesList)
            {
                var property = data.GetType().GetProperty(item.Key);
                property.SetValue(data, null);
            }
        }
    }
}
