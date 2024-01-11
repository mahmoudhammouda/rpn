using Rpnw.Infrastructure.Repository;
using Rpnw.Infrastructure.Repository.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.Common;
using Rpnw.CrossCutting;

namespace Rpnw.Infrastructure.Impl.Repository
{
    public class DapperGenericRepository<T> : IGenericRepository<T> where T : ModelEntity
    {
        private IDbConnection _dbConnection;

        private readonly string connectionString = " ";

        public DapperGenericRepository(IDbConnection connection)
        {
            // _connection = new SqlConnection(connectionString);
            _dbConnection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public int Add(T entity)
        {

            int newId = 0;
            try
            {
                string tableName = GetTableName();
                string columns = GetColumns(excludeKey: true);
                string properties = GetPropertyNames(excludeKey: true);
                //string query = $"INSERT INTO {tableName} ({columns}) VALUES ({properties})";
                string query = $"INSERT INTO {tableName} ({columns}) VALUES ({properties}); SELECT last_insert_rowid();";

                //rowsEffected = _dbConnection.Execute(query, entity);
                newId = _dbConnection.QuerySingle<int>(query, entity);


            }
            catch (Exception ex)
            {
                throw;
            }

            return newId;

        }

        public bool Delete(T entity)
        {
            int rowsEffected = 0;
            try
            {
                string tableName = GetTableName();
                string keyColumn = GetKeyColumnName();
                string keyProperty = GetKeyPropertyName();
                string query = $"DELETE FROM {tableName} WHERE {keyColumn} = @{keyProperty}";

                rowsEffected = _dbConnection.Execute(query, entity);
            }
            catch (Exception ex) { }

            return rowsEffected > 0 ? true : false;
        }

        public IEnumerable<T> GetAll()
        {
            IEnumerable<T> result = null;
            try
            {
                string tableName = GetTableName();
                string query = $"SELECT * FROM {tableName}";

                result = _dbConnection.Query<T>(query);
            }
            catch (Exception ex) { }

            return result;
        }

        public T GetById(int id)
        {
            IEnumerable<T> result = null;
            try
            {
                string tableName = GetTableName();
                string keyColumn = GetKeyColumnName();
                string query = $"SELECT * FROM {tableName} WHERE {keyColumn} = '{id}'";

                result = _dbConnection.Query<T>(query);
            }
            catch (Exception ex) { }

            return result.FirstOrDefault();
        }

        public bool Update(T entity)
        {
            int rowsEffected = 0;
            try
            {
                string tableName = GetTableName();
                string keyColumn = GetKeyColumnName();
                string keyProperty = GetKeyPropertyName();

                StringBuilder query = new StringBuilder();
                query.Append($"UPDATE {tableName} SET ");

                foreach (var property in GetProperties(true))
                {
                    var columnAttr = property.GetCustomAttribute<ColumnAttribute>();

                    string propertyName = property.Name;
                    string columnName = columnAttr.Name;

                    query.Append($"{columnName} = @{propertyName},");
                }

                query.Remove(query.Length - 1, 1);

                query.Append($" WHERE {keyColumn} = @{keyProperty}");

                rowsEffected = _dbConnection.Execute(query.ToString(), entity);
            }
            catch (Exception ex)
            {
                throw;
            }

            return rowsEffected > 0 ? true : false;
        }

        public IEnumerable<T> FindByCriteria(object criteria)
        {
            var queryBuilder = new StringBuilder($"SELECT * FROM {GetTableName()} WHERE ");
            var properties = criteria.GetType().GetProperties();
            var parameters = new DynamicParameters();
            bool isFirst = true;

            foreach (var property in properties)
            {
                var value = property.GetValue(criteria);
                if (value != null)
                {
                    if (!isFirst)
                    {
                        queryBuilder.Append(" AND ");
                    }
                    queryBuilder.Append($"{property.Name} = @{property.Name}");
                    parameters.Add($"@{property.Name}", value);
                    isFirst = false;
                }
            }

            if (isFirst) // No criteria were added, so return all records
            {
                return GetAll();
            }

            var query = queryBuilder.ToString();
            return _dbConnection.Query<T>(query, parameters);
        }


        public PageResult<T> FindByCriteria(object criteria, int pageIndex, int pageSize)
        {
            var queryBuilder = new StringBuilder($"SELECT * FROM {GetTableName()} WHERE ");
            var countQueryBuilder = new StringBuilder($"SELECT COUNT(*) FROM {GetTableName()} WHERE ");
            var parameters = new DynamicParameters();
            bool isFirst = true;

            foreach (var property in criteria.GetType().GetProperties())
            {
                var value = property.GetValue(criteria);
                if (value != null)
                {
                    if (!isFirst)
                    {
                        queryBuilder.Append(" AND ");
                        countQueryBuilder.Append(" AND ");
                    }
                    queryBuilder.Append($"{property.Name} = @{property.Name}");
                    countQueryBuilder.Append($"{property.Name} = @{property.Name}");
                    parameters.Add($"@{property.Name}", value);
                    isFirst = false;
                }
            }

            if (isFirst) // No criteria were added, so remove the WHERE clause
            {
                queryBuilder = new StringBuilder($"SELECT * FROM {GetTableName()}");
                countQueryBuilder = new StringBuilder($"SELECT COUNT(*) FROM {GetTableName()}");
            }

            //queryBuilder.Append($" ORDER BY {GetKeyColumnName()} OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");
            queryBuilder.Append($" ORDER BY {GetKeyColumnName()} LIMIT @PageSize OFFSET @Offset");
            parameters.Add("@Offset", pageIndex * pageSize);
            parameters.Add("@PageSize", pageSize);

      
                var totalItemCount = _dbConnection.ExecuteScalar<int>(countQueryBuilder.ToString(), parameters);
                var items = _dbConnection.Query<T>(queryBuilder.ToString(), parameters).ToList();

                return new PageResult<T>
                {
                    Items = items,
                    TotalCount = totalItemCount
                };
          
        }

        public PageResult<T> GetAll(int pageIndex, int pageSize)
        {
            // Ici, nous n'avons pas de critères spécifiques, donc nous passons un objet vide
            return FindByCriteria(new object(), pageIndex, pageSize);
        }

        private string GetTableName()
        {
            string tableName = "";
            var type = typeof(T);
            var tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr != null)
            {
                tableName = tableAttr.Name;
                return tableName;
            }

            return type.Name + "s";
        }
        public static string GetKeyColumnName()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object[] keyAttributes = property.GetCustomAttributes(typeof(KeyAttribute), true);

