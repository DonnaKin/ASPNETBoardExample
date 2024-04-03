<%@ Page Title="투비콘 | 게시판" Language="C#" MasterPageFile="~/Master/Site.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Project.Donna.Board.List" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="container">
        <div class="sub_top">
            <div class="inner">
                <span class="sub_title">LIST</span>
            </div>
        </div>
        <section>
            <div class="inner">
                <div class="board_list_top">
                    <div class="left">
                        <div class="total">총 <strong><asp:Label ID="lblTotal" runat="server"></asp:Label></strong>개</div>
                    </div>
                    <div class="right">
                        <select id="selSearchType" class="select_box">
                            <option value="0">검색조건</option>
                            <option value="Subject">제목</option>
                            <option value="Contents">내용</option>
                            <option value="WriterName">글쓴이</option>
                        </select>
                        <div class="search_box">
                            <input type="text" id="search" class="input_search" placeholder="검색어를 입력해주세요." onkeyup="fnEnterSearch()">
                            <button type="button" class="btn_search" id="btnSearch" onclick="fnSearchClick()">검색</button>
                        </div>
                    </div>
                </div>
                <div class="board_list_wrap">
                    <ul class="thead">
                        <li class="th numb">번호</li>
                        <li class="th subject">제목</li>
                        <li class="th name">글쓴이</li>
                        <li class="th date">등록일</li>
                        <li class="th hit">조회</li>
                    </ul>
                    <!-- 목록 -->
                    <asp:Repeater id="repList" runat="server">
                        <ItemTemplate>
                            <ul class="tbody">
                                <li class="td numb">
                                    <span><%# Eval("Rownum") %></span>
                                </li>
                                <li class="td subject">
                                    <a href="/Board/View?BoardID=<%# Eval("BoardID").ToString() %>"><%# Eval("Subject").ToString() %></a>
                                    <%# DateTime.Now.AddDays(-1) < Convert.ToDateTime(Eval("WriteDate").ToString()) ? "<span class=\"icon_new\">NEW</span>" : ""  %>
                                </li>
                                <li class="td name">
                                    <span><%# Eval("WriterName").ToString() %></span>
                                </li>
                                <li class="td date">
                                    <span><%# ((DateTime)Eval("WriteDate")).ToString(("yyyy-MM-dd")) %></span>
                                </li>
                                <li class="td hit">
                                    <span><%# Eval("BoardViews").ToString() %></span>
                                </li>
                            </ul>
                        </ItemTemplate>
                    </asp:Repeater>
                    <div class="empty_box" id="divEmpty" runat="server">
                        <p>게시글이 없습니다.</p>
                    </div>
                </div>
                <div class="pager_box" id="divPaging" runat="server">
                </div>
                <div class="nav_box">
                    <div class="edit_box">
                        <a href="/Board/Write" class="btn type_gray">글쓰기</a>
                    </div>
                </div>
            </div>
        </section>
    </div>
    <asp:HiddenField ID="hidSearchType" runat="server" />
    <asp:HiddenField ID="hidSearchText" runat="server" />
</asp:Content>
<asp:Content ID="Footer" ContentPlaceHolderID="FooterContent" runat="server">
<script type="text/javascript">
    var vURL_href = window.location.href;
    var vURL = new URL(vURL_href);

    $(function () {
        var vSearchType = getParameterByName('SearchType');
        var vSearchText = getParameterByName('SearchText');
        if (vSearchType != "") {
            $("#selSearchType").val(vSearchType).prop("selected", true);
        }

        if (vSearchText != "") {
            $("#search").val(vSearchText);
        }
    });

    function fnMovePage(page) {
        vURL.searchParams.set("PageNum", page);
        window.location.search = vURL.searchParams;
    }

    function fnEnterSearch() {
        if (window.event.keyCode == 13) {
            fnSearchClick();
        }
    }

    function fnSearchClick() {
        var strSearchType = $("#selSearchType").val();
        var strSearchText = $("#search").val();

        if (strSearchType == "0") {
            alert("검색조건을 선택해주세요");
            $("#selSearchType").focus();
            return false;
        }

        if (strSearchText == "") {
            alert("검색어를 입력해주세요");
            $("#search").focus();
            return false;
        }

        vURL.searchParams.set("SearchType", strSearchType);
        vURL.searchParams.set("SearchText", strSearchText);
        vURL.searchParams.set("PageNum", 1); //검색 시 페이지 1로 이동

        window.location.search = vURL.searchParams;
    }
</script>
</asp:Content>
