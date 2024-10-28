using OneTrack.PM.Entities.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Reflection;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace OneTrack.PM.Entities.Helpers
{
    public static class DbContextExtensions
    {
        public static async Task<Tuple<IReadOnlyList<T>, DbParameterCollection>> ExecListStoredByNameAsync<T>(this OneTrackPMContext context, string storedName, params object[] parameters) where T : class, new()
        {
            await context.Database.OpenConnectionAsync();
            DbCommand cmd = context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = storedName;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            if (parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);
            IReadOnlyList<T> resultlist;
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                resultlist = reader.MapTolistAsyn<T>().ToList();
            }
            return new Tuple<IReadOnlyList<T>, DbParameterCollection>(resultlist, cmd.Parameters);
        }
        public static async Task<IReadOnlyList<T>> ExecListStoredByNameAsync<T>(this OneTrackPMContext context, bool withoutOutputParameters, string storedName, params object[] parameters) where T : new()
        {
            await context.Database.OpenConnectionAsync();
            DbCommand cmd = context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = storedName;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            if (parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);
            IReadOnlyList<T> resultlist;
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                resultlist = reader.MapTolistAsyn<T>().ToList();
            }
            return resultlist;
        }
        public static async Task<T> ExecuteStoredByNameAsync<T>(this OneTrackPMContext context, string storedName, params object[] parameters) where T : new()
        {
            await context.Database.OpenConnectionAsync();
            DbCommand cmd = context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = storedName;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            if (parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);
            T resultlist;
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                resultlist = reader.MapTolistAsyn<T>().FirstOrDefault();
            }
            return resultlist;
        }
        public static List<T> MapTolistAsyn<T>(this DbDataReader dr) where T : new()
        {
            if (dr != null && dr.HasRows)
            {
                var entity = typeof(T);
                var entities = new List<T>();
                var propDict = new Dictionary<string, PropertyInfo>();
                var props = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                propDict = props.ToDictionary(p => p.Name.ToUpper(), p => p);
                while (dr.Read())
                {
                    T newObject = new T();
                    for (int index = 0; index < dr.FieldCount; index++)
                    {
                        if (propDict.ContainsKey(dr.GetName(index).ToUpper()))
                        {
                            var info = propDict[dr.GetName(index).ToUpper()];
                            if (info != null && info.CanWrite)
                            {
                                var val = dr.GetValue(index);
                                info.SetValue(newObject, (val == DBNull.Value) ? null : val, null);
                            }
                        }
                    }
                    entities.Add(newObject);
                }
                return entities;
            }
            return new List<T>();
        }
    }
}
