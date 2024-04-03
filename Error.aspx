<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Master/Site.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="Project.Donna.Error" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="container">
        <div class="sub_top">
            <div class="inner">
                <span class="sub_title">에러페이지</span>
            </div>
        </div>
        <section>
            <div class="inner">
                <div class="board_view_top">
                    <h3 class="view_title">에러가 발생하였으니 잠시 후 시도해주세요.</h3>
                </div>
                <div class="nav_box">
                    <a href="/Board/List" class="btn type_primary">메인으로 이동</a>
                </div>
            </div>
        </section>
    </div>        
</asp:Content>
