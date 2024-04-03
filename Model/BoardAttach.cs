using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Donna.Model
{
    public class BoardAttach
    {
        //파일ID
        public string FileID { get; set; }

        //게시물ID
        public string BoardID { get; set; }

        //파일이름
        public string FileName { get; set; }

        //파일 확장자
        public string FileExtension { get; set; }

        //파일 경로
        public string FilePath { get; set; }

        //파일저장이름
        public string FileSavedName { get; set; }

        //파일등록일자
        public DateTime CreateDate { get; set; }

        //파일삭제일자
        public DateTime DeleteDate { get; set; }

    }
}