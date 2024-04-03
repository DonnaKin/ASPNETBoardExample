using Microsoft.Ajax.Utilities;
using Project.Donna.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Project.Donna.Board
{
    public partial class View : Common.Common
    {
        public string strBoardID = string.Empty;
        DataTable dt = null;
        protected string strBoardPW = string.Empty;
        public string strReferrer = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            strBoardID = Request.QueryString["BoardID"];
            if (String.IsNullOrEmpty(strBoardID) && !IsGuid(strBoardID)) //게시목록에서 넘어온 BoardID가 없을 경우 목록으로 이동
            {
                Util.Move("게시물 정보를 조회할 수 없습니다.", "/Board/List");
            }

            //Referrer 체크
            if(Request.UrlReferrer != null && Request.UrlReferrer.ToString().IndexOf("/Board/List?") > 0)
            {
                strReferrer = Request.UrlReferrer.ToString();
            }
            else
            {
                strReferrer = "/Board/List";
            }
            
            if (!IsPostBack)
            {
                //데이터 바인드
                GridData();
                //댓글 조회
                GridCommentData(); //StringBuiler 사용
                //GridCommentData_Repeater(); //Repeater + ajax 사용

                //조회 수 증가
                SetMessageView();
            }
        }

        #region {게시물 관련}
        // 게시물 정보 불러오기
        protected void GridData()
        {
            try
            {
                using (Business.Board biz = new Business.Board(ConfigurationManager.ConnectionStrings["Board"].ConnectionString))
                {
                    Model.BoardMessage board = biz.Detail(new Model.BoardMessage { BoardID = strBoardID });

                    if (board != null)
                    {
                        if(board.IsDelete == "N")
                        {
                            lblSubject.Text = board.Subject;
                            divContents.InnerHtml = ReplaceString(board.Contents);
                            lblWriterName.Text = board.WriterName;
                            lblWriteDate.Text = board.WriteDate.ToString("yyyy-MM-dd");
                            hidBoardPW.Value = board.BoardPW;

                            //파일 목록 가져오기
                            List<Model.BoardAttach> liAttachList = biz.FileInfo(new Model.BoardAttach { BoardID = strBoardID });

                            if (liAttachList != null && liAttachList.Count > 0)
                            {
                                repAttachList.DataSource = liAttachList;
                                repAttachList.DataBind();
                            }
                        }
                        else //삭제된 게시물일 경우
                        {
                            Util.Move("삭제된 게시물 입니다.", "/Board/List");
                        }
                        
                    }
                    else
                    {
                        Util.Move("게시물 정보를 조회할 수 없습니다.", "/Board/List");
                    }
                }
            }
            catch(Exception ex)
            {
                Common.Error.WebHandler(ex);
            }

        }

        //게시물 수정
        protected void btnModifyMessage_Click(object sender, EventArgs e)
        {
            try
            {
                //비밀번호 검증
                if(hidBoardPW.Value.Equals(EncryptPassword(txtPassword_M.Text)))
                {
                    string strKey = ConfigurationManager.AppSettings["brdKey"];
                    //파라미터 암호화
                    string strEncrypt = EncryptString(strBoardID+","+ strKey, strKey);

                    //게시물 수정 이동
                    Util.Move("", "/Board/Write?BoardID=" + strBoardID + "&Key="+ strEncrypt);
                }
                else
                {
                    //비밀번호 검증 실패
                    Util.Back("비밀번호를 다시 한 번 확인해주세요.");
                }

            }
            catch (Exception ex)
            {
                Common.Error.WebHandler(ex);
            }
        }

        //게시물 삭제
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (hidBoardPW.Value.Equals(EncryptPassword(txtPassword_D.Text))) // 비밀번호 비교
                {
                    using (Business.Board biz = new Business.Board(ConfigurationManager.ConnectionStrings["Board"].ConnectionString))
                    {
                        int iReturn = 0;
                        iReturn = biz.DeleteMessage(new Model.BoardMessage { BoardID = strBoardID });

                        if (iReturn > 0)
                        {
                            Util.Move("게시물이 삭제되었습니다.", "/Board/List");
                        }

                    }
                }
                else
                {
                    //비밀번호 검증 실패
                    Util.Back("비밀번호를 다시 한 번 확인해주세요.");
                }
            }
            catch(Exception ex)
            {
                Common.Error.WebHandler(ex);
            }
        }

        // 조회수 증가
        protected void SetMessageView()
        {
            try
            {
                using (Business.Board biz = new Business.Board(ConfigurationManager.ConnectionStrings["Board"].ConnectionString))
                {
                    biz.UpperMessage(new Model.BoardMessage { BoardID = strBoardID });
                }
            }
            catch(Exception ex)
            {
                Common.Error.WebHandler(ex);
            }
        }
        #endregion

        #region {댓글 가져오기 - Repeater}
        protected void GridCommentData_Repeater()
        {
            try
            {
                using (Business.Board biz = new Business.Board(ConfigurationManager.ConnectionStrings["Board"].ConnectionString))
                {
                    StringBuilder sb = new StringBuilder();
                    // repeater 사용
                    //댓글 목록 가져오기
                    //List<Model.BoardComment> liCommentList = biz.CommentList(new Model.BoardComment { BoardID = strBoardID }); // 전체 댓글 조회
                    List<Model.BoardComment> liCommentList = biz.CommentListDetail(new Model.BoardComment() // 최상위 댓글만 조회
                    {
                        BoardID = strBoardID,
                        CommentID = "00000000-0000-0000-0000-000000000000"
                    });

                    if (liCommentList != null && liCommentList.Count > 0)
                    {
                        lblTotal.Text = biz.CommentCount(new Model.BoardComment { BoardID = strBoardID }).ToString(); //댓글 건수 조회

                        //repCommentList.DataSource = liCommentList.Where(w => w.ParentID == "00000000-0000-0000-0000-000000000000").ToList();
                        repCommentList.DataSource = liCommentList;
                        repCommentList.DataBind();
                        divEmptyComment.Visible = false;


                    }
                    else
                    {
                        lblTotal.Text = "0";
                        repCommentList.Visible = false;
                        divEmptyComment.Visible = true;
                    }
                }
            }
            catch(Exception ex)
            {
                Common.Error.WebHandler(ex);
            }
        }

        #endregion

        #region {댓글 관련}
        protected void GridCommentData()
        {
            try
            {
                using (Business.Board biz = new Business.Board(ConfigurationManager.ConnectionStrings["Board"].ConnectionString))
                {
                    StringBuilder sb = new StringBuilder();

                    //datatable 파싱 후 html 생성
                    dt = biz.dtCommentList(new Model.BoardComment { BoardID = strBoardID });
                    lblTotal.Text = dt.Rows.Count.ToString();

                    if (dt.Rows.Count > 0)
                    {
                        divEmptyComment.Visible = false;

                        sb.Append("<ul>");
                        foreach (DataRow row in dt.Rows)
                        {

                            string strNew = "";
                            DateTime dtTime = Convert.ToDateTime(row["WriteDate"].ToString());

                            //최근 게시물일 경우 (하루 전)
                            if (DateTime.Now.AddDays(-1) < dtTime)
                            {
                                strNew = "new";
                            }

                            if (row["Depth"].ToString() == "1")
                            {
                                sb.AppendFormat("<li><div class=\"comment_box {0}\">", strNew);
                                sb.AppendFormat("<div class=\"comment_info\"><span class=\"name\">{0}</span><span class=\"date\">{1}</span>", row["WriterName"].ToString(), row["WriteDate"].ToString());

                                if (row["DelFlag"].ToString() == "N")
                                    sb.AppendFormat("<span class=\"reply\"><a href=\"javascript:void(0);\" onclick=\"fnCreateReply(this)\" style=\"cursor:pointer;\" id=\"aCommID_{0}\">덧글</a></span>", row["CommentID"].ToString());

                                sb.AppendFormat("</div><div class=\"comment_text\"><p>{0}</p></div>", row["Contents"].ToString());

                                if (row["DelFlag"].ToString() == "N") // 이미 삭제된 댓글일 경우 삭제 버튼이 보일 필요 없음.
                                    sb.AppendFormat("<button type=\"button\" class=\"btn_comment_delete popup_open\" id=\"btnCommID_{0}\" onclick=\"fnCommentDelete(this)\">삭제</button>", row["CommentID"].ToString());

                                sb.AppendFormat("<input type=\"hidden\" id=\"hidCommentID_{0}\" value=\"{1}\">", row["CommentID"].ToString(), row["CommentPW"].ToString());
                                sb.Append("</div>");
                                sb.Append(DisplayChildComments(dt, row["CommentID"].ToString()));
                                sb.Append("</li>");
                            }
                        }
                        sb.Append("</ul>");
                        divCommentList.InnerHtml = sb.ToString();
                    }
                    else
                    {
                        divEmptyComment.Visible = true;
                    }


                }
            }
            catch(Exception ex)
            {
                Common.Error.WebHandler(ex);
            }
        }

        #region {덧글 표시}
        protected string DisplayChildComments(DataTable pCommDT, string pParentID)
        {
            StringBuilder sb = new StringBuilder();
            DataRow[] dr = pCommDT.Select("ParentId = '" + pParentID + "'");

            if (dr.Length > 0)
            {
                sb.Append("<ul class=\"reply\">");
                foreach (DataRow row in dr)
                {
                    string strNew = "";
                    DateTime dtTime = Convert.ToDateTime(row["WriteDate"].ToString());

                    //최근 게시물일 경우 (하루 전)
                    if (DateTime.Now.AddDays(-1) < dtTime)
                    {
                        strNew = "new";
                    }

                    sb.AppendFormat("<li><div class=\"comment_box {0}\">", strNew);
                    sb.AppendFormat("<div class=\"comment_info\"><span class=\"name\">{0}</span><span class=\"date\">{1}</span>", row["WriterName"].ToString(), row["WriteDate"].ToString());
                    
                    if (row["DelFlag"].ToString() == "N")
                        sb.AppendFormat("<span class=\"reply\"><a href=\"javascript:void(0);\" onclick=\"fnCreateReply(this)\" style=\"cursor:pointer;\" id=\"aCommID_{0}\">덧글</a></span>", row["CommentID"].ToString());
                    
                    sb.AppendFormat("</div><div class=\"comment_text\"><p>{0}</p></div>", row["Contents"].ToString());
                    
                    if (row["DelFlag"].ToString() == "N")
                        sb.AppendFormat("<button type=\"button\" class=\"btn_comment_delete popup_open\" id=\"btnCommID_{0}\" onclick=\"fnCommentDelete(this)\">삭제</button>", row["CommentID"].ToString());
                    
                    sb.AppendFormat("<input type=\"hidden\" id=\"hidCommentID_{0}\" value=\"{1}\">", row["CommentID"].ToString(), row["CommentPW"].ToString());
                    sb.Append("</div>");

                    sb.Append(DisplayChildComments(dt, row["CommentId"].ToString()));

                    sb.Append("</li>");
                }
                sb.Append("</ul>");
            }

            return sb.ToString();
        }
        #endregion

        //댓글 등록
        protected void btnCreateComment_Click(object sender, EventArgs e)
        {
            try
            {
                //댓글 등록 소스 구현
                using (Business.Board biz = new Business.Board(ConfigurationManager.ConnectionStrings["Board"].ConnectionString))
                {
                    int iResult = biz.Create(new Model.BoardComment()
                    {
                        BoardID = strBoardID,
                        WriterName = txtCommName.Text,
                        Contents = commentWrite.Value,
                        ParentID = hidCommentID.Value == "" ? null : hidCommentID.Value,
                        CommentPW = EncryptPassword(txtCommPassword.Text)
                    }) ;

                    if (iResult > 0)
                    {
                        //댓글 등록 성공
                        Util.Move("등록이 완료되었습니다.", "/Board/View?BoardID=" + strBoardID);
                    }
                    else
                    {
                        //댓글 등록 실패
                        Util.Back("비밀번호를 다시 한 번 확인해주세요.");
                    }
                }

            }
            catch (Exception ex)
            {
                Common.Error.WebHandler(ex);
            }
        }

        //댓글삭제
        protected void btnDeleteComment_Click(object sender, EventArgs e)
        {
            try
            {
                string strCommentPW = txtPassword_CD.Text;
                using (Business.Board biz = new Business.Board(ConfigurationManager.ConnectionStrings["Board"].ConnectionString))
                {
                    //댓글 비밀번호 비교
                    bool bReturn = biz.CommPasswordCheck(new Model.BoardComment()
                    {
                        BoardID = strBoardID,
                        CommentID = hidCommentID.Value,
                        CommentPW = txtPassword_CD.Text
                    }, "CD");

                    if (bReturn)
                    {
                        //댓글 삭제
                        int i = biz.Delete(new Model.BoardComment
                        {
                            BoardID = strBoardID,
                            CommentID = hidCommentID.Value
                        });

                        if(i > 0)
                        {
                            Util.Move("삭제가 완료되었습니다.", "/Board/View?BoardID=" + strBoardID);
                        }
                        else
                        {
                            Util.Back("삭제 중 오류가 발생하였습니다.");
                        }

                    }
                    else
                    {
                        //비밀번호 검증 실패
                        Util.Back("비밀번호를 다시 한 번 확인해주세요.");
                    }

                }
            }
            catch (Exception ex)
            {
                Common.Error.WebHandler(ex);
            }
        }

        #endregion
    }
}