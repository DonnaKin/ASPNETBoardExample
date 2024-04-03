using Project.Donna.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.Expressions;

namespace Project.Donna.Service
{
    public class Board  : Common.Common
    {
        private string strConn = string.Empty;
        public Board(string pConn)
        {
            strConn = pConn;
        }

        //게시목록 조회
        public List<Model.BoardMessage> List(int pPage, int pSize, string pSearchType, string pSearchText) {
            List<Model.BoardMessage> lists = null;
            List<SqlParameter> liParam = new List<SqlParameter> {
                new SqlParameter("@PageNum", pPage),
                new SqlParameter("@PageNo", pSize),
                new SqlParameter("@SearchType", pSearchType),
                new SqlParameter("@SearchText", pSearchText)
            };

            using (DataTable dt = DBHelper.ExecuteDataTable(strConn, "USP_BOARD_LIST", liParam))
            {
                if (dt.Rows.Count > 0) {
                    lists = new List<Model.BoardMessage>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        Model.BoardMessage boardList = new Model.BoardMessage() {
                            RowNum = Int32.Parse(dr["Rownum"].ToString()),
                            BoardID = dr["BOARDID"].ToString(),
                            Subject = dr["SUBJECT"].ToString(),
                            WriterName = dr["WRITERNAME"].ToString(),
                            BoardViews = Int32.Parse(dr["BOARDVIEWS"].ToString()),
                            ChkAgree = dr["CHKAGREE"].ToString(),
                            WriteDate = Convert.ToDateTime(dr["WRITEDATE"].ToString())
                        };

                        lists.Add(boardList);
                    }
                }

            }

            return lists;
        }

        //게시목록 갯수
        public int GetBoardCount(string pSearchType, string pSearchText) {
            int iResult = 0;
            List<SqlParameter> liParam = new List<SqlParameter>
            {
                new SqlParameter("@SearchType", pSearchType),
                new SqlParameter("@SearchText", pSearchText)
            };

            using (DataTable dt = DBHelper.ExecuteDataTable(strConn, "USP_BOARD_LIST_COUNT", liParam))
            {
                iResult = Int32.Parse(dt.Rows[0]["CNT"].ToString());
            };

            return iResult;
        }

        //게시물 등록
        public string Regist(Model.BoardMessage pBoard)
        {
            string strReturn = string.Empty;
            List<SqlParameter> liParam = new List<SqlParameter> {
                new SqlParameter("@SUBJECT", pBoard.Subject),
                new SqlParameter("@CONTENTS", pBoard.Contents),
                new SqlParameter("@WRITERNAME", pBoard.WriterName),
                new SqlParameter("@BOARDPW", pBoard.BoardPW),
                new SqlParameter("@CHKAGREE", pBoard.ChkAgree)
            };

            using (DataTable dt = DBHelper.ExecuteDataTable(strConn, "USP_BOARD_MESSAGE_REGIST", liParam))
            {
                if (dt.Rows.Count > 0)
                {
                    strReturn = dt.Rows[0][0].ToString();
                }

            }

            return strReturn;
        }

        //게시물 수정
        public string Update(Model.BoardMessage pBoard)
        {
            string strReturn = string.Empty;
            List<SqlParameter> liParam = new List<SqlParameter> {
                new SqlParameter("@BOARDID", pBoard.BoardID),
                new SqlParameter("@SUBJECT", pBoard.Subject),
                new SqlParameter("@CONTENTS", pBoard.Contents),
                new SqlParameter("@WRITERNAME", pBoard.WriterName),
                new SqlParameter("@BOARDPW", pBoard.BoardPW),
                new SqlParameter("@CHKAGREE", pBoard.ChkAgree)
            };

            using (DataTable dt = DBHelper.ExecuteDataTable(strConn, "USP_BOARD_MESSAGE_MODIFY", liParam))
            {
                if (dt.Rows.Count > 0)
                {
                    strReturn = dt.Rows[0][0].ToString();
                }

            }

            return strReturn;
        }

        //게시물 첨부파일 등록
        public int FileRegist(Model.BoardAttach pAttach)
        {
            int iReturn = 0;
            List<SqlParameter> liParam = new List<SqlParameter> {
                new SqlParameter("@BOARDID", pAttach.BoardID),
                new SqlParameter("@FILENAME", pAttach.FileName),
                new SqlParameter("@FILESAVEDNAME", pAttach.FileSavedName),
                new SqlParameter("@FILEEXTENSION", pAttach.FileExtension),
                new SqlParameter("@FILEPATH", pAttach.FilePath)
                
            };

            iReturn = DBHelper.ExecuteNonQuery(strConn, "USP_BOARD_FILE_REGIST", liParam);

            return iReturn;
        }

