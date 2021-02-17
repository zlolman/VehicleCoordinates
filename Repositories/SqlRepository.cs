using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleCoordinates.Interfaces;
using VehicleCoordinates.Models;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace VehicleCoordinates.Repositories
{
    public class SqlRepository : IRepository
    {
        private readonly string connectionString = "Server=localhost;Database=Vehicles;Trusted_Connection=True;MultipleActiveResultSets=True;";
        public SqlRepository() 
        {
        }
        public async Task<List<Coordinate>> GetAll() 
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            string sqlExpression = "SELECT * FROM Coordinates";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                Coordinate coordinate = new Coordinate() { Id = reader.GetInt32(0), X_axis = reader.GetDouble(1), Y_axis = reader.GetDouble(2) };
                                coordinates.Add(coordinate);
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }                
            }
            return coordinates;
        }
        public async Task<Coordinate> Get(int id) 
        {
            Coordinate coordinate = null;
            string sqlExpression = "SELECT * FROM Coordinates WHERE Id = @Id}";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlParameter idParam = new SqlParameter("@Id", id);
                    command.Parameters.Add(idParam);
                    await command.ExecuteReaderAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        coordinate = new Coordinate() { Id = reader.GetInt32(0), X_axis = reader.GetDouble(1), Y_axis = reader.GetDouble(2) };
                    }
                    
                } 
                catch (SqlException ex) 
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
                
            }
            return coordinate;
        }
        public async void Create(Coordinate coordinate) 
        {
            string sqlExpression = "INSERT INTO Coordinates (X_axis, Y_axis) VALUES (@X_axis, @Y_axis)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try 
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlParameter xParam = new SqlParameter("@X_axis", coordinate.X_axis);
                    command.Parameters.Add(xParam);
                    SqlParameter yParam = new SqlParameter("@Y_axis", coordinate.Y_axis);
                    command.Parameters.Add(yParam);
                    await command.ExecuteNonQueryAsync();
                } 
                catch (SqlException ex) 
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
                
            }
        }
        public async void Update(Coordinate coordinate) 
        {
            string sqlExpression = "UPDATE Coordinates SET X_axis = @X_axis, Y_axis = @Y_axis WHERE Id = @Id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try 
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlParameter xParam = new SqlParameter("@X_axis", coordinate.X_axis);
                    command.Parameters.Add(xParam);
                    SqlParameter yParam = new SqlParameter("@Y_axis", coordinate.Y_axis);
                    command.Parameters.Add(yParam);
                    SqlParameter idParam = new SqlParameter("@Id", coordinate.Id);
                    command.Parameters.Add(idParam);
                    await command.ExecuteNonQueryAsync();

                } catch (SqlException ex) 
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
                
            }
        }
        public async void Delete(int id) 
        {
            string sqlExpression = "DELETE FROM Coordinates WHERE Id = @Id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try 
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlParameter idParam = new SqlParameter("@Id", id);
                    command.Parameters.Add(idParam);
                    int i = await command.ExecuteNonQueryAsync();
                } 
                catch (SqlException ex) 
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
                
            }
        }
    }
}
