// Мод-меню для Undertale. Версия 0.5
// M (или русская Ь) — открыть/закрыть меню. 1 — ноуклип, 2 — бессмертие,
// маленькая кнопка справа в меню — секретный меч (босс с одного удара).
EnsureDataLoaded();

var group = new UndertaleModLib.Compiler.CodeImportGroup(Data);

// Ноуклип: движение сквозь стены (работает, пока включён)
group.QueueAppend("gml_Object_obj_mainchara_Step_0", @"
if (variable_global_exists(""modNoclip""))
{
    if (keyboard_check_pressed(ord(""N""))) global.modNoclip = !global.modNoclip;
    if (global.modNoclip)
    {
        x += (keyboard_check(vk_right) - keyboard_check(vk_left)) * 3;
        y += (keyboard_check(vk_down) - keyboard_check(vk_up)) * 3;
        global.modNX = x;
        global.modNY = y;
    }
}
");

// Конец кадра: стены уже оттолкнули героя — возвращаем его туда, куда он шёл
group.QueueAppend("gml_Object_obj_mainchara_Step_2", @"
if (variable_global_exists(""modNX""))
{
    if (global.modNoclip)
    {
        x = global.modNX;
        y = global.modNY;
    }
}
");

// Логика меню, бессмертия и меча — в объекте, который живёт всю игру
group.QueueAppend("gml_Object_obj_time_Step_1", @"
if (!variable_global_exists(""modMenu""))
{
    global.modMenu = 0;
    global.modNoclip = 0;
    global.modGod = 0;
    global.modSword = 0;
    global.modStab = 0;
    global.modStabX = 0;
    global.modStabY = 0;
    global.modKnight = 0;
    global.modFace = 0;
    global.modMoving = 0;
    global.modWalk = 0;
    global.modPX = 0;
    global.modPY = 0;
    global.modThrow = 0;
    global.modThrowDir = 0;
    global.modRegen = 0;
    global.modTX = 0;
    global.modTY = 0;
    global.modFlame = 0;
    global.modFast = 0;
    global.modFastOn = 0;
    global.modDash = 0;
    global.modDashDX = 0;
    global.modDashDY = 0;
    global.modFC1 = make_color_rgb(95, 224, 255);
    global.modFC2 = make_color_rgb(30, 140, 255);
    global.modFC3 = make_color_rgb(150, 235, 255);
    global.modSeen[0] = 0;
    global.modSeen[1] = 0;
    global.modSeen[2] = 0;
    // Счётчик побед рыцаря хранится в отдельном файле, чтобы не пропадал
    ini_open(""modknight.ini"");
    global.modKills = ini_read_real(""knight"", ""kills"", 0);
    ini_close();
}
var modKey = keyboard_check_pressed(ord(""M""));
if (keyboard_lastchar == ""ь"" || keyboard_lastchar == ""Ь"")
{
    modKey = 1;
    keyboard_lastchar = """";
}
if (modKey) global.modMenu = !global.modMenu;
if (global.modMenu)
{
    var s = display_get_gui_width() / 640;
    var px = 20 * s;
    var py = 60 * s;
    var pw = 260 * s;
    var mx = device_mouse_x_to_gui(0);
    var my = device_mouse_y_to_gui(0);
    if (mouse_check_button_pressed(mb_left))
    {
        if (mx > px && mx < px + 200 * s && my > py + 40 * s && my < py + 70 * s) global.modNoclip = !global.modNoclip;
        if (mx > px && mx < px + 200 * s && my > py + 80 * s && my < py + 110 * s) global.modGod = !global.modGod;
        if (mx > px + pw - 24 * s && mx < px + pw - 4 * s && my > py + 130 * s && my < py + 150 * s) global.modSword = !global.modSword;
        if (mx > px && mx < px + 200 * s && my > py + 120 * s && my < py + 150 * s)
        {
            global.modKnight = !global.modKnight;
            if (!global.modKnight && instance_exists(obj_mainchara)) obj_mainchara.visible = 1;
        }
        if (mx > px && mx < px + 240 * s && my > py + 160 * s && my < py + 190 * s) global.modFlame = (global.modFlame + 1) mod 4;
        if (mx > px && mx < px + 240 * s && my > py + 200 * s && my < py + 230 * s) global.modFast = !global.modFast;
    }
    if (keyboard_check_pressed(ord(""1""))) global.modNoclip = !global.modNoclip;
    if (keyboard_check_pressed(ord(""2""))) global.modGod = !global.modGod;
    if (keyboard_check_pressed(ord(""3""))) global.modSword = !global.modSword;
    if (keyboard_check_pressed(ord(""5""))) global.modFlame = (global.modFlame + 1) mod 4;
    if (keyboard_check_pressed(ord(""6""))) global.modFast = !global.modFast;
    if (keyboard_check_pressed(ord(""4"")))
    {
        global.modKnight = !global.modKnight;
        if (!global.modKnight && instance_exists(obj_mainchara)) obj_mainchara.visible = 1;
    }
}
if (global.modGod) global.hp = global.maxhp;
// Момент удара: полоска прицела начала исчезать (так при любом оружии) или появился разрез
var modHit = instance_exists(obj_slice);
if (instance_exists(obj_target))
{
    if (obj_target.fade == 1) modHit = 1;
}
if (global.modSword && global.modStab <= 0 && modHit)
{
    var n = array_length_1d(global.monsterhp);
    for (var i = 0; i < n; i += 1)
    {
        if (global.monsterhp[i] > 1) global.monsterhp[i] = 1;
    }
    // Цель — центр первого живого врага
    global.modStabX = 320;
    global.modStabY = 120;
    var m = array_length_1d(global.monsterinstance);
    for (var j = m - 1; j >= 0; j -= 1)
    {
        if (instance_exists(global.monsterinstance[j]))
        {
            global.modStabX = (global.monsterinstance[j].bbox_left + global.monsterinstance[j].bbox_right) / 2;
            global.modStabY = (global.monsterinstance[j].bbox_top + global.monsterinstance[j].bbox_bottom) / 2;
        }
    }
    global.modStab = 50;
}
if (global.modStab > 0) global.modStab -= 1;
");

// Рисование: окно меню и анимация меча
group.QueueAppend("gml_Object_obj_time_Draw_64", @"
if (variable_global_exists(""modMenu""))
{
    var gw = display_get_gui_width();
    var gh = display_get_gui_height();
    var s = gw / 640;
    draw_set_alpha(1);
    draw_set_font(fnt_maintext);
    if (global.modMenu)
    {
        var px = 20 * s;
        var py = 60 * s;
        var pw = 260 * s;
        draw_set_color(c_black);
        draw_rectangle(px, py, px + pw, py + 270 * s, false);
        draw_set_color(c_white);
        draw_rectangle(px, py, px + pw, py + 270 * s, true);
        draw_rectangle(px + 2 * s, py + 2 * s, px + pw - 2 * s, py + 268 * s, true);
        draw_set_color(c_yellow);
        draw_text_transformed(px + 12 * s, py + 10 * s, ""MOD MENU  [M]"", s, s, 0);
        var t1 = ""1) NOCLIP: OFF"";
        draw_set_color(c_white);
        if (global.modNoclip) { t1 = ""1) NOCLIP: ON""; draw_set_color(c_lime); }
        draw_text_transformed(px + 12 * s, py + 45 * s, t1, s, s, 0);
        var t2 = ""2) GOD MODE: OFF"";
        draw_set_color(c_white);
        if (global.modGod) { t2 = ""2) GOD MODE: ON""; draw_set_color(c_lime); }
        draw_text_transformed(px + 12 * s, py + 85 * s, t2, s, s, 0);
        var t4 = ""4) KNIGHT: OFF"";
        draw_set_color(c_white);
        if (global.modKnight) { t4 = ""4) KNIGHT: ON  [U]""; draw_set_color(c_aqua); }
        draw_text_transformed(px + 12 * s, py + 125 * s, t4, s, s, 0);
        var t5 = ""5) FLAME: BLUE"";
        if (global.modFlame == 1) t5 = ""5) FLAME: RED"";
        if (global.modFlame == 2) t5 = ""5) FLAME: PURPLE"";
        if (global.modFlame == 3) t5 = ""5) FLAME: GREEN"";
        draw_set_color(global.modFC1);
        draw_text_transformed(px + 12 * s, py + 165 * s, t5, s, s, 0);
        var t6 = ""6) FAST BATTLE: OFF"";
        draw_set_color(c_white);
        if (global.modFast) { t6 = ""6) FAST BATTLE: ON""; draw_set_color(c_lime); }
        draw_text_transformed(px + 12 * s, py + 205 * s, t6, s, s, 0);
        draw_set_color(c_gray);
        draw_text_transformed(px + 12 * s, py + 240 * s, ""KNIGHT WINS: "" + string(global.modKills), s, s, 0);
        // Секретная кнопка: маленький тёмный квадрат справа внизу
        draw_set_color(make_color_rgb(40, 40, 40));
        if (global.modSword) draw_set_color(c_red);
        draw_rectangle(px + pw - 24 * s, py + 130 * s, px + pw - 4 * s, py + 150 * s, false);
    }
    if (global.modSword)
    {
        draw_set_color(c_red);
        draw_text_transformed(gw - 90 * s, 8 * s, ""SWORD"", s, s, 0);
    }
    if (global.modStab > 0)
    {
        var vw = view_wview[0];
        if (vw <= 0) vw = 640;
        var k = gw / vw;
        var tx = (global.modStabX - view_xview[0]) * k;
        var ty = (global.modStabY - view_yview[0]) * (gh / max(1, view_hview[0]));
        var t = 50 - global.modStab;
        var p = min(1, t / 12);
        var bx = tx + (1 - p) * 420 * s;
        if (!global.modKnight)
        {
        // Меч летит справа и вонзается остриём в цель
        draw_set_color(make_color_rgb(200, 210, 230));
        draw_rectangle(bx, ty - 5 * s, bx + 120 * s, ty + 5 * s, false);
        draw_triangle(bx - 18 * s, ty, bx, ty - 5 * s, bx, ty + 5 * s, false);
        draw_set_color(c_white);
        draw_line(bx, ty, bx + 118 * s, ty);
        draw_set_color(make_color_rgb(230, 180, 40));
        draw_rectangle(bx + 120 * s, ty - 22 * s, bx + 128 * s, ty + 22 * s, false);
        draw_set_color(make_color_rgb(110, 60, 30));
        draw_rectangle(bx + 128 * s, ty - 4 * s, bx + 165 * s, ty + 4 * s, false);
        draw_set_color(make_color_rgb(230, 180, 40));
        draw_circle(bx + 170 * s, ty, 6 * s, false);
        }
        if (t >= 12)
        {
            var q = (t - 12) / 38;
            // Вспышка
            draw_set_alpha(max(0, 0.8 - q * 3));
            draw_set_color(c_white);
            draw_rectangle(0, 0, gw, gh, false);
            // Красные кольца из точки удара
            draw_set_alpha(1 - q);
            draw_set_color(c_red);
            if (global.modKnight) draw_set_color(global.modFC1);
            draw_circle(tx - 18 * s, ty, (10 + q * 90) * s, true);
            draw_circle(tx - 18 * s, ty, (5 + q * 60) * s, true);
            draw_circle(tx - 18 * s, ty, 8 * s * (1 - q), false);
            draw_set_alpha(1);
            draw_set_color(c_red);
            draw_text_transformed(tx - 60 * s, ty - 70 * s, ""CRITICAL!"", s * 1.5, s * 1.5, 0);
        }
        draw_set_alpha(1);
    }
    draw_set_color(c_white);
}
");

// ===== Рыцарь: бросок меча (U) =====
group.QueueAppend("gml_Object_obj_time_Step_1", @"
if (variable_global_exists(""modKnight""))
{
    if (!variable_global_exists(""modStep"")) global.modStep = 0;
    if (instance_exists(obj_mainchara))
    {
        if (global.modKnight && global.modThrow <= 0 && keyboard_check_pressed(ord(""U"")))
        {
            global.modThrow = 24;
            global.modThrowDir = global.modFace;
            global.modTX = round((obj_mainchara.bbox_left + obj_mainchara.bbox_right) / 2) + 10;
            global.modTY = obj_mainchara.bbox_bottom - 31;
        }
    }
    if (global.modThrow > 0)
    {
        global.modThrow -= 1;
        if (global.modThrow == 0) global.modRegen = 15;
    }
    if (global.modRegen > 0) global.modRegen -= 1;
}
");

// ===== Рыцарь: рисуется вместо героя прямо в мире игры =====
group.QueuePrepend("gml_Object_obj_mainchara_Draw_0", @"
if (variable_global_exists(""modKnight""))
{
    if (global.modKnight)
    {
        var sw;
        sw[0] = ""..W.."";
        sw[1] = "".WCW."";
        sw[2] = "".CBC."";
        sw[3] = "".CBC."";
        sw[4] = "".CBC."";
        sw[5] = "".CBC."";
        sw[6] = "".CBC."";
        sw[7] = "".CBC."";
        sw[8] = "".CBC."";
        sw[9] = "".CBC."";
        sw[10] = "".CBC."";
        sw[11] = "".CBC."";
        sw[12] = "".CBC."";
        sw[13] = "".CBC."";
        sw[14] = "".CBC."";
        sw[15] = ""KRRRK"";
        sw[16] = ""..g.."";
        sw[17] = ""..g.."";
        sw[18] = ""..g.."";
        sw[19] = "".KRK."";
        // Направление — по реальному движению героя
        var mdx = x - xprevious;
        var mdy = y - yprevious;
        if (abs(mdx) + abs(mdy) > 0.1)
        {
            if (abs(mdx) > abs(mdy))
            {
                if (mdx > 0) global.modFace = 2; else global.modFace = 3;
            }
            else
            {
                if (mdy > 0) global.modFace = 0; else global.modFace = 1;
            }
        }
        var kf = global.modFace;
        var kn;
        if (kf == 0)
        {
            kn[0] = ""..H........H.."";
            kn[1] = ""..gH......Hg.."";
            kn[2] = ""...gKKKKKKg..."";
            kn[3] = ""...KGGGGGGK..."";
            kn[4] = ""...KGHGGHGK..."";
            kn[5] = ""...KgKKKKgK..."";
            kn[6] = ""...KKEKKEKK..."";
            kn[7] = ""...KgKKKKgK..."";
            kn[8] = ""....KgGGgK...."";
            kn[9] = ""..KKKKKKKKKK.."";
            kn[10] = "".KGHGKRRKGHGK."";
            kn[11] = ""KGGGKRRRRKGGGK"";
            kn[12] = ""KgKKRRGGRRKKgK"";
            kn[13] = ""KgKRRGHHGRRKgK"";
            kn[14] = ""KgKRRRGGRRRKgK"";
            kn[15] = ""KGKRRRRRRRRKGK"";
            kn[16] = ""KgKgGGGGGGgKgK"";
            kn[17] = "".KKRRRRRRRRKK."";
            kn[18] = ""...KGGKKGGK..."";
            kn[19] = ""...KGHKKHGK..."";
            kn[20] = ""...KrrKKrrK..."";
            kn[21] = ""...KGGKKGGK..."";
            kn[22] = ""....KGKKGK...."";
            kn[23] = ""....KGKKGK...."";
            kn[24] = "".....K..K....."";
        }
        else if (kf == 1)
        {
            kn[0] = ""..H........H.."";
            kn[1] = ""..gH......Hg.."";
            kn[2] = ""...gKKKKKKg..."";
            kn[3] = ""...KGGGGGGK..."";
            kn[4] = ""...KGHGGHGK..."";
            kn[5] = ""...KgKKKKgK..."";
            kn[6] = ""...KgKKKKgK..."";
            kn[7] = ""...KgKKKKgK..."";
            kn[8] = ""....KgGGgK...."";
            kn[9] = ""..KKKKKKKKKK.."";
            kn[10] = "".KGHGKRRKGHGK."";
            kn[11] = ""KGGGKRRRRKGGGK"";
            kn[12] = ""KgKKRRRRRRKKgK"";
            kn[13] = ""KgKRRRRRRRRKgK"";
            kn[14] = ""KgKRRRRRRRRKgK"";
            kn[15] = ""KGKRRRRRRRRKGK"";
            kn[16] = ""KgKgGGGGGGgKgK"";
            kn[17] = "".KKRRRRRRRRKK."";
            kn[18] = ""...KGGKKGGK..."";
            kn[19] = ""...KGHKKHGK..."";
            kn[20] = ""...KrrKKrrK..."";
            kn[21] = ""...KGGKKGGK..."";
            kn[22] = ""....KGKKGK...."";
            kn[23] = ""....KGKKGK...."";
            kn[24] = "".....K..K....."";
        }
        else if (kf == 2)
        {
            kn[0] = ""....H........."";
            kn[1] = ""....gH........"";
            kn[2] = "".....gKKKK...."";
            kn[3] = "".....KGGGGK..."";
            kn[4] = "".....KGHGGGK.."";
            kn[5] = "".....KgGKKKK.."";
            kn[6] = "".....KgGKKEK.."";
            kn[7] = "".....KgGKKKK.."";
            kn[8] = ""......KgGGK..."";
            kn[9] = "".....KKKKKK..."";
            kn[10] = ""....KGHGKRK..."";
            kn[11] = ""....KGGKRRK..."";
            kn[12] = "".....KgKRRK..."";
            kn[13] = "".....KgKRRK..."";
            kn[14] = "".....KgKRRK..."";
            kn[15] = "".....KGKRRK..."";
            kn[16] = ""......KgGGK..."";
            kn[17] = ""......KKRRK..."";
            kn[18] = ""......KGGK...."";
            kn[19] = ""......KGHK...."";
            kn[20] = ""......KrrK...."";
            kn[21] = ""......KGGK...."";
            kn[22] = "".......KGK...."";
            kn[23] = "".......KGK...."";
            kn[24] = ""........K....."";
        }
        else if (kf == 3)
        {
            kn[0] = "".........H...."";
            kn[1] = ""........Hg...."";
            kn[2] = ""....KKKKg....."";
            kn[3] = ""...KGGGGK....."";
            kn[4] = ""..KGGGHGK....."";
            kn[5] = ""..KKKKGgK....."";
            kn[6] = ""..KEKKGgK....."";
            kn[7] = ""..KKKKGgK....."";
            kn[8] = ""...KGGgK......"";
            kn[9] = ""...KKKKKK....."";
            kn[10] = ""...KRKGHGK...."";
            kn[11] = ""...KRRKGGK...."";
            kn[12] = ""...KRRKgK....."";
            kn[13] = ""...KRRKgK....."";
            kn[14] = ""...KRRKgK....."";
            kn[15] = ""...KRRKGK....."";
            kn[16] = ""...KGGgK......"";
            kn[17] = ""...KRRKK......"";
            kn[18] = ""....KGGK......"";
            kn[19] = ""....KHGK......"";
            kn[20] = ""....KrrK......"";
            kn[21] = ""....KGGK......"";
            kn[22] = ""....KGK......."";
            kn[23] = ""....KGK......."";
            kn[24] = "".....K........"";
        }
        // Левитация: парит над землёй и плавно качается
        var hv = round(sin(current_time / 350) * 2);
        var kcx = round((bbox_left + bbox_right) / 2);
        var bx = kcx - 7;
        var by = bbox_bottom - 25 - 5 + hv;
        // Рывок: позади остаются цветные призраки рыцаря
        if (global.modDash > 0)
        {
            for (var gi = 1; gi <= 3; gi += 1)
            {
                draw_set_alpha(0.45 - gi * 0.12);
                draw_set_color(global.modFC1);
                var gx = bx - global.modDashDX * gi * 6;
                var gy = by - global.modDashDY * gi * 6;
                for (var gr = 0; gr < 25; gr += 1)
                {
                    for (var gc = 0; gc < 14; gc += 1)
                    {
                        if (string_char_at(kn[gr], gc + 1) != ""."") draw_rectangle(gx + gc, gy + gr, gx + gc + 0.5, gy + gr + 0.5, false);
                    }
                }
            }
            draw_set_alpha(1);
        }
        // Тень на земле: меньше, когда он выше
        draw_set_alpha(0.35);
        draw_set_color(c_black);
        draw_ellipse(kcx - 6 + hv, bbox_bottom - 2, kcx + 5 - hv, bbox_bottom + 1, false);
        draw_set_alpha(1);
        // Чёрные щупальца за спиной (рисуются до тела, поэтому сзади)
        var ta;
        ta[0] = 205; ta[1] = 160; ta[2] = 125; ta[3] = 55; ta[4] = 20; ta[5] = -25;
        var tox = kcx;
        var toy = by + 12;
        for (var ti = 0; ti < 6; ti += 1)
        {
            for (var ts = 0; ts < 16; ts += 1)
            {
                var wav = sin(current_time / 220 + ti * 1.7 + ts * 0.45) * ts * 0.35;
                var tpx = round(tox + lengthdir_x(ts, ta[ti]) + lengthdir_x(wav, ta[ti] + 90));
                var tpy = round(toy + lengthdir_y(ts, ta[ti]) + lengthdir_y(wav, ta[ti] + 90));
                var tth = 1;
                if (ts < 9) tth = 2;
                draw_set_color(make_color_rgb(8, 6, 14));
                draw_rectangle(tpx, tpy, tpx + tth - 0.5, tpy + tth - 0.5, false);
                if (tth == 2)
                {
                    draw_set_color(make_color_rgb(36, 26, 52));
                    draw_rectangle(tpx, tpy, tpx + 0.5, tpy + 0.5, false);
                }
            }
        }
        // Синее пламя под ногами
        for (var fl = 0; fl < 4; fl += 1)
        {
            { var qx = kcx + irandom_range(-3, 2); var qy = by + 25 + irandom(3); draw_set_color(choose(global.modFC1, global.modFC2, c_white)); draw_rectangle(qx, qy, qx + 0.5, qy + 0.5, false); }
        }
        for (var r = 0; r < 25; r += 1)
        {
            var row = kn[r];
            for (var c = 0; c < 14; c += 1)
            {
                var ch = string_char_at(row, c + 1);
                if (ch != ""."")
                {
                    var col = c_black;
                    if (ch == ""K"") col = make_color_rgb(4, 4, 8);
                    else if (ch == ""R"") col = make_color_rgb(30, 30, 38);
                    else if (ch == ""r"") col = make_color_rgb(18, 18, 24);
                    else if (ch == ""G"") col = make_color_rgb(58, 60, 70);
                    else if (ch == ""g"") col = make_color_rgb(38, 39, 47);
                    else if (ch == ""H"") col = make_color_rgb(110, 115, 128);
                    else if (ch == ""E"") col = global.modFC1;
                    else if (ch == ""W"") col = c_white;
                    else if (ch == ""C"") col = global.modFC1;
                    else if (ch == ""B"") col = global.modFC2;
                    { var qx = bx + c; var qy = by + r; draw_set_color(col); draw_rectangle(qx, qy, qx + 0.5, qy + 0.5, false); }
                }
            }
        }
        // Синее пламя из глаз
        if (kf != 1)
        {
            for (var c2 = 0; c2 < 14; c2 += 1)
            {
                if (string_char_at(kn[6], c2 + 1) == ""E"")
                {
                    if (random(1) < 0.7) { var qx = bx + c2; var qy = by + 5; draw_set_color(global.modFC3); draw_rectangle(qx, qy, qx + 0.5, qy + 0.5, false); }
                    if (random(1) < 0.4) { var qx = bx + c2 + choose(-1, 0, 1); var qy = by + 4; draw_set_color(global.modFC1); draw_rectangle(qx, qy, qx + 0.5, qy + 0.5, false); }
                    if (random(1) < 0.2) { var qx = bx + c2; var qy = by + 3; draw_set_color(c_white); draw_rectangle(qx, qy, qx + 0.5, qy + 0.5, false); }
                }
            }
        }
        // Два длинных левитирующих меча по бокам
        var sb = round(sin(current_time / 300) * 2);
        for (var side = 0; side < 2; side += 1)
        {
            var show = 1;
            if (side == 1 && global.modThrow > 0) show = 0;
            if (side == 1 && global.modRegen > 0 && (global.modRegen mod 2) == 1) show = 0;
            if (show)
            {
                var sx = bx - 8;
                var sy = by - 1 + sb;
                if (side == 1)
                {
                    sx = bx + 17;
                    sy = by - 1 - sb;
                }
                for (var r3 = 0; r3 < 20; r3 += 1)
                {
                    for (var c3 = 0; c3 < 5; c3 += 1)
                    {
                        var ch = string_char_at(sw[r3], c3 + 1);
                        if (ch != ""."")
                        {
                    var col = c_black;
                    if (ch == ""K"") col = make_color_rgb(4, 4, 8);
                    else if (ch == ""R"") col = make_color_rgb(30, 30, 38);
                    else if (ch == ""r"") col = make_color_rgb(18, 18, 24);
                    else if (ch == ""G"") col = make_color_rgb(58, 60, 70);
                    else if (ch == ""g"") col = make_color_rgb(38, 39, 47);
                    else if (ch == ""H"") col = make_color_rgb(110, 115, 128);
                    else if (ch == ""E"") col = global.modFC1;
                    else if (ch == ""W"") col = c_white;
                    else if (ch == ""C"") col = global.modFC1;
                    else if (ch == ""B"") col = global.modFC2;
                            { var qx = sx + c3; var qy = sy + r3; draw_set_color(col); draw_rectangle(qx, qy, qx + 0.5, qy + 0.5, false); }
                        }
                    }
                }
                // Голубое пламя по клинку
                for (var f = 0; f < 4; f += 1)
                {
                    { var qx = sx + choose(0, 4); var qy = sy + irandom(14); draw_set_color(choose(global.modFC1, c_white, global.modFC3)); draw_rectangle(qx, qy, qx + 0.5, qy + 0.5, false); }
                }
            }
        }
        // Брошенный меч (U)
        if (global.modThrow > 0)
        {
            var dist = (24 - global.modThrow) * 8;
            var tsx = global.modTX;
            var tsy = global.modTY;
            if (global.modThrowDir == 0) tsy += dist;
            if (global.modThrowDir == 1) tsy -= dist;
            if (global.modThrowDir == 2) tsx += dist;
            if (global.modThrowDir == 3) tsx -= dist;
            for (var r4 = 0; r4 < 20; r4 += 1)
            {
                for (var c4 = 0; c4 < 5; c4 += 1)
                {
                    var ch = string_char_at(sw[r4], c4 + 1);
                    if (ch != ""."")
                    {
                        var px4 = c4;
                        var py4 = r4;
                        if (global.modThrowDir == 0) py4 = 19 - r4;
                        if (global.modThrowDir == 3) { px4 = r4; py4 = c4; }
                        if (global.modThrowDir == 2) { px4 = 19 - r4; py4 = c4; }
                    var col = c_black;
                    if (ch == ""K"") col = make_color_rgb(4, 4, 8);
                    else if (ch == ""R"") col = make_color_rgb(30, 30, 38);
                    else if (ch == ""r"") col = make_color_rgb(18, 18, 24);
                    else if (ch == ""G"") col = make_color_rgb(58, 60, 70);
                    else if (ch == ""g"") col = make_color_rgb(38, 39, 47);
                    else if (ch == ""H"") col = make_color_rgb(110, 115, 128);
                    else if (ch == ""E"") col = global.modFC1;
                    else if (ch == ""W"") col = c_white;
                    else if (ch == ""C"") col = global.modFC1;
                    else if (ch == ""B"") col = global.modFC2;
                        { var qx = tsx + px4; var qy = tsy + py4; draw_set_color(col); draw_rectangle(qx, qy, qx + 0.5, qy + 0.5, false); }
                    }
                }
            }
            for (var tr = 0; tr < 6; tr += 1)
            {
                { var qx = tsx + 2 + irandom_range(-3, 3); var qy = tsy + 9 + irandom_range(-4, 4); draw_set_color(choose(global.modFC1, c_white)); draw_rectangle(qx, qy, qx + 0.5, qy + 0.5, false); }
            }
        }
        draw_set_color(c_white);
        exit;
    }
}
");

// ===== Рыцарь: ультра-атака в бою =====
group.QueueAppend("gml_Object_obj_time_Draw_64", @"
if (variable_global_exists(""modKnight""))
{
    var gw = display_get_gui_width();
    var gh = display_get_gui_height();
    var sw;
    sw[0] = ""..W.."";
    sw[1] = "".WCW."";
    sw[2] = "".CBC."";
    sw[3] = "".CBC."";
    sw[4] = "".CBC."";
    sw[5] = "".CBC."";
    sw[6] = "".CBC."";
    sw[7] = "".CBC."";
    sw[8] = "".CBC."";
    sw[9] = "".CBC."";
    sw[10] = "".CBC."";
    sw[11] = "".CBC."";
    sw[12] = "".CBC."";
    sw[13] = "".CBC."";
    sw[14] = "".CBC."";
    sw[15] = ""KRRRK"";
    sw[16] = ""..g.."";
    sw[17] = ""..g.."";
    sw[18] = ""..g.."";
    sw[19] = "".KRK."";
    // Ультра-атака в бою: рыцарь берёт летающий меч и кидает его во врага
    if (global.modKnight && global.modStab > 0)
    {
        var s = gw / 640;
        var bk = gw / 640;
        if (view_wview[0] > 0) bk = gw / view_wview[0];
        var tx = (global.modStabX - view_xview[0]) * bk;
        var ty = (global.modStabY - view_yview[0]) * (gh / max(1, view_hview[0]));
        var t = 50 - global.modStab;
        var p = min(1, t / 12);
        var ps = 4 * s;
        // Летит снизу (из рук героя) остриём вверх прямо в сердце
        var fx = gw / 2 + (tx - gw / 2) * p - 2.5 * ps;
        var fy = gh * 0.8 + (ty - gh * 0.8) * p;
        for (var r5 = 0; r5 < 20; r5 += 1)
        {
            for (var c5 = 0; c5 < 5; c5 += 1)
            {
                var ch5 = string_char_at(sw[r5], c5 + 1);
                if (ch5 != ""."")
                {
                    var col5 = make_color_rgb(92, 97, 106);
                    if (ch5 == ""W"") col5 = c_white;
                    else if (ch5 == ""C"") col5 = global.modFC1;
                    else if (ch5 == ""B"") col5 = global.modFC2;
                    else if (ch5 == ""R"") col5 = make_color_rgb(30, 30, 38);
                    else if (ch5 == ""K"") col5 = make_color_rgb(4, 4, 8);
                    draw_set_color(col5);
                    draw_rectangle(fx + c5 * ps, fy + r5 * ps, fx + (c5 + 1) * ps - 1, fy + (r5 + 1) * ps - 1, false);
                }
            }
        }
        draw_set_alpha(0.6);
        draw_set_color(global.modFC1);
        for (var tr2 = 0; tr2 < 6; tr2 += 1)
        {
            draw_circle(fx + 2.5 * ps + random_range(-6, 6) * s, fy + random_range(0, 60) * s, (2 + random(4)) * s, false);
        }
        draw_set_alpha(1);
    }
    draw_set_color(c_white);
}
");

// ===== Режим «Рыцарь» в бою: своя душа, щит и удар щупальцами =====
group.QueueAppend("gml_Object_obj_time_Step_1", @"
if (variable_global_exists(""modKnight""))
{
    if (!variable_global_exists(""modShield""))
    {
        global.modShield = 20;
        global.modLastHP = global.hp;
        global.modCalm = 0;
        global.modKHit = 0;
        global.modKX = 320;
        global.modKY = 120;
        global.modDefA = 0;
        global.modDefCD = 0;
        global.modDefFX = 0;
        global.modDefX = 0;
        global.modDefY = 0;
    }
    if (global.modKnight && instance_exists(obj_heart))
    {
        // Щит: забирает урон на себя, пока не кончится
        if (global.hp < global.modLastHP)
        {
            var kdmg = global.modLastHP - global.hp;
            var kabs = min(global.modShield, kdmg);
            global.hp += kabs;
            global.modShield -= kabs;
            global.modCalm = 0;
        }
        // Щит медленно восстанавливается, если долго не получать урон
        global.modCalm += 1;
        if (global.modCalm > 90 && (global.modCalm mod 20) == 0 && global.modShield < 20) global.modShield += 1;
        // Удар щупальцами: при каждой атаке врагу прилетает ещё 20 урона
        var khit = instance_exists(obj_slice);
        if (instance_exists(obj_target))
        {
            if (obj_target.fade == 1) khit = 1;
        }
        if (khit && global.modKHit <= 0 && !global.modSword)
        {
            var km = array_length_1d(global.monsterinstance);
            var kfound = 0;
            for (var kj = 0; kj < km; kj += 1)
            {
                if (!kfound && instance_exists(global.monsterinstance[kj]))
                {
                    kfound = 1;
                    if (global.monsterhp[kj] > 1) global.monsterhp[kj] = max(1, global.monsterhp[kj] - 20);
                    global.modKX = (global.monsterinstance[kj].bbox_left + global.monsterinstance[kj].bbox_right) / 2;
                    global.modKY = (global.monsterinstance[kj].bbox_top + global.monsterinstance[kj].bbox_bottom) / 2;
                }
            }
            global.modKHit = 90;
        }
        // Щупальца-защитники: кружат вокруг души и иногда сбивают пулю
        global.modDefA += 6;
        if (global.modDefCD > 0) global.modDefCD -= 1;
        var dcx = obj_heart.x + 8;
        var dcy = obj_heart.y + 8;
        for (var di = 0; di < 2; di += 1)
        {
            var dox = dcx + lengthdir_x(22, global.modDefA + di * 180);
            var doy = dcy + lengthdir_y(22, global.modDefA + di * 180);
            if (global.modDefCD <= 0 && instance_exists(blt_parent))
            {
                var db = instance_nearest(dox, doy, blt_parent);
                if (db != noone)
                {
                    if (point_distance(dox, doy, (db.bbox_left + db.bbox_right) / 2, (db.bbox_top + db.bbox_bottom) / 2) < 16)
                    {
                        with (db) instance_destroy();
                        global.modDefCD = 40;
                        global.modDefFX = 10;
                        global.modDefX = dox;
                        global.modDefY = doy;
                    }
                }
            }
        }
    }
    else
    {
        // Вне боя щит снова полный
        global.modShield = 20;
        global.modCalm = 0;
    }
    if (global.modKHit > 0) global.modKHit -= 1;
    if (global.modDefFX > 0) global.modDefFX -= 1;
    global.modLastHP = global.hp;
}
");

group.QueueAppend("gml_Object_obj_time_Draw_64", @"
if (variable_global_exists(""modShield""))
{
    if (global.modKnight && instance_exists(obj_heart))
    {
        var kgw = display_get_gui_width();
        var kgh = display_get_gui_height();
        var ks = kgw / 640;
        var kvw = view_wview[0];
        if (kvw <= 0) kvw = 640;
        var kvh = view_hview[0];
        if (kvh <= 0) kvh = 480;
        var kkx = kgw / kvw;
        var kky = kgh / kvh;
        // Душа рыцаря: чёрная с голубой обводкой
        with (obj_heart)
        {
            if (visible)
            {
                var hx = (x - view_xview[0]) * kkx;
                var hy = (y - view_yview[0]) * kky;
                d3d_set_fog(true, global.modFC1, 0, 0);
                draw_sprite_ext(sprite_index, image_index, hx - kkx, hy, kkx, kky, 0, c_white, 1);
                draw_sprite_ext(sprite_index, image_index, hx + kkx, hy, kkx, kky, 0, c_white, 1);
                draw_sprite_ext(sprite_index, image_index, hx, hy - kky, kkx, kky, 0, c_white, 1);
                draw_sprite_ext(sprite_index, image_index, hx, hy + kky, kkx, kky, 0, c_white, 1);
                d3d_set_fog(true, make_color_rgb(8, 6, 14), 0, 0);
                draw_sprite_ext(sprite_index, image_index, hx, hy, kkx, kky, 0, c_white, 1);
                d3d_set_fog(false, c_black, 0, 0);
            }
        }
        // Щупальца-защитники вокруг души
        var dhx = (obj_heart.x + 8 - view_xview[0]) * kkx;
        var dhy = (obj_heart.y + 8 - view_yview[0]) * kky;
        for (var dj = 0; dj < 2; dj += 1)
        {
            for (var dq = 0; dq < 5; dq += 1)
            {
                var dang = global.modDefA + dj * 180 - dq * 9;
                var ddx = dhx + lengthdir_x(22 * kkx, dang);
                var ddy = dhy + lengthdir_y(22 * kky, dang);
                var drr = (5 - dq * 0.8) * kkx;
                draw_set_color(make_color_rgb(8, 6, 14));
                draw_circle(ddx, ddy, drr, false);
                if (dq == 0)
                {
                    draw_set_color(global.modFC1);
                    draw_circle(ddx, ddy, drr, true);
                }
            }
        }
        if (global.modDefFX > 0)
        {
            draw_set_alpha(global.modDefFX / 10);
            draw_set_color(global.modFC1);
            var dfx = (global.modDefX - view_xview[0]) * kkx;
            var dfy = (global.modDefY - view_yview[0]) * kky;
            draw_circle(dfx, dfy, (18 - global.modDefFX) * 2 * ks, true);
            draw_circle(dfx, dfy, (18 - global.modDefFX) * ks, true);
            draw_set_alpha(1);
        }
        // Полоска щита
        draw_set_font(fnt_maintext);
        draw_set_color(global.modFC1);
        draw_text_transformed(20 * ks, 12 * ks, ""SHIELD"", ks, ks, 0);
        draw_set_color(make_color_rgb(20, 30, 50));
        draw_rectangle(110 * ks, 16 * ks, 210 * ks, 30 * ks, false);
        draw_set_color(global.modFC1);
        if (global.modShield > 0) draw_rectangle(110 * ks, 16 * ks, (110 + global.modShield * 5) * ks, 30 * ks, false);
        draw_set_color(c_white);
        draw_rectangle(110 * ks, 16 * ks, 210 * ks, 30 * ks, true);
        // Щупальца бьют во врага
        if (global.modKHit > 50)
        {
            var kt = (90 - global.modKHit) / 40;
            var ktx = (global.modKX - view_xview[0]) * kkx;
            var kty = (global.modKY - view_yview[0]) * kky;
            var kp = min(1, kt * 2.5);
            for (var kn = 0; kn < 3; kn += 1)
            {
                var ksx = kgw * (0.25 + kn * 0.25);
                var ksy = kgh;
                for (var kq = 0; kq < 24; kq += 1)
                {
                    var kf = kq / 24 * kp;
                    var kwav = sin(current_time / 90 + kn * 2 + kq * 0.6) * 14 * ks * (1 - kf);
                    var kpx = ksx + (ktx - ksx) * kf + kwav;
                    var kpy = ksy + (kty - ksy) * kf;
                    var krad = (9 - kq * 0.3) * ks;
                    draw_set_color(make_color_rgb(8, 6, 14));
                    draw_circle(kpx, kpy, krad, false);
                    draw_set_color(make_color_rgb(60, 40, 90));
                    draw_circle(kpx - krad * 0.3, kpy - krad * 0.3, krad * 0.35, false);
                }
            }
            if (kp >= 1)
            {
                draw_set_alpha(max(0, 1 - (kt - 0.4) * 2));
                draw_set_color(global.modFC1);
                draw_text_transformed(ktx - 40 * ks, kty - 60 * ks, ""+20"", ks * 1.5, ks * 1.5, 0);
                draw_set_alpha(1);
            }
        }
        draw_set_color(c_white);
    }
}
");

// ===== Бессмертие везде, даже в последнем бою с Флауи =====
// Урон не проходит, а проигрыш отменяется, даже если меню в этом бою не работает
group.QueuePrepend("gml_Script_scr_damagestandard", @"
if (variable_global_exists(""modGod""))
{
    if (global.modGod)
    {
        global.hp = global.maxhp;
        exit;
    }
}
");
group.QueuePrepend("gml_Script_scr_gameover", @"
if (variable_global_exists(""modGod""))
{
    if (global.modGod)
    {
        global.hp = global.maxhp;
        exit;
    }
}
");
group.QueuePrepend("gml_Script_scr_gameoverb", @"
if (variable_global_exists(""modGod""))
{
    if (global.modGod)
    {
        global.hp = global.maxhp;
        exit;
    }
}
");
group.QueuePrepend("gml_Object_obj_heart_Step_0", @"
if (variable_global_exists(""modGod""))
{
    if (global.modGod) global.hp = global.maxhp;
}
");

// В последнем бою с Флауи здоровье хранится отдельно (global.my_hp) — его пули просто не ранят
group.QueuePrepend("gml_Object_obj_floweybullet_parent_Collision_1587", @"
if (variable_global_exists(""modGod""))
{
    if (global.modGod) exit;
}
");
group.QueuePrepend("gml_Object_obj_floweybullet_parent_Other_22", @"
if (variable_global_exists(""modGod""))
{
    if (global.modGod) exit;
}
");

// ===== Версия 0.5: цвет пламени, быстрый бой, счётчик побед =====
group.QueueAppend("gml_Object_obj_time_Step_1", @"
if (variable_global_exists(""modFlame""))
{
    // Цвет пламени
    if (global.modFlame == 0) { global.modFC1 = make_color_rgb(95, 224, 255); global.modFC2 = make_color_rgb(30, 140, 255); global.modFC3 = make_color_rgb(150, 235, 255); }
    if (global.modFlame == 1) { global.modFC1 = make_color_rgb(255, 90, 80); global.modFC2 = make_color_rgb(200, 20, 30); global.modFC3 = make_color_rgb(255, 180, 170); }
    if (global.modFlame == 2) { global.modFC1 = make_color_rgb(200, 110, 255); global.modFC2 = make_color_rgb(120, 40, 220); global.modFC3 = make_color_rgb(230, 190, 255); }
    if (global.modFlame == 3) { global.modFC1 = make_color_rgb(110, 255, 140); global.modFC2 = make_color_rgb(20, 170, 60); global.modFC3 = make_color_rgb(190, 255, 200); }
    // Быстрый бой: когда пуль нет (разговоры, меню), игра идёт в 2 раза быстрее
    if (global.modFast && instance_exists(obj_heart))
    {
        var fsp = 60;
        if (instance_exists(blt_parent)) fsp = 30;
        room_speed = fsp;
        global.modFastOn = 1;
    }
    else
    {
        if (global.modFastOn)
        {
            room_speed = 30;
            global.modFastOn = 0;
        }
    }
    // Счётчик побед рыцаря
    var inb = 0;
    if (instance_exists(obj_heart) && variable_global_exists(""monsterhp""))
    {
        if (variable_global_exists(""monsterinstance"")) inb = 1;
    }
    if (inb)
    {
        var vn = min(3, min(array_length_1d(global.monsterhp), array_length_1d(global.monsterinstance)));
        for (var vi = 0; vi < vn; vi += 1)
        {
            if (global.modSeen[vi] == 0 && global.monsterhp[vi] > 0)
            {
                if (instance_exists(global.monsterinstance[vi])) global.modSeen[vi] = 1;
            }
            if (global.modSeen[vi] == 1 && global.monsterhp[vi] <= 0)
            {
                global.modSeen[vi] = 2;
                if (global.modKnight)
                {
                    global.modKills += 1;
                    ini_open(""modknight.ini"");
                    ini_write_real(""knight"", ""kills"", global.modKills);
                    ini_close();
                }
            }
        }
    }
    else
    {
        global.modSeen[0] = 0;
        global.modSeen[1] = 0;
        global.modSeen[2] = 0;
    }
}
");

// ===== Версия 0.5: рывок рыцаря на Shift =====
group.QueueAppend("gml_Object_obj_mainchara_Step_0", @"
if (variable_global_exists(""modDash""))
{
    if (global.modKnight && global.modDash <= 0 && global.interact == 0 && keyboard_check_pressed(vk_shift))
    {
        global.modDash = 7;
        global.modDashDX = 0;
        global.modDashDY = 0;
        if (global.modFace == 0) global.modDashDY = 1;
        if (global.modFace == 1) global.modDashDY = -1;
        if (global.modFace == 2) global.modDashDX = 1;
        if (global.modFace == 3) global.modDashDX = -1;
    }
    if (global.modDash > 0)
    {
        // Перед стеной рывок останавливается
        if (place_meeting(x + global.modDashDX * 5, y + global.modDashDY * 5, obj_solidparent)) global.modDash = 0;
        else
        {
            x += global.modDashDX * 5;
            y += global.modDashDY * 5;
            global.modDash -= 1;
        }
    }
}
");

group.Import();
ScriptMessage("Мод-меню установлено: M — меню, 4 — рыцарь, U — бросок меча, Shift — рывок, 5 — цвет пламени, 6 — быстрый бой");
