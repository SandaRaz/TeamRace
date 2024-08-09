﻿
using Npgsql;

namespace Evaluation.Models.Cnx
{
    public class Connex
    {
        static string server = "localhost";
        static string port = "5432";
        static string userId = "postgres";
        static string password = "mdpprom15";
        static string database = "evaltest";
        public static NpgsqlConnection getConnection()
        {
            String connectionString = $"Server={server};Port={port};User Id={userId};Password={password};Database={database};";
            NpgsqlConnection cnx = new NpgsqlConnection(connectionString);
            return cnx;
        }

        public static String createId(NpgsqlConnection cnx, String nomsequence, String prefixe, int len)
        {
            String id = "";
            Boolean closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            int seq = -1;
            String sql = "SELECT nextval(@nomsequence)";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@nomsequence", nomsequence);
                Object tempSeq = command.ExecuteScalar() ?? throw new Exception($"SELECT nextval({nomsequence}) return NULL");
                seq = Convert.ToInt32(tempSeq);
            }

            if (seq == -1)
            {
                cnx.Close();
                throw new Exception($"Erreur lors de la recuperation de la valeur de la sequence: seq = -1");
            }
            else
            {
                int nbZero = len - (prefixe.Length + seq.ToString().Length);
                id = $"{prefixe}{new string('0', nbZero)}{seq}";
            }

            if (closed)
            {
                cnx.Close();
            }
            return id;
        }
    }
}
