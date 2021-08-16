// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function docReady(fn) {
    // see if DOM is already available
    if (document.readyState === "complete" || document.readyState === "interactive") {
        // call on next available tick
        setTimeout(fn, 1);
    } else {
        document.addEventListener("DOMContentLoaded", fn);
    }
}

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}
function copyToClipboard(element) {
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val($(element).text()).select();
    document.execCommand("copy");
    $temp.remove();
}
function copyToClipboard2(element, startText) {
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val(startText + ' ' + $(element).text()).select();
    document.execCommand("copy");
    $temp.remove();
}
function copyToClipboard3(fullText) {
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val(fullText).select();
    document.execCommand("copy");
    $temp.remove();
}
function onModalSaveButtonClick(startText) {
    var text = startText;
    var $checked = $(".verse-list-item");
    for (var i = 1; i < $checked.length + 1; i++) {
        var $item = $checked[i - 1];
        if ($item != undefined) {
            var $input = $item.previousElementSibling;
            if ($input.checked) {
                var $name = "#verse-hidden-" + i;
                var itemVal = $($name).val();

                text += itemVal + " ";
                $input.checked = false;
            }
        }
    }

    var $temp = $("<input>");
    $("#copyVersesModal").append($temp);
    $temp.val(text).select();
    document.execCommand("copy");
    $temp.remove();

    $("#close-modal-btn").click();

    //var $myModalEl = $('#copyVersesModal');
    //var $myModal = new bootstrap.Modal($myModalEl, {});
    //$myModal.toggle();
}