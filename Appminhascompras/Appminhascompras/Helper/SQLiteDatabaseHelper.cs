using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Appminhascompras.Model;
using SQLite;

namespace Appminhascompras.Helper
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }
        public Task<int> Insert(Produto p)
        {
            return _conn.InsertAsync(p);
        }

        public Task<List<Produto>> Update(Produto p)
        {
            string sql = "UPDATE produto SET descricao=?, quantidade=?, preco=? WHERE id=?";

            return _conn.QueryAsync<Produto>(sql, p.descricao, p.quantidade, p.preco, p.id);
        }

        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }

        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.id == id);
        }

        public Task<List<Produto>> Search(string buscou)
        {
            string sql = "SELECT * FROM produto WHERE descricao LIKE '%" + buscou + "%' ";

            return _conn.QueryAsync<Produto>(sql);
        }
    }
}
