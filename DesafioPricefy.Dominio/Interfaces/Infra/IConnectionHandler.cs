using Npgsql;

namespace DesafioPricefy.Dominio.Interfaces.Infra
{
    public interface IConnectionHandler
    {
        NpgsqlConnection Create(string connectionString = null);
    }
}