        //게시물 첨부파일 삭제
        public int FileDelete(Model.BoardAttach pAttach)
        {
            int iReturn = 0;
            List<SqlParameter> liParam = new List<SqlParameter> {
                new SqlParameter("@BOARDID", pAttach.BoardID),
                new SqlParameter("@FILEID", pAttach.FileID)
            };

            iReturn = DBHelper.ExecuteNonQuery(strConn, "USP_BOARD_FILE_DELETE", liParam);

            return iReturn;
        }

        //게시물 상세 조회
        public Model.BoardMessage Detail(Model.BoardMessage board)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@BOARDID", board.BoardID)
            };

            //게시물 조회
            using (DataTable dt = DBHelper.ExecuteDataTable(strConn, "USP_BOARD_MESSAGE_INFO", parameters))
            {
                if (dt.Rows.Count == 1)
                {
                    DataRow dr = dt.Rows[0];
                    return board = new Model.BoardMessage()
                    {
                        BoardID = dr["BOARDID"].ToString().ToUpper(),
                        Subject = dr["SUBJECT"].ToString(),
                        Contents = dr["CONTENTS"].ToString(),
                        WriterName = dr["WRITERNAME"].ToString(),
                        BoardPW = dr["BOARDPW"].ToString(),
                        ChkAgree = dr["CHKAGREE"].ToString(),
                        BoardViews = Int32.Parse(dr["BOARDVIEWS"].ToString()),
                        WriteDate = Convert.ToDateTime(dr["WRITEDATE"].ToString()),
                        IsDelete = dr["ISDELETE"].ToString()
                    };
                }
            }
            return null;
        }

        //게시물 첨부파일 목록 조회 - List
        public List<Model.BoardAttach> FileInfo(Model.BoardAttach attach)
        {
            List<Model.BoardAttach> lists = null;
            List<SqlParameter> liParam = new List<SqlParameter> {
                new SqlParameter("@BOARDID", attach.BoardID)
            };

            using (DataTable dt = DBHelper.ExecuteDataTable(strConn, "USP_BOARD_FILE_INFO", liParam))
            {
                if (dt.Rows.Count > 0)
                {
                    lists = new List<Model.BoardAttach>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        Model.BoardAttach fileList = new Model.BoardAttach()
                        {
                            FileID = dr["FILEID"].ToString(),
                            BoardID = dr["BOARDID"].ToString(),
                            FileName = dr["FILENAME"].ToString(),
                            FileSavedName = dr["FILESAVEDNAME"].ToString(),
                            FileExtension = dr["FILEEXTENSION"].ToString(),
                            FilePath = dr["FILEPATH"].ToString(),
                            CreateDate = Convert.ToDateTime(dr["CREATEDATE"].ToString())
                        };

                        lists.Add(fileList);
                    }
                }

            }

            return lists;
        }

        //게시물 첨부파일 목록 조회 - DataTable
        public DataTable dtFileInfo(Model.BoardAttach attach)
        {
            List<SqlParameter> liParam = new List<SqlParameter> {
                new SqlParameter("@BOARDID", attach.BoardID)
            };

            DataTable dt = DBHelper.ExecuteDataTable(strConn, "USP_BOARD_FILE_INFO", liParam);

            return dt;
        }

        //게시물 조회수 증가
        public int UpperMessage(Model.BoardMessage pBoard)
        {
            int iReturn = 0;
            List<SqlParameter> liParam = new List<SqlParameter> {
                new SqlParameter("@BOARDID", pBoard.BoardID)
            };

            iReturn = DBHelper.ExecuteNonQuery(strConn, "USP_BOARD_VIEWCOUNT_MODIFY", liParam);

            return iReturn;
        }

        //게시물 삭제
        public int DeleteMessage(Model.BoardMessage pBoard)
        {
            int iReturn = 0;
            List<SqlParameter> liParam = new List<SqlParameter> {
                new SqlParameter("@BOARDID", pBoard.BoardID)
            };

            iReturn = DBHelper.ExecuteNonQuery(strConn, "USP_BOARD_MESSAGE_DELETE", liParam);

            return iReturn;
        }

        //게시물 댓글 조회 - List
        public List<Model.BoardComment> CommentList(Model.BoardComment comment)
        {
            List<Model.BoardComment> lists = null;
            List<SqlParameter> liParam = new List<SqlParameter> {
                new SqlParameter("@BOARDID", comment.BoardID)
            };

            using (DataTable dt = DBHelper.ExecuteDataTable(strConn, "USP_BOARD_COMMENT_INFO", liParam))
            {
                if (dt.Rows.Count > 0)
                {
                    lists = new List<Model.BoardComment>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        Model.BoardComment commList = new Model.BoardComment()
                        {
                            CommentID = dr["COMMENTID"].ToString(),
                            ParentID = dr["PARENTID"].ToString(),
                            Contents = dr["CONTENTS"].ToString(),
                            WriterName = dr["WRITERNAME"].ToString(),
                            CommentPW = dr["COMMENTPW"].ToString(),
                            WriteDate = dr["WRITEDATE"].ToString(),
                            DelFlag = dr["DELFLAG"].ToString(),
                            Sort = dr["SORT"].ToString(),
                            Depth = dr["DEPTH"].ToString()
                        };

                        lists.Add(commList);
                    }
                }

            }

            return lists;
        }

        // 게시물 댓글 조회 - DataTable
        public DataTable dtCommentList(Model.BoardComment comment)
        {
            List<SqlParameter> liParam = new List<SqlParameter> {
                new SqlParameter("@BOARDID", comment.BoardID)
            };

            DataTable dt = DBHelper.ExecuteDataTable(strConn, "USP_BOARD_COMMENT_INFO", liParam);

            return dt;
        }

        // 게시물 하위 댓글 조회
        public List<Model.BoardComment> CommentListDetail(Model.BoardComment comment)
        {
            List<Model.BoardComment> lists = null;
            List<SqlParameter> liParam = new List<SqlParameter> {
                new SqlParameter("@BOARDID", comment.BoardID),
                new SqlParameter("@COMMENTID", comment.CommentID)
            };

            using (DataTable dt = DBHelper.ExecuteDataTable(strConn, "USP_BOARD_COMMENT_DETAIL", liParam))
            {
                if (dt.Rows.Count > 0)
                {
                    lists = new List<Model.BoardComment>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        Model.BoardComment commList = new Model.BoardComment()
                        {
                            CommentID = dr["COMMENTID"].ToString(),
                            ParentID = dr["PARENTID"].ToString(),
                            Contents = dr["CONTENTS"].ToString(),
                            WriterName = dr["WRITERNAME"].ToString(),
                            WriteDate = dr["WRITEDATE"].ToString(),
                            DelFlag = dr["DELFLAG"].ToString()
                        };

                        lists.Add(commList);
                    }
                }

            }

            return lists;
        }

        // 게시물 댓글 개수 조회
        public int CommentCount(Model.BoardComment comment)
        {
            int iReturn = 0;
            List<SqlParameter> liParam = new List<SqlParameter> {
                new SqlParameter("@BOARDID", comment.BoardID)

            };
            using (DataTable dt = DBHelper.ExecuteDataTable(strConn, "USP_BOARD_COMMENT_COUNT", liParam))
            {
                iReturn = Int32.Parse(dt.Rows[0][0].ToString());
            }

            return iReturn;
        }

        // 댓글 등록
        public int Create(Model.BoardComment comment)
        {
            int iReturn = 0;
            List<SqlParameter> liParam = new List<SqlParameter> {
                new SqlParameter("@BOARDID", comment.BoardID),
                new SqlParameter("@WRITERNAME", comment.WriterName),
                new SqlParameter("@CONTENTS", comment.Contents),
                new SqlParameter("@PARENTID", comment.ParentID),
                new SqlParameter("@COMMENTPW", comment.CommentPW)
                
            };

            iReturn = DBHelper.ExecuteNonQuery(strConn, "USP_BOARD_COMMENT_REGIST", liParam);

            return iReturn;
        }

        // 댓글 삭제
        public int Delete(Model.BoardComment comment)
        {
            int iReturn = 0;
            List<SqlParameter> liParam = new List<SqlParameter> {
                new SqlParameter("@BOARDID", comment.BoardID),
                new SqlParameter("@COMMENTID", comment.CommentID)

            };

            iReturn = DBHelper.ExecuteNonQuery(strConn, "USP_BOARD_COMMENT_DELETE", liParam);

            return iReturn;
        }

        // 비밀번호 검증
        public bool CommPasswordCheck(Model.BoardComment comment, string pMode)
        {
            bool bReturn = false;

            List<SqlParameter> liParam = new List<SqlParameter> {
                new SqlParameter("@MODE", pMode),
                new SqlParameter("@BOARDID", comment.BoardID),
                new SqlParameter("@COMMENTID", comment.CommentID),
                new SqlParameter("@COMMENTPW",  EncryptPassword(comment.CommentPW))
            };

            using (DataTable dt = DBHelper.ExecuteDataTable(strConn, "USP_BOARD_PASSWORD_CHECK", liParam))
            {
                if (dt.Rows.Count > 0)
                {
                     bReturn = bool.Parse(dt.Rows[0][0].ToString());
                }
            }
            return bReturn;
        }

    }
}