<%@ Page Title="투비콘 | 게시판" Language="C#" MasterPageFile="~/Master/Site.Master" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Project.Donna.Board.View" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <div id="container">
            <div class="sub_top">
                <div class="inner">
                    <span class="sub_title">VIEW</span>
                </div>
            </div>
            <section>
                <div class="inner">
                    <div class="board_view_top">
                        <h3 class="view_title"><asp:Literal ID="lblSubject" runat="server"></asp:Literal></h3>
                        <ul class="view_info">
                            <li class="name"><asp:Literal ID="lblWriterName" runat="server"></asp:Literal></li>
                            <li class="date"><asp:Literal ID="lblWriteDate" runat="server"></asp:Literal></li>
                        </ul>
                    </div>
                    <div class="board_view_wrap">
                        <div class="contents_view_box" id="divContents" runat="server">
                        </div>
                        <div class="file_view_box">
                            <h3>첨부파일</h3>
                            <ul>
                                <!-- 목록 -->
                                <asp:Repeater id="repAttachList" runat="server">
                                    <ItemTemplate>
                                        <li><a href="/uploads/<%#Eval("BOARDID") %>/<%#Eval("FILESAVEDNAME") %>" download="<%#Eval("FILENAME")%><%#Eval("FILEEXTENSION") %>"><%#Eval("FILENAME")%><%#Eval("FILEEXTENSION") %></a></li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </div>
                    </div>
                    <div class="nav_box">
                        <a href="<%= strReferrer %>" class="btn type_primary">목록</a> <!-- referrer 체크 -->
                        <div class="edit_box">
                            <a href="javascript:void(0);" class="btn type_gray popup_open">수정</a>
                            <a href="javascript:void(0);>" class="btn type_gray popup_open">삭제</a>
                        </div>
                    </div>
                    <div class="board_comment_wrap">
                        <h3 class="comment_title">댓글 <strong><asp:Literal ID="lblTotal" runat="server"></asp:Literal></strong></h3>
                        <div class="comment_list_box" id="divCommentList" runat="server">
                            <!-- 댓글 목록 -->
                            <ul id="ulCommentList">
                                <asp:Repeater ID="repCommentList" runat="server">
                                    <ItemTemplate>
                                        <li id="liComm_<%# Container.ItemIndex %>" commid ="<%# Eval("CommentID") %>">
                                            <div class="comment_box <%# DateTime.Now.AddDays(-1) < Convert.ToDateTime(Eval("WriteDate").ToString()) ? "new" : "" %>">
                                                <div class="comment_info">
                                                    <span class="name"><%# Eval("WriterName") %></span>
                                                    <span class="date"><%# Eval("WriteDate") %></span>
                                                    <%# Eval("DelFlag").ToString() != "Y" ? "<span class=\"reply\"><a href=\"javascript:void(0);\" onclick=\"fnCreateReply(this)\" style=\"cursor:pointer\" id=\"a_" + Eval("CommentID") + "\">덧글</a></span>" : "" %>
                                                </div>
                                                <div class="comment_text">
                                                    <p><%# Eval("Contents") %></p>
                                                </div>
                                                <%# Eval("DelFlag").ToString() != "Y" ? "<button type=\"button\" class=\"btn_comment_delete popup_open\" id=\"btn_"+ Eval("CommentID") + "\" onclick=\"fnCommentDelete(this)\">삭제</button>":"" %>
                                            </div>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                            <!-- 댓글 없음 .hide -->
                            <div class="empty_box" id="divEmptyComment" runat="server">
                                <p>댓글이 없습니다.</p>
                            </div>
                        </div>
                        <div class="comment_write_box">
                            <h3 style="width:auto; max-width:30%">댓글작성</h3>
                            <a id="aCommCancel" href="javascript:void(0);" style="visibility:hidden; font-size: 14px;font-weight: 400;color: var(--gray-70); width:70%; line-height:24px; margin-left:15px;" onclick="fnCommCancel();">실행취소</a>
                            <div class="input_box">
                                <!-- .error -->
                                <asp:TextBox ID="txtCommName" runat="server" CssClass="form_input" placeholder="이름" onkeyup="fnEmptyCheck(this)" ClientIDMode="Static"></asp:TextBox>
                                <p class="error_message">이름을 입력해주세요.</p>
                            </div>
                            <div class="input_box">
                                <!-- .error -->
                                <asp:TextBox ID="txtCommPassword" runat="server" CssClass="form_input" placeholder="비밀번호" onkeyup="fnEmptyCheck(this)" MaxLength="20" TextMode="Password" ClientIDMode="Static"></asp:TextBox>
                                <p class="error_message">비밀번호는 4~20자까지 가능합니다.</p>
                            </div>
                            <div class="textarea_box">
                                <!-- .error -->
                                <textarea id="commentWrite" class="form_textarea" placeholder="댓글을 입력해 주세요." onKeyUp="fnEmptyCheck(this)" runat="server"></textarea>
                                <p class="error_message">댓글내용을 입력해주세요.</p> 
                            </div>
                            <asp:LinkButton ID="btnCreateComment" runat="server" CssClass="btn type_primary" Text="댓글등록" OnClientClick="return fnValidationCheck();" OnClick="btnCreateComment_Click"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </section>
        </div>
        
        <asp:HiddenField ID="hidBoardPW" runat="server" />
        <asp:HiddenField ID="hidCommentID" runat="server" />
</asp:Content>

<asp:Content id="Popup" ContentPlaceHolderID="PopupContent" runat="server">
    <div id="popup">
        <!-- 게시글 수정 -->
        <div class="popup_wrap type_center">
            <div class="popup_section">
                <h3 class="popup_title">게시글을 수정하시겠습니까?</h3>
                <p class="popup_text">
                    글 작성시 입력한 비밀번호를 입력하여
                    <br>
                    글을 수정할 수 있습니다.
                </p>
                <div class="input_box">
                    <!-- .error -->
                    <asp:TextBox ID="txtPassword_M" runat="server" CssClass="form_input" placeholder="비밀번호 입력" MaxLength="20" onkeyup="fnEmptyCheck(this)" TextMode="Password" autocomplete="off" ClientIDMode="Static" ></asp:TextBox>
                    <p class="error_message">비밀번호는 4~20자까지 가능합니다.</p>
                </div>
            </div>
            <div class="popup_btn_box">
                <button type="button" class="btn type_gray popup_close" onclick="fnInit('M');">취소</button>
                <asp:LinkButton ID="btnModifyMessage" runat="server" CssClass="btn type_primary" Text="확인" OnClick="btnModifyMessage_Click" OnClientClick="return fnPasswordCheck('M');"></asp:LinkButton>
            </div>
        </div>
        <!-- 게시글 삭제 -->
        <div class="popup_wrap type_center">
            <div class="popup_section">
                <h3 class="popup_title">게시글을 삭제하시겠습니까?</h3>
                <p class="popup_text">
                    글 작성시 입력한 비밀번호를 입력하여
                    <br>
                    글을 삭제할 수 있습니다.
                </p>
                <div class="input_box">
                    <!-- .error -->
                    <asp:TextBox ID="txtPassword_D" runat="server" CssClass="form_input" placeholder="비밀번호 입력" MaxLength="20" onkeyup="fnEmptyCheck(this)" TextMode="Password" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    <p class="error_message">비밀번호는 4~20자까지 가능합니다.</p>
                </div>
            </div>
            <div class="popup_btn_box">
                <button type="button" class="btn type_gray popup_close" onclick="fnInit('D');">취소</button>
                <asp:LinkButton id="btnDeleteMessage" runat="server" CssClass="btn type_primary" Text="확인" OnClick="btnDelete_Click" OnClientClick="return fnPasswordCheck('D');"></asp:LinkButton>
            </div>
        </div>
        <!-- 댓글 삭제 -->
        <div class="popup_wrap type_center">
            <div class="popup_section">
                <h3 class="popup_title">댓글을 삭제하시겠습니까?</h3>
                <p class="popup_text">
                    댓글 작성시 입력한 비밀번호를 입력하여
                    <br>
                    댓글을 삭제할 수 있습니다.
                </p>
                <div class="input_box">
                    <!-- .error -->
                    <asp:TextBox ID="txtPassword_CD" runat="server" CssClass="form_input" placeholder="비밀번호 입력" MaxLength="20" onkeyup="fnEmptyCheck(this)" TextMode="Password" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    <p class="error_message">비밀번호는 4~20자까지 가능합니다.</p>
                </div>
            </div>
            <div class="popup_btn_box">
                <button type="button" class="btn type_gray popup_close" onclick="fnInit('CD');">취소</button>
                <asp:Button ID="btnDeleteComment" runat="server" CssClass="btn type_primary" Text="확인" OnClientClick="return fnPasswordCheck('CD');" OnClick="btnDeleteComment_Click" />
            </div>
        </div>
        <div class="popup_bg"></div>
    </div>
</asp:Content>

<asp:Content ID="Footer" ContentPlaceHolderID="FooterContent" runat="server">
     <script type="text/javascript"> 
         var vBoardID = "<%= strBoardID %>";

         //에디터 세팅
         const viewer = new toastui.Editor.factory({
             el: document.querySelector("#MainContent_divContents"),
             viewer: true,
             height: '500px',
             initialValue: $("#MainContent_divContents").val()
         });

         $(function () {
             //fnReplyComment($("#ulCommentList"));
         });

         //대댓글 정보 불러오기
         function fnReplyComment(pObj) {
             
             $(pObj).find("li").each(function (idx, item) {
                 var vCommentID = $(item).attr("commid");
                 var contents = "";
                 
                 if (vCommentID != "" && vCommentID != null) {
                     $.ajax({
                         type: "POST"
                         , url: "/Api/BoardComment/CommentInfo"
                         , data:
                         {
                             "BoardID": vBoardID,
                             "CommentID" : vCommentID
                         }
                         , dataType: "json"
                         , async : true
                         , success: function (json) {
                             if (json != null) {
                                 contents += "<ul class=\"reply\">";

                                 $.each(json, function (i, comm) {
                                     contents += "<li commid=\"" + comm.CommentID + "\">";
                                     contents += "    <div class=\"comment_box\">";
                                     contents += "        <div class=\"comment_info\">";
                                     contents += "            <span class=\"name\">" + comm.WriterName + "</span>";
                                     contents += "            <span class=\"date\">" + comm.WriteDate + "</span>";
                                     if (comm.DelFlag != "Y") {
                                         contents += "<span class=\"reply\">";
                                         contents += "    <a href=\"javascript:void(0);\" onclick=\"fnCreateReply(this)\" style=\"cursor:pointer\" id=\"a_" + comm.CommentID + "\">덧글</a>";
                                         contents += "</span>";
                                     }
                                     contents += "        </div>";
                                     contents += "        <div class=\"comment_text\">";
                                     contents += "            <p>" + comm.Contents + "</p>";
                                     contents += "        </div>";
                                     if (comm.DelFlag != "Y") {
                                         contents += "<button type=\"button\" class=\"btn_comment_delete popup_open\" id=\"btn_" + comm.CommentID +"\" onclick=\"fnCommentDelete(this)\">삭제</button>";
                                     }
                                     contents += "    </div>";
                                     contents += "</li>";
                                 });

                                 contents += "</ul>";
                                 $(item).append(contents);

                                 //팝업 이벤트 바인딩
                                 popupLayer();

                                 //대댓글 호출
                                 setTimeout(function () {
                                     fnReplyComment($(item));
                                 }, 0);
                                 
                                 
                             }
                         },
                         error: function (json) {
                             alert("댓글을 불러오던 중 오류가 발생하였습니다.");
                         }
                     }); // end of ajax
                 } // end of if
                 
             }); //end of each
         } // end of function

         function fnValidationCheck() {
             var vReturn = true;

             //유효성 검사
             //내용
             if (MainContent_commentWrite.value == "") {
                 MainContent_commentWrite.focus();
                 MainContent_commentWrite.className += ' error';
                 vReturn = false;
             } else {
                 MainContent_commentWrite.classList.remove('error');
             }

             //비밀번호
             if (txtCommPassword.value != "") {
                 // 비밀번호 체크 (최소 4자, 최대 20자)
                 if (txtCommPassword.length < 4 || txtCommPassword.length > 20) {
                     txtCommPassword.focus();
                     txtCommPassword.className += ' error'
                     vReturn = false;
                 } else {
                     txtCommPassword.classList.remove('error');
                 }
             } else {
                 txtCommPassword.focus();
                 txtCommPassword.className += ' error'
                 vReturn = false;
             }

             //작성자
             if (txtCommName.value == "") {
                 txtCommName.focus();
                 txtCommName.className += ' error';
                 vReturn = false;
             } else {
                 txtCommName.classList.remove('error');
             }

             return vReturn;
         }

         //댓글 작성
         function fnCreateComment() {
             return fnValidationCheck();
             /*
             if (fnValidationCheck()) {
                 $.ajax({
                     type: "POST"
                     , url: "/Api/BoardComment/Create"
                     , data:
                     {
                         "BoardID": vBoardID,
                         "Writername": commentName.value,
                         "Contents": commentWrite.value,
                         "CommentPW": SHA256(commentPassword.value),
                         "ParentID": $("#MainContent_hidCommentID").val()
                     }
                     , dataType: "json"
                     , success: function (result) {
                         alert("등록되었습니다.");
                         location.reload();
                     },
                     error: function (result) {
                         alert("등록중 오류가 발생하였습니다.");
                     }

                 });
             }
             else {
                 return false;
             }
             */
         }

         function fnPasswordCheck(pMode) {
             var vPassword = $("#txtPassword_" + pMode).val();

             //비밀번호
             if (vPassword != "") {
                 // 비밀번호 체크 (최소 4자, 최대 20자)
                 if (vPassword.length < 4 || vPassword.length > 20) {
                     $("#txtPassword_" + pMode).focus();
                     $("#txtPassword_" + pMode).addClass("error");
                     return false;
                 } else {
                     $("#txtPassword_" + pMode).removeClass("error");
                 }
             } else {
                 $("#txtPassword_" + pMode).focus();
                 $("#txtPassword_" + pMode).addClass("error");
                 return false;
             }

             //비밀번호 검증
             /*
             $.ajax({
                 type: "POST"
                 , url: "/Api/Board/ChkPW"
                 , data:
                 {
                     "BoardID": vBoardID,
                     "CommentID": $("#MainContent_hidCommentID").val(),
                     "CommentPW": vPassword,
                     "Mode": pMode
                 }
                 , dataType: "json"
                 , success: function (result) {
                     var res = JSON.parse(result);

                     if (res != null) {
                         if (res.Result == "Success") {
                             return true;
                             
                             if (pMode == "M") { //수정
                                 location.href = "/Board/Write?BoardID=" + vBoardID;
                             }
                             else if (pMode == "D") { //삭제
                                 $("#PopupContent_btnMessageDelete").click();
                             }
                             else if (pMode == "CD") { //댓글삭제
                                 $.ajax({
                                     type: "GET"
                                     , url: "/Api/Board/DeleteComment/" + vBoardID + "/" + $("#MainContent_hidCommentID").val()
                                     , dataType: "json"
                                     , success: function (result) {
                                         var resultObj = JSON.parse(result);

                                         if (resultObj.CommResult.Return == "Success") {
                                             alert("삭제되었습니다.");
                                             location.reload();
                                         }
                                         else {
                                             alert("삭제 중 오류가 발생하였습니다.");
                                         }
                                     }
                                     , error: function (result) {
                                         alert("삭제 중 오류가 발생하였습니다.");
                                     }
                                 });
                             }
                             
                         }
                         else {
                             $("#txtPassword_" + pMode).focus();
                             $("#txtPassword_" + pMode).addClass("error");
                             $("#txtPassword_" + pMode).siblings(".error_message").text("비밀번호가 일치하지 않습니다.");
                             return false;
                         }
                     }
                 },
                 error: function (result) {
                     $("#txtPassword_" + pMode).focus();
                     $("#txtPassword_" + pMode).addClass("error");
                     $("#txtPassword_" + pMode).siblings(".error_message").text("비밀번호가 일치하지 않습니다.");
                     return false;
                 }

             });*/
             return true;
         }

         //댓글삭제
         function fnCommentDelete(obj) {
             var vID = $(obj).attr("id").split("_")[1];
             $("#MainContent_hidCommentID").val(vID);
         }

         //덧글작성
         function fnCreateReply(obj) {
             $("#commentName").focus();
             var vCommentID = $(obj).attr("id").split("_")[1];
             $("#MainContent_hidCommentID").val(vCommentID);

             var vCommentName = $(obj).parent().siblings()[0].textContent;

             $(".comment_write_box h3").text(vCommentName + "에 대한 덧글 작성");
             $("#aCommCancel").css("visibility", "visible");
         }

         //덧글작성취소
         function fnCommCancel() {
             $("#MainContent_hidCommentID").val("00000000-0000-0000-0000-000000000000");
             $(".comment_write_box h3").text("댓글작성");
             $("#aCommCancel").css("visibility", "hidden");
         }

         //공백문자체크
         function fnEmptyCheck(pObj) {
             if ($(pObj).attr("type") == "password" && $(pObj).val().length < 4) {
                 $(pObj).addClass("error");
             }
             else if ($(pObj).val().length < 1) {
                 $(pObj).addClass("error");
             }
             else {
                 $(pObj).removeClass("error");
             }

             if (window.event.keyCode == 13) {
                 var vMode = pObj.id.substring(pObj.id.lastIndexOf("_") + 1, pObj.id.length);

                 switch (vMode) {
                     case "M": document.getElementById("<%= btnModifyMessage.ClientID %>").click(); break;
                     case "D": document.getElementById("<%= btnDeleteMessage.ClientID %>").click(); break;
                     case "CD": document.getElementById("<%= btnDeleteComment.ClientID %>").click(); break;
                     default: break;
                 }
             }

         }

         function fnInit(pMode) {
             $("#txtPassword_" + pMode).val('');
             $("#txtPassword_" + pMode).removeClass("error");
         }
     </script>
</asp:Content>
