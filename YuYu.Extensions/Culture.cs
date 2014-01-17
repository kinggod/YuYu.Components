using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 区域性
    /// </summary>
    public enum Culture
    {
        /// <summary>
        /// 固定区域性
        /// </summary>
        Empty = 0x007F,

        /// <summary>
        /// 南非荷兰语
        /// </summary>
        af = 0x0036,

        /// <summary>
        /// 南非荷兰语（南非）
        /// </summary>
        af_ZA = 0x0436,

        /// <summary>
        /// 阿尔巴尼亚语
        /// </summary>
        sq = 0x001C,

        /// <summary>
        /// 阿尔巴尼亚语（阿尔巴尼亚）
        /// </summary>
        sq_AL = 0x041C,

        /// <summary>
        /// 阿拉伯语
        /// </summary>
        ar = 0x0001,

        /// <summary>
        /// 阿拉伯语（阿尔及利亚）
        /// </summary>
        ar_DZ = 0x1401,

        /// <summary>
        /// 阿拉伯语（巴林）
        /// </summary>
        ar_BH = 0x3C01,

        /// <summary>
        /// 阿拉伯语（埃及）
        /// </summary>
        ar_EG = 0x0C01,

        /// <summary>
        /// 阿拉伯语（伊拉克）
        /// </summary>
        ar_IQ = 0x0801,

        /// <summary>
        /// 阿拉伯语（约旦）
        /// </summary>
        ar_JO = 0x2C01,

        /// <summary>
        /// 阿拉伯语（科威特）
        /// </summary>
        ar_KW = 0x3401,

        /// <summary>
        /// 阿拉伯语（黎巴嫩）
        /// </summary>
        ar_LB = 0x3001,

        /// <summary>
        /// 阿拉伯语（利比亚）
        /// </summary>
        ar_LY = 0x1001,

        /// <summary>
        /// 阿拉伯语（摩洛哥）
        /// </summary>
        ar_MA = 0x1801,

        /// <summary>
        /// 阿拉伯语（阿曼）
        /// </summary>
        ar_OM = 0x2001,

        /// <summary>
        /// 阿拉伯语（卡塔尔）
        /// </summary>
        ar_QA = 0x4001,

        /// <summary>
        /// 阿拉伯语（沙特阿拉伯）
        /// </summary>
        ar_SA = 0x0401,

        /// <summary>
        /// 阿拉伯语（叙利亚）
        /// </summary>
        ar_SY = 0x2801,

        /// <summary>
        /// 阿拉伯语（突尼斯）
        /// </summary>
        ar_TN = 0x1C01,

        /// <summary>
        /// 阿拉伯语（阿联酋）
        /// </summary>
        ar_AE = 0x3801,

        /// <summary>
        /// 阿拉伯语（也门）
        /// </summary>
        ar_YE = 0x2401,

        /// <summary>
        /// 亚美尼亚语
        /// </summary>
        hy = 0x002B,

        /// <summary>
        /// 亚美尼亚语（亚美尼亚）
        /// </summary>
        hy_AM = 0x042B,

        /// <summary>
        /// 阿泽里语
        /// </summary>
        az = 0x002C,

        /// <summary>
        /// 阿泽里语（阿塞拜疆，西里尔语）
        /// </summary>
        az_Cyrl_AZ = 0x082C,

        /// <summary>
        /// 阿泽里语（阿塞拜疆，拉丁语）
        /// </summary>
        az_Latn_AZ = 0x042C,

        /// <summary>
        /// 巴斯克语
        /// </summary>
        eu = 0x002D,

        /// <summary>
        /// 巴斯克语（巴斯克地区）
        /// </summary>
        eu_ES = 0x042D,

        /// <summary>
        /// 白俄罗斯语
        /// </summary>
        be = 0x0023,

        /// <summary>
        /// 白俄罗斯语（白俄罗斯）
        /// </summary>
        be_BY = 0x0423,

        /// <summary>
        /// 保加利亚语
        /// </summary>
        bg = 0x0002,

        /// <summary>
        /// 保加利亚语（保加利亚）
        /// </summary>
        bg_BG = 0x0402,

        /// <summary>
        /// 加泰罗尼亚语
        /// </summary>
        ca = 0x0003,

        /// <summary>
        /// 加泰罗尼亚语（加泰罗尼亚地区）
        /// </summary>
        ca_ES = 0x0403,

        /// <summary>
        /// 中文（香港特别行政区，中国）
        /// </summary>
        zh_HK = 0x0C04,

        /// <summary>
        /// 中文（澳门特别行政区）
        /// </summary>
        zh_MO = 0x1404,

        /// <summary>
        /// 中文（中国）
        /// </summary>
        zh_CN = 0x0804,

        /// <summary>
        /// 中文（简体）
        /// </summary>
        zh_Hans = 0x0004,

        /// <summary>
        /// 中文（新加坡）
        /// </summary>
        zh_SG = 0x1004,

        /// <summary>
        /// 中文（台湾）
        /// </summary>
        zh_TW = 0x0404,

        /// <summary>
        /// 中文（繁体）
        /// </summary>
        zh_Hant = 0x7C04,

        /// <summary>
        /// 克罗地亚语
        /// </summary>
        hr = 0x001A,

        /// <summary>
        /// 克罗地亚语（克罗地亚）
        /// </summary>
        hr_HR = 0x041A,

        /// <summary>
        /// 捷克语
        /// </summary>
        cs = 0x0005,

        /// <summary>
        /// 捷克语（捷克共和国）
        /// </summary>
        cs_CZ = 0x0405,

        /// <summary>
        /// 丹麦语
        /// </summary>
        da = 0x0006,

        /// <summary>
        /// 丹麦语（丹麦）
        /// </summary>
        da_DK = 0x0406,

        /// <summary>
        /// 迪维希语
        /// </summary>
        dv = 0x0065,

        /// <summary>
        /// 迪维希语（马尔代夫）
        /// </summary>
        dv_MV = 0x0465,

        /// <summary>
        /// 荷兰语
        /// </summary>
        nl = 0x0013,

        /// <summary>
        /// 荷兰语（比利时）
        /// </summary>
        nl_BE = 0x0813,

        /// <summary>
        /// 荷兰语（荷兰）
        /// </summary>
        nl_NL = 0x0413,

        /// <summary>
        /// 英语
        /// </summary>
        en = 0x0009,

        /// <summary>
        /// 英语（澳大利亚）
        /// </summary>
        en_AU = 0x0C09,

        /// <summary>
        /// 英语（伯利兹）
        /// </summary>
        en_BZ = 0x2809,

        /// <summary>
        /// 英语（加拿大）
        /// </summary>
        en_CA = 0x1009,

        /// <summary>
        /// 英语（加勒比）
        /// </summary>
        en_029 = 0x2409,

        /// <summary>
        /// 英语（爱尔兰）
        /// </summary>
        en_IE = 0x1809,

        /// <summary>
        /// 英语（牙买加）
        /// </summary>
        en_JM = 0x2009,

        /// <summary>
        /// 英语（新西兰）
        /// </summary>
        en_NZ = 0x1409,

        /// <summary>
        /// 英语（菲律宾）
        /// </summary>
        en_PH = 0x3409,

        /// <summary>
        /// 英语（南非）
        /// </summary>
        en_ZA = 0x1C09,

        /// <summary>
        /// 英语（特立尼达和多巴哥）
        /// </summary>
        en_TT = 0x2C09,

        /// <summary>
        /// 英语（英国）
        /// </summary>
        en_GB = 0x0809,

        /// <summary>
        /// 英语（美国）
        /// </summary>
        en_US = 0x0409,

        /// <summary>
        /// 英语（津巴布韦）
        /// </summary>
        en_ZW = 0x3009,

        /// <summary>
        /// 爱沙尼亚语
        /// </summary>
        et = 0x0025,

        /// <summary>
        /// 爱沙尼亚语（爱沙尼亚）
        /// </summary>
        et_EE = 0x0425,

        /// <summary>
        /// 法罗语
        /// </summary>
        fo = 0x0038,

        /// <summary>
        /// 法罗语（法罗群岛）
        /// </summary>
        fo_FO = 0x0438,

        /// <summary>
        /// 波斯语
        /// </summary>
        fa = 0x0029,

        /// <summary>
        /// 波斯语（伊朗）
        /// </summary>
        fa_IR = 0x0429,

        /// <summary>
        /// 芬兰语
        /// </summary>
        fi = 0x000B,

        /// <summary>
        /// 芬兰语（芬兰）
        /// </summary>
        fi_FI = 0x040B,

        /// <summary>
        /// 法语
        /// </summary>

        fr = 0x000C,

        /// <summary>
        /// 法语（比利时）
        /// </summary>
        fr_BE = 0x080C,

        /// <summary>
        /// 法语（加拿大）
        /// </summary>
        fr_CA = 0x0C0C,

        /// <summary>
        /// 法语（法国）
        /// </summary>
        fr_FR = 0x040C,

        /// <summary>
        /// 法语（卢森堡）
        /// </summary>
        fr_LU = 0x140C,

        /// <summary>
        /// 法语（摩纳哥）
        /// </summary>
        fr_MC = 0x180C,

        /// <summary>
        /// 法语（瑞士）
        /// </summary>
        fr_CH = 0x100C,

        /// <summary>
        /// 加利西亚语
        /// </summary>
        gl = 0x0056,

        /// <summary>
        /// 加利西亚语（西班牙）
        /// </summary>
        gl_ES = 0x0456,

        /// <summary>
        /// 格鲁吉亚语
        /// </summary>
        ka = 0x0037,

        /// <summary>
        /// 格鲁吉亚语（格鲁吉亚）
        /// </summary>
        ka_GE = 0x0437,

        /// <summary>
        /// 德语
        /// </summary>
        de = 0x0007,

        /// <summary>
        /// 德语（奥地利）
        /// </summary>
        de_AT = 0x0C07,

        /// <summary>
        /// 德语（德国）
        /// </summary>
        de_DE = 0x0407,

        /// <summary>
        /// 德语（列支敦士登）
        /// </summary>
        de_LI = 0x1407,

        /// <summary>
        /// 德语（卢森堡）
        /// </summary>
        de_LU = 0x1007,

        /// <summary>
        /// 德语（瑞士）
        /// </summary>
        de_CH = 0x0807,

        /// <summary>
        /// 希腊语
        /// </summary>
        el = 0x0008,

        /// <summary>
        /// 希腊语（希腊）
        /// </summary>
        el_GR = 0x0408,

        /// <summary>
        /// 古吉拉特语
        /// </summary>
        gu = 0x0047,

        /// <summary>
        /// 古吉拉特语（印度）
        /// </summary>
        gu_IN = 0x0447,

        /// <summary>
        /// 希伯来语
        /// </summary>
        he = 0x000D,

        /// <summary>
        /// 希伯来语（以色列）
        /// </summary>
        he_IL = 0x040D,

        /// <summary>
        /// 印地语
        /// </summary>
        hi = 0x0039,

        /// <summary>
        /// 印地语（印度）
        /// </summary>
        hi_IN = 0x0439,

        /// <summary>
        /// 匈牙利语
        /// </summary>
        hu = 0x000E,

        /// <summary>
        /// 匈牙利语（匈牙利）
        /// </summary>
        hu_HU = 0x040E,

        /// <summary>
        /// 冰岛语
        /// </summary>
        @is = 0x000F,

        /// <summary>
        /// 冰岛语（冰岛）
        /// </summary>
        is_IS = 0x040F,

        /// <summary>
        /// 印度尼西亚语
        /// </summary>
        id = 0x0021,

        /// <summary>
        /// 印度尼西亚语（印度尼西亚）
        /// </summary>
        id_ID = 0x0421,

        /// <summary>
        /// 意大利语
        /// </summary>
        it = 0x0010,

        /// <summary>
        /// 意大利语（意大利）
        /// </summary>
        it_IT = 0x0410,

        /// <summary>
        /// 意大利语（瑞士）
        /// </summary>
        it_CH = 0x0810,

        /// <summary>
        /// 日语
        /// </summary>
        ja = 0x0011,

        /// <summary>
        /// 日语（日本）
        /// </summary>
        ja_JP = 0x0411,

        /// <summary>
        /// 卡纳达语
        /// </summary>
        kn = 0x004B,

        /// <summary>
        /// 卡纳达语（印度）
        /// </summary>
        kn_IN = 0x044B,

        /// <summary>
        /// 哈萨克语
        /// </summary>
        kk = 0x003F,

        /// <summary>
        /// 哈萨克语（哈萨克斯坦）
        /// </summary>
        kk_KZ = 0x043F,

        /// <summary>
        /// 贡根语
        /// </summary>
        kok = 0x0057,

        /// <summary>
        /// 贡根语（印度）
        /// </summary>
        kok_IN = 0x0457,

        /// <summary>
        /// 朝鲜语
        /// </summary>
        ko = 0x0012,

        /// <summary>
        /// 朝鲜语（韩国）
        /// </summary>
        ko_KR = 0x0412,

        /// <summary>
        /// 吉尔吉斯语
        /// </summary>
        ky = 0x0040,

        /// <summary>
        /// 吉尔吉斯语（吉尔吉斯坦）
        /// </summary>
        ky_KG = 0x0440,

        /// <summary>
        /// 拉脱维亚语
        /// </summary>
        lv = 0x0026,

        /// <summary>
        /// 拉脱维亚语（拉脱维亚）
        /// </summary>
        lv_LV = 0x0426,

        /// <summary>
        /// 立陶宛语
        /// </summary>
        lt = 0x0027,

        /// <summary>
        /// 立陶宛语（立陶宛）
        /// </summary>
        lt_LT = 0x0427,

        /// <summary>
        /// 马其顿语
        /// </summary>
        mk = 0x002F,

        /// <summary>
        /// 马其顿语（马其顿，FYROM）
        /// </summary>
        mk_MK = 0x042F,

        /// <summary>
        /// 马来语
        /// </summary>
        ms = 0x003E,

        /// <summary>
        /// 马来语（文莱达鲁萨兰）
        /// </summary>
        ms_BN = 0x083E,

        /// <summary>
        /// 马来语（马来西亚）
        /// </summary>
        ms_MY = 0x043E,

        /// <summary>
        /// 马拉地语
        /// </summary>
        mr = 0x004E,

        /// <summary>
        /// 马拉地语（印度）
        /// </summary>
        mr_IN = 0x044E,

        /// <summary>
        /// 蒙古语
        /// </summary>
        mn = 0x0050,

        /// <summary>
        /// 蒙古语（蒙古）
        /// </summary>
        mn_MN = 0x0450,

        /// <summary>
        /// 挪威语
        /// </summary>
        no = 0x0014,

        /// <summary>
        /// 挪威语（伯克梅尔，挪威）
        /// </summary>
        nb_NO = 0x0414,

        /// <summary>
        /// 挪威语（尼诺斯克，挪威）
        /// </summary>
        nn_NO = 0x0814,

        /// <summary>
        /// 波兰语
        /// </summary>
        pl = 0x0015,

        /// <summary>
        /// 波兰语（波兰）
        /// </summary>
        pl_PL = 0x0415,

        /// <summary>
        /// 葡萄牙语
        /// </summary>
        pt = 0x0016,

        /// <summary>
        /// 葡萄牙语（巴西）
        /// </summary>
        pt_BR = 0x0416,

        /// <summary>
        /// 葡萄牙语（葡萄牙）
        /// </summary>
        pt_PT = 0x0816,

        /// <summary>
        /// 旁遮普语
        /// </summary>
        pa = 0x0046,

        /// <summary>
        /// 旁遮普语（印度）
        /// </summary>
        pa_IN = 0x0446,

        /// <summary>
        /// 罗马尼亚语
        /// </summary>
        ro = 0x0018,

        /// <summary>
        /// 罗马尼亚语（罗马尼亚）
        /// </summary>
        ro_RO = 0x0418,

        /// <summary>
        /// 俄语
        /// </summary>
        ru = 0x0019,

        /// <summary>
        /// 俄语（俄罗斯）
        /// </summary>
        ru_RU = 0x0419,

        /// <summary>
        /// 梵语
        /// </summary>
        sa = 0x004F,

        /// <summary>
        /// 梵语（印度）
        /// </summary>
        sa_IN = 0x044F,

        /// <summary>
        /// 塞尔维亚语（塞尔维亚，西里尔语）
        /// </summary>
        sr_Cyrl_CS = 0x0C1A,

        /// <summary>
        /// 塞尔维亚语（塞尔维亚，拉丁语）
        /// </summary>
        sr_Latn_CS = 0x081A,

        /// <summary>
        /// 斯洛伐克语
        /// </summary>
        sk = 0x001B,

        /// <summary>
        /// 斯洛伐克语（斯洛伐克）
        /// </summary>
        sk_SK = 0x041B,

        /// <summary>
        /// 斯洛文尼亚语
        /// </summary>
        sl = 0x0024,

        /// <summary>
        /// 斯洛文尼亚语（斯洛文尼亚）
        /// </summary>
        sl_SI = 0x0424,

        /// <summary>
        /// 西班牙语
        /// </summary>
        es = 0x000A,

        /// <summary>
        /// 西班牙语（阿根廷）
        /// </summary>
        es_AR = 0x2C0A,

        /// <summary>
        /// 西班牙语（玻利维亚）
        /// </summary>
        es_BO = 0x400A,

        /// <summary>
        /// 西班牙语（智利）
        /// </summary>
        es_CL = 0x340A,

        /// <summary>
        /// 西班牙语（哥伦比亚）
        /// </summary>
        es_CO = 0x240A,

        /// <summary>
        /// 西班牙语（哥斯达黎加）
        /// </summary>
        es_CR = 0x140A,

        /// <summary>
        /// 西班牙语（多米尼加共和国）
        /// </summary>
        es_DO = 0x1C0A,

        /// <summary>
        /// 西班牙语（厄瓜多尔）
        /// </summary>
        es_EC = 0x300A,

        /// <summary>
        /// 西班牙语（萨尔瓦多）
        /// </summary>
        es_SV = 0x440A,

        /// <summary>
        /// 西班牙语（危地马拉）
        /// </summary>
        es_GT = 0x100A,

        /// <summary>
        /// 西班牙语（洪都拉斯）
        /// </summary>
        es_HN = 0x480A,

        /// <summary>
        /// 西班牙语（墨西哥）
        /// </summary>
        es_MX = 0x080A,

        /// <summary>
        /// 西班牙语（尼加拉瓜）
        /// </summary>
        es_NI = 0x4C0A,

        /// <summary>
        /// 西班牙语（巴拿马）
        /// </summary>
        es_PA = 0x180A,

        /// <summary>
        /// 西班牙语（巴拉圭）
        /// </summary>
        es_PY = 0x3C0A,

        /// <summary>
        /// 西班牙语（秘鲁）
        /// </summary>
        es_PE = 0x280A,

        /// <summary>
        /// 西班牙语（波多黎各）
        /// </summary>
        es_PR = 0x500A,

        /// <summary>
        /// 西班牙语（西班牙）
        /// </summary>
        es_ES = 0x0C0A,

        /// <summary>
        /// 西班牙语（西班牙，传统排序）
        /// </summary>
        es_ES_tradnl = 0x040A,

        /// <summary>
        /// 西班牙语（乌拉圭）
        /// </summary>
        es_UY = 0x380A,

        /// <summary>
        /// 西班牙语（委内瑞拉）
        /// </summary>
        es_VE = 0x200A,

        /// <summary>
        /// 斯瓦希里语
        /// </summary>
        sw = 0x0041,

        /// <summary>
        /// 斯瓦希里语（肯尼亚）
        /// </summary>
        sw_KE = 0x0441,

        /// <summary>
        /// 瑞典语
        /// </summary>
        sv = 0x001D,

        /// <summary>
        /// 瑞典语（芬兰）
        /// </summary>
        sv_FI = 0x081D,

        /// <summary>
        /// 瑞典语（瑞典）
        /// </summary>
        sv_SE = 0x041D,

        /// <summary>
        /// 叙利亚语
        /// </summary>
        syr = 0x005A,

        /// <summary>
        /// 叙利亚语（叙利亚）
        /// </summary>
        syr_SY = 0x045A,

        /// <summary>
        /// 泰米尔语
        /// </summary>
        ta = 0x0049,

        /// <summary>
        /// 泰米尔语（印度）
        /// </summary>
        ta_IN = 0x0449,

        /// <summary>
        /// 鞑靼语
        /// </summary>
        tt = 0x0044,

        /// <summary>
        /// 鞑靼语（俄罗斯）
        /// </summary>
        tt_RU = 0x0444,

        /// <summary>
        /// 泰卢固语
        /// </summary>
        te = 0x004A,

        /// <summary>
        /// 泰卢固语（印度）
        /// </summary>
        te_IN = 0x044A,

        /// <summary>
        /// 泰语
        /// </summary>
        th = 0x001E,

        /// <summary>
        /// 泰语（泰国）
        /// </summary>
        th_TH = 0x041E,

        /// <summary>
        /// 土耳其语
        /// </summary>
        tr = 0x001F,

        /// <summary>
        /// 土耳其语（土耳其）
        /// </summary>
        tr_TR = 0x041F,

        /// <summary>
        /// 乌克兰语
        /// </summary>
        uk = 0x0022,

        /// <summary>
        /// 乌克兰语（乌克兰）
        /// </summary>
        uk_UA = 0x0422,

        /// <summary>
        /// 乌尔都语
        /// </summary>
        ur = 0x0020,

        /// <summary>
        /// 乌尔都语（巴基斯坦）
        /// </summary>
        ur_PK = 0x0420,

        /// <summary>
        /// 乌兹别克语
        /// </summary>
        uz = 0x0043,

        /// <summary>
        /// 乌兹别克语（乌兹别克斯坦，西里尔语）
        /// </summary>
        uz_Cyrl_UZ = 0x0843,

        /// <summary>
        /// 乌兹别克语（乌兹别克斯坦，拉丁语）
        /// </summary>
        uz_Latn_UZ = 0x0443,

        /// <summary>
        /// 越南语
        /// </summary>
        vi = 0x002A,

        /// <summary>
        /// 越南语（越南）
        /// </summary>
        vi_VN = 0x042A,
    }
}