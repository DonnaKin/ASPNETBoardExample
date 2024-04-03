using System;
using Project.Donna;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Project.Donna.Business;
using System.Configuration;
using System.Text;
using Project.Donna.Common;

namespace Project.Donna.Board
{
    public partial class List : Common.Common
    {
        public int iTotal = 0;
        public int iPageNum = 1;
        public int iPageSize = 6;
        public string strSearchType = string.Empty;
        public string strSearchText = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //검색 Get으로 구현
                //if (!Check.IsNone(Request["PageNum"]))
                //    iPageNum = Int32.Parse(Request["PageNum"].ToString());
                iPageNum = Check.IsNone(Request["PageNum"], 1);

                if (!Check.IsNone(Request["SearchType"]))
                    strSearchType = Request["SearchType"].ToString();

                if (!Check.IsNone(Request["SearchText"]))
                    strSearchText = Request["SearchText"].ToString();

                //목록 불러오기
                GridData();
            }
        }

        protected void GridData()
        {
            try
            {
                using (Business.Board biz = new Business.Board(ConfigurationManager.ConnectionStrings["Board"].ConnectionString))
                {
                    //게시물 목록 가져오기
                    List<Model.BoardMessage> liBoardList = biz.List(iPageNum, iPageSize, strSearchType, strSearchText);

                    //게시물 총 갯수 가져오기
                    iTotal = biz.Count(strSearchType, strSearchText);
                    lblTotal.Text = iTotal.ToString();


                    if (liBoardList != null && liBoardList.Count > 0)
                    {
                        repList.DataSource = liBoardList;
                        repList.DataBind();
                        divEmpty.Visible = false;
                    }
                    else
                    {
                        repList.Visible = false;
                        divEmpty.Visible = true;
                    }

                    //페이징
                    StringBuilder sbPage = new StringBuilder();
                    //int iPage = iTotal / iPageSize;
                    int iPage = ((iPageSize - 1) + iTotal) / iPageSize;
                    if (iPageNum > 1)
                    {
                        sbPage.Append("<a href=\"javascript:void(0);\" class=\"btn btn_prev\" onclick=\"fnMovePage(1);\">이전</a>");
                    }
                    sbPage.Append("<div class=\"pager_number\">");
                    for (int i = 0; i < iPage; i++)
                    {
                        if (iPageNum == i + 1)
                        {
                            sbPage.AppendFormat("<a href=\"javascript:void(0);\" onclick=\"fnMovePage({0});\" class=\"active\">{0}</a>", i + 1);
                        }
                        else
                        {
                            sbPage.AppendFormat("<a href=\"javascript:void(0);\" onclick=\"fnMovePage({0});\">{0}</a>", i + 1);
                        }

                    }
                    sbPage.Append("</div>");
                    if ((iPageNum != 1 && iPageNum < iPage) || (iPageNum == 1 && iPage > 1))
                    {
                        sbPage.AppendFormat("<a href=\"javascript:void(0);\" class=\"btn btn_next\" onclick=\"fnMovePage({0});\">다음</a>", iPage);
                    }

                    divPaging.InnerHtml = sbPage.ToString();

                }
            }
            catch(Exception ex)
            {
                Common.Error.WebHandler(ex);
            }
        }
    }
}