/// <reference path="store.js" />
/// <reference path="jquery-1.8.3.js" />
(function ($) {
    $.fn.exceltable = function (options) {
        var opts = $.extend({}, $.fn.exceltable.defaults, options),
            __this = $(this),
            __table = $(this).children(opts.excelTableSelector);
        if (opts.debug)
            $(document.body).append('<div id="debuger" style="position:fixed;z-index:99999;top:10px;right:10px;background-color:#fcc;"></div>');
        if (!($.browser.msie && parseFloat($.browser.version) < 8)) {
            __table.css({ width: '100%' });
            __table.attr({ border: 0, cellpadding: 0, cellspacing: 0 });
        }
        __table.css({ borderCollapse: 'separate', borderSpacing: 0, tableLayout: 'fixed', overflow: 'hidden' });
        if (opts.horizontalResizable)
            __table.find('tr>*').css({ overflow: 'hidden', whiteSpace: 'nowrap', textOverflow: 'ellipsis', wordBreak: 'keep-all' });
        if (opts.debug) {
            __table.css({ backgroundColor: '#f00' });
            __table.find('tr>td').css({ backgroundColor: '#fff' });
        }
        if ($.browser.mozilla || $.safari || ($.browser.msie && parseFloat($.browser.version) < 8)) {//转换元素为block
            var columnStyles = new Array(), totalWidth = $.browser.mozilla ? -3 : 0;
            $.each($(__table.find('tr').get(0)).children(), function (i, element) {
                var innerWidth = $(element).innerWidth(), paddingLeft = parseInt($(element).css('padding-left')), paddingRight = parseInt($(element).css('padding-right')), borderLeftWidth = parseInt($(element).css('border-left-width')), borderRightWidth = parseInt($(element).css('border-right-width'));
                columnStyles[i] = { width: innerWidth - paddingLeft - paddingRight, paddingLeft: paddingLeft, paddingRight: paddingRight, borderLeftWidth: borderLeftWidth, borderRightWidth: borderRightWidth };
                totalWidth += innerWidth + borderLeftWidth + borderRightWidth;
            });
            __table.css({ display: 'block', width: totalWidth });
            $(__table.children()).css({ display: 'block', overflow: 'hidden' });
            $.each(__table.find('tr'), function (i, trElement) {
                var height = $($(trElement).children()).height(), paddingTop = parseInt($($(trElement).children()).css('padding-top')), paddingBottom = parseInt($($(trElement).children()).css('padding-bottom'));
                $(trElement).css({ display: 'block', clear: 'both' });
                $.each($(trElement).children(), function (j, element) {
                    var columnStyle = columnStyles[j];
                    var padding = paddingTop + 'px ' + columnStyle.paddingRight + 'px ' + paddingBottom + 'px ' + columnStyle.paddingLeft + 'px';
                    $(element).css({ display: 'block', float: 'left', width: columnStyle.width - ($.browser.mozilla ? 1 : 0), height: height, margin: 0, padding: padding, borderLeftWidth: columnStyle.borderLeftWidth, borderRightWidth: columnStyle.borderRightWidth, overflow: 'hidden' });
                });
            });
            if ($.browser.msie && __this.css('position') == 'static')
                __this.css({ position: 'relative' });
        }
        if (opts.storage)
            try {
                var excelTableData = $.parseJSON(opts.storage.get(window.location.pathname));// JSON.parse(opts.storage.get(window.location.pathname));
                if (opts.debug) {
                    $("#debuger").html('Debug:<br/>excelTableData: ' + excelTableData);
                    $("#debuger").html($("#debuger").html() + '<br/>excelTableData.tableWidth: ' + excelTableData.tableWidth);
                    $("#debuger").html($("#debuger").html() + '<br/>excelTableData.rowHeights: ' + excelTableData.rowHeights);
                    $("#debuger").html($("#debuger").html() + '<br/>excelTableData.columnWidths: ' + excelTableData.columnWidths);
                }
                if (excelTableData) {
                    if (excelTableData.tableWidth)
                        __table.width(excelTableData.tableWidth);
                    $.each(__table.find('tr'), function (i, trElement) {
                        $.each($(trElement).children(), function (j, element) {
                            var _element = $(element);
                            if (opts.verticalResizable && excelTableData.rowHeights)
                                if (!_element.attr('rowspan'))
                                    $($(trElement).children()).height(excelTableData.rowHeights[i]);
                            if (opts.verticalResizable && excelTableData.columnWidths) {
                                var __width = excelTableData.columnWidths[j];
                                if (__width == 'undefined' || __width < opts.minColumnWidth)
                                    __width = opts.minColumnWidth;
                                _element.width(__width);
                            }
                        });
                    });
                }
            } catch (exception) {
                if (opts.debug)
                    $("#debuger").html($("#debuger").html() + '<br/>' + exception.message);
            }
        $.each(__table.find(opts.fixedHorizontalSelector), function (i, element) {
            var zIndex = parseInt($(element).css('z-index'));
            $(element).css({ position: 'relative', zIndex: (isNaN(zIndex) ? 0 : zIndex) + 1 });
        });
        $.each(__table.find(opts.fixedVerticalSelector), function (i, element) {
            var zIndex = parseInt($(element).css('z-index'));
            $(element).css({ position: 'relative', zIndex: (isNaN(zIndex) ? 0 : zIndex) + 1 });
        });
        $.each(__table.find(opts.fixedAllSelector), function (i, element) {
            var zIndex = parseInt($(element).css('z-index'));
            $(element).css({ position: 'relative', zIndex: (isNaN(zIndex) ? 0 : zIndex) + 1 });
        });
        $(__this).scroll(function () {
            var _top = $(__this).scrollTop(), _left = $(__this).scrollLeft();
            __table.find(opts.fixedAllSelector).css({ top: _top, left: _left });//滚动时固定
            __table.find(opts.fixedVerticalSelector).css({ top: _top });//vertical垂直方向滚动时固定
            __table.find(opts.fixedHorizontalSelector).css({ left: _left });//horizontal水平方向滚动时固定
        });
        if (opts.horizontalResizable || opts.verticalResizable) {//支持调整列宽行高
            var __resizeData = { resizable: null, parent: null, targetIndex: 0, startX: 0, startY: 0, tableWidth: 0, tableHeight: 0, targetWidth: 0, targetHeight: 0 },
                __resizeFunction = {
                    selectstart: function () { return false; },
                    saveExcelTableData: function () {
                        if (opts.storage)
                            try {
                                var excelTableData = { tableWidth: 0, rowHeights: new Array(), columnWidths: new Array() };
                                excelTableData.tableWidth = __table.width();
                                $.each(__table.find('tr'), function (i, trElement) {
                                    $.each($(trElement).children(), function (j, element) {
                                        var _element = $(element);
                                        if (!_element.attr('rowspan'))
                                            excelTableData.rowHeights[i] = _element.height();
                                        if (i == 0)
                                            excelTableData.columnWidths[j] = _element.width();
                                    });
                                });
                                if (opts.debug) {
                                    $("#debuger").html($("#debuger").html() + '<br/>excelTableData: ' + excelTableData);
                                    $("#debuger").html($("#debuger").html() + '<br/>excelTableData.tableWidth: ' + excelTableData.tableWidth);
                                    $("#debuger").html($("#debuger").html() + '<br/>excelTableData.rowHeights: ' + excelTableData.rowHeights);
                                    $("#debuger").html($("#debuger").html() + '<br/>excelTableData.columnWidths: ' + excelTableData.columnWidths);
                                }
                                var excelTableStorageData = '{"tableWidth":' + excelTableData.tableWidth;
                                if (opts.verticalResizable)
                                    excelTableStorageData += ',"rowHeights":[' + excelTableData.rowHeights + ']';
                                if (opts.horizontalResizable)
                                    excelTableStorageData += ',"columnWidths":[' + excelTableData.columnWidths + ']';
                                excelTableStorageData += '}';
                                opts.storage.set(window.location.pathname, excelTableStorageData);//JSON.stringify(excelTableData));
                            } catch (exception) {
                                if (opts.debug)
                                    $("#debuger").html($("#debuger").html() + '<br/>' + exception.message);
                            }
                    },
                    mousemove: function (ev) {
                        var e = ev || window.event;
                        if (opts.debug)
                            $("#debuger").html('Debug:<br/>direction: ' + __resizeData.resizable + '<br/>parent: ' + __resizeData.parent + '<br/>targetIndex: ' + __resizeData.targetIndex + '<br/>startX: ' + __resizeData.startX + '<br/>startY: ' + __resizeData.startY + '<br/>tableWidth: ' + __resizeData.tableWidth + '<br/>tableHeight: ' + __resizeData.tableHeight + '<br/>targetWidth: ' + __resizeData.targetWidth + '<br/>targetHeight: ' + __resizeData.targetHeight + '<br/>screenX: ' + e.screenX + '<br/>screenY: ' + e.screenY);
                        if (__resizeData.resizable == 'horizontal') {
                            var __width = e.screenX - __resizeData.startX;
                            if (__resizeData.targetWidth + __width < opts.minColumnWidth)
                                __width = opts.minColumnWidth - __resizeData.targetWidth;
                            __table.css({ width: __resizeData.tableWidth + __width + 1 });
                            $.each(__table.find('tr'), function (i, element) {
                                $($(element).children().get(__resizeData.targetIndex)).css({ width: __resizeData.targetWidth + __width });
                            });
                        }
                        else if (__resizeData.resizable == 'vertical') {
                            var __height = e.screenY - __resizeData.startY;
                            if (__resizeData.targetHeight + __height < opts.minRowHeight)
                                __height = opts.minRowHeight - __resizeData.targetHeight;
                            $(__resizeData.parent.children().get(__resizeData.targetIndex)).css({ height: __resizeData.targetHeight + __height });
                            $($(__resizeData.parent.children().get(__resizeData.targetIndex)).children()).css({ height: __resizeData.targetHeight + __height });
                        }
                    },
                    mouseup: function () {
                        $(document.body).css('cursor', 'default');
                        $(document).unbind('selectstart', __resizeFunction.selectstart);
                        $(document).unbind('mousemove', __resizeFunction.mousemove);
                        $(document).unbind('mouseup', __resizeFunction.mouseup);
                        __resizeData = { resizable: null, parent: null, targetIndex: 0, startX: 0, startY: 0, tableWidth: 0, tableHeight: 0, targetWidth: 0, targetHeight: 0 };
                        __resizeFunction.saveExcelTableData();
                    }
                },
                __horizontalResizeHandler = $('<span data-id="resizehandler" data-resizable="horizontal"></span>').css(opts.horizontalResizeHandlerStyle),
                __verticalResizeHandler = $('<span data-id="resizehandler" data-resizable="vertical"></span>').css(opts.verticalResizeHandlerStyle);
            if (opts.debug) {
                __horizontalResizeHandler.css({ backgroundColor: '#f00' });
                __verticalResizeHandler.css({ backgroundColor: '#f00' });
            }
            $('[data-id=resizehandler]').live('mousedown', function (ev) {
                var e = ev || window.event, element = $(this).parent();
                __resizeData.resizable = $(this).attr('data-resizable');
                if (__resizeData.resizable == 'horizontal')
                    __resizeData.targetIndex = element.index();
                else if (__resizeData.resizable == 'vertical') {
                    __resizeData.parent = element.parent().parent();
                    __resizeData.targetIndex = element.parent().index();
                }
                __resizeData.startX = e.screenX;
                __resizeData.startY = e.screenY;
                __resizeData.tableWidth = __table.width();
                __resizeData.tableHeight = __table.height();
                __resizeData.targetWidth = element.width();
                __resizeData.targetHeight = element.height();
                $(document.body).css('cursor', $(this).css('cursor'));
                $(document).bind('selectstart', __resizeFunction.selectstart);
                $(document).bind('mousemove', __resizeFunction.mousemove);
                $(document).bind('mouseup', __resizeFunction.mouseup);
            });
            $.each(__table.find(opts.allResizableSelector), function (i, element) {
                $(element).css({ position: 'relative' });
                if (opts.horizontalResizable) {
                    if ($.browser.msie && parseFloat($.browser.version) < 8)
                        __horizontalResizeHandler.css({ height: $(element).innerHeight() - opts.horizontalResizeHandlerStyle.top - opts.horizontalResizeHandlerStyle.bottom });
                    $(element).append(__horizontalResizeHandler.clone());
                }
                if (opts.verticalResizable)
                    $(element).append(__verticalResizeHandler.clone());
            });
            if (opts.horizontalResizable) {
                $.each(__table.find(opts.horizontalResizableSelector), function (i, element) {
                    $(element).css({ position: 'relative' });
                    if ($.browser.msie && parseFloat($.browser.version) < 8)
                        __horizontalResizeHandler.css({ height: $(element).innerHeight() - opts.horizontalResizeHandlerStyle.top - opts.horizontalResizeHandlerStyle.bottom });
                    $(element).append(__horizontalResizeHandler.clone());
                });
            }
            if (opts.verticalResizable) {
                $.each(__table.find(opts.verticalResizableSelector), function (i, element) {
                    $(element).css({ position: 'relative' });
                    $(element).append(__verticalResizeHandler.clone());
                });
            }
        }
        if (opts.editable) {//支持单元格编辑
            var input = $('<input type="text"/>').css({ margin: 0, padding: 0, backgroun: 'none', backgroundColor: 'transparent', border: 'none 0' });
            __table.find(opts.editableSelector).dblclick(function () {
                var ____this = $(this);
                if (____this.attr('data-editing'))
                    return;
                ____this.attr('data-editing', true);
                input.val(____this.text());
                input.css({ width: ____this.width(), height: ____this.height() });
                input.bind('focusout', function () {
                    var value = input.val();
                    ____this.removeAttr('data-editing');
                    ____this.html(value);
                    if (opts.editComplete)
                        opts.editComplete(value, ____this);
                });
                input.bind('keydown', function (ev) {
                    if ((ev || window.event).keyCode == 13) {
                        var value = input.val();
                        ____this.removeAttr('data-editing');
                        ____this.html(value);
                        if (opts.editComplete)
                            opts.editComplete(value, ____this);
                    }
                });
                ____this.html('');
                ____this.append(input);
                input.focus();
            });
        }
    };
    $.fn.exceltable.defaults = {
        debug: false,
        storage: null,
        excelTableSelector: '[data-display=excel]',
        fixedAllSelector: '[data-fixed=all]',
        fixedVerticalSelector: '[data-fixed=vertical]',
        fixedHorizontalSelector: '[data-fixed=horizontal]',
        horizontalResizable: true,
        horizontalResizableSelector: '[data-resizable=horizontal]',
        horizontalResizeHandlerStyle: { position: 'absolute', top: 0, right: 0, bottom: 0, width: 4, height: 'auto', fontSize: 0, lineHeight: 0, cursor: 'ew-resize' },
        minColumnWidth: 10,
        verticalResizable: true,
        verticalResizableSelector: '[data-resizable=vertical]',
        verticalResizeHandlerStyle: { position: 'absolute', right: 0, bottom: 0, left: 0, width: 'auto', height: 4, fontSize: 0, lineHeight: 0, cursor: 'ns-resize' },
        minRowHeight: 0,
        allResizableSelector: '[data-resizable=all]',
        editable: false,
        editableSelector: '[data-editable=true]',
        editComplete: function (value, cell) { }
    };
})(jQuery);