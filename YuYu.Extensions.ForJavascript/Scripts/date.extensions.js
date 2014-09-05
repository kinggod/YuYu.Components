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
    this.setMilliseconds(this.getMilliseconds() + millionSeconds);
    return this;
};
Date.prototype.addSeconds = function (seconds) {
    this.setSeconds(this.getSeconds() + seconds);
    return this;
};
Date.prototype.addMinutes = function (minutes) {
    this.setMinutes(this.getMinutes() + minutes);
    return this;
};
Date.prototype.addHours = function (hours) {
    this.setHours(this.getHours() + hours);
    return this;
};
Date.prototype.addDays = function (days) {
    this.setDate(this.getDate() + days);
    return this;
};
Date.prototype.addMonths = function (months) {
    this.setMonth(this.getMonth() + months);
    return this;
};
Date.prototype.addYears = function (years) {
    this.setYear(this.getYear() + years);
    return this;
};
Date.fromJSON = function (jsonDate) {
    try {
        return new Date(parseFloat(jsonDate.replace('/Date(', '').replace(')/', '')));
    } catch (exception) {
        return null;
    }
};