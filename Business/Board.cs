using Project.Donna.Model;
using Project.Donna.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Project.Donna.Business
{
    public class Board : IDisposable
    {
        private Service.Board _board = null;
        public Board(string pConn) { 
            _board = new Service.Board(pConn);
        }

        //게시 목록
        public List<Model.BoardMessage> List(int pPage, int pSize, string pSearchType, string pSearchText) { 
            return _board.List(pPage, pSize, pSearchType, pSearchText);
        }

        //게시 목록 갯수
        public int Count(string pSearchType, string pSearchText) {
            return _board.GetBoardCount(pSearchType, pSearchText);
        }

        //게시물 작성
        public string Regist(Model.BoardMessage pBoard)
        {
            return _board.Regist(pBoard);
        }

        //게시물 수정
        public string Update(Model.BoardMessage pBoard)
        {
            return _board.Update(pBoard);
        }

        //첨부파일 등록
        public int FileRegist(Model.BoardAttach pAttach)
        {
            return _board.FileRegist(pAttach);
        }

        //첨부파일 삭제
        public int FileDelete(Model.BoardAttach pAttach)
        {
            return _board.FileDelete(pAttach);
        }

        //첨부파일 정보 List
        public List<Model.BoardAttach> FileInfo(Model.BoardAttach attach)
        {
            return _board.FileInfo(attach);
        }

        //첨부파일 정보 DataTable
        public DataTable dtFileInfo(Model.BoardAttach attach)
        {
            return _board.dtFileInfo(attach);
        }

        //게시물 상세 정보
        public Model.BoardMessage Detail(Model.BoardMessage board)
        {
            return _board.Detail(board);
        }

        //게시물 조회 수 증가
        public int UpperMessage(Model.BoardMessage board)
        {
            return _board.UpperMessage(board);
        }

        //게시물 삭제
        public int DeleteMessage(Model.BoardMessage board)
        {
            return _board.DeleteMessage(board);
        }

        //댓글 전체 내용 조회 List
        public List<Model.BoardComment> CommentList(Model.BoardComment comment)
        {
            return _board.CommentList(comment);
        }

        //댓글 전체 내용 조회 DataTable
        public DataTable dtCommentList(Model.BoardComment comment)
        {
            return _board.dtCommentList(comment);
        }

        //하위 댓글 조회
        public List<Model.BoardComment> CommentListDetail(Model.BoardComment comment)
        {
            return _board.CommentListDetail(comment);
        }

        //게시물 댓글 갯수
        public int CommentCount(Model.BoardComment comment)
        {
            return _board.CommentCount(comment);
        }

        //댓글 작성
        public int Create(Model.BoardComment comment)
        {
            return _board.Create(comment);
        }

        //댓글 삭제
        public int Delete(Model.BoardComment comment)
        {
            return _board.Delete(comment);
        }

        //비밀번호 비교
        public bool CommPasswordCheck(Model.BoardComment comment, string pMode)
        {
            return _board.CommPasswordCheck(comment, pMode);
        }


        public void Dispose()
        {
            _board = null;
        }

        
    }
}