using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Certification70_487_Framework4._6.Unit_2.ADO_NET
{
    [TestClass]
    public class ADONetTest
    {
        [TestMethod]
        public void SQLDataAdapter_Test()
        {
            using (SqlConnection sqlConnection = new SqlConnection("Data Source=.; Initial Catalog=CertificationCourse; Integrated Security=true;"))
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.Employee WHERE LastName LIKE '%' + @LastName +'%'", sqlConnection))
                {
                    command.Parameters.AddWithValue("@LastName", "ara");
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command))
                    {
                        DataSet currentSet = new DataSet("CurrentSet");
                        sqlDataAdapter.Fill(currentSet);
                        sqlDataAdapter.Update(currentSet);
                    }
                }
            }
        }
    }
}
