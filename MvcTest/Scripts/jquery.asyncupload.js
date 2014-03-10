(function ($, __window) {
    $.fn.asyncupload = function (uploadUrl, options) {
        var __this = $(this);
        __this.parent().css({ position: 'relative' });
        var opts = $.extend({}, $.fn.asyncupload.defaults, options), id = "YuYu_FileField_" + Math.random(), html5Enable = false,
            top = __this.position().top, left = __this.position().left, width = parseInt(__this.css('width')), height = parseInt(__this.css('height')),
            marginTop = __this.css('margin-top'), marginRight = __this.css('margin-right'), marginBottom = __this.css('margin-bottom'), marginLeft = __this.css('margin-left'),
            paddingTop = __this.css('padding-top'), paddingRight = __this.css('padding-right'), paddingBottom = __this.css('padding-bottom'), paddingLeft = __this.css('padding-left'),
            borderTopStyle = __this.css('border-top-style'), borderRightStyle = __this.css('border-right-style'), borderBottomStyle = __this.css('border-bottom-style'), borderLeftStyle = __this.css('border-left-style'),
            borderTopWidth = __this.css('border-top-width'), borderRightWidth = __this.css('border-right-width'), borderBottomWidth = __this.css('border-bottom-width'), borderLeftWidth = __this.css('border-left-width'),
            borderTopColor = __this.css('border-top-color'), borderRightColor = __this.css('border-right-color'), borderBottomColor = __this.css('border-bottom-color'), borderLeftColor = __this.css('border-left-color');
        __this.css({ opacity: 0 });
        try {
            html5Enable = new FormData() != 'undefined';
        } catch (e) {
            html5Enable = false;
        }
        var yuyu_FileField = html5Enable ? $('<input type="file" id="' + id + '" />').css({ position: 'absolute', top: '-1px', right: '-1px', width: '1000px', height: '500px', margin: '0', padding: '0', opacity: '0', filter: 'alpha(opacity=0)' }) : null,
            yuyu_FileInfo = __this.val() == '' ? $('<span>未选择文件</span>').css({ display: 'block', marginRight: '60px', color: '#999', overflow: 'hidden', whiteSpace: 'nowrap', textOverflow: 'ellipsis' }) : $('<span>已上传文件</span>').css({ display: 'block', marginRight: '60px', color: '#393', overflow: 'hidden', whiteSpace: 'nowrap', textOverflow: 'ellipsis' }),
            yuyu_UploadProgress = $('<span></span>').css({ display: 'block', float: 'right', width: '60px', color: '#090', textAlign: 'right', overflow: 'hidden', whiteSpace: 'nowrap', textOverflow: 'ellipsis' }),
            yuyu_UploadProgressBar = $('<div></div>').css({ width: '0', height: '0', borderBottom: (height - parseInt(height * 0.9)) + 'px solid #00f', fontSize: '0', lineHeight: '0', overflow: 'hidden' });
        __this.after($('<div></div>').css({ position: 'absolute', top: top, left: left, width: width + 'px', height: height + 'px', marginTop: marginTop, marginRight: marginRight, marginBottom: marginBottom, marginLeft: marginLeft, paddingTop: paddingTop, paddingRight: paddingRight, paddingBottom: paddingBottom, paddingLeft: paddingLeft, borderTopStyle: borderTopStyle, borderRightStyle: borderRightStyle, borderBottomStyle: borderBottomStyle, borderLeftStyle: borderLeftStyle, borderTopWidth: borderTopWidth, borderRightWidth: borderRightWidth, borderBottomWidth: borderBottomWidth, borderLeftWidth: borderLeftWidth, borderTopColor: borderTopColor, borderRightColor: borderRightColor, borderBottomColor: borderBottomColor, borderLeftColor: borderLeftColor, borderSpacing: '1px', borderCollapse: 'collapse', fontSize: '12px' }).append($('<div></div>').css({ float: 'right', width: '70px', height: height + 'px', overflow: 'hidden', backgroundColor: '#ccc' }).append($(html5Enable ? '<div></div>' : '<div id="' + id + '"></div>').css({ position: 'relative', width: '70px', height: height + 'px', lineHeight: height + 'px', textAlign: 'center', fontSize: '12px', backgroundColor: '#ccc', overflow: 'hidden' }).append(html5Enable ? $('<span>选择文件</span>') : null).append(yuyu_FileField))).append($('<div></div>').css({ marginRight: '72px' }).append($('<div></div>').css({ height: parseInt(height * 0.9) + 'px', lineHeight: parseInt(height * 0.9) + 'px' }).append(yuyu_UploadProgress).append(yuyu_FileInfo)).append($('<div></div>').css({ width: '100%', margin: '0', padding: '0', backgroundColor: '#ccc' }).append(yuyu_UploadProgressBar))));
        if (html5Enable) {
            yuyu_FileField.change(function () {
                var file = document.getElementById(id).files[0];
                if (file) {
                    var fileSize = file.size > 1024 * 1024 ? (Math.round(file.size * 100 / (1024 * 1024)) / 100).toString() + 'MB' : (Math.round(file.size * 100 / 1024) / 100).toString() + 'KB';
                    yuyu_FileInfo.text(file.name + "(" + fileSize + ")");
                    if (opts.fileExtAllowd != 'undefined') {
                        var dotIndex = file.name.lastIndexOf('.');
                        var fileExt = file.name.substring(dotIndex > -1 ? dotIndex : 0);
                        if (opts.fileExtAllowd != '*.*' && opts.fileExtAllowd.indexOf(fileExt) < 0) {
                            yuyu_UploadProgress.text("格式错误！");
                            return;
                        }
                    }
                    if (file.size > opts.fileSizeLimit) {
                        yuyu_UploadProgress.text("文件太大！");
                        return;
                    }
                    var fd = new FormData(), xhr = new XMLHttpRequest();
                    fd.append("File", file);
                    xhr.upload.addEventListener("progress", function (evt) {
                        yuyu_UploadProgress.text(evt.lengthComputable ? Math.round(evt.loaded * 100 / evt.total) + "%" : "正在上传…");
                        var w = parseFloat(yuyu_UploadProgressBar.parent().css("width"));
                        yuyu_UploadProgressBar.css("width", Math.round(evt.loaded * w / evt.total) + "px");
                    }, false);
                    xhr.addEventListener("load", function (evt) {
                        __this.val(evt.target.responseText);
                        yuyu_UploadProgress.text("上传成功！");
                        yuyu_UploadProgressBar.css("width", "100%");
                        if (opts.callback)
                            opts.callback(evt.target.responseText);
                    }, false);
                    xhr.addEventListener("error", function () {
                        yuyu_UploadProgress.text("上传失败！");
                    }, false);
                    xhr.addEventListener("abort", function () {
                        yuyu_UploadProgress.text("上传取消！");
                    }, false);
                    if (uploadUrl && uploadUrl != '') {
                        xhr.open("POST", uploadUrl);
                        xhr.send(fd);
                    }
                }
            });
        }
        else {
            __this.after($('<script type="text/javascript" src="/swfupload/swfupload.js"></script>'));
            __window.onload = function () {
                var swfupload = new SWFUpload({
                    flash_url: "../swfUpload/swfupload.swf",
                    flash9_url: "../swfUpload/swfupload_fp9.swf",
                    upload_url: uploadUrl,
                    //post_params: { "PHPSESSID": "" },
                    file_size_limit: opts.fileSizeLimit + "B",
                    file_types: opts.fileExtAllowd == 'undefined' ? "*.*" : opts.fileExtAllowd,
                    //file_types_description: "All Files",
                    file_upload_limit: 1,
                    file_queue_limit: 1,
                    //custom_settings: {
                    //    progressTarget: "fsUploadProgress",
                    //    cancelButtonId: "btnCancel"
                    //},
                    debug: false,
                    //button_image_url: "../Images/swfupload.png",
                    button_window_mode: 'transparent',
                    button_width: '70',
                    button_height: height,
                    button_placeholder_id: id,
                    button_text: '<span class="btn">选择文件</span>',
                    button_text_style: ".btn {display:block;text-align:center;font-size:12px;}",
                    button_text_top_padding: (height - ($.browser.msie || $.browser.opera ? 20 : 14)) / 2,
                    file_dialog_complete_handler: function () {
                        if (uploadUrl && uploadUrl != '')
                            this.startUpload();
                    },
                    upload_start_handler: function (file) {
                        if (file) {
                            var fileSize = file.size > 1024 * 1024 ? (Math.round(file.size * 100 / (1024 * 1024)) / 100).toString() + 'MB' : (Math.round(file.size * 100 / 1024) / 100).toString() + 'KB';
                            yuyu_FileInfo.text(file.name + "(" + file.size + ")");
                        }
                    },
                    upload_progress_handler: function (file, bytesLoaded) {
                        var progress = bytesLoaded / file.size;
                        if (progress >= 1)
                            progress = 1;
                        yuyu_UploadProgress.text(Math.round(progress * 100) + "%");
                        var w = parseFloat(yuyu_UploadProgressBar.parent().css("width"));
                        yuyu_UploadProgressBar.css("width", Math.round(progress * w) + "px");
                    },
                    upload_error_handler: function () {
                        yuyu_UploadProgress.text("上传失败！");
                    },
                    upload_success_handler: function (file, serverData) {
                        __this.val(serverData);
                        if (opts.callback)
                            opts.callback(serverData);
                    },
                    upload_complete_handler: function () {
                        yuyu_UploadProgress.text("上传成功！");
                        yuyu_UploadProgressBar.css("width", "100%");
                    }
                });
            }
        }
    };
    $.fn.asyncupload.defaults = { fileSizeLimit: 4194304, fileExtAllowd: '*.jpg;*.jpeg;*.png;*.gif;*.bmp' };
})(jQuery, Window);