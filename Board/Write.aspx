<%@ Page Title="투비콘 | 게시판" Language="C#" MasterPageFile="~/Master/Site.Master" AutoEventWireup="true" CodeBehind="Write.aspx.cs" Inherits="Project.Donna.Board.Write" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<div id="container">
    <div class="sub_top">
        <div class="inner">
            <span class="sub_title">WRITE</span>
        </div>
    </div>
    <section>
        <div class="inner">
            <div class="board_write_wrap">
                <div class="input_box">
                    <label for="subject" class="form_label">제목</label>
                    <!-- .error -->
                    <asp:TextBox runat="server" ID="txtSubject" CssClass="form_input" ClientIDMode="Static" placeholder="제목 입력" Required="true" onKeyUp="fnEmptyCheck('Subject')" />
                    <p class="error_message">제목을 입력해주세요.</p>
                </div>
                <div class="input_box half">
                    <label for="userName" class="form_label">작성자</label>
                    <!-- .error -->
                    <asp:TextBox runat="server" ID="txtWriterName" CssClass="form_input" ClientIDMode="Static" placeholder="이름 입력" Required="true" onKeyUp="fnEmptyCheck('WriterName')" />
                    <p class="error_message">이름을 입력해주세요.</p>
                </div>
                <div class="input_box half">
                    <!-- .error -->
                    <label for="userPassword" class="form_label">비밀번호</label>
                    <asp:TextBox runat="server" ID="txtPassword" CssClass="form_input" ClientIDMode="Static" placeholder="비밀번호 입력" onKeyUp="fnEmptyCheck('Password')" TextMode="Password" MaxLength="20" Required="true" autocomplete="off" />
                    <p class="error_message">비밀번호는 4~20자까지 가능합니다.</p>
                </div>
                <div class="textarea_box">
                    <!-- .error -->
                    <label for="contents" class="form_label">내용</label>
                    <div id="divContent" runat="server"></div>
                    <p class="error_message">내용을 입력해주세요.</p>
                </div>
                <div class="upload_box">
                    <h3 class="form_label">첨부파일<em>(최대 3개까지 업로드 가능)</em></h3> 
                    <em id="emFileCancel" style="display:none; font-size:13px; font-weight:400; color:var(--gray-70);"><a href="javascript:void(0);" onclick="fnFileInit();">파일첨부 취소</a></em>
                    <ul id="ulUploadFile" runat="server">
                        <asp:Repeater ID="rptFile" runat="server">
                            <ItemTemplate>
                                <li class="file_wrap" style="height:38px;" id="liFile_<%#Container.ItemIndex %>" fileType="old" fileID="<%# Eval("FileID") %>" filesavedname="<%# Eval("FileSavedName") %>" >
                                    <span class="file_name"><%# Eval("FileName") %><%# Eval("FileExtension") %></span>
                                    <%--<button type="button" class="btn_delete" onclick="fnFileDelete(<%# Container.ItemIndex %>);">삭제</button>--%>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                        <li class="file_wrap" id="liFileUpload">
                            <label for="MainContent_iUploadFile" class="btn_upload">첨부하기</label>
                            <asp:FileUpload ID="iUploadFile" CssClass="form_file hide" runat="server" onchange="fnFileUpload(this);" AllowMultiple="true"/>
                        </li>
                    </ul>
                </div>
                <div class="terms_box">
                    <!-- .error -->
                    <div class="check_box">
                        <input type="checkbox" id="termsAgree" runat="server">
                        <label for="MainContent_termsAgree">
                            <span class="icon_chkbox"></span>
                            [필수] 개인정보 수집/이용 동의
                        </label>
                    </div>
                    <div class="terms_text_wrap">
                        <p>
                            1. 개인정보의 수집항목 및 수집방법<br>
                            통계청 나라통계사이트에서는 기본적인 회원 서비스 제공을 위한 필수정보로 실명인증정보와 가입정보로 구분하여 다음의 정보를 수집하고 있습니다. 
                            필수정보를 입력해주셔야 회원 서비스 이용이 가능합니다.<br>
                            <br>
                            가. 수집하는 개인정보의 항목<br>
                            * 수집하는 필수항목<br>
                            - 실명인증정보 : 이름, 휴대전화번호, 본인 인증 또는 I-PIN(개인식별번호), GPKI<br>
                            - 가입정보 : 아이디, 비밀번호, 성명, 이메일, 전화번호, 휴대전화번호, 기관명<br>
                            * 선택항목<br>
                            - 주소, 기관의 부서명<br>
                            <br>
                            [컴퓨터에 의해 자동으로 수집되는 정보]<br>
                            인터넷 서비스 이용과정에서 아래 개인정보 항목이 자동으로 생성되어 수집될 수 있습니다.<br> 
                            - IP주소, 서비스 이용기록, 방문기록 등<br>
                            <br>
                            나. 개인정보 수집방법<br>
                            홈페이지 회원가입을 통한 수집<br>
                            <br>
                            2. 개인정보의 수집/이용 목적 및 보유/이용 기간<br>
                            통계청 나라통계사이트에서는 정보주체의 회원 가입일로부터 서비스를 제공하는 기간 동안에 한하여 통계청 나라통계사이트 서비스를 이용하기 위한 최소한의 개인정보를 보유 및 이용 하게 됩니다. 
                            회원가입 등을 통해 개인정보의 수집·이용, 제공 등에 대해 동의하신 내용은 언제든지 철회하실 수 있습니다. 
                            회원 탈퇴를 요청하거나 수집/이용목적을 달성하거나 보유/이용기간이 종료한 경우, 사업 폐지 등의 사유발생시 개인 정보를 지체 없이 파기합니다.<br>
                            <br>
                            * 실명인증정보<br>
                            - 개인정보 수집항목 : 이름, 휴대폰 본인 인증 또는 I-PIN(개인식별번호), GPKI<br>
                            - 개인정보의 수집·이용목적   : 홈페이지 이용에 따른 본인 식별/인증절차에 이용<br>
                            - 개인정보의 보유 및 이용기간 : I-PIN / GPKI는 별도로 저장하지 않으며 실명인증용으로만 이용<br>
                            <br>
                            * 가입정보<br>
                            - 개인정보 수집항목 : 아이디, 비밀번호, 성명, 이메일, 전화번호, 휴대전환번호, 기관명<br>
                            - 개인정보의 수집·이용목적 : 홈페이지 서비스 이용 및 회원관리, 불량회원의 부정 이용방지, 민원신청 및 처리 등<br>
                            - 개인정보의 보유 및 이용기간 : 2년 또는 회원탈퇴시<br>
                            <br>
                            정보주체는 개인정보의 수집·이용목적에 대한 동의를 거부할 수 있으며, 동의 거부시 통계청 나라통계사이트에 회원가입이 되지 않으며, 통계청 나라통계사이트에서 제공하는 서비스를 이용할 수 없습니다.<br>
                            <br>
                            3. 수집한 개인정보 제3자 제공<br>
                            통계청 나라통계사이트에서는 정보주체의 동의, 법률의 특별한 규정 등 개인정보 보호법 제17조 및 제18조에 해당하는 경우에만 개인정보를 제3자에게 제공합니다.<br>
                            <br>
                            4. 개인정보 처리업무 안내<br>
                            통계청 나라통계사이트에서는 개인정보의 취급위탁은 하지 않고 있으며, 원활한 서비스 제공을 위해 아래의 기관을 통한 실명인증 및 공공 I-PIN, GPKI 인증을 하고 있습니다.<br>
                            <br>
                            * 수탁업체<br>
                            - 행정자치부<br>
                            · 위탁업무 내용 : 공공 I-PIN, GPKI 인증<br>
                            · 개인정보 보유 및 이용 기간 : 행정자치부에서는 이미 보유하고 있는 개인정보이기 때문에 별도로 저장하지 않음
                        </p>
                    </div>
                    <p class="error_message">개인정보 약관에 동의해주세요.</p>
                </div>
            </div>
            <div class="nav_box">
                <a href="javascript:history.back();" class="btn type_gray">취소</a>
                <asp:Button runat="server" ID="btnRegist" CssClass="btn type_primary" OnClientClick="return fnRegMessage();" OnClick="btnRegist_Click" Text="확인" style="cursor:pointer;" />
            </div>
        </div>
    </section>
    <asp:HiddenField ID="hidContent" runat="server" />
    <asp:HiddenField ID="hidChkAgree" runat="server" />
    <asp:HiddenField ID="hidDelFileInfo" runat="server" />
