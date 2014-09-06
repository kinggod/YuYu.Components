//表示全局唯一标识符 (GUID)。
function Guid(input) {
    //存放32位数值的数组
    var array = new Array();
    array.initByOther = function () {
        for (var i = 0; i < 32; i++)
            array.push('0');
    };
    array.initByString = function (input) {
        input = input.replace(/\{|\(|\)|\}|\-/g, '');
        input = input.toLowerCase();
        if (input.length != 32 || input.search(/[^0-9,a-f]/i) >= 0)
            array.initByOther();
        else
            for (var i = 0; i < input.length; i++)
                array.push(input[i]);
    };
    if (typeof (input) === 'string')  //如果构造函数的参数为字符串
        array.initByString(input);
    else
        array.initByOther();
    //返回一个值，该值指示 Guid 的两个实例是否表示同一个值。
    this.equals = function (other) {
        if (other && other.isGuid)
            return this.toString() == other.toString();
        return false;
    };
    //Guid对象的标记
    this.isGuid = true;
    /*
    返回 Guid 类的此实例值的 String 表示形式。
    根据所提供的格式说明符，返回此 Guid 实例值的 String 表示形式。
    N  32 位： xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    D  由连字符分隔的 32 位数字 xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
    B  括在大括号中、由连字符分隔的 32 位数字：{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}
    P  括在圆括号中、由连字符分隔的 32 位数字：(xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)
    */
    this.toString = function (format) {
        if (format)
            switch (format) {
                case 'N':
                    return array.toString().replace(/,/g, '');
                case 'D':
                    var str = array.slice(0, 8) + '-' + array.slice(8, 12) + '-' + array.slice(12, 16) + '-' + array.slice(16, 20) + '-' + array.slice(20, 32);
                    str = str.replace(/,/g, '');
                    return str;
                case 'B':
                    var str = this.toString(array, 'D');
                    str = '{' + str + '}';
                    return str;
                case 'P':
                    var str = this.toString(array, 'D');
                    str = '(' + str + ')';
                    return str;
                default:
                    return new Guid();
            }
        else
            return this.toString('D')
    };
};
//Guid 类的默认实例，其值保证均为零。
Guid.Empty = new Guid();
//初始化 Guid 类的一个新实例。
Guid.NewGuid = function () {
    var string = '';
    for (var i = 0; i < 32; i++)
        string += Math.floor(Math.random() * 16.0).toString(16);
    return new Guid(string);
};
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
Number.prototype.plus = function (arg) {
    var r1, r2, m;
    try { r1 = this.toString().split('.')[1].length } catch (e) { r1 = 0 }
    try { r2 = arg.toString().split('.')[1].length } catch (e) { r2 = 0 }
    m = Math.pow(10, Math.max(r1, r2))
    return (this * m + arg * m) / m;
};
Number.prototype.minus = function (arg) {
    var r1, r2, m, n;
    try { r1 = this.toString().split('.')[1].length } catch (e) { r1 = 0 }
    try { r2 = arg.toString().split('.')[1].length } catch (e) { r2 = 0 }
    m = Math.pow(10, Math.max(r1, r2));
    n = (r1 >= r2) ? r1 : r2;
    return Number(((this * m - arg * m) / m).toFixed(n));
};
Number.prototype.multipy = function (arg) {
    var m = 0, s1 = this.toString(), s2 = arg.toString();
    try { m += s1.split('.')[1].length } catch (e) { }
    try { m += s2.split('.')[1].length } catch (e) { }
    return Number(s1.replace('.', '')) * Number(s2.replace('.', '')) / Math.pow(10, m)
};
Number.prototype.divide = function (arg) {
    var t1 = 0, t2 = 0, r1, r2;
    try { t1 = this.toString().split('.')[1].length } catch (e) { }
    try { t2 = arg.toString().split('.')[1].length } catch (e) { }
    r1 = Number(this.toString().replace('.', ''))
    r2 = Number(arg.toString().replace('.', ''))
    return (r1 / r2) * Math.pow(10, t2 - t1);
};
Number.prototype.toCNNumber = function (upperOrLower) {
    var i = parseInt(this), f = '';
    if (i >= 100000000)
        i = 100000000;
    else if (i >= 10000)
        i = 10000;
    else if (i >= 1000)
        i = 1000;
    else if (i >= 100)
        i = 100;
    else if (i >= 10)
        i = 10;
    else if (i < 0)
        i = 0;
    if (i > 100000000)
        i /= 100000000;
    if (upperOrLower) {
        switch (i) {
            case 0:
                f = '零';
                break;
            case 1:
                f = '壹';
                break;
            case 2:
                f = '贰';
                break;
            case 3:
                f = '叁';
                break;
            case 4:
                f = '肆';
                break;
            case 5:
                f = '伍';
                break;
            case 6:
                f = '陆';
                break;
            case 7:
                f = '柒';
                break;
            case 8:
                f = '捌';
                break;
            case 9:
                f = '玖';
                break;
            case 10:
                f = '拾';
                break;
            case 100:
                f = '佰';
                break;
            case 1000:
                f = '仟';
                break;
            case 10000:
                f = '万';
                break;
            case 100000000:
                f = '亿';
                break;
            default:
                break;
        }
    }
    else {
        switch (i) {
            case 0:
                f = '〇';
                break;
            case 1:
                f = '一';
                break;
            case 2:
                f = '二';
                break;
            case 3:
                f = '三';
                break;
            case 4:
                f = '四';
                break;
            case 5:
                f = '五';
                break;
            case 6:
                f = '六';
                break;
            case 7:
                f = '七';
                break;
            case 8:
                f = '八';
                break;
            case 9:
                f = '九';
                break;
            case 10:
                f = '十';
                break;
            case 100:
                f = '百';
                break;
            case 1000:
                f = '千';
                break;
            case 10000:
                f = '万';
                break;
            case 100000000:
                f = '亿';
                break;
            default:
                break;
        }
    }
    return f;
};
Number.prototype.toCN = function (currency) {
    var num = 0,//整数部分
    dec,//小数部分
    output0 = false,//输出0
    s = '',
    unit = function (i) {
        var s;
        switch (i) {
            case 0:
                s = '角';
                break;
            case 1:
                s = '分';
                break;
            case 2:
                s = '厘';
                break;
            default:
                break;
        }
        return s;
    };
    num = parseInt(this);
    dec = this.minus(num);
    if (num < 0)
        s += '负';
    if (num > 1) {
        var l = num;
        for (var ii = 4; ii >= 0; ii--) {
            var level = Math.pow(10000, ii);
            if (num >= level) {
                l = num % level;
                num = num.divide(level);
                if (num > 19) {
                    var j = 1000;
                    while (num % (j * 10) >= 1) {
                        var tmp = parseInt(num / j);
                        if (tmp != 0) {
                            s += (tmp).toCNNumber(currency);
                            if (j > 1)
                                s += j.toCNNumber(currency);
                            output0 = true;
                        }
                        else if (output0) {
                            s += (0).toCNNumber(currency);
                            output0 = false;
                        }
                        if (j == 1)
                            break;
                        num %= j;
                        j = j.divide(10);
                    }
                }
                else if (num >= 10) {
                    s += (10).toCNNumber(currency);
                    if (num % 10 > 0) {
                        s += (num % 10).toCNNumber(currency);
                        output0 = true;
                    }
                }
                else
                    s += num.toCNNumber(currency);
                if (level > 1)
                    s += level.toCNNumber(currency);
            }
            num = l;
        }
    }
    else
        s += num.toCNNumber(currency);
    if (dec > 0) {
        //处理小数部分
        s += currency ? '圆' : '点';
        if (currency) {
            if (output0) {
                s += (0).toCNNumber(currency);
                output0 = false;
            }
            var i = 0;
            do {
                dec = dec.multipy(10);
                var dd = parseInt(dec);
                dec = dec.minus(dd);
                if (dd > 0) {
                    s += dd.toCNNumber(currency);
                    s += unit(i);
                    output0 = true;
                }
                else if (dec > 0 && output0) {
                    s += (0).toCNNumber(currency);
                    output0 = false;
                }
                i++;
                if (i > 2)
                    break;
            } while (dec > 0);
        }
        else {
            do {
                dec = dec.multipy(10);
                var dd = parseInt(dec);
                dec = dec.minus(dd);
                s += dd.toCNNumber(currency);
            } while (dec > 0)
        }
    }
    else if (currency)
        s += '圆整';
    return s;
};