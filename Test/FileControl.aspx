<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileControl.aspx.cs" Inherits="Project.Donna.Test.FileControl" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>멀티파일 테스트</title>
    <link rel="stylesheet" href="../resources/css/reset.css"/>
    <link rel="stylesheet" href="../resources/css/fonts.css"/>
    <link rel="stylesheet" href="../resources/css/common.css"/>
    <script src="../lib/js/jquery-3.5.1.min.js"></script>
    <script src="../resources/js/common.js"></script>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div id="container">
            <div class="sub_top">
                <div class="inner">
                    <span class="sub_title">WRITE</span>
                </div>
            </div>
            <section>
                <div class="inner">
                    <div class="board_write_wrap">
                        <div class="upload_box">
                            <h3 class="form_label">첨부파일 <em>(10MB 이내 파일 첨부 가능)</em></h3>
                            <ul id="fileInputs">
                                <li class="file_wrap">
                                    <span class="file_name">첨부파일03.hwp</span>
                                    <button type="button" class="btn_delete">삭제</button>
                                </li>
                                <li class="file_wrap">
                                    <label for="uploadFile" class="btn_upload">첨부하기</label>
                                    <input type="file" id="uploadFile" class="form_file hide" name="files" />
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="nav_box">
                        <asp:LinkButton ID="btnFileUpload" runat="server" Text="확인" OnClick="btnFileUpload_Click" CssClass="btn type_primary"></asp:LinkButton>
                        <input type="button" value="+" onclick="addFileInput()" />
                        <input type="button" value="Upload" onclick="uploadFiles()" />
                        <br />
                        <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>
                    </div>
                </div>
            </section>
        </div>
    </form>
    <script type="text/javascript">
        function addFileInput() {
            var fileInput = document.createElement('input');
            fileInput.type = 'file';
            fileInput.name = 'files';
            fileInput.multiple = true;
            /*
            <li class="file_wrap">
    <label for="uploadFile" class="btn_upload">첨부하기</label>
    <input type="file" id="uploadFile" class="form_file hide" name="files" />
</li>
            */
            var vHtml = "";
            vHtml += "<li class=\"file_wrap\">";
            vHtml += "    <label for=\"uploadFile\" class=\"btn_upload\">첨부하기</label>";
            vHtml += "    <input type=\"file\" id=\"uploadFile\" class=\"form_file hide\" name=\"files\" />";
            vHtml += "</li>";

            //document.getElementById('fileInputs').appendChild(fileInput);
            $("#fileInputs").append(vHtml);
        }

        function uploadFiles() {
            var files = document.querySelectorAll('input[type=file]');
            var formData = new FormData();
            var xhr = new XMLHttpRequest();

            files.forEach(function (fileInput) {
                if (fileInput.files.length > 0) {
                    for (var i = 0; i < fileInput.files.length; i++) {
                        formData.append('files[]', fileInput.files[i]);
                    }
                }
            });

            xhr.open('POST', 'FileControl.aspx', true);
            xhr.setRequestHeader('Content-type', 'multipart/form-data');
            xhr.onreadystatechange = function () {
                if (xhr.ready === XMLHttpRequest.DONE) {
                    if (xhr.status === 200) {
                        document.getElementById('lblResult').innerHTML = xhr.responseText;
                    }
                    else {
                        document.getElementById('lblResult').innerHTML = "오류가 발생하였습니다.";
                    }
                }
            };

            xhr.send(formData);

        }
    </script>
</body>
</html>
