// Мод-меню для Undertale. Версия 1.0
// M (или русская Ь) — открыть/закрыть меню. 1 — ноуклип, 2 — бессмертие,
// маленькая кнопка справа в меню — секретный меч (босс с одного удара).
// 7 — Священный рыцарь (HOLY KNIGHT), T — его тёмная вспышка.
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
    global.modCharge = 0;
    global.modBig = 0;
    global.modSummon = 0;
    global.modSumX = 0;
    global.modSumY = 0;
    global.modIntro = 0;
    global.modWasBattle = 0;
    global.modDeity = 0;
    global.modDHit = 0;
    global.modDX = 320;
    global.modDY = 120;
    global.modDFlash = 0;
    global.modDReady = 1;
    global.modTrailN = 0;
    for (var tri = 0; tri < 12; tri += 1)
    {
        global.modTrailT[tri] = 0;
        global.modTrailX[tri] = 0;
        global.modTrailY[tri] = 0;
    }
    global.modArmorCol = make_color_rgb(110, 115, 128);
    global.modNoEnc = 0;
    global.modGold = 0;
    global.modHeal = 0;
    global.modGo = 0;
    global.modPhoto = 0;
    global.modPlace = 0;
    global.modLang = 0;
    global.modSig = """";
    global.modPlaceEN[0] = ""RUINS"";
    global.modPlaceRU[0] = ""РУИНЫ"";
    global.modPlaceRoom[0] = ""room_ruins1"";
    global.modPlaceEN[1] = ""SNOWDIN"";
    global.modPlaceRU[1] = ""СНОУДИН"";
    global.modPlaceRoom[1] = ""room_tundra1"";
    global.modPlaceEN[2] = ""WATERFALL"";
    global.modPlaceRU[2] = ""ВОДОПАД"";
    global.modPlaceRoom[2] = ""room_water1"";
    global.modPlaceEN[3] = ""HOTLAND"";
    global.modPlaceRU[3] = ""ХОТЛЕНД"";
    global.modPlaceRoom[3] = ""room_fire1"";
    global.modPlaceEN[4] = ""NEW HOME"";
    global.modPlaceRU[4] = ""НОВЫЙ ДОМ"";
    global.modPlaceRoom[4] = ""room_castle_front"";
    // Русский пиксельный шрифт для меню (в шрифтах игры нет русских букв)
    global.modFont = ds_map_create();
    ds_map_add(global.modFont, ""А"", "".###.#...##...#######...##...##...#"");
    ds_map_add(global.modFont, ""Б"", ""######....#....####.#...##...#####."");
    ds_map_add(global.modFont, ""В"", ""####.#...##...#####.#...##...#####."");
    ds_map_add(global.modFont, ""Г"", ""######....#....#....#....#....#...."");
    ds_map_add(global.modFont, ""Д"", ""..###.#..#.#..#.#..#.#..#######...#"");
    ds_map_add(global.modFont, ""Е"", ""######....#....####.#....#....#####"");
    ds_map_add(global.modFont, ""Ё"", "".#.#.######....####.#....#....#####"");
    ds_map_add(global.modFont, ""Ж"", ""#.#.##.#.#.###...#...###.#.#.##.#.#"");
    ds_map_add(global.modFont, ""З"", "".###.#...#....#..##.....##...#.###."");
    ds_map_add(global.modFont, ""И"", ""#...##...##..###.#.###..##...##...#"");
    ds_map_add(global.modFont, ""Й"", ""..#..#...##..###.#.###..##...##...#"");
    ds_map_add(global.modFont, ""К"", ""#...##..#.#.#..##...#.#..#..#.#...#"");
    ds_map_add(global.modFont, ""Л"", ""..###.#..#.#..#.#..#.#..##...##...#"");
    ds_map_add(global.modFont, ""М"", ""#...###.###.#.##.#.##...##...##...#"");
    ds_map_add(global.modFont, ""Н"", ""#...##...##...#######...##...##...#"");
    ds_map_add(global.modFont, ""О"", "".###.#...##...##...##...##...#.###."");
    ds_map_add(global.modFont, ""П"", ""######...##...##...##...##...##...#"");
    ds_map_add(global.modFont, ""Р"", ""####.#...##...#####.#....#....#...."");
    ds_map_add(global.modFont, ""С"", "".###.#...##....#....#....#...#.###."");
    ds_map_add(global.modFont, ""Т"", ""#####..#....#....#....#....#....#.."");
    ds_map_add(global.modFont, ""У"", ""#...##...##...#.####....##...#.###."");
    ds_map_add(global.modFont, ""Ф"", ""..#...###.#.#.##.#.#.###...#....#.."");
    ds_map_add(global.modFont, ""Х"", ""#...##...#.#.#...#...#.#.#...##...#"");
    ds_map_add(global.modFont, ""Ц"", ""#..#.#..#.#..#.#..#.#..#.#####....#"");
    ds_map_add(global.modFont, ""Ч"", ""#...##...##...#.####....#....#....#"");
    ds_map_add(global.modFont, ""Ш"", ""#.#.##.#.##.#.##.#.##.#.##.#.######"");
    ds_map_add(global.modFont, ""Щ"", ""#.#.##.#.##.#.##.#.##.#.######....#"");
    ds_map_add(global.modFont, ""Ъ"", ""##....#....#....###..#..#.#..#.###."");
    ds_map_add(global.modFont, ""Ы"", ""#...##...##...####.##.#.##.#.####.#"");
    ds_map_add(global.modFont, ""Ь"", ""#....#....#....####.#...##...#####."");
    ds_map_add(global.modFont, ""Э"", "".###.#...#....#..###....##...#.###."");
    ds_map_add(global.modFont, ""Ю"", ""#..#.#.#.##.#.####.##.#.##.#.##..#."");
    ds_map_add(global.modFont, ""Я"", "".#####...##...#.####..#.#.#..##...#"");
    ds_map_add(global.modFont, ""0"", "".###.#...##..###.#.###..##...#.###."");
    ds_map_add(global.modFont, ""1"", ""..#...##....#....#....#....#...###."");
    ds_map_add(global.modFont, ""2"", "".###.#...#....#...#...#...#...#####"");
    ds_map_add(global.modFont, ""3"", ""#####...#...#.....#.....##...#.###."");
    ds_map_add(global.modFont, ""4"", ""...#...##..#.#.#..#.#####...#....#."");
    ds_map_add(global.modFont, ""5"", ""######....####.....#....##...#.###."");
    ds_map_add(global.modFont, ""6"", ""..##..#...#....####.#...##...#.###."");
    ds_map_add(global.modFont, ""7"", ""#####....#...#...#...#....#....#..."");
    ds_map_add(global.modFont, ""8"", "".###.#...##...#.###.#...##...#.###."");
    ds_map_add(global.modFont, ""9"", "".###.#...##...#.####....#...#..##.."");
    ds_map_add(global.modFont, "")"", "".#.....#.....#....#....#...#...#..."");
    ds_map_add(global.modFont, ""("", ""...#...#...#....#....#.....#.....#."");
    ds_map_add(global.modFont, "":"", "".......#....#.........#....#......."");
    ds_map_add(global.modFont, ""-"", ""................###................"");
    ds_map_add(global.modFont, ""+"", "".......#....#..#####..#....#......."");
    ds_map_add(global.modFont, ""["", "".###..#....#....#....#....#....###."");
    ds_map_add(global.modFont, ""]"", "".###....#....#....#....#....#..###."");
    ds_map_add(global.modFont, ""."", ""..........................##...##.."");
    ds_map_add(global.modFont, ""!"", ""..#....#....#....#....#.........#.."");
    ds_map_add(global.modFont, ""U"", ""#...##...##...##...##...##...#.###."");
    ds_map_add(global.modFont, ""F"", ""######....#....####.#....#....#...."");
    ds_map_add(global.modFont, ""J"", ""..###...#....#....#....#.#..#..##.."");
    ds_map_add(global.modFont, ""G"", "".###.#...##....#.####...##...#.###."");
    ds_map_add(global.modFont, ""R"", ""####.#...##...#####.#.#..#..#.#...#"");
    ds_map_add(global.modFont, ""N"", ""#...###..##.#.##..###...##...##...#"");
    ds_map_add(global.modFont, ""M"", ""#...###.###.#.##.#.##...##...##...#"");
    ds_map_add(global.modFont, ""H"", ""#...##...##...#######...##...##...#"");
    ds_map_add(global.modFont, ""K"", ""#...##..#.#.#..##...#.#..#..#.#...#"");
    ds_map_add(global.modFont, ""E"", ""######....#....####.#....#....#####"");
    ds_map_add(global.modFont, ""A"", "".###.#...##...#######...##...##...#"");
    ds_map_add(global.modFont, ""O"", "".###.#...##...##...##...##...#.###."");
    ds_map_add(global.modFont, ""P"", ""####.#...##...#####.#....#....#...."");
    ds_map_add(global.modFont, ""C"", "".###.#...##....#....#....#...#.###."");
    ds_map_add(global.modFont, ""T"", ""#####..#....#....#....#....#....#.."");
    ds_map_add(global.modFont, ""X"", ""#...##...#.#.#...#...#.#.#...##...#"");
    ds_map_add(global.modFont, ""B"", ""####.#...##...#####.#...##...#####."");
    // Счётчик побед и настройки хранятся в отдельном файле, чтобы не пропадали
    ini_open(""modknight.ini"");
    global.modKills = ini_read_real(""knight"", ""kills"", 0);
    global.modGod = ini_read_real(""set"", ""god"", 0);
    global.modKnight = ini_read_real(""set"", ""knight"", 0);
    global.modFlame = ini_read_real(""set"", ""flame"", 0);
    global.modFast = ini_read_real(""set"", ""fast"", 0);
    global.modDeity = ini_read_real(""set"", ""deity"", 0);
    global.modWalk = ini_read_real(""set"", ""walk"", 0);
    global.modNoEnc = ini_read_real(""set"", ""noenc"", 0);
    global.modLang = ini_read_real(""set"", ""lang"", 0);
    ini_close();
}
var modKey = keyboard_check_pressed(ord(""M""));
if (keyboard_lastchar == ""ь"" || keyboard_lastchar == ""Ь"")
{
    modKey = 1;
    keyboard_lastchar = """";
}
if (modKey)
{
    global.modMenu = !global.modMenu;
    global.modPhoto = 0;
}
if (global.modMenu)
{
    var s = display_get_gui_width() / 640;
    var px = 20 * s;
    var py = 60 * s;
    var pw = 300 * s;
    var px2 = px + pw + 10 * s;
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
            if (global.modKnight) global.modDeity = 0;
            if (!global.modKnight && instance_exists(obj_mainchara)) obj_mainchara.visible = 1;
        }
        if (mx > px && mx < px + 240 * s && my > py + 160 * s && my < py + 190 * s) global.modFlame = (global.modFlame + 1) mod 4;
        if (mx > px && mx < px + 240 * s && my > py + 200 * s && my < py + 230 * s) global.modFast = !global.modFast;
        // Кнопка языка в правом верхнем углу меню
        if (mx > px + pw - 56 * s && mx < px + pw - 8 * s && my > py + 6 * s && my < py + 32 * s) global.modLang = !global.modLang;
        // Второе окно
        if (mx > px2 && mx < px2 + pw)
        {
            if (my > py + 40 * s && my < py + 70 * s) global.modWalk = !global.modWalk;
            if (my > py + 75 * s && my < py + 105 * s) global.modNoEnc = !global.modNoEnc;
            if (my > py + 110 * s && my < py + 140 * s) global.modGold = 1;
            if (my > py + 145 * s && my < py + 175 * s) global.modHeal = 1;
            if (my > py + 180 * s && my < py + 210 * s) global.modPlace = (global.modPlace + 1) mod 5;
            if (my > py + 215 * s && my < py + 245 * s) global.modGo = 1;
        }
    }
    if (keyboard_check_pressed(ord(""8""))) global.modWalk = !global.modWalk;
    if (keyboard_check_pressed(ord(""9""))) global.modNoEnc = !global.modNoEnc;
    if (keyboard_check_pressed(ord(""0""))) global.modGold = 1;
    if (keyboard_check_pressed(ord(""H""))) global.modHeal = 1;
    if (keyboard_check_pressed(ord(""J""))) global.modPlace = (global.modPlace + 1) mod 5;
    if (keyboard_check_pressed(ord(""G""))) global.modGo = 1;
    if (keyboard_check_pressed(ord(""1""))) global.modNoclip = !global.modNoclip;
    if (keyboard_check_pressed(ord(""2""))) global.modGod = !global.modGod;
    if (keyboard_check_pressed(ord(""3""))) global.modSword = !global.modSword;
    if (keyboard_check_pressed(ord(""5""))) global.modFlame = (global.modFlame + 1) mod 4;
    if (keyboard_check_pressed(ord(""6""))) global.modFast = !global.modFast;
    // DEITY-START
    var deityKey = keyboard_check_pressed(ord(""7""));
    if (mouse_check_button_pressed(mb_left))
    {
        if (mx > px && mx < px + 240 * s && my > py + 240 * s && my < py + 270 * s) deityKey = 1;
    }
    if (deityKey)
    {
        global.modDeity = !global.modDeity;
        if (global.modDeity)
        {
            global.modKnight = 0;
            // Низкий звук при появлении
            var dvs = asset_get_index(""snd_rumble"");
            if (dvs < 0) dvs = asset_get_index(""snd_impact"");
            if (dvs < 0) dvs = asset_get_index(""snd_power"");
            if (dvs >= 0) snd_play(dvs);
        }
        if (!global.modDeity && instance_exists(obj_mainchara)) obj_mainchara.visible = 1;
    }
    // DEITY-END
    if (keyboard_check_pressed(ord(""4"")))
    {
        global.modKnight = !global.modKnight;
        if (global.modKnight) global.modDeity = 0;
        if (!global.modKnight && instance_exists(obj_mainchara)) obj_mainchara.visible = 1;
    }
}
// +100 золота
if (variable_global_exists(""modGold""))
{
    if (global.modGold)
    {
        global.gold += 100;
        global.modGold = 0;
        var gsn = asset_get_index(""snd_item"");
        if (gsn >= 0) snd_play(gsn);
    }
}
// Лечебные предметы: заполняем пустые места в инвентаре
if (variable_global_exists(""modHeal""))
{
    if (global.modHeal)
    {
        for (var hi = 0; hi < 8; hi += 1)
        {
            if (global.item[hi] == 0) global.item[hi] = 1;
        }
        global.modHeal = 0;
        var hsn = asset_get_index(""snd_heal_c"");
        if (hsn >= 0) snd_play(hsn);
    }
}
// Телепорт в выбранное место
if (variable_global_exists(""modGo""))
{
    if (global.modGo)
    {
        global.modGo = 0;
        var tpr = asset_get_index(global.modPlaceRoom[global.modPlace]);
        if (tpr >= 0 && instance_exists(obj_mainchara) && global.interact == 0)
        {
            global.modMenu = 0;
            global.interact = 3;
            global.entrance = 1;
            room_goto(tpr);
        }
    }
}
// Настройки сохраняются, как только что-то поменялось
var msig = string(global.modGod) + string(global.modKnight) + string(global.modFlame) + string(global.modFast) + string(global.modDeity) + string(global.modWalk) + string(global.modNoEnc) + string(global.modLang);
if (msig != global.modSig)
{
    global.modSig = msig;
    ini_open(""modknight.ini"");
    ini_write_real(""set"", ""god"", global.modGod);
    ini_write_real(""set"", ""knight"", global.modKnight);
    ini_write_real(""set"", ""flame"", global.modFlame);
    ini_write_real(""set"", ""fast"", global.modFast);
    ini_write_real(""set"", ""deity"", global.modDeity);
    ini_write_real(""set"", ""walk"", global.modWalk);
    ini_write_real(""set"", ""noenc"", global.modNoEnc);
    ini_write_real(""set"", ""lang"", global.modLang);
    ini_close();
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
    // Язык: русский рисуем своим пиксельным шрифтом
    var fs = asset_get_index(""scr_modtext"");
    var ru = 0;
    if (global.modLang && fs >= 0) ru = 1;
    if (global.modMenu && !global.modPhoto)
    {
        var px = 20 * s;
        var py = 60 * s;
        var pw = 300 * s;
        var px2 = px + pw + 10 * s;
        draw_set_color(c_black);
        draw_rectangle(px, py, px + pw, py + 310 * s, false);
        draw_rectangle(px2, py, px2 + pw, py + 255 * s, false);
        draw_set_color(c_white);
        draw_rectangle(px, py, px + pw, py + 310 * s, true);
        draw_rectangle(px + 2 * s, py + 2 * s, px + pw - 2 * s, py + 308 * s, true);
        draw_rectangle(px2, py, px2 + pw, py + 255 * s, true);
        draw_rectangle(px2 + 2 * s, py + 2 * s, px2 + pw - 2 * s, py + 253 * s, true);
        draw_set_color(c_yellow);
        if (ru) script_execute(fs, px + 12 * s, py + 10 * s + 3 * s, ""МОД-МЕНЮ [M]"", 1.5 * s);
        else draw_text_transformed(px + 12 * s, py + 10 * s, ""MOD MENU  [M]"", s, s, 0);
        // Кнопка языка
        draw_set_color(make_color_rgb(40, 40, 60));
        draw_rectangle(px + pw - 56 * s, py + 6 * s, px + pw - 8 * s, py + 32 * s, false);
        draw_set_color(c_aqua);
        draw_rectangle(px + pw - 56 * s, py + 6 * s, px + pw - 8 * s, py + 32 * s, true);
        if (ru) draw_text_transformed(px + pw - 48 * s, py + 8 * s, ""EN"", s, s, 0);
        else draw_text_transformed(px + pw - 48 * s, py + 8 * s, ""RU"", s, s, 0);
        var t1 = ""1) NOCLIP: OFF"";
        var r1 = ""1) СКВОЗЬ СТЕНЫ: ВЫКЛ"";
        draw_set_color(c_white);
        if (global.modNoclip) { t1 = ""1) NOCLIP: ON""; r1 = ""1) СКВОЗЬ СТЕНЫ: ВКЛ""; draw_set_color(c_lime); }
        if (ru) script_execute(fs, px + 12 * s, py + 45 * s + 3 * s, r1, 1.5 * s);
        else draw_text_transformed(px + 12 * s, py + 45 * s, t1, s, s, 0);
        var t2 = ""2) GOD MODE: OFF"";
        var r2 = ""2) БЕССМЕРТИЕ: ВЫКЛ"";
        draw_set_color(c_white);
        if (global.modGod) { t2 = ""2) GOD MODE: ON""; r2 = ""2) БЕССМЕРТИЕ: ВКЛ""; draw_set_color(c_lime); }
        if (ru) script_execute(fs, px + 12 * s, py + 85 * s + 3 * s, r2, 1.5 * s);
        else draw_text_transformed(px + 12 * s, py + 85 * s, t2, s, s, 0);
        var t4 = ""4) KNIGHT: OFF"";
        var r4 = ""4) РЫЦАРЬ: ВЫКЛ"";
        draw_set_color(c_white);
        if (global.modKnight) { t4 = ""4) KNIGHT: ON  [U]""; r4 = ""4) РЫЦАРЬ: ВКЛ [U]""; draw_set_color(c_aqua); }
        if (ru) script_execute(fs, px + 12 * s, py + 125 * s + 3 * s, r4, 1.5 * s);
        else draw_text_transformed(px + 12 * s, py + 125 * s, t4, s, s, 0);
        var t5 = ""5) FLAME: BLUE"";
        var r5 = ""5) ПЛАМЯ: СИНЕЕ"";
        if (global.modFlame == 1) { t5 = ""5) FLAME: RED""; r5 = ""5) ПЛАМЯ: КРАСНОЕ""; }
        if (global.modFlame == 2) { t5 = ""5) FLAME: PURPLE""; r5 = ""5) ПЛАМЯ: ФИОЛЕТОВОЕ""; }
        if (global.modFlame == 3) { t5 = ""5) FLAME: GREEN""; r5 = ""5) ПЛАМЯ: ЗЕЛЁНОЕ""; }
        draw_set_color(global.modFC1);
        if (ru) script_execute(fs, px + 12 * s, py + 165 * s + 3 * s, r5, 1.5 * s);
        else draw_text_transformed(px + 12 * s, py + 165 * s, t5, s, s, 0);
        var t6 = ""6) FAST BATTLE: OFF"";
        var r6 = ""6) БЫСТРЫЙ БОЙ: ВЫКЛ"";
        draw_set_color(c_white);
        if (global.modFast) { t6 = ""6) FAST BATTLE: ON""; r6 = ""6) БЫСТРЫЙ БОЙ: ВКЛ""; draw_set_color(c_lime); }
        if (ru) script_execute(fs, px + 12 * s, py + 205 * s + 3 * s, r6, 1.5 * s);
        else draw_text_transformed(px + 12 * s, py + 205 * s, t6, s, s, 0);
        // DEITY-START
        var t7 = ""7) HOLY KNIGHT: OFF"";
        var r7 = ""7) СВЯЩ. РЫЦАРЬ: ВЫКЛ"";
        draw_set_color(c_white);
        if (global.modDeity) { t7 = ""7) HOLY KNIGHT: ON""; r7 = ""7) СВЯЩ. РЫЦАРЬ: ВКЛ""; draw_set_color(make_color_rgb(230, 190, 60)); }
        if (ru) script_execute(fs, px + 12 * s, py + 245 * s + 3 * s, r7, 1.5 * s);
        else draw_text_transformed(px + 12 * s, py + 245 * s, t7, s, s, 0);
        // DEITY-END
        draw_set_color(c_gray);
        if (ru) script_execute(fs, px + 12 * s, py + 280 * s + 3 * s, ""ПОБЕД РЫЦАРЯ: "" + string(global.modKills), 1.5 * s);
        else draw_text_transformed(px + 12 * s, py + 280 * s, ""KNIGHT WINS: "" + string(global.modKills), s, s, 0);
        // Секретная кнопка: маленький тёмный квадрат справа
        draw_set_color(make_color_rgb(40, 40, 40));
        if (global.modSword) draw_set_color(c_red);
        draw_rectangle(px + pw - 24 * s, py + 130 * s, px + pw - 4 * s, py + 150 * s, false);
        // Второе окно
        draw_set_color(c_yellow);
        if (ru) script_execute(fs, px2 + 12 * s, py + 10 * s + 3 * s, ""ЕЩЁ"", 1.5 * s);
        else draw_text_transformed(px2 + 12 * s, py + 10 * s, ""MORE"", s, s, 0);
        var t8 = ""8) FAST WALK: OFF"";
        var r8 = ""8) БЫСТРЫЙ ШАГ: ВЫКЛ"";
        draw_set_color(c_white);
        if (global.modWalk) { t8 = ""8) FAST WALK: ON""; r8 = ""8) БЫСТРЫЙ ШАГ: ВКЛ""; draw_set_color(c_lime); }
        if (ru) script_execute(fs, px2 + 12 * s, py + 45 * s + 3 * s, r8, 1.5 * s);
        else draw_text_transformed(px2 + 12 * s, py + 45 * s, t8, s, s, 0);
        var t9 = ""9) NO BATTLES: OFF"";
        var r9 = ""9) БЕЗ БОЁВ: ВЫКЛ"";
        draw_set_color(c_white);
        if (global.modNoEnc) { t9 = ""9) NO BATTLES: ON""; r9 = ""9) БЕЗ БОЁВ: ВКЛ""; draw_set_color(c_lime); }
        if (ru) script_execute(fs, px2 + 12 * s, py + 80 * s + 3 * s, r9, 1.5 * s);
        else draw_text_transformed(px2 + 12 * s, py + 80 * s, t9, s, s, 0);
        draw_set_color(c_yellow);
        if (ru) script_execute(fs, px2 + 12 * s, py + 115 * s + 3 * s, ""0) +100 ЗОЛОТА"", 1.5 * s);
        else draw_text_transformed(px2 + 12 * s, py + 115 * s, ""0) +100 GOLD"", s, s, 0);
        draw_set_color(c_white);
        if (ru) script_execute(fs, px2 + 12 * s, py + 150 * s + 3 * s, ""H) ЛЕЧЕБНЫЕ ПРЕДМЕТЫ"", 1.5 * s);
        else draw_text_transformed(px2 + 12 * s, py + 150 * s, ""H) HEALING ITEMS"", s, s, 0);
        draw_set_color(c_aqua);
        if (ru) script_execute(fs, px2 + 12 * s, py + 185 * s + 3 * s, ""J) МЕСТО: "" + global.modPlaceRU[global.modPlace], 1.5 * s);
        else draw_text_transformed(px2 + 12 * s, py + 185 * s, ""J) PLACE: "" + global.modPlaceEN[global.modPlace], s, s, 0);
        draw_set_color(c_lime);
        if (ru) script_execute(fs, px2 + 12 * s, py + 220 * s + 3 * s, ""G) ТЕЛЕПОРТ"", 1.5 * s);
        else draw_text_transformed(px2 + 12 * s, py + 220 * s, ""G) TELEPORT"", s, s, 0);
    }
    if (global.modSword && !global.modPhoto)
    {
        draw_set_color(c_red);
        if (ru) script_execute(fs, gw - 60 * s, 10 * s, ""МЕЧ"", 1.5 * s);
        else draw_text_transformed(gw - 90 * s, 8 * s, ""SWORD"", s, s, 0);
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
        // Синий луч бьёт снизу прямо во врага
        var lbx = gw / 2;
        var lby = gh;
        var lex = lbx + (tx - lbx) * p;
        var ley = lby + (ty - lby) * p;
        var lfade = 1;
        if (t > 38) lfade = max(0, 1 - (t - 38) / 12);
        var lw = (14 + sin(current_time / 30) * 4) * s * lfade;
        draw_set_alpha(0.35 * lfade);
        draw_set_color(make_color_rgb(40, 110, 255));
        draw_line_width(lbx, lby, lex, ley, lw * 2.2);
        draw_set_alpha(0.8 * lfade);
        draw_set_color(make_color_rgb(70, 160, 255));
        draw_line_width(lbx, lby, lex, ley, lw);
        draw_set_alpha(lfade);
        draw_set_color(make_color_rgb(190, 235, 255));
        draw_line_width(lbx, lby, lex, ley, lw * 0.45);
        draw_set_color(c_white);
        draw_line_width(lbx, lby, lex, ley, lw * 0.15);
        // искры вдоль луча
        for (var lq = 0; lq < 10; lq += 1)
        {
            var lk = random(1);
            draw_set_color(choose(c_white, make_color_rgb(120, 200, 255)));
            draw_circle(lbx + (lex - lbx) * lk + random_range(-lw, lw), lby + (ley - lby) * lk + random_range(-lw, lw), (1 + random(3)) * s, false);
        }
        draw_set_alpha(1);
        if (t >= 12)
        {
            var q = (t - 12) / 38;
            // Вспышка
            draw_set_alpha(max(0, 0.8 - q * 3));
            draw_set_color(c_white);
            draw_rectangle(0, 0, gw, gh, false);
            // Красные кольца из точки удара
            draw_set_alpha(1 - q);
            draw_set_color(make_color_rgb(70, 160, 255));
            draw_circle(tx - 18 * s, ty, (10 + q * 90) * s, true);
            draw_circle(tx - 18 * s, ty, (5 + q * 60) * s, true);
            draw_circle(tx - 18 * s, ty, 8 * s * (1 - q), false);
            draw_set_alpha(1);
            draw_set_color(c_red);
            if (ru) script_execute(fs, tx - 40 * s, ty - 70 * s, ""КРИТ!"", 3 * s);
            else draw_text_transformed(tx - 60 * s, ty - 70 * s, ""CRITICAL!"", s * 1.5, s * 1.5, 0);
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
        if (global.modKnight && global.modThrow <= 0 && keyboard_check(ord(""U"")))
        {
            global.modCharge += 1;
            if (global.modCharge == 30)
            {
                var sndi = asset_get_index(""snd_power"");
                if (sndi >= 0) snd_play(sndi);
            }
        }
        if (global.modKnight && global.modThrow <= 0 && keyboard_check_released(ord(""U"")) && global.modCharge > 0)
        {
            global.modBig = 0;
            if (global.modCharge >= 30) global.modBig = 1;
            global.modCharge = 0;
            var sndi = asset_get_index(""snd_laz"");
            if (sndi >= 0) snd_play(sndi);
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
    if (!keyboard_check(ord(""U""))) global.modCharge = 0;
    // Щупальце из земли (Y)
    if (instance_exists(obj_mainchara))
    {
        if (global.modKnight && global.modSummon <= 0 && global.interact == 0 && keyboard_check_pressed(ord(""Y"")))
        {
            global.modSummon = 60;
            global.modSumX = round((obj_mainchara.bbox_left + obj_mainchara.bbox_right) / 2);
            global.modSumY = obj_mainchara.bbox_bottom;
            if (global.modFace == 0) global.modSumY += 26;
            if (global.modFace == 1) global.modSumY -= 22;
            if (global.modFace == 2) global.modSumX += 26;
            if (global.modFace == 3) global.modSumX -= 26;
            var sndi = asset_get_index(""snd_spearrise"");
            if (sndi >= 0) snd_play(sndi);
        }
    }
    if (global.modSummon > 0) global.modSummon -= 1;
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
        // Тёмная аура, когда рыцарь стоит на месте
        if (abs(mdx) + abs(mdy) <= 0.1)
        {
            for (var ai = 0; ai < 10; ai += 1)
            {
                var aph = frac(current_time / 1400 + ai / 10);
                var aax = kcx + round(sin(ai * 2.3 + current_time / 600) * (6 + (ai mod 4) * 2));
                var aay = by + 25 - round(aph * 34);
                draw_set_alpha(1 - aph);
                draw_set_color(make_color_rgb(20, 14, 30));
                if ((ai mod 4) == 0) draw_set_color(global.modFC1);
                draw_rectangle(aax, aay, aax + 1.5, aay + 1.5, false);
            }
            draw_set_alpha(1);
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
                    else if (ch == ""H"") col = global.modArmorCol;
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
                    else if (ch == ""H"") col = global.modArmorCol;
                    else if (ch == ""E"") col = global.modFC1;
                    else if (ch == ""W"") col = c_white;
                    else if (ch == ""C"") col = global.modFC1;
                    else if (ch == ""B"") col = global.modFC2;
                            { var qx = sx + c3; var qy = sy + r3; draw_set_color(col); draw_rectangle(qx, qy, qx + 0.5, qy + 0.5, false); }
                        }
                    }
                }
                // Заряд сверхудара: правый меч светится
                if (side == 1 && global.modCharge > 0)
                {
                    var chn = min(global.modCharge, 30);
                    draw_set_alpha(0.25 + chn / 50);
                    draw_set_color(global.modFC1);
                    draw_rectangle(sx - 1, sy - 1, sx + 5.5, sy + 20 + 0.5, true);
                    for (var cq = 0; cq < chn / 3; cq += 1)
                    {
                        var cqx = sx + irandom_range(-3, 7);
                        var cqy = sy + irandom(20);
                        draw_rectangle(cqx, cqy, cqx + 0.5, cqy + 0.5, false);
                    }
                    if (chn >= 30 && ((current_time div 90) mod 2) == 0)
                    {
                        draw_set_color(c_white);
                        draw_rectangle(sx - 2, sy - 2, sx + 6.5, sy + 20 + 1.5, true);
                    }
                    draw_set_alpha(1);
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
            var tsc = 1 + global.modBig * 2;
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
                    else if (ch == ""H"") col = global.modArmorCol;
                    else if (ch == ""E"") col = global.modFC1;
                    else if (ch == ""W"") col = c_white;
                    else if (ch == ""C"") col = global.modFC1;
                    else if (ch == ""B"") col = global.modFC2;
                        draw_set_color(col);
                        draw_rectangle(tsx + px4 * tsc, tsy + py4 * tsc, tsx + px4 * tsc + tsc - 0.5, tsy + py4 * tsc + tsc - 0.5, false);
                    }
                }
            }
            for (var tr = 0; tr < 6; tr += 1)
            {
                { var qx = tsx + (2 + irandom_range(-3, 3)) * tsc; var qy = tsy + (9 + irandom_range(-4, 4)) * tsc; draw_set_color(choose(global.modFC1, c_white)); draw_rectangle(qx, qy, qx + 0.5, qy + 0.5, false); }
            }
        }
        // Щупальце из земли (Y)
        if (global.modSummon > 0)
        {
            var smt = (60 - global.modSummon) / 60;
            var smh = round(sin(smt * pi) * 40);
            draw_set_alpha(0.5);
            draw_set_color(c_black);
            draw_ellipse(global.modSumX - 9, global.modSumY - 3, global.modSumX + 9, global.modSumY + 3, false);
            draw_set_alpha(1);
            for (var sq = 0; sq < smh; sq += 1)
            {
                var swv = round(sin(current_time / 150 + sq * 0.25) * sq * 0.18);
                var sth = max(1, 5 - sq / 9);
                draw_set_color(make_color_rgb(8, 6, 14));
                draw_rectangle(global.modSumX + swv - sth / 2, global.modSumY - sq, global.modSumX + swv + sth / 2, global.modSumY - sq + 0.5, false);
                if ((sq mod 6) == 3)
                {
                    draw_set_color(global.modFC1);
                    draw_rectangle(global.modSumX + swv, global.modSumY - sq, global.modSumX + swv + 0.5, global.modSumY - sq + 0.5, false);
                }
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
    if (0)
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
            var shold = global.modShield;
            global.modShield -= kabs;
            if (shold > 0 && global.modShield <= 0)
            {
                var sndi = asset_get_index(""snd_break1"");
                if (sndi >= 0) snd_play(sndi);
            }
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
                var acol = global.modFC1;
                if (global.modShield <= 0) acol = c_gray;
                d3d_set_fog(true, acol, 0, 0);
                if (global.modShield > 10)
                {
                    draw_sprite_ext(sprite_index, image_index, hx - 2 * kkx, hy, kkx, kky, 0, c_white, 1);
                    draw_sprite_ext(sprite_index, image_index, hx + 2 * kkx, hy, kkx, kky, 0, c_white, 1);
                    draw_sprite_ext(sprite_index, image_index, hx, hy - 2 * kky, kkx, kky, 0, c_white, 1);
                    draw_sprite_ext(sprite_index, image_index, hx, hy + 2 * kky, kkx, kky, 0, c_white, 1);
                }
                draw_sprite_ext(sprite_index, image_index, hx - kkx, hy, kkx, kky, 0, c_white, 1);
                draw_sprite_ext(sprite_index, image_index, hx + kkx, hy, kkx, kky, 0, c_white, 1);
                draw_sprite_ext(sprite_index, image_index, hx, hy - kky, kkx, kky, 0, c_white, 1);
                draw_sprite_ext(sprite_index, image_index, hx, hy + kky, kkx, kky, 0, c_white, 1);
                d3d_set_fog(true, make_color_rgb(8, 6, 14), 0, 0);
                draw_sprite_ext(sprite_index, image_index, hx, hy, kkx, kky, 0, c_white, 1);
                d3d_set_fog(false, c_black, 0, 0);
                // Щит разбит — на душе трещина
                if (global.modShield <= 0)
                {
                    draw_set_color(c_gray);
                    draw_line_width(hx + 8 * kkx, hy + 2 * kky, hx + 6 * kkx, hy + 7 * kky, kkx);
                    draw_line_width(hx + 6 * kkx, hy + 7 * kky, hx + 10 * kkx, hy + 10 * kky, kkx);
                    draw_line_width(hx + 10 * kkx, hy + 10 * kky, hx + 7 * kkx, hy + 14 * kky, kkx);
                }
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
        var kfs = asset_get_index(""scr_modtext"");
        if (!global.modPhoto)
        {
            if (global.modLang && kfs >= 0) script_execute(kfs, 20 * ks, 14 * ks, ""ЩИТ"", 2 * ks);
            else draw_text_transformed(20 * ks, 12 * ks, ""SHIELD"", ks, ks, 0);
        }
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
    // Броня рыцаря светится, пока цел щит
    global.modArmorCol = make_color_rgb(110, 115, 128);
    if (variable_global_exists(""modShield""))
    {
        if (global.modShield > 0) global.modArmorCol = global.modFC3;
    }
    // Эпичный вход в бой
    var inb2 = instance_exists(obj_heart);
    if (inb2 && !global.modWasBattle && global.modKnight) global.modIntro = 45;
    global.modWasBattle = inb2;
    if (global.modIntro > 0) global.modIntro -= 1;
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
        var sndi = asset_get_index(""snd_swing"");
        if (sndi >= 0) snd_play(sndi);
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

// ===== Версия 0.6: эпичный вход в бой =====
group.QueueAppend("gml_Object_obj_time_Draw_64", @"
if (variable_global_exists(""modIntro""))
{
  if (global.modIntro > 0)
  {
    var igw = display_get_gui_width();
    var igh = display_get_gui_height();
    var is = igw / 640;
    var it = 45 - global.modIntro;
    var ia = min(1, min(it / 6, global.modIntro / 8));
    var ir;
    ir[0] = ""..H........H.."";
    ir[1] = ""..gH......Hg.."";
    ir[2] = ""...gKKKKKKg..."";
    ir[3] = ""...KGGGGGGK..."";
    ir[4] = ""...KGHGGHGK..."";
    ir[5] = ""...KgKKKKgK..."";
    ir[6] = ""...KKEKKEKK..."";
    ir[7] = ""...KgKKKKgK..."";
    ir[8] = ""....KgGGgK...."";
    ir[9] = ""..KKKKKKKKKK.."";
    ir[10] = "".KGHGKRRKGHGK."";
    ir[11] = ""KGGGKRRRRKGGGK"";
    ir[12] = ""KgKKRRGGRRKKgK"";
    ir[13] = ""KgKRRGHHGRRKgK"";
    ir[14] = ""KgKRRRGGRRRKgK"";
    ir[15] = ""KGKRRRRRRRRKGK"";
    ir[16] = ""KgKgGGGGGGgKgK"";
    ir[17] = "".KKRRRRRRRRKK."";
    ir[18] = ""...KGGKKGGK..."";
    ir[19] = ""...KGHKKHGK..."";
    ir[20] = ""...KrrKKrrK..."";
    ir[21] = ""...KGGKKGGK..."";
    ir[22] = ""....KGKKGK...."";
    ir[23] = ""....KGKKGK...."";
    ir[24] = "".....K..K....."";
    draw_set_alpha(ia * 0.6);
    draw_set_color(c_black);
    draw_rectangle(0, 0, igw, igh, false);
    draw_set_alpha(ia);
    var ips = 6 * is;
    var ix = igw / 2 - 7 * ips - (1 - min(1, it / 8)) * 120 * is;
    var iy = igh / 2 - 12.5 * ips;
    for (var irr = 0; irr < 25; irr += 1)
    {
        for (var icc = 0; icc < 14; icc += 1)
        {
            var ich = string_char_at(ir[irr], icc + 1);
            if (ich != ""."")
            {
                var icol = make_color_rgb(4, 4, 8);
                if (ich == ""R"") icol = make_color_rgb(30, 30, 38);
                else if (ich == ""r"") icol = make_color_rgb(18, 18, 24);
                else if (ich == ""G"") icol = make_color_rgb(58, 60, 70);
                else if (ich == ""g"") icol = make_color_rgb(38, 39, 47);
                else if (ich == ""H"") icol = global.modArmorCol;
                else if (ich == ""E"") icol = global.modFC1;
                draw_set_color(icol);
                draw_rectangle(ix + icc * ips, iy + irr * ips, ix + (icc + 1) * ips - 1, iy + (irr + 1) * ips - 1, false);
            }
        }
    }
    // Взмах меча: светящаяся дуга
    if (it >= 10 && it < 26)
    {
        var isw = (it - 10) / 16;
        for (var iq = 0; iq < 14; iq += 1)
        {
            var iang = 140 - (isw * 200) + iq * 6;
            draw_set_alpha(ia * (1 - iq / 14));
            draw_set_color(global.modFC1);
            draw_circle(igw / 2 + lengthdir_x(150 * is, iang), igh / 2 + lengthdir_y(150 * is, iang), (12 - iq * 0.7) * is, false);
        }
    }
    draw_set_alpha(1);
    draw_set_color(c_white);
  }
}
");

// DEITY-START
// ===== Священный рыцарь: огромный левитирующий боец с 15 мечами (7 в меню) =====
group.QueuePrepend("gml_Object_obj_mainchara_Draw_0", @"
if (variable_global_exists(""modDeity""))
{
    if (global.modDeity)
    {
        // Куда смотрит — по движению героя
        var dmx = x - xprevious;
        var dmy = y - yprevious;
        if (abs(dmx) + abs(dmy) > 0.1)
        {
            if (abs(dmx) > abs(dmy))
            {
                if (dmx > 0) global.modFace = 2; else global.modFace = 3;
            }
            else
            {
                if (dmy > 0) global.modFace = 0; else global.modFace = 1;
            }
        }
        var dkf = global.modFace;
        var dv;
        if (dkf == 0)
        {
            dv[0] = ""..OOo....oOO.."";
            dv[1] = "".OoOOo..oOOoO."";
            dv[2] = ""Oo.oOKhhKOo.oO"";
            dv[3] = ""O..OKhSShKO..O"";
            dv[4] = ""Oo.oKSSSSKo.oO"";
            dv[5] = "".OooKsKKsKooO."";
            dv[6] = ""..OoKRKKRKoO.."";
            dv[7] = ""...oKsSSsKo..."";
            dv[8] = ""....KssssK...."";
            dv[9] = "".....KKKK....."";
            dv[10] = "".KKKKKssKKKKK."";
            dv[11] = ""KhSSKLMMLKSShK"";
            dv[12] = ""KSSsKMMMMKsSSK"";
            dv[13] = ""KLMKLMmmMLKMLK"";
            dv[14] = ""KMLKMmKKmMKLMK"";
            dv[15] = "".KMKKMKKMKKMK."";
            dv[16] = "".KMKMmKKmMKMK."";
            dv[17] = "".KhKKYYYYKKhK."";
            dv[18] = ""..K.KyRRyK.K.."";
            dv[19] = ""....KDPPDK...."";
            dv[20] = ""....KDddDK...."";
            dv[21] = ""...KDdPPdDK..."";
            dv[22] = ""...KdDddDdK..."";
            dv[23] = ""....KdKKdK...."";
            dv[24] = "".....KddK....."";
            dv[25] = ""......dd......"";
        }
        else if (dkf == 1)
        {
            dv[0] = ""..OOo....oOO.."";
            dv[1] = "".OoOOo..oOOoO."";
            dv[2] = ""Oo.oOKhhKOo.oO"";
            dv[3] = ""O..OKhSShKO..O"";
            dv[4] = ""Oo.oKSSSSKo.oO"";
            dv[5] = "".OooKSSSSKooO."";
            dv[6] = ""..OoKsSSsKoO.."";
            dv[7] = ""...oKSssSKo..."";
            dv[8] = ""....KssssK...."";
            dv[9] = "".....KKKK....."";
            dv[10] = "".KKKKKssKKKKK."";
            dv[11] = ""KhSSKMMMMKSShK"";
            dv[12] = ""KSSsKMMMMKsSSK"";
            dv[13] = ""KMMKMMmmMMKMMK"";
            dv[14] = ""KMMKMmKKmMKMMK"";
            dv[15] = "".KMKKMKKMKKMK."";
            dv[16] = "".KMKMmKKmMKMK."";
            dv[17] = "".KhKKYYYYKKhK."";
            dv[18] = ""..K.KyyyyK.K.."";
            dv[19] = ""....KDPPDK...."";
            dv[20] = ""....KDddDK...."";
            dv[21] = ""...KDdPPdDK..."";
            dv[22] = ""...KdDddDdK..."";
            dv[23] = ""....KdKKdK...."";
            dv[24] = "".....KddK....."";
            dv[25] = ""......dd......"";
        }
        else if (dkf == 2)
        {
            dv[0] = ""....oOOo......"";
            dv[1] = ""...Oo..oO....."";
            dv[2] = ""..OoOOo.KKK..."";
            dv[3] = "".Oo...oOhSSK.."";
            dv[4] = "".O..Oo.OSSSSK."";
            dv[5] = "".O.O..OKsKKKK."";
            dv[6] = "".Oo.oOoKSKRRK."";
            dv[7] = ""..OooO.KsSSK.."";
            dv[8] = ""........KssK.."";
            dv[9] = "".......KsK...."";
            dv[10] = "".....KKKKKK..."";
            dv[11] = ""....KhSSSKLK.."";
            dv[12] = ""....KSSsKMMK.."";
            dv[13] = ""....KLMKMMmK.."";
            dv[14] = ""....KMLKMmK..."";
            dv[15] = "".....KMKmMK..."";
            dv[16] = "".....KMKMmK..."";
            dv[17] = "".....KhKYYK..."";
            dv[18] = ""......KKyyK..."";
            dv[19] = ""......KDPK...."";
            dv[20] = ""......KDdK...."";
            dv[21] = "".....KDdPK...."";
            dv[22] = "".....KdDdK...."";
            dv[23] = ""......KdK....."";
            dv[24] = ""......Kd......"";
            dv[25] = "".......d......"";
        }
        else if (dkf == 3)
        {
            dv[0] = ""......oOOo...."";
            dv[1] = "".....Oo..oO..."";
            dv[2] = ""...KKK.oOOoO.."";
            dv[3] = ""..KSShOo...oO."";
            dv[4] = "".KSSSSO.oO..O."";
            dv[5] = "".KKKKsKO..O.O."";
            dv[6] = "".KRRKSKoOo.oO."";
            dv[7] = ""..KSSsK.OooO.."";
            dv[8] = ""..KssK........"";
            dv[9] = ""....KsK......."";
            dv[10] = ""...KKKKKK....."";
            dv[11] = ""..KLKSSShK...."";
            dv[12] = ""..KMMKsSSK...."";
            dv[13] = ""..KmMMKMLK...."";
            dv[14] = ""...KmMKLMK...."";
            dv[15] = ""...KMmKMK....."";
            dv[16] = ""...KmMKMK....."";
            dv[17] = ""...KYYKhK....."";
            dv[18] = ""...KyyKK......"";
            dv[19] = ""....KPDK......"";
            dv[20] = ""....KdDK......"";
            dv[21] = ""....KPdDK....."";
            dv[22] = ""....KdDdK....."";
            dv[23] = "".....KdK......"";
            dv[24] = ""......dK......"";
            dv[25] = ""......d......."";
        }
        var dsw;
        dsw[0] = ""..W.."";
        dsw[1] = ""..W.."";
        dsw[2] = "".WCB."";
        dsw[3] = "".WCB."";
        dsw[4] = "".WCB."";
        dsw[5] = "".WCB."";
        dsw[6] = "".WCB."";
        dsw[7] = "".WCB."";
        dsw[8] = ""YYYYY"";
        dsw[9] = ""..y.."";
        dsw[10] = ""..y.."";
        dsw[11] = "".yYy."";
        // Левитация: огромный, парит высоко и медленно качается
        var dhv = round(sin(current_time / 400) * 3);
        var dcx = round((bbox_left + bbox_right) / 2);
        var dbx = dcx - 14;
        var dby = bbox_bottom - 52 - 12 + dhv;
        draw_set_alpha(0.35);
        draw_set_color(c_black);
        draw_ellipse(dcx - 14 + dhv, bbox_bottom - 3, dcx + 13 - dhv, bbox_bottom + 2, false);
        // Тёмная вспышка (T): вокруг темнеет, по земле бежит красное кольцо
        var dfl = 0;
        if (global.modDFlash > 0) dfl = sin((60 - global.modDFlash) / 60 * pi);
        if (dfl > 0)
        {
            draw_set_alpha(dfl * 0.55);
            draw_set_color(c_black);
            draw_rectangle(view_xview[0], view_yview[0], view_xview[0] + view_wview[0], view_yview[0] + view_hview[0], false);
            draw_set_alpha(dfl);
            draw_set_color(make_color_rgb(200, 20, 20));
            var drg = (60 - global.modDFlash) * 2.5;
            draw_ellipse(dcx - drg, bbox_bottom - drg / 3, dcx + drg, bbox_bottom + drg / 3, true);
            draw_ellipse(dcx - drg + 2, bbox_bottom - drg / 3 + 1, dcx + drg - 2, bbox_bottom + drg / 3 - 1, true);
            draw_set_alpha(1);
        }
        // Огненный след: где он пролетает, остаются угольки
        if (abs(dmx) + abs(dmy) > 0.1 && ((current_time div 60) mod 2) == 0)
        {
            global.modTrailN = (global.modTrailN + 1) mod 12;
            global.modTrailX[global.modTrailN] = dcx + irandom_range(-4, 4);
            global.modTrailY[global.modTrailN] = bbox_bottom - irandom(2);
            global.modTrailT[global.modTrailN] = 30;
        }
        for (var dti = 0; dti < 12; dti += 1)
        {
            if (global.modTrailT[dti] > 0)
            {
                global.modTrailT[dti] -= 1;
                var dtl = global.modTrailT[dti] / 30;
                var dtx = global.modTrailX[dti] + irandom_range(-1, 1);
                var dty = global.modTrailY[dti] - round((1 - dtl) * 7);
                draw_set_alpha(dtl);
                draw_set_color(choose(make_color_rgb(255, 40, 20), make_color_rgb(255, 130, 30), make_color_rgb(255, 230, 120)));
                draw_rectangle(dtx, dty, dtx + 1.5, dty + 1.5, false);
                draw_set_color(make_color_rgb(200, 20, 15));
                draw_rectangle(dtx + 1, dty + 3, dtx + 2.5, dty + 4.5, false);
            }
        }
        draw_set_alpha(1);
        // Тёмно-красный ореол с шипами за головой
        draw_set_alpha(0.55 + sin(current_time / 300) * 0.2);
        draw_set_color(make_color_rgb(150, 20, 15));
        draw_circle(dcx, dby + 12, 11, true);
        draw_circle(dcx, dby + 12, 12, true);
        for (var dk = 0; dk < 12; dk += 1)
        {
            var dka = dk * 30 + current_time / 60;
            draw_line(dcx + lengthdir_x(12, dka), dby + 12 + lengthdir_y(12, dka), dcx + lengthdir_x(15, dka), dby + 12 + lengthdir_y(15, dka));
        }
        draw_set_alpha(1);
        // 15 мечей веером по бокам и над головой
        var dscx = dcx;
        var dscy = dby + 26;
        for (var di = 0; di < 15; di += 1)
        {
            var dang = 90 + (di - 7) * 13 + sin(current_time / 700 + di * 0.5) * 3;
            var drad = 17 + sin(current_time / 350 + di) * 2 + dfl * 45;
            for (var dr = 0; dr < 12; dr += 1)
            {
                for (var dc = 0; dc < 5; dc += 1)
                {
                    var dch = string_char_at(dsw[dr], dc + 1);
                    if (dch != ""."")
                    {
                    var dcol = make_color_rgb(4, 4, 8);
                    if (dch == ""D"") dcol = make_color_rgb(24, 18, 36);
                    else if (dch == ""d"") dcol = make_color_rgb(14, 10, 22);
                    else if (dch == ""P"") dcol = make_color_rgb(60, 40, 90);
                    else if (dch == ""Y"") dcol = make_color_rgb(230, 190, 60);
                    else if (dch == ""y"") dcol = make_color_rgb(150, 110, 30);
                    else if (dch == ""W"") dcol = make_color_rgb(225, 225, 235);
                    else if (dch == ""S"") dcol = make_color_rgb(70, 72, 84);
                    else if (dch == ""s"") dcol = make_color_rgb(40, 42, 50);
                    else if (dch == ""h"") dcol = make_color_rgb(150, 155, 170);
                    else if (dch == ""R"") dcol = make_color_rgb(255, 40, 20);
                    else if (dch == ""O"") dcol = make_color_rgb(175, 155, 115);
                    else if (dch == ""o"") dcol = make_color_rgb(105, 90, 65);
                    else if (dch == ""M"") dcol = make_color_rgb(70, 55, 75);
                    else if (dch == ""m"") dcol = make_color_rgb(45, 35, 50);
                    else if (dch == ""L"") dcol = make_color_rgb(115, 95, 120);
                    else if (dch == ""C"") dcol = make_color_rgb(160, 165, 180);
                    else if (dch == ""B"") dcol = make_color_rgb(95, 100, 115);
                    draw_set_color(dcol);
                        var dal = drad + (11 - dr) * 2;
                        var dsd = (dc - 2) * 1.5;
                        var dpx = dscx + lengthdir_x(dal, dang) + lengthdir_x(dsd, dang - 90);
                        var dpy = dscy + lengthdir_y(dal, dang) + lengthdir_y(dsd, dang - 90);
                        draw_rectangle(dpx - 1, dpy - 1, dpx + 0.5, dpy + 0.5, false);
                    }
                }
            }
        }
        // Тело в 2 раза больше обычного, всегда лицом вперёд
        for (var dr2 = 0; dr2 < 26; dr2 += 1)
        {
            var drow = dv[dr2];
            for (var dc2 = 0; dc2 < 14; dc2 += 1)
            {
                var dch = string_char_at(drow, dc2 + 1);
                if (dch != ""."")
                {
                    var dcol = make_color_rgb(4, 4, 8);
                    if (dch == ""D"") dcol = make_color_rgb(24, 18, 36);
                    else if (dch == ""d"") dcol = make_color_rgb(14, 10, 22);
                    else if (dch == ""P"") dcol = make_color_rgb(60, 40, 90);
                    else if (dch == ""Y"") dcol = make_color_rgb(230, 190, 60);
                    else if (dch == ""y"") dcol = make_color_rgb(150, 110, 30);
                    else if (dch == ""W"") dcol = make_color_rgb(225, 225, 235);
                    else if (dch == ""S"") dcol = make_color_rgb(70, 72, 84);
                    else if (dch == ""s"") dcol = make_color_rgb(40, 42, 50);
                    else if (dch == ""h"") dcol = make_color_rgb(150, 155, 170);
                    else if (dch == ""R"") dcol = make_color_rgb(255, 40, 20);
                    else if (dch == ""O"") dcol = make_color_rgb(175, 155, 115);
                    else if (dch == ""o"") dcol = make_color_rgb(105, 90, 65);
                    else if (dch == ""M"") dcol = make_color_rgb(70, 55, 75);
                    else if (dch == ""m"") dcol = make_color_rgb(45, 35, 50);
                    else if (dch == ""L"") dcol = make_color_rgb(115, 95, 120);
                    else if (dch == ""C"") dcol = make_color_rgb(160, 165, 180);
                    else if (dch == ""B"") dcol = make_color_rgb(95, 100, 115);
                    draw_set_color(dcol);
                    draw_rectangle(dbx + dc2 * 2, dby + dr2 * 2, dbx + dc2 * 2 + 1.5, dby + dr2 * 2 + 1.5, false);
                }
            }
        }
        // Пламя из глаз
        if (dkf == 0)
        {
        for (var ef = 0; ef < 5; ef += 1)
        {
            var efh = random(1);
            var ex = dbx + 10 + round(random_range(-1, 1) * (1 + efh * 3) + sin(current_time / 90 + ef) * efh * 3);
            var ey = dby + 12 - round(efh * 22);
            draw_set_alpha(1 - efh * 0.8);
            draw_set_color(make_color_rgb(255, 40, 20));
            if (efh < 0.25) draw_set_color(make_color_rgb(255, 230, 120));
            else if (efh < 0.5) draw_set_color(make_color_rgb(255, 130, 30));
            var efs = 2 - efh;
            draw_rectangle(ex, ey, ex + efs, ey + efs, false);
        }
        for (var ef = 0; ef < 5; ef += 1)
        {
            var efh = random(1);
            var ex = dbx + 16 + round(random_range(-1, 1) * (1 + efh * 3) + sin(current_time / 90 + ef) * efh * 3);
            var ey = dby + 12 - round(efh * 22);
            draw_set_alpha(1 - efh * 0.8);
            draw_set_color(make_color_rgb(255, 40, 20));
            if (efh < 0.25) draw_set_color(make_color_rgb(255, 230, 120));
            else if (efh < 0.5) draw_set_color(make_color_rgb(255, 130, 30));
            var efs = 2 - efh;
            draw_rectangle(ex, ey, ex + efs, ey + efs, false);
        }
        draw_set_alpha(0.35 + random(0.2));
        draw_set_color(make_color_rgb(255, 40, 20));
        draw_circle(dbx + 10 + 1, dby + 12 + 1, 3, false);
        draw_circle(dbx + 16 + 1, dby + 12 + 1, 3, false);
        draw_set_alpha(1);
        }
        if (dkf == 2)
        {
        for (var ef = 0; ef < 5; ef += 1)
        {
            var efh = random(1);
            var ex = dbx + 20 + round(random_range(-1, 1) * (1 + efh * 3) + sin(current_time / 90 + ef) * efh * 3);
            var ey = dby + 12 - round(efh * 22);
            draw_set_alpha(1 - efh * 0.8);
            draw_set_color(make_color_rgb(255, 40, 20));
            if (efh < 0.25) draw_set_color(make_color_rgb(255, 230, 120));
            else if (efh < 0.5) draw_set_color(make_color_rgb(255, 130, 30));
            var efs = 2 - efh;
            draw_rectangle(ex, ey, ex + efs, ey + efs, false);
        }
        for (var ef = 0; ef < 5; ef += 1)
        {
            var efh = random(1);
            var ex = dbx + 22 + round(random_range(-1, 1) * (1 + efh * 3) + sin(current_time / 90 + ef) * efh * 3);
            var ey = dby + 12 - round(efh * 22);
            draw_set_alpha(1 - efh * 0.8);
            draw_set_color(make_color_rgb(255, 40, 20));
            if (efh < 0.25) draw_set_color(make_color_rgb(255, 230, 120));
            else if (efh < 0.5) draw_set_color(make_color_rgb(255, 130, 30));
            var efs = 2 - efh;
            draw_rectangle(ex, ey, ex + efs, ey + efs, false);
        }
        draw_set_alpha(0.35 + random(0.2));
        draw_set_color(make_color_rgb(255, 40, 20));
        draw_circle(dbx + 20 + 1, dby + 12 + 1, 3, false);
        draw_circle(dbx + 22 + 1, dby + 12 + 1, 3, false);
        draw_set_alpha(1);
        }
        if (dkf == 3)
        {
        for (var ef = 0; ef < 5; ef += 1)
        {
            var efh = random(1);
            var ex = dbx + 4 + round(random_range(-1, 1) * (1 + efh * 3) + sin(current_time / 90 + ef) * efh * 3);
            var ey = dby + 12 - round(efh * 22);
            draw_set_alpha(1 - efh * 0.8);
            draw_set_color(make_color_rgb(255, 40, 20));
            if (efh < 0.25) draw_set_color(make_color_rgb(255, 230, 120));
            else if (efh < 0.5) draw_set_color(make_color_rgb(255, 130, 30));
            var efs = 2 - efh;
            draw_rectangle(ex, ey, ex + efs, ey + efs, false);
        }
        for (var ef = 0; ef < 5; ef += 1)
        {
            var efh = random(1);
            var ex = dbx + 6 + round(random_range(-1, 1) * (1 + efh * 3) + sin(current_time / 90 + ef) * efh * 3);
            var ey = dby + 12 - round(efh * 22);
            draw_set_alpha(1 - efh * 0.8);
            draw_set_color(make_color_rgb(255, 40, 20));
            if (efh < 0.25) draw_set_color(make_color_rgb(255, 230, 120));
            else if (efh < 0.5) draw_set_color(make_color_rgb(255, 130, 30));
            var efs = 2 - efh;
            draw_rectangle(ex, ey, ex + efs, ey + efs, false);
        }
        draw_set_alpha(0.35 + random(0.2));
        draw_set_color(make_color_rgb(255, 40, 20));
        draw_circle(dbx + 4 + 1, dby + 12 + 1, 3, false);
        draw_circle(dbx + 6 + 1, dby + 12 + 1, 3, false);
        draw_set_alpha(1);
        }
        draw_set_color(c_white);
        exit;
    }
}
");
// DEITY-END

// DEITY-START
// ===== Священный рыцарь: все зовут тебя лордом и дрожат от страха =====
// Перед тем как появится текст разговора — меняем слова
group.QueuePrepend("gml_Object_OBJ_WRITER_Create_0", @"
if (variable_global_exists(""modDeity""))
{
    if (global.modDeity && variable_global_exists(""msg""))
    {
        var ln = array_length_1d(global.msg);
        for (var li = 0; li < ln; li += 1)
        {
            var lm = global.msg[li];
            if (is_string(lm))
            {
                lm = string_replace_all(lm, ""innocent one"", ""lord"");
                lm = string_replace_all(lm, ""Innocent one"", ""Lord"");
                lm = string_replace_all(lm, ""little one"", ""lord"");
                lm = string_replace_all(lm, ""Little one"", ""Lord"");
                lm = string_replace_all(lm, ""young one"", ""lord"");
                lm = string_replace_all(lm, ""Young one"", ""Lord"");
                lm = string_replace_all(lm, ""small one"", ""lord"");
                lm = string_replace_all(lm, ""Small one"", ""Lord"");
                lm = string_replace_all(lm, ""little human"", ""lord"");
                lm = string_replace_all(lm, ""little kid"", ""lord"");
                lm = string_replace_all(lm, ""little child"", ""lord"");
                lm = string_replace_all(lm, ""human"", ""lord"");
                lm = string_replace_all(lm, ""Human"", ""Lord"");
                lm = string_replace_all(lm, ""HUMAN"", ""LORD"");
                lm = string_replace_all(lm, ""kiddo"", ""lord"");
                lm = string_replace_all(lm, ""Kiddo"", ""Lord"");
                lm = string_replace_all(lm, "" kid."", "" lord."");
                lm = string_replace_all(lm, ""Kid."", ""Lord."");
                lm = string_replace_all(lm, "" child."", "" lord."");
                lm = string_replace_all(lm, ""Child."", ""Lord."");
                lm = string_replace_all(lm, "" boy."", "" lord."");
                lm = string_replace_all(lm, ""Boy."", ""Lord."");
                lm = string_replace_all(lm, "" kid,"", "" lord,"");
                lm = string_replace_all(lm, ""Kid,"", ""Lord,"");
                lm = string_replace_all(lm, "" child,"", "" lord,"");
                lm = string_replace_all(lm, ""Child,"", ""Lord,"");
                lm = string_replace_all(lm, "" boy,"", "" lord,"");
                lm = string_replace_all(lm, ""Boy,"", ""Lord,"");
                lm = string_replace_all(lm, "" kid!"", "" lord!"");
                lm = string_replace_all(lm, ""Kid!"", ""Lord!"");
                lm = string_replace_all(lm, "" child!"", "" lord!"");
                lm = string_replace_all(lm, ""Child!"", ""Lord!"");
                lm = string_replace_all(lm, "" boy!"", "" lord!"");
                lm = string_replace_all(lm, ""Boy!"", ""Lord!"");
                lm = string_replace_all(lm, "" kid?"", "" lord?"");
                lm = string_replace_all(lm, ""Kid?"", ""Lord?"");
                lm = string_replace_all(lm, "" child?"", "" lord?"");
                lm = string_replace_all(lm, ""Child?"", ""Lord?"");
                lm = string_replace_all(lm, "" boy?"", "" lord?"");
                lm = string_replace_all(lm, ""Boy?"", ""Lord?"");
                lm = string_replace_all(lm, "" kid^"", "" lord^"");
                lm = string_replace_all(lm, ""Kid^"", ""Lord^"");
                lm = string_replace_all(lm, "" child^"", "" lord^"");
                lm = string_replace_all(lm, ""Child^"", ""Lord^"");
                lm = string_replace_all(lm, "" boy^"", "" lord^"");
                lm = string_replace_all(lm, ""Boy^"", ""Lord^"");
                lm = string_replace_all(lm, "" kid&"", "" lord&"");
                lm = string_replace_all(lm, ""Kid&"", ""Lord&"");
                lm = string_replace_all(lm, "" child&"", "" lord&"");
                lm = string_replace_all(lm, ""Child&"", ""Lord&"");
                lm = string_replace_all(lm, "" boy&"", "" lord&"");
                lm = string_replace_all(lm, ""Boy&"", ""Lord&"");
                lm = string_replace_all(lm, "" kid/"", "" lord/"");
                lm = string_replace_all(lm, ""Kid/"", ""Lord/"");
                lm = string_replace_all(lm, "" child/"", "" lord/"");
                lm = string_replace_all(lm, ""Child/"", ""Lord/"");
                lm = string_replace_all(lm, "" boy/"", "" lord/"");
                lm = string_replace_all(lm, ""Boy/"", ""Lord/"");
                lm = string_replace_all(lm, "" kid "", "" lord "");
                lm = string_replace_all(lm, ""Kid "", ""Lord "");
                lm = string_replace_all(lm, "" child "", "" lord "");
                lm = string_replace_all(lm, ""Child "", ""Lord "");
                lm = string_replace_all(lm, "" boy "", "" lord "");
                lm = string_replace_all(lm, ""Boy "", ""Lord "");
                lm = string_replace_all(lm, "" kid-"", "" lord-"");
                lm = string_replace_all(lm, ""Kid-"", ""Lord-"");
                lm = string_replace_all(lm, "" child-"", "" lord-"");
                lm = string_replace_all(lm, ""Child-"", ""Lord-"");
                lm = string_replace_all(lm, "" boy-"", "" lord-"");
                lm = string_replace_all(lm, ""Boy-"", ""Lord-"");
                lm = string_replace_all(lm, "" kid'"", "" lord'"");
                lm = string_replace_all(lm, ""Kid'"", ""Lord'"");
                lm = string_replace_all(lm, "" child'"", "" lord'"");
                lm = string_replace_all(lm, ""Child'"", ""Lord'"");
                lm = string_replace_all(lm, "" boy'"", "" lord'"");
                lm = string_replace_all(lm, ""Boy'"", ""Lord'"");
                global.msg[li] = lm;
            }
        }
    }
}
");
// Следующие страницы разговора — тоже
group.QueueAppend("gml_Object_obj_time_Step_1", @"
if (instance_exists(OBJ_WRITER))
{
if (variable_global_exists(""modDeity""))
{
    if (global.modDeity && variable_global_exists(""msg""))
    {
        var ln = array_length_1d(global.msg);
        for (var li = 0; li < ln; li += 1)
        {
            var lm = global.msg[li];
            if (is_string(lm))
            {
                lm = string_replace_all(lm, ""innocent one"", ""lord"");
                lm = string_replace_all(lm, ""Innocent one"", ""Lord"");
                lm = string_replace_all(lm, ""little one"", ""lord"");
                lm = string_replace_all(lm, ""Little one"", ""Lord"");
                lm = string_replace_all(lm, ""young one"", ""lord"");
                lm = string_replace_all(lm, ""Young one"", ""Lord"");
                lm = string_replace_all(lm, ""small one"", ""lord"");
                lm = string_replace_all(lm, ""Small one"", ""Lord"");
                lm = string_replace_all(lm, ""little human"", ""lord"");
                lm = string_replace_all(lm, ""little kid"", ""lord"");
                lm = string_replace_all(lm, ""little child"", ""lord"");
                lm = string_replace_all(lm, ""human"", ""lord"");
                lm = string_replace_all(lm, ""Human"", ""Lord"");
                lm = string_replace_all(lm, ""HUMAN"", ""LORD"");
                lm = string_replace_all(lm, ""kiddo"", ""lord"");
                lm = string_replace_all(lm, ""Kiddo"", ""Lord"");
                lm = string_replace_all(lm, "" kid."", "" lord."");
                lm = string_replace_all(lm, ""Kid."", ""Lord."");
                lm = string_replace_all(lm, "" child."", "" lord."");
                lm = string_replace_all(lm, ""Child."", ""Lord."");
                lm = string_replace_all(lm, "" boy."", "" lord."");
                lm = string_replace_all(lm, ""Boy."", ""Lord."");
                lm = string_replace_all(lm, "" kid,"", "" lord,"");
                lm = string_replace_all(lm, ""Kid,"", ""Lord,"");
                lm = string_replace_all(lm, "" child,"", "" lord,"");
                lm = string_replace_all(lm, ""Child,"", ""Lord,"");
                lm = string_replace_all(lm, "" boy,"", "" lord,"");
                lm = string_replace_all(lm, ""Boy,"", ""Lord,"");
                lm = string_replace_all(lm, "" kid!"", "" lord!"");
                lm = string_replace_all(lm, ""Kid!"", ""Lord!"");
                lm = string_replace_all(lm, "" child!"", "" lord!"");
                lm = string_replace_all(lm, ""Child!"", ""Lord!"");
                lm = string_replace_all(lm, "" boy!"", "" lord!"");
                lm = string_replace_all(lm, ""Boy!"", ""Lord!"");
                lm = string_replace_all(lm, "" kid?"", "" lord?"");
                lm = string_replace_all(lm, ""Kid?"", ""Lord?"");
                lm = string_replace_all(lm, "" child?"", "" lord?"");
                lm = string_replace_all(lm, ""Child?"", ""Lord?"");
                lm = string_replace_all(lm, "" boy?"", "" lord?"");
                lm = string_replace_all(lm, ""Boy?"", ""Lord?"");
                lm = string_replace_all(lm, "" kid^"", "" lord^"");
                lm = string_replace_all(lm, ""Kid^"", ""Lord^"");
                lm = string_replace_all(lm, "" child^"", "" lord^"");
                lm = string_replace_all(lm, ""Child^"", ""Lord^"");
                lm = string_replace_all(lm, "" boy^"", "" lord^"");
                lm = string_replace_all(lm, ""Boy^"", ""Lord^"");
                lm = string_replace_all(lm, "" kid&"", "" lord&"");
                lm = string_replace_all(lm, ""Kid&"", ""Lord&"");
                lm = string_replace_all(lm, "" child&"", "" lord&"");
                lm = string_replace_all(lm, ""Child&"", ""Lord&"");
                lm = string_replace_all(lm, "" boy&"", "" lord&"");
                lm = string_replace_all(lm, ""Boy&"", ""Lord&"");
                lm = string_replace_all(lm, "" kid/"", "" lord/"");
                lm = string_replace_all(lm, ""Kid/"", ""Lord/"");
                lm = string_replace_all(lm, "" child/"", "" lord/"");
                lm = string_replace_all(lm, ""Child/"", ""Lord/"");
                lm = string_replace_all(lm, "" boy/"", "" lord/"");
                lm = string_replace_all(lm, ""Boy/"", ""Lord/"");
                lm = string_replace_all(lm, "" kid "", "" lord "");
                lm = string_replace_all(lm, ""Kid "", ""Lord "");
                lm = string_replace_all(lm, "" child "", "" lord "");
                lm = string_replace_all(lm, ""Child "", ""Lord "");
                lm = string_replace_all(lm, "" boy "", "" lord "");
                lm = string_replace_all(lm, ""Boy "", ""Lord "");
                lm = string_replace_all(lm, "" kid-"", "" lord-"");
                lm = string_replace_all(lm, ""Kid-"", ""Lord-"");
                lm = string_replace_all(lm, "" child-"", "" lord-"");
                lm = string_replace_all(lm, ""Child-"", ""Lord-"");
                lm = string_replace_all(lm, "" boy-"", "" lord-"");
                lm = string_replace_all(lm, ""Boy-"", ""Lord-"");
                lm = string_replace_all(lm, "" kid'"", "" lord'"");
                lm = string_replace_all(lm, ""Kid'"", ""Lord'"");
                lm = string_replace_all(lm, "" child'"", "" lord'"");
                lm = string_replace_all(lm, ""Child'"", ""Lord'"");
                lm = string_replace_all(lm, "" boy'"", "" lord'"");
                lm = string_replace_all(lm, ""Boy'"", ""Lord'"");
                global.msg[li] = lm;
            }
        }
    }
}
}
");
// DEITY-END



// DEITY-START
// ===== Священный рыцарь: атака дождём мечей и тёмная вспышка (T) =====
group.QueueAppend("gml_Object_obj_time_Step_1", @"
if (variable_global_exists(""modDHit""))
{
    if (global.modDeity && instance_exists(obj_heart))
    {
        // Своя атака: при каждом ударе на врага падает дождь из 15 мечей (+40 урона)
        var dhit = instance_exists(obj_slice);
        if (instance_exists(obj_target))
        {
            if (obj_target.fade == 1) dhit = 1;
        }
        // Новый удар засчитывается, только когда прицел прошлого удара исчез
        if (!instance_exists(obj_target) && !instance_exists(obj_slice)) global.modDReady = 1;
        if (dhit && global.modDReady && global.modDHit <= 0 && !global.modSword)
        {
            global.modDReady = 0;
            var dmn = array_length_1d(global.monsterinstance);
            var dfound = 0;
            for (var dj = 0; dj < dmn; dj += 1)
            {
                if (!dfound && instance_exists(global.monsterinstance[dj]))
                {
                    dfound = 1;
                    if (global.monsterhp[dj] > 1) global.monsterhp[dj] = max(1, global.monsterhp[dj] - 40);
                    global.modDX = (global.monsterinstance[dj].bbox_left + global.monsterinstance[dj].bbox_right) / 2;
                    global.modDY = (global.monsterinstance[dj].bbox_top + global.monsterinstance[dj].bbox_bottom) / 2;
                }
            }
            global.modDHit = 70;
            var das = asset_get_index(""snd_laz"");
            if (das >= 0) snd_play(das);
        }
    }
    if (global.modDHit > 0) global.modDHit -= 1;
    if (global.modDFlash > 0) global.modDFlash -= 1;
    // Тёмная вспышка на карте (T)
    if (global.modDeity && global.modDFlash <= 0 && instance_exists(obj_mainchara))
    {
        if (global.interact == 0 && keyboard_check_pressed(ord(""T"")))
        {
            global.modDFlash = 60;
            var dfs = asset_get_index(""snd_bombsplosion"");
            if (dfs < 0) dfs = asset_get_index(""snd_power"");
            if (dfs >= 0) snd_play(dfs);
        }
    }
}
");
group.QueueAppend("gml_Object_obj_time_Draw_64", @"
if (variable_global_exists(""modDHit""))
{
    if (global.modDeity && instance_exists(obj_heart))
    {
        var rgw = display_get_gui_width();
        var rgh = display_get_gui_height();
        var rs = rgw / 640;
        var rvw = view_wview[0];
        if (rvw <= 0) rvw = 640;
        var rvh = view_hview[0];
        if (rvh <= 0) rvh = 480;
        var rkx = rgw / rvw;
        var rky = rgh / rvh;
        // Своя душа: тёмно-красная, в золотой обводке, с золотыми рожками
        with (obj_heart)
        {
            if (visible)
            {
                var rhx = (x - view_xview[0]) * rkx;
                var rhy = (y - view_yview[0]) * rky;
                d3d_set_fog(true, make_color_rgb(230, 190, 60), 0, 0);
                draw_sprite_ext(sprite_index, image_index, rhx - rkx, rhy, rkx, rky, 0, c_white, 1);
                draw_sprite_ext(sprite_index, image_index, rhx + rkx, rhy, rkx, rky, 0, c_white, 1);
                draw_sprite_ext(sprite_index, image_index, rhx, rhy - rky, rkx, rky, 0, c_white, 1);
                draw_sprite_ext(sprite_index, image_index, rhx, rhy + rky, rkx, rky, 0, c_white, 1);
                d3d_set_fog(true, make_color_rgb(150, 0, 20), 0, 0);
                draw_sprite_ext(sprite_index, image_index, rhx, rhy, rkx, rky, 0, c_white, 1);
                d3d_set_fog(false, c_black, 0, 0);
                draw_set_color(make_color_rgb(230, 190, 60));
                draw_rectangle(rhx + 1 * rkx, rhy - 3 * rky, rhx + 3 * rkx, rhy, false);
                draw_rectangle(rhx + 13 * rkx, rhy - 3 * rky, rhx + 15 * rkx, rhy, false);
                draw_rectangle(rhx, rhy - 5 * rky, rhx + 2 * rkx, rhy - 3 * rky, false);
                draw_rectangle(rhx + 14 * rkx, rhy - 5 * rky, rhx + 16 * rkx, rhy - 3 * rky, false);
            }
        }
        // Дождь из мечей падает на врага
        if (global.modDHit > 20)
        {
            var rt = (70 - global.modDHit) / 50;
            var rtx = (global.modDX - view_xview[0]) * rkx;
            var rty = (global.modDY - view_yview[0]) * rky;
            for (var ri = 0; ri < 15; ri += 1)
            {
                var rp = min(1, max(0, (rt - ri / 15 * 0.5) / 0.35));
                if (rp > 0 && rp < 1)
                {
                    var rx = rtx + (ri - 7) * 16 * rs;
                    var ry = -60 * rs + (rty + 20 * rs + 60 * rs) * rp;
                    // клинок острием вниз
                    draw_set_color(make_color_rgb(225, 225, 235));
                    draw_rectangle(rx - 3 * rs, ry - 40 * rs, rx, ry - 8 * rs, false);
                    draw_set_color(make_color_rgb(120, 125, 140));
                    draw_rectangle(rx, ry - 40 * rs, rx + 3 * rs, ry - 8 * rs, false);
                    draw_set_color(make_color_rgb(225, 225, 235));
                    draw_triangle(rx - 3 * rs, ry - 8 * rs, rx + 3 * rs, ry - 8 * rs, rx, ry, false);
                    // гарда и рукоять
                    draw_set_color(make_color_rgb(230, 190, 60));
                    draw_rectangle(rx - 9 * rs, ry - 44 * rs, rx + 9 * rs, ry - 40 * rs, false);
                    draw_set_color(make_color_rgb(150, 110, 30));
                    draw_rectangle(rx - 2 * rs, ry - 56 * rs, rx + 2 * rs, ry - 44 * rs, false);
                }
            }
            if (rt > 0.6)
            {
                var rq = (rt - 0.6) / 0.4;
                draw_set_alpha(1 - rq);
                draw_set_color(make_color_rgb(220, 20, 20));
                draw_circle(rtx, rty, (10 + rq * 100) * rs, true);
                draw_circle(rtx, rty, (6 + rq * 70) * rs, true);
                draw_set_font(fnt_maintext);
                draw_set_color(make_color_rgb(255, 60, 40));
                draw_text_transformed(rtx - 40 * rs, rty - 80 * rs, ""+40"", rs * 1.8, rs * 1.8, 0);
                draw_set_alpha(1);
            }
        }
        draw_set_color(c_white);
    }
}
");
// DEITY-END


// ===== Версия 0.8: скрипт, который рисует русский текст своим пиксельным шрифтом =====
group.QueueReplace("gml_Script_scr_modtext", @"
// argument0, argument1 — где; argument2 — текст; argument3 — размер одной точки
var tx = argument0;
var ty = argument1;
var str = argument2;
var ps = argument3;
var n = string_length(str);
for (var i = 1; i <= n; i += 1)
{
    var ch = string_char_at(str, i);
    if (ds_map_exists(global.modFont, ch))
    {
        var g = ds_map_find_value(global.modFont, ch);
        for (var k = 0; k < 35; k += 1)
        {
            if (string_char_at(g, k + 1) == ""#"")
            {
                var gx = tx + (k mod 5) * ps;
                var gy = ty + (k div 5) * ps;
                draw_rectangle(gx, gy, gx + ps - 1, gy + ps - 1, false);
            }
        }
    }
    tx += 6 * ps;
}
");

// ===== Версия 0.8: быстрый шаг =====
group.QueueAppend("gml_Object_obj_mainchara_Step_0", @"
if (variable_global_exists(""modWalk""))
{
    if (global.modWalk && global.interact == 0 && !global.modNoclip)
    {
        var wdx = (keyboard_check(vk_right) - keyboard_check(vk_left)) * 3;
        var wdy = (keyboard_check(vk_down) - keyboard_check(vk_up)) * 3;
        if (wdx != 0 && !place_meeting(x + wdx, y, obj_solidparent)) x += wdx;
        if (wdy != 0 && !place_meeting(x, y + wdy, obj_solidparent)) y += wdy;
    }
}
");

// ===== Версия 0.8: без случайных боёв (со следующей комнаты) =====
group.QueueAppend("gml_Script_scr_steps", @"
if (variable_global_exists(""modNoEnc""))
{
    if (global.modNoEnc) steps = 999999;
}
");

group.Import();
ScriptMessage("Мод-меню установлено: M — меню, 4 — рыцарь, U — бросок меча, Shift — рывок, 5 — цвет пламени, 6 — быстрый бой, зажми U — сверхудар, Y — щупальце, второе окно 8/9/0/H/J/G, язык RU/EN");