                if (keyAttributes != null && keyAttributes.Length > 0)
                {
                    object[] columnAttributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);

                    if (columnAttributes != null && columnAttributes.Length > 0)
                    {
                        ColumnAttribute columnAttribute = (ColumnAttribute)columnAttributes[0];
                        return columnAttribute.Name;
                    }
                    else
                    {
                        return property.Name;
                    }
                }
            }

            return null;
        }

        private string GetColumns(bool excludeKey = false)
        {
            var type = typeof(T);
            var columns = string.Join(", ", type.GetProperties()
                .Where(p => !excludeKey || !p.IsDefined(typeof(KeyAttribute)))
                .Select(p =>
                {
                    var columnAttr = p.GetCustomAttribute<ColumnAttribute>();
                    return columnAttr != null ? columnAttr.Name : p.Name;
                }));

            return columns;
        }

        protected string GetPropertyNames(bool excludeKey = false)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            var values = string.Join(", ", properties.Select(p =>
            {
                return $"@{p.Name}";
            }));

            return values;
        }

        protected IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            return properties;
        }

        protected string GetKeyPropertyName()
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.GetCustomAttribute<KeyAttribute>() != null);

            if (properties.Any())
            {
                return properties.FirstOrDefault().Name;
            }

            return null;
        }
    }
}
