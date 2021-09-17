function docReady(fn) {
    // see if DOM is already available
    if (document.readyState === "complete" || document.readyState === "interactive") {
        // call on next available tick
        setTimeout(fn, 1);
    } else {
        document.addEventListener("DOMContentLoaded", fn);
    }
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
function onModalSaveButtonClick(chapter, translation) {
    var stext = chapter + ":";
    var text = "";
    var first = true;
    var nextSeq = false;
    var current = 1;
    var count = 0;
    var number = "";
    var $checked = $(".verse-list-item");
    for (var i = 1; i < $checked.length + 1; i++) {
        var $item = $checked[i - 1];
        if ($item != undefined) {
            var $input = $item.previousElementSibling;
            if ($input.checked) {
                var $name = "#verse-hidden-" + i;
                var itemVal = $($name).val();

                number = $($name).attr("data-bs-number");
                if (number != undefined) {
                    if (!first) {
                        if ((current + 1) == i) {
                            // do nothing
                        } else {
                            stext += ",";
                            stext += number;
                        }
                    } else {
                        if (nextSeq) { stext += ","; }
                        stext += number;
                    }
                }

                text += itemVal + " ";
                $input.checked = false;
                first = false;
                current = i;
                count++;
            } else {
                if (!first) {
                    first = true;
                    if (count > 1) {
                        stext += "-";
                        stext += number;
                        count = 0;
                        nextSeq = true;
                    } else if (count == 1) {                        
                        count = 0;
                        nextSeq = true;
                    }
                }
            }
        }
    }

    if (count > 1) {
        stext += "nn";
    }

    stext += " „" + text.trim() + "” " + translation;

    var $temp = $("<input>");
    $("#copyVersesModal").append($temp);
    $temp.val(stext).select();
    document.execCommand("copy");
    $temp.remove();

    $("#close-modal-btn").click();
}

function copyNav() {
    var $nav = $('nav[id^="chapterNav"]:last');
    var num = parseInt($nav.prop("id").match(/\d+/g), 10) + 1;
    var $cloned = $nav.clone().prop('id', 'chapterNav' + num);
    $("#divDownload").prepend($cloned);


    //$(document).on("click", function (event) {
    //    var $trigger = $("span[id^='verseMenuText_']");
    //    if ($trigger !== event.target && !$trigger.has(event.target).length) {
    //        $("ul[id^='verseDropDownMenu_']").hide();
    //    }
    //});
}

function showDopdownMenu(e, dropDownId) {
    $("ul[id^='verseDropDownMenu_']").each(function (index, element) {
        if (element.id != dropDownId) {
            $(element).hide();
        } else {
            $(element).toggle("show");
        }
    });
}
function hideDopdownMenu(dropDownId) {
    var $menu = $('#' + dropDownId);
    $menu.toggle('show');
}