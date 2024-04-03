using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Xml;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Project.Donna;

namespace Project.Donna.Controllers
{
    public class BoardController : ApiController
    {
        //게시물 검색 - 사용안함
        [HttpGet]
        [Route("BoardSearch/{SearchType}/{SearchKeyword}")]
        public List<Model.BoardMessage> Search(string pSearchType, string pSearchText = "")
        {

            List<Model.BoardMessage> lists = new List<Model.BoardMessage>();

            try
            {
                using (Business.Board biz = new Business.Board(ConfigurationManager.ConnectionStrings["Board"].ConnectionString))
                {
                    lists = biz.List(1, 6, pSearchType, pSearchText);
                }
            }
            catch(Exception ex)
            {
                Common.Error.WebHandler(ex);
            }

            return lists;
        }

        //댓글 작성
        [HttpPost]
        [Route("Api/BoardComment/Create")]
        public HttpResponseMessage CommCreate(CommentRequest commRequest)
        {

            HttpResponseMessage resp = null;

            try
            {

                using(Business.Board biz = new Business.Board(ConfigurationManager.ConnectionStrings["Board"].ConnectionString))
                {
                    CommentResponse json = new CommentResponse();
                    int iResult = biz.Create(new Model.BoardComment()
                    {
                        BoardID = commRequest.BoardID,
                        WriterName = commRequest.WriterName,
                        Contents = commRequest.Contents,
                        ParentID = commRequest.ParentID,
                        CommentPW = commRequest.CommentPW
                    });

                    if (iResult > 0)
                    {
                        json.Result = "SUCCESS";
                    }
                    else
                    {
                        json.Result = "FAIL";
                    }

                    JsonSerializerSettings jss = new JsonSerializerSettings();
                    jss.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    jss.NullValueHandling = NullValueHandling.Ignore;

                    string strResult = JsonConvert.SerializeObject(commRequest, jss);
                    resp = new HttpResponseMessage()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Content = new StringContent(strResult, Encoding.UTF8, "application/json")
                    };
                }

                
            }
            catch (Exception e)
            {
                resp = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }

            return resp;
        }

        //하위댓글 정보 조회
        [HttpPost]
        [Route("Api/BoardComment/CommentInfo")]
        public List<Model.BoardComment> CommentInfo(CommentRequest commRequest)
        {

            List<Model.BoardComment> lists = new List<Model.BoardComment>();

            try
            {
                using (Business.Board biz = new Business.Board(ConfigurationManager.ConnectionStrings["Board"].ConnectionString))
                {
                    lists = biz.CommentListDetail(new Model.BoardComment()
                    {
                        BoardID = commRequest.BoardID,
                        CommentID = commRequest.CommentID
                    });
                }
            }
            catch(Exception ex)
            {
                Common.Error.WebHandler(ex);
            }

            return lists;
        }

        //비밀번호 비교
        [HttpPost]
        [Route("Api/Board/ChkPW")]
        public string PasswordCheck(CommentRequest commRequest)
        {
            CommentResponse json = new CommentResponse();

            try
            {
                using (Business.Board biz = new Business.Board(ConfigurationManager.ConnectionStrings["Board"].ConnectionString))
                {
                    bool bReturn = biz.CommPasswordCheck(new Model.BoardComment()
                    {
                        BoardID = commRequest.BoardID,
                        CommentID = commRequest.CommentID,
                        CommentPW = commRequest.CommentPW
                    }, commRequest.Mode);

                    if (bReturn)
                    {
                        json.Result = "Success";
                    }
                    else
                    {
                        json.Result = "Error";
                    }
                }
            }
            catch (Exception e)
            {
                json.Result = "Error";
            }

            return JsonConvert.SerializeObject(json);
        }

        //댓글삭제
        [HttpGet]
        [Route("Api/Board/DeleteComment/{boardID}/{commentID}")]
        public string DeleteComment(string boardID, string commentID)
        {

            CommentResponse json = new CommentResponse();

            try
            {
                using (Business.Board biz = new Business.Board(ConfigurationManager.ConnectionStrings["Board"].ConnectionString))
                {

                    biz.Delete(new Model.BoardComment {
                        BoardID = boardID,
                        CommentID = commentID 
                    });

                }

                json.Result = "Success";
            }
            catch (Exception e)
            {
                json.Result = "Error";
            }

            return JsonConvert.SerializeObject(json);
        }



        private class CommentResponse
        {
            public string Result { get; set; }
        }

        public class CommentRequest
        {
            public string BoardID { get; set; }

            public string CommentID { get; set; }

            public string Contents { get; set; }

            public string ParentID { get; set; }

            public string WriterName { get; set; }

            public string CommentPW { get; set; }

            public string Mode { get; set; }
        }
    }
}