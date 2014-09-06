Date.prototype.format = function (format) {
    var z = {
        y: this.getFullYear(),
        M: this.getMonth() + 1,
        d: this.getDate(),
        h: this.getHours(),
        m: this.getMinutes(),
        s: this.getSeconds()
    };
    return format.replace(/(y+|M+|d+|h+|m+|s+)/g, function (v) {
        return ((v.length > 1 ? '0' : '') + eval('z.' + v.slice(-1))).slice(-(v.length > 2 ? v.length : 2));
    });
};
Date.prototype.addMillionSeconds = function (millionSeconds) {
    var date = new Date(this.getTime());
    date.setMilliseconds(date.getMilliseconds() + millionSeconds);
    return date;
};
Date.prototype.addSeconds = function (seconds) {
    var date = new Date(this.getTime());
    date.setSeconds(this.getSeconds() + seconds);
    return date;
};
Date.prototype.addMinutes = function (minutes) {
    var date = new Date(this.getTime());
    date.setMinutes(date.getMinutes() + minutes);
    return date;
};
Date.prototype.addHours = function (hours) {
    var date = new Date(this.getTime());
    date.setHours(date.getHours() + hours);
    return date;
};
Date.prototype.addDays = function (days) {
    var date = new Date(this.getTime());
    date.setDate(date.getDate() + days);
    return date;
};
Date.prototype.addMonths = function (months) {
    var date = new Date(this.getTime());
    date.setMonth(date.getMonth() + months);
    return date;
};
Date.prototype.addYears = function (years) {
    var date = new Date(this.getTime());
    date.setYear(date.getYear() + years);
    return date;
};
Date.fromJSON = function (jsonDate) {
    try {
        return new Date(parseFloat(jsonDate.replace('/Date(', '').replace(')/', '')));
    } catch (exception) {
        return null;
    }
};