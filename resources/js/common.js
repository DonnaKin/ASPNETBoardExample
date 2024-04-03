$(function () {
    popupLayer();
});


//* ─────────────────── popup ─────────────────── *//
function popupLayer() {
    $('.popup_open').on('click', function () {
        var btnIndex = $('.popup_open').index(this);
        var popLth = $('#popup .popup_wrap').length;

        if (btnIndex > 2) {
            btnIndex = 2;
        }
        
        if (popLth == 1) {
            popupOn();
        } else {
            $('#popup .popup_wrap').addClass('hide');
            $('#popup .popup_wrap').eq(btnIndex).removeClass('hide');
            popupOn();
        }
        return false;
    });

    $('.popup_close').on('click',function(){ 
        popupOff();
        return false;
    });
}
function popupOn() {
    $('#popup').fadeIn(200);
    $('#popup').addClass('on');
}
function popupOff() {
    $('#popup').fadeOut(200);
    $('#popup').removeClass('on');
}

function fnRemoveHtml(pStr) {
    var strHtml = pStr;

    strHtml = strHtml.replace(/</g, "&lt;");
    strHtml = strHtml.replace(/>/g, "&gt;");
    strHtml = strHtml.replace(/\"/g, "&quot;");
    strHtml = strHtml.replace(/\'/g, "&#39;");

    return strHtml;
}

function fnInputHtml(pStr) {
    var strHtml = pStr;

    strHtml = strHtml.replace(/&lt;/g, "<");
    strHtml = strHtml.replace(/&gt;/g, ">");
    strHtml = strHtml.replace(/&quot;/g, "\"");
    strHtml = strHtml.replace(/&#39;/g, "\'");

    return strHtml;
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}