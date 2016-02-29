////*$(document).ready(function () {
//    EmployeesPresenter.init();
//});

//var EmployeesPresenter = {
//    TdEmails: null,
//    DefaultPictureUrl: $("table#search-table").data("employees-default-picture"),

//    init: function () {
//        TdEmails = $("td.td-email");
//        var emails = new Array();
//        TdEmails.each(function () {
//            var $this = $(this);
//            var email = $this.find("span.email-text").text();
//            if (emails.indexOf(email) == -1) {
//                emails.push(email);
//            }
//        });

//        emails.sort();

//        var url = $("table#search-table").data("employees-extra-data-url");
//        $.ajax({
//            type: "POST",
//            url: url,
//            data: JSON.stringify({ emails: emails }),
//            contentType: 'application/json; charset=utf-8',
//            dataType: 'json',
//            success: function (response) {
//                if (response.IsSuccess) {
//                    TdEmails.each(function () {
//                        var $this = $(this);
//                        var email = $this.find("span.email-text").text();
//                        var employee = response.Result[email];
//                        if (employee != undefined) {
//                            if (employee.PictureUrl == null || employee.PictureUrl == undefined) {
//                                employee.PictureUrl = EmployeesPresenter.DefaultPictureUrl;
//                            }

//                            EmployeesPresenter.createTooltip($this, employee.Name, employee.Position, employee.PictureUrl, employee.Skype);
//                            //EmployeesPresenter.showMoreHolderData($this, employee.Name, employee.Position, employee.PictureUrl, employee.Skype);
//                        }                        
//                        $this.find("img.loading").remove();
//                    });
//                }
//                else {
//                    $("img.loading").remove();
//                }
//            }
//        });
//    },

//    // fill up the fields when the additional data for the users is loaded
//    showMoreHolderData: function (element, name, position, photoUrl, skype) {
//        // replace is changing the size of the photo, c - means centered
//        element.find('.holder-photo').attr('style', 'background-image: url(' + photoUrl.replace('s220', 's45-c') + ')');
//        element.find('.holder-skype').attr('href', 'skype:' + skype + '?chat');
//        element.find('.holder-location').attr('href', '/Employee/Location?email=' + encodeURIComponent(element.children('div.email-text').text()));
//    },

//    createTooltip: function (element, name, position, photoUrl, skype) {
//        var contentRoot = $("<div/>")
//        $("<img />")
//            .addClass("tooltip-pic")
//            .attr('src', photoUrl)
//            .appendTo(contentRoot);

//        var $userInfoWrapper = $("<div/>")
//                                .addClass("tooptip-info")
//                                .appendTo(contentRoot);

//        $("<div />")
//            .addClass("tooltip-info-item")
//            .text(name)
//            .appendTo($userInfoWrapper);

//        $("<div />")
//            .addClass("tooltip-info-item")
//            .text(position)
//            .appendTo($userInfoWrapper);        
        
//        element.tooltip({
//            show: false,
//            hide: false,
//            content: function () {
//                return contentRoot;
//            },
//            position: {
//                my: "center bottom-20",
//                at: "center top+13",
//                using: function (position, feedback) {
//                    $(this).css(position);
//                    $("<div>")
//                      .addClass("arrow")
//                      .addClass(feedback.vertical)
//                      .addClass(feedback.horizontal)
//                      .appendTo(this);
//                }
//            }
//        });

//        if (0 === element.find('.contact-container').length) {
//            var $contactContainer = $('<div />')
//                            .attr('class', 'contact-container')
//                            .css({ 'text-align': 'left' })
//                            .appendTo(element);

//            var $location = $("<span/>")
//                                .addClass("location")
//                                .css({ "visibility": "hidden" })
//                                .appendTo($contactContainer);

//            $("<img />")
//               .attr("src", "/Content/images/location.jpg")
//               .appendTo($('<a />')
//                .attr('href', '/Employee/Location?email=' + encodeURIComponent(element.children('div.email-text').text()))
//                .attr('target', '_blank')
//                .appendTo($location));

//            var $skypeInfo = $("<span/>")
//                                .addClass("skype-chat")
//                                .css({ "visibility": "hidden" })
//                                .appendTo($contactContainer);

//            $("<img />")
//                .attr("src", "/Content/images/skype-icon.gif")
//                .appendTo($("<a />")
//                            .attr("href", "skype:" + skype + "?chat")
//                            .appendTo($skypeInfo));
//        }

//        element.hover(
//          function () {
//              var $this = $(this);
//              $this.find("span.skype-chat").css("visibility", "visible");
//              $this.find('span.location').css("visibility", "visible");
//          },
//          function () {
//              var $this = $(this);
//              $this.find("span.skype-chat").css("visibility", "hidden");
//              $this.find('span.location').css("visibility", "hidden");
              
//          }
//        );
//    }
//}