</div>
</asp:Content>
<asp:Content ID="Footer" ContentPlaceHolderID="FooterContent" runat="server">
    <script type="text/javascript">
        var vBoardID = "<%=strBoardID%>";
        var fileNo = 0;

        //에디터 세팅
        const editor = new toastui.Editor({
            el: document.querySelector("#MainContent_divContent"),
            height: '500px',
            initialEditType: "wysiwyg",
            placeholder: "내용을 입력해 주세요.",
            previewStyle: 'tab',
            toolbarItems: [
                ['heading', 'bold', 'italic', 'strike'],
                ['hr', 'quote'],
                ['ul', 'ol', 'task', 'indent', 'outdent'],
                ['table', 'link'],
                ['scrollSync']
            ]
        });

        $(document).ready(function () {
            if (vBoardID != "") {
                editor.setHTML(fnInputHtml(editor.initialHTML));
            }
            $("#txtSubject").focus();
            if ($("#MainContent_ulUploadFile > li").length > 1) {
                $("#liFileUpload").hide();
                $("#emFileCancel").show();
            }
            else {
                $("#liFileUpload").show();
                $("#emFileCancel").hide();
            }
        });

        //첨부파일 초기화
        function fnFileInit() {
            //첨부파일 정보가 있을 경우 기존 파일 삭제 정보 저장
            $("#MainContent_ulUploadFile").find("li").not("[id=liFileUpload]").each(function (idx, item) {
                var delFileInfo = $("#MainContent_hidDelFileInfo").val();
                var liFileInfo = $(item).attr("fileid") + "," + $(item).attr("filesavedname");

                if ($(item).attr("filetype") == "old") {
                    $("#MainContent_hidDelFileInfo").val(delFileInfo + liFileInfo + "|");
                }
            });

            $("#MainContent_ulUploadFile li").not("[id=liFileUpload]").remove();
            $("#MainContent_iUploadFile").val('');
            $("#liFileUpload").show();
            $("#emFileCancel").hide();
        }

        //첨부파일 제한
        function fnFileLimit() {
            if ($("#MainContent_ulUploadFile > li").length > 3) {
                alert("최대 첨부 갯수는 1개 입니다.");
            }
        }

        // 첨부파일 업로드
        function fnFileUpload(pObj) {
            var vMaxFileCnt = 3;
            var vFileHtml = "";
            fileNo = $("#MainContent_ulUploadFile > li[id^='liFile_']").length;

            for (const file of pObj.files) {
                if (fnFileValidation(file)) {
                    var vFileName = file.name;

                    if (vFileName != "") {
                        vFileHtml += "<li class=\"file_wrap\" style=\"height:38px;\" id=\"liFile_" + fileNo + "\" filetype=\"new\">"
                        vFileHtml += "<span class=\"file_name\">" + vFileName + "</span>";
                        //vFileHtml += "<button type=\"button\" class=\"btn_delete\" onclick=\"fnFileDelete(" + fileNo + ");\">삭제</button></li>";
                    }
                    fileNo++;
                }
            }

            $("#MainContent_ulUploadFile").prepend(vFileHtml);

            //최대첨부 갯수 초과 시 업로드 영역 숨김처리
            if ($("#MainContent_ulUploadFile > li").length > 1) {
                $("#liFileUpload").hide();
                $("#emFileCancel").show();
            }
            else {
                $("#liFileUpload").show();
                $("#emFileCancel").hide();
            }
        }

        function fnFileValidation(pObj) {
            const fileType = ['application/pdf', 'image/gif', 'image/jpeg', 'image/png', 'text/plain'];
            const fileExtension = ['pdf', 'gif', 'jpg', 'jpeg', 'png', 'txt', 'xlsx', 'pptx', 'docx'];

            var vText = pObj.name.substring(pObj.name.lastIndexOf(".") + 1, pObj.name.length);
            //vText = pObj.type;

            if (pObj.size > (10 * 1024 * 1024)) {
                alert("10MB 초과의 파일은 첨부할 수 없습니다.");
                return false;
            } //else if (!fileType.includes(vText)) {
            else if (!fileExtension.includes(vText)) {
                alert("허용되지 않은 확장자는 제외되었습니다.");
                return false;
            } else {
                return true;
            }
        }

        //첨부파일 삭제
        function fnFileDelete(pFileNo) {
            //첨부파일 정보가 있을 경우 기존 파일 삭제 정보 저장
            var delFileInfo = $("#MainContent_hidDelFileInfo").val();
            var liFIleInfo = $("#liFile_" + pFileNo).attr("fileid") + "," + $("#liFile_" + pFileNo).attr("filesavedname");

            if ($("#liFile_" + pFileNo).attr("filetype") == "old") {
                $("#MainContent_hidDelFileInfo").val(delFileInfo + liFIleInfo + "|");
            }

            $("#liFile_" + pFileNo).remove();

            if ($("#MainContent_ulUploadFile > li").length < 4) {
                $("#liFileUpload").show();
            }
        }

        // 게시등록
        function fnRegMessage() {
            if (!fnValidationCheck()) {
                return false;
            }
            else {
                $("#MainContent_hidContent").val(fnRemoveHtml(editor.getHTML()));
                $("#MainContent_hidChkAgree").val("Y");
            }
            return true;
        }

        // Validation 체크
        function fnValidationCheck() {
            var vReturn = true;

            //비밀번호
            if ($("#txtPassword").val().length < 4 || $("#txtPassword").val().length > 20) {
                $("#txtPassword").addClass("error");
                $("#txtPassword").focus();
                vReturn = false;
            }

            //작성자
            if ($("#txtWriterName").val() == "") {
                $("#txtWriterName").addClass("error");
                $("#txtWriterName").focus();
                vReturn = false;
            }

            //제목
            if ($("#txtSubject").val() == "") {
                $("#txtSubject").addClass("error");
                $("#txtSubject").focus();
                vReturn = false;
            }

            //내용
            if (editor.getMarkdown() == "") {
                alert("내용을 입력해주세요");
                vReturn = false;
            }

            //개인정보 동의여부
            if (!$("#MainContent_termsAgree").prop("checked")) {
                alert("개인정보 수집/이용에 동의하여 주십시오.");
                vReturn = false;
            }

            return vReturn;
        }

        function fnEmptyCheck(pMode) {
            if (pMode == "Password" && $("#txt" + pMode).val().length < 4) {
                $("#txt" + pMode).addClass("error");
            }
            else if ($("#txt" + pMode).val().length > 0) {
                $("#txt" + pMode).removeClass("error");
            }
            else {
                $("#txt" + pMode).addClass("error");
            }
        }
    </script>
</asp:Content>

