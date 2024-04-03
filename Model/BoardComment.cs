using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Donna.Model
{
    public class BoardComment
    {

        //댓글 ID
        public string CommentID { get; set; }

        //게시물 ID
        public string BoardID { get; set; }

        //댓글 내용
        public string Contents { get; set; }

        //상위 댓글 ID
        public string ParentID { get; set; }

        //작성자 이름
        public string WriterName { get; set; }

        //댓글 비밀번호
        public string CommentPW { get; set; }

        //댓글 순서
        public string Sort {  get; set; }

        public string Depth { get; set; }

        //삭제여부
        public string DelFlag { get; set; }

        //작성일자
        public string WriteDate { get; set; }

        //삭제일자
        public string DeleteDate { get; set; }

        //총갯수
        public int CommTotal { get; set; }

    }
}