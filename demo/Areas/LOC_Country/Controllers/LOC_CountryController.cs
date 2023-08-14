
using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using demo.Areas.LOC_Country.Models;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace demo.Areas.LOC_Country.Controllers
{
    [Area("LOC_Country")]
    [Route("LOC_Country/{Controller}/{action}")]
    public class LOC_CountryController : Controller
    {
        private IConfiguration Configuration;

        #region Con
        public LOC_CountryController(IConfiguration _configuration)
        {

            Configuration = _configuration;

        }
        #endregion

        #region Index

        public IActionResult Index()
        {
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();
            SqlCommand objcmd = conn.CreateCommand();
            objcmd.CommandType = System.Data.CommandType.StoredProcedure;
            objcmd.CommandText = "PR_LOC_Country_SelectAll";
          
            DataTable dt = new DataTable();
            SqlDataReader objSDR = objcmd.ExecuteReader();
            dt.Load(objSDR);
            return View("Index", dt);
        }
        #endregion

        #region ADD
        public IActionResult Add(int? CountryID)
        {
            if (CountryID != null)
            {
                string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
                SqlConnection conn = new SqlConnection(connectionstr);
                conn.Open();
                SqlCommand objcmd = conn.CreateCommand();
                objcmd.CommandType = System.Data.CommandType.StoredProcedure;
                objcmd.CommandText = "PR_LOC_Country_SelectByPK";
                objcmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = CountryID;
                DataTable dt = new DataTable();
                SqlDataReader objSDR = objcmd.ExecuteReader();
                dt.Load(objSDR);

                LOC_CountryModel modelLOC_Country = new LOC_CountryModel();
                foreach (DataRow dr in dt.Rows)
                {

                    modelLOC_Country.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelLOC_Country.CountryName = dr["CountryName"].ToString();
                    modelLOC_Country.CountryCode = dr["CountryCode"].ToString();
                    modelLOC_Country.PhotoPath = dr["PhotoPath"].ToString();
                    modelLOC_Country.CountryAddDate = Convert.ToDateTime(dr["CountryAddDate"]);
                    modelLOC_Country.CountryModifiedDate = Convert.ToDateTime(dr["CountryModifiedDate"]);

                }
                return View("LOC_CountryAddEdit", modelLOC_Country);
            }

            return View("LOC_CountryAddEdit");
        }
        #endregion
        #region Delete
        public IActionResult Delete(int CountryID)
        {
            try
            {
                string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
                SqlConnection conn = new SqlConnection(connectionstr);
                conn.Open();
                SqlCommand objcmd = conn.CreateCommand();
                objcmd.CommandType = System.Data.CommandType.StoredProcedure;
                objcmd.CommandText = "PR_LOC_Country_Delete";
                objcmd.Parameters.AddWithValue("@CountryID", CountryID);
                objcmd.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                return RedirectToAction("Index");
            }
          
        }

        #endregion
        
        #region Save
        public IActionResult Save(LOC_CountryModel modelLOC_Country)

        {
            if (modelLOC_Country.File != null)
            {
                string FilePath = "wwwroot\\Upload";
                string path = Path.Combine(Directory.GetCurrentDirectory(), FilePath);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fileNamewithPath = Path.Combine(path, modelLOC_Country.File.FileName);
                modelLOC_Country.PhotoPath = "~" + FilePath.Replace("wwwroot\\", "/") + "/" + modelLOC_Country.File.FileName;

                using (var stream = new FileStream(fileNamewithPath, FileMode.Create))
                {
                    modelLOC_Country.File.CopyTo(stream);
                }
            }

            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();
            SqlCommand objcmd = conn.CreateCommand();
            objcmd.CommandType = System.Data.CommandType.StoredProcedure;


            if (modelLOC_Country.CountryID == null)
            {
                objcmd.CommandText = "PR_LOC_Country_Insert";
                objcmd.Parameters.Add("@CountryAddDate", SqlDbType.DateTime).Value = DBNull.Value;


            }
            else
            {
                objcmd.CommandText = "PR_LOC_Country_Update";
                objcmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelLOC_Country.CountryID;

            }
            objcmd.Parameters.Add("@CountryName", SqlDbType.VarChar).Value = modelLOC_Country.CountryName;
            objcmd.Parameters.Add("@CountryCode", SqlDbType.VarChar).Value = modelLOC_Country.CountryCode;
            objcmd.Parameters.Add("@PhotoPath", SqlDbType.NVarChar).Value = modelLOC_Country.PhotoPath;

            objcmd.Parameters.Add("@CountryModifiedDate", SqlDbType.DateTime).Value = DBNull.Value;


            if (Convert.ToBoolean(objcmd.ExecuteNonQuery()))
            {
                if (modelLOC_Country.CountryID == null)
                {
                    TempData["CountryInsertMsg"] = "Record Inserted Successfully";
                }
                else
                {
                    TempData["CountryInsertMsg"] = "Record updated Successfully";
                }
            }

            conn.Close();
            return RedirectToAction("Index");
        }

        #endregion
    }

}
