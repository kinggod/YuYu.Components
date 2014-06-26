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
Date.fromJSON = function (JSONDate) {
    try {
        return new Date(parseFloat(dateString.replace('/Date(', '').replace(')/', '')));
    } catch (exception) {
        alert(exception.message);
    }
};