// Write your JavaScript code.
$(document).ready(function () {
    if ($(window).width() >= 991) {
        var rellax = new Rellax('.rellax', {
            center: true
        });
        var rellaxHeader = new Rellax('.rellax-header');
        var rellaxText = new Rellax('.rellax-text');
    }
    

});



$(function () {
//Reply form Toggle
    $('.reply-trigger').on('click', function () {
        $parent_box = $(this).closest('.media');
        $parent_box.siblings().find('.reply-textarea').hide();
        $parent_box.find('.reply-textarea').toggle();
    });
//Like Dislike
    $(".like, .dislike").click(function () {
        var obj = { commentid: $(this).data("comment-id"), likeordislike: $(this).data("like-or-dislike") , postid: $(this).data("story-id") };
        $.ajax({
            type: "POST",
            url: "/Stories/UpdateCommentLike",
            data: obj,
            
            success: function (response) {
                if (response.item2 === "Like") {
                    document.getElementById("likeincmt"+response.item1).innerHTML = '<i class="fa fa-thumbs-up fa-lg"></i> &nbsp;' + response.item3;
                    document.getElementById("dislikeincmt" +response.item1).innerHTML = '<i class="fa fa-thumbs-o-down fa-lg"></i> &nbsp;' + response.item4;
                }
                else if (response.item2 === "Dislike") {
                    document.getElementById("likeincmt" +response.item1).innerHTML = '<i class="fa fa-thumbs-o-up fa-lg"></i> &nbsp;' + response.item3;
                    document.getElementById("dislikeincmt" +response.item1).innerHTML = '<i class="fa fa-thumbs-down fa-lg"></i> &nbsp;' + response.item4;
                }
                else {
                    document.getElementById("likeincmt" +response.item1).innerHTML = '<i class="fa fa-thumbs-o-up fa-lg"></i> &nbsp;' + response.item3;
                    document.getElementById("dislikeincmt" +response.item1).innerHTML = '<i class="fa fa-thumbs-o-down fa-lg"></i> &nbsp;' + response.item4;
                }
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    });
//Reply Form
    $("#loadReplyPartialView").click(function () {
        $.ajax({
            url: "Stories/LoadReplyPartialView",
            type: "get",
            //data: $("form").serialize(), //if you need to post Model data, use this
            success: function (result) {
                $("#ReplyFormDiv").html(result);
            }
        });
    });
});












//TimeAgo
(function timeAgo(selector) {

    var templates = {
        prefix: "",
        suffix: " ago",
        seconds: "less than a minute",
        minute: "about a minute",
        minutes: "%d minutes",
        hour: "about an hour",
        hours: "about %d hours",
        day: "a day",
        days: "%d days",
        month: "about a month",
        months: "%d months",
        year: "about a year",
        years: "%d years"
    };
    var template = function (t, n) {
        return templates[t] && templates[t].replace(/%d/i, Math.abs(Math.round(n)));
    };

    var timer = function (time) {
        if (!time) return;
        time = time.replace(/\.\d+/, ""); // remove milliseconds
        time = time.replace(/-/, "/").replace(/-/, "/");
        time = time.replace(/T/, " ").replace(/Z/, " UTC");
        time = time.replace(/([\+\-]\d\d)\:?(\d\d)/, " $1$2"); // -04:00 -> -0400
        time = new Date(time * 1000 || time);

        var now = new Date();
        var seconds = ((now.getTime() - time) * .001) >> 0;
        var minutes = seconds / 60;
        var hours = minutes / 60;
        var days = hours / 24;
        var years = days / 365;

        return templates.prefix + (
            seconds < 45 && template('seconds', seconds) || seconds < 90 && template('minute', 1) || minutes < 45 && template('minutes', minutes) || minutes < 90 && template('hour', 1) || hours < 24 && template('hours', hours) || hours < 42 && template('day', 1) || days < 30 && template('days', days) || days < 45 && template('month', 1) || days < 365 && template('months', days / 30) || years < 1.5 && template('year', 1) || template('years', years)) + templates.suffix;
    };

    var elements = document.getElementsByClassName('timeago');
    for (var i in elements) {
        var $this = elements[i];
        if (typeof $this === 'object') {
            $this.innerHTML = timer($this.getAttribute('title') || $this.getAttribute('datetime'));
        }
    }
    // update time every minute
    setTimeout(timeAgo, 60000);

})();