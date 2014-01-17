/// <reference path="jquery-1.8.3.js" />
(function ($, w) {
    $.fn.waterfalllayout = function (options) {
        var opts = $.extend({}, $.fn.waterfall.defaults, options);
        var __this = $(this);
        if (!__this)
            return;
        var __window = $(w);
        if (__this.css("position") != "relative" && __this.css("position") != "absolute")
            __this.css({ position: "relative" });
        var waterfall_columncount = opts.minColumnCount, waterfall_offsetLeft = 0, waterfall_offsetTop = __this.offset() ? __this.offset().top : 0, waterfall_minheight = 0, waterfall_maxheight = 0, loading = true,
        caculateheight = function () {
            var count = __this.find(opts.waterfallItemSelector).size();
            var minheight = -1, maxheight = -1;
            for (var i = 0; i < waterfall_columncount; i++) {
                var item = __this.find(opts.waterfallItemSelector).get(count - 1 - i);
                var height = parseInt($(item).css("top")) + parseInt($(item).outerHeight(false));
                if (minheight == -1 || minheight > height)
                    minheight = height;
                if (maxheight == -1 || maxheight < height)
                    maxheight = height;
            }
            waterfall_minheight = minheight + waterfall_offsetTop;
            waterfall_maxheight = maxheight;
        },
        drawlayout = function () {
            __this.find(opts.waterfallItemSelector).css({ position: "absolute", margin: 0 });
            var waterfall_wrapperwidth = __this.width();
            waterfall_columncount = parseInt((waterfall_wrapperwidth + opts.itemSpan) / (opts.columnWidth + opts.itemSpan));
            if (waterfall_columncount < opts.minColumnCount)
                waterfall_columncount = opts.minColumnCount;
            waterfall_offsetLeft = (waterfall_wrapperwidth - opts.columnWidth * waterfall_columncount - opts.itemSpan * (waterfall_columncount - 1)) / 2;
            var waterfall_items = __this.find(opts.waterfallItemSelector);
            $.each(waterfall_items, function (i) {
                var waterfall_item = waterfall_items.get(i);
                var left = 0, top = 0;
                if (i < waterfall_columncount) {
                    left = i * (opts.columnWidth + opts.itemSpan) + waterfall_offsetLeft;
                    top = 0;
                }
                else {
                    var waterfall_upitem = waterfall_items.get(i - waterfall_columncount);
                    left = (i % waterfall_columncount) * (opts.columnWidth + opts.itemSpan) + waterfall_offsetLeft;
                    top = parseInt($(waterfall_upitem).css("top")) + parseInt($(waterfall_upitem).outerHeight(false)) + opts.itemSpan;
                }
                $(waterfall_item).css({ left: left, top: top });
                $(waterfall_item).animate({ opacity: 1 });
            });
            caculateheight();
            __this.css("height", waterfall_maxheight + "px");
            if (__window.height() > waterfall_minheight && opts.loadmore)
                opts.loadmore(__this, drawlayout);
            loading = false;
        };
        __window.on("resize", function () {
            drawlayout();
        });
        __window.on("scroll", function () {
            var scrolltop = __window.scrollTop() + __window.height();
            if (scrolltop >= waterfall_minheight && !loading) {
                loading = true;
                if (opts.loadmore)
                    opts.loadmore(__this, drawlayout);
            }
        });
        $(document).ready(function () { drawlayout(); });
    };
    $.fn.waterfall.defaults = { waterfallItemSelector: ".waterfallitem", columnWidth: 224, itemSpan: 10, minColumnCount: 1, loadmore: function (waterfallwrapper) { } };
})(jQuery, window);