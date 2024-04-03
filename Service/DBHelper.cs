using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace Project.Donna.Service
{
    public class DBHelper
    {
        //DataSet Return
        public static DataSet ExecuteDataSet(string pConn, string pQryName, List<SqlParameter> pParam)
        {
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(pConn))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = pQryName;
                    cmd.Parameters.AddRange(pParam.ToArray());

                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                    {
                        adap.Fill(ds);
                    }
                }
            }

            return ds;
        }

        //DataTable Return
        public static DataTable ExecuteDataTable(string pConn, string pQryName, List<SqlParameter> pParam)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(pConn))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = pQryName;
                    cmd.Parameters.AddRange(pParam.ToArray());

                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                    {
                        adap.Fill(dt);
                    }
                }
            }

            return dt;
        }

        //int Return
        public static int ExecuteNonQuery(string pConn, string pQryName, List<SqlParameter> pParam)
        {
            DataSet ds = new DataSet();
            int iResult = 0;

            using (SqlConnection con = new SqlConnection(pConn))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = pQryName;
                    cmd.Parameters.AddRange(pParam.ToArray());

                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }
                    iResult = cmd.ExecuteNonQuery();
                }
            }

            return iResult;
        }

    }
}