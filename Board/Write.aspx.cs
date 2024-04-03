using Project.Donna;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.EnterpriseServices;
using static System.Net.WebRequestMethods;
using System.Text;
using Project.Donna.Common;

namespace Project.Donna.Board
{
    public partial class Write : Common.Common
    {
        public string strMode = string.Empty; // M : 수정
        public string strBoardID = string.Empty; // 게시글 수정일 경우 BoardID 저장
        public string strBoardKey = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            strBoardID = Request.QueryString["BoardID"];
            strBoardKey = Request.QueryString["Key"];
            if (String.IsNullOrEmpty(strBoardID) && String.IsNullOrEmpty(strBoardKey))
            {
                strMode = "C";
            }
            else
            {
                //파라미터 검증
                if (!BoardKeyCheck(strBoardKey) || !IsGuid(strBoardID))
                {
                    Util.Move("잘못된 접근입니다.", "/Board/List");
                }
                else
                {
                    strMode = "M";
                }
            }

            if (!IsPostBack)
            {
                if(strMode == "M")
                {
                    // 수정시 게시물 정보 불러오기
                    BindData();
                }
            }
        }

        protected void BindData()
        {
            try
            {
                using (Business.Board biz = new Business.Board(ConfigurationManager.ConnectionStrings["Board"].ConnectionString))
                {
                    Model.BoardMessage board = biz.Detail(new Model.BoardMessage { BoardID = strBoardID });

                    if (board != null)
                    {
                        txtSubject.Text = board.Subject;
                        divContent.InnerHtml = ReplaceString(board.Contents);
                        txtWriterName.Text = board.WriterName;

                        if (board.ChkAgree == "Y")
                        {
                            termsAgree.Attributes.Add("checked", "true");
                            hidChkAgree.Value = "Y";
                        }

                        //첨부파일 조회
                        using (Business.Board bizFile = new Business.Board(ConfigurationManager.ConnectionStrings["Board"].ConnectionString))
                        {
                            //파일 목록 가져오기
                            List<Model.BoardAttach> liAttachList = bizFile.FileInfo(new Model.BoardAttach { BoardID = strBoardID });

                            if (liAttachList != null && liAttachList.Count > 0)
                            {
                                rptFile.DataSource = liAttachList;
                                rptFile.DataBind();
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Common.Error.WebHandler(ex);
            }
        }

        //게시물 작성
        protected void btnRegist_Click(object sender, EventArgs e)
        {
            string strPassword = string.Empty;

            //비밀번호 암호화
            strPassword = EncryptPassword(txtPassword.Text);
            
            Model.BoardMessage board = new Model.BoardMessage
            {
                BoardID = strBoardID,
                Subject = txtSubject.Text,
                WriterName = txtWriterName.Text,
                BoardPW = strPassword,
                Contents = hidContent.Value,
                ChkAgree = hidChkAgree.Value
            };

            try
            {
                using (Business.Board biz = new Business.Board(ConfigurationManager.ConnectionStrings["Board"].ConnectionString))
                {
                    string strReturn = string.Empty;
                    bool bReturn = false;

                    if (strMode == "C")
                    {
                        strReturn = biz.Regist(board);
                    }
                    else
                    {
                        strReturn = biz.Update(board);

                        //기존 파일 중 삭제 파일이 있을 경우
                        if (hidDelFileInfo.Value != "")
                        {
                            BoardFileDelete(hidDelFileInfo.Value, strReturn);
                        }
                    }

                    // 파일 업로드
                    //if (iUploadFile.PostedFiles.Count > 0)
                    if (iUploadFile.HasFile && iUploadFile.PostedFile.ContentLength > 0)
                    {
                        bReturn = BoardFileUpload(iUploadFile, strReturn);
                    }
                    else
                    {
                        bReturn = true;
                    }

                    if (bReturn && strReturn != "")
                    {
                        Response.Redirect("/Board/View?BoardID=" + strReturn);
                    }

                }
            } catch(Exception ex)
            {
                Common.Error.WebHandler(ex);
            }
        }

        //파일 업로드
        protected bool BoardFileUpload(FileUpload pFile, string pBoardID)
        {
            bool bReturn = false;

            string strUploadPath = Server.MapPath("/uploads/" + pBoardID + "/");

            try
            {
                foreach (HttpPostedFile uploadedFile in pFile.PostedFiles)
                {
                    string strFileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
                    string strFileExtension = Path.GetExtension(uploadedFile.FileName);
                    string strSaveFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + strFileExtension;

                    if (!Directory.Exists(strUploadPath))
                    {
                        Directory.CreateDirectory(strUploadPath);
                    }

                    string strFilePath = Path.Combine(strUploadPath, strSaveFileName);

                    uploadedFile.SaveAs(strFilePath);

                    
                    //첨부파일 정보 DB 저장
                    Model.BoardAttach attach = new Model.BoardAttach
                    {
                        BoardID = pBoardID,
                        FileName = strFileName,
                        FileSavedName = strSaveFileName,
                        FileExtension = strFileExtension,
                        FilePath = strUploadPath+strSaveFileName
                    };

                    using (Business.Board biz = new Business.Board(ConfigurationManager.ConnectionStrings["Board"].ConnectionString))
                    {
                        biz.FileRegist(attach);
                    }
                }

                bReturn = true;

            } catch(Exception e)
            {
                bReturn = false;
            }
            

            return bReturn;
        }

        //등록된 파일 삭제 (실제 데이터만 삭제)
        protected bool BoardFileDelete(string pFileInfo, string pBoardID)
        {
            bool bReturn = false;

            string[] strFileInfo = pFileInfo.Split(new char[] { '|' });

            try
            {
                for(int i =0; i < strFileInfo.Length - 1; i++)
                {
                    string strFileID = strFileInfo[i].Split(new char[] { ',' })[0];
                    string strFileSavedName = strFileInfo[i].Split(new char[] { ',' })[1];
                    string strUploadPath = Server.MapPath("/uploads/" + pBoardID + "/");

                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(strUploadPath + strFileSavedName);
                    try
                    {
                        fileInfo.Delete();
                        bReturn = true;
                    }
                    catch (Exception e)
                    {
                        bReturn = false;
                    }

                    //첨부파일 정보 DB 업데이트
                    Model.BoardAttach attach = new Model.BoardAttach
                    {
                        BoardID = pBoardID,
                        FileID = strFileID
                    };

                    using (Business.Board biz = new Business.Board(ConfigurationManager.ConnectionStrings["Board"].ConnectionString))
                    {
                        biz.FileDelete(attach);
                    }
                }

                bReturn = true;

            }
            catch (Exception e)
            {
                bReturn = false;
            }

            return bReturn;
        }

        private bool BoardKeyCheck(string pBoardKey)
        {
            bool bReturn = false;
            string strKey = ConfigurationManager.AppSettings["brdKey"];
            string strPlain = string.Empty;
            try
            {
                
                strPlain = DecryptString(pBoardKey, strKey);
                string strDecBoardID = strPlain.Split(',')[0];
                string strDecKey = strPlain.Split(',')[1];

                if (strDecKey == strKey && strDecBoardID == strBoardID)
                {
                    bReturn = true;
                }
                else
                {
                    bReturn= false;
                }
            }
            catch(Exception e)
            {
                bReturn = false;
            }
            

            return bReturn;
        }
    }
}