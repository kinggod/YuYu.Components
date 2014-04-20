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