using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.WebRequestMethods;

namespace Project.Donna.Test
{
    public partial class FileControl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strUploadPath = Server.MapPath("/uploads/filetest/20240319/");

            if (Request.Files.Count > 0)
            {
                var uploadedFiles = 0;

                for(int i =0; i< Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    if(file != null && file.ContentLength > 0)
                    {
                        try
                        {
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            var filePath = strUploadPath + fileName;

                            if (!Directory.Exists(strUploadPath))
                            {
                                Directory.CreateDirectory(strUploadPath);
                            }

                            file.SaveAs(filePath);
                            uploadedFiles++;
                        }
                        catch(Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                lblResult.Text = $"{uploadedFiles} 개의 파일이 업로드 되었습니다.";
            }
        }

        protected void btnFileUpload_Click(object sender, EventArgs e)
        {
            string strUploadPath = Server.MapPath("/uploads/filetest/20240319/");

            if(IsPostBack)
            {
                var uploadedFiles = new List<string>();

                foreach (string fileKey in Request.Files)
                {
                    var file = Request.Files[fileKey];
                    if (file.ContentLength > 0)
                    {
                        try
                        {
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            var filePath = strUploadPath + fileName;

                            file.SaveAs(filePath);
                            uploadedFiles.Add(fileName);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }

                lblResult.Text = $"{uploadedFiles.Count} 개의 파일이 업로드 되었습니다.";
            }

            #region {사용 안함}
            /*
            if (iUploadFile.HasFile)
            {
                foreach (HttpPostedFile uploadedFile in iUploadFile.PostedFiles)
                {
                    string strFileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
                    string strFileExtension = Path.GetExtension(uploadedFile.FileName);
                    string strSaveFileName = strFileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + strFileExtension;

                    if (!Directory.Exists(strUploadPath))
                    {
                        Directory.CreateDirectory(strUploadPath);
                    }

                    string strFilePath = Path.Combine(strUploadPath, strSaveFileName);

                    uploadedFile.SaveAs(strFilePath);
                }
            }*/
            #endregion
        }
    }
}