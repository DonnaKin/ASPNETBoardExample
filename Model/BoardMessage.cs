using System;

namespace Project.Donna.Model
{
    public class BoardMessage
    {
        //게시물 번호
        public int RowNum { get; set; }

        //게시물 ID
        public string BoardID {  get; set; }

        //게시물 제목
        public string Subject {  get; set; }

        //게시물 내용
        public string Contents {  get; set; }

        //게시물 작성자
        public string WriterName { get; set; }

        //게시물 비밀번호
        public string BoardPW { get; set; }

        //조회수
        public int BoardViews { get; set; } 

        //개인정보 동의여부
        public string ChkAgree { get; set; }

        //작성일자
        public DateTime WriteDate { get; set; }

        //수정일자
        public DateTime ModifyDate { get; set; }

        //삭제일자
        public DateTime DeleteDate { get; set; }

        //삭제여부
        public string IsDelete { get; set; }

        //페이지 사이즈
        public int iPageSize { get; set; }

        //현재 페이지
        public int iPageNum { get; set; }
    }
}