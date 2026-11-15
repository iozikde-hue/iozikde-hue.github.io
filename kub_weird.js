// Kub Weird — 20 странных элементов для Sandboxels. Вкладка «Weird».
(function () {
  var CAT = "weird";
  function rnd(a) { return a[Math.floor(Math.random() * a.length)]; }
  // радужный цвет по углу 0..360 → "#rrggbb" (игра хранит цвета как rgb)
  function hsl(h) {
    h = ((h % 360) + 360) % 360 / 60;
    var c = 0.8, x = c * (1 - Math.abs(h % 2 - 1)), m = 0.2, r = 0, g = 0, b = 0;
    if (h < 1) { r = c; g = x; } else if (h < 2) { r = x; g = c; } else if (h < 3) { g = c; b = x; }
    else if (h < 4) { g = x; b = c; } else if (h < 5) { r = x; b = c; } else { r = c; b = x; }
    return "#" + [r, g, b].map(function (v) { return ("0" + Math.round((v + m) * 255).toString(16)).slice(-2); }).join("");
  }
  function near(pixel, coords, fn) {
    for (var i = 0; i < coords.length; i++) {
      var x = pixel.x + coords[i][0], y = pixel.y + coords[i][1];
      if (!isEmpty(x, y, true)) fn(pixelMap[x][y], x, y);
    }
  }
  function movable(p) { return p && elements[p.element] && elements[p.element].movable !== false && elements[p.element].hardness !== 1 && elements[p.element].category !== CAT; }
  function powder(pixel) {
    if (!tryMove(pixel, pixel.x, pixel.y + 1)) {
      var d = Math.random() < 0.5 ? -1 : 1;
      if (!tryMove(pixel, pixel.x + d, pixel.y + 1)) tryMove(pixel, pixel.x - d, pixel.y + 1);
    }
  }

  // 1. Кубовирус: всё, чего касается, превращает в разноцветные кубики
  elements.kub_virus = {
    name: "кубовирус", color: ["#3bff7a", "#00c853"], category: CAT, state: "solid", density: 1000,
    tick: function (pixel) {
      near(pixel, adjacentCoords, function (p) {
        if (p.element !== "kub_virus" && p.element !== "kub_block" && elements[p.element].hardness !== 1 && Math.random() < 0.05) changePixel(p, "kub_virus");
      });
      if (Math.random() < 0.01) changePixel(pixel, "kub_block");
      doDefaults(pixel);
    }
  };
  elements.kub_block = { name: "кубик-блок", color: ["#ff4f4f", "#ffd84a", "#4fc3ff", "#7cff4f", "#c84fff"], behavior: behaviors.WALL, category: CAT, state: "solid", density: 2000, hidden: true };

  // 2. Антипесок: падает вверх
  elements.kub_antisand = {
    name: "антипесок", color: ["#77d5e6", "#5ab8cc"], category: CAT, state: "solid", density: 1500,
    tick: function (pixel) {
      if (!tryMove(pixel, pixel.x, pixel.y - 1)) {
        var d = Math.random() < 0.5 ? -1 : 1;
        if (!tryMove(pixel, pixel.x + d, pixel.y - 1)) tryMove(pixel, pixel.x - d, pixel.y - 1);
      }
      doDefaults(pixel);
    }
  };

  // 3. Телепорт-пыль: иногда прыгает в случайное место
  elements.kub_teleport_dust = {
    name: "телепорт-пыль", color: ["#b388ff", "#7c4dff", "#e1bee7"], category: CAT, state: "solid", density: 1200,
    tick: function (pixel) {
      if (Math.random() < 0.01) {
        var x = Math.floor(Math.random() * width), y = Math.floor(Math.random() * height);
        if (isEmpty(x, y)) { movePixel(pixel, x, y); return; }
      }
      powder(pixel);
      doDefaults(pixel);
    }
  };

  // 4. Диско-жижа: жидкость, которая всё время меняет цвет
  elements.kub_disco = {
    name: "диско-жижа", color: ["#ff00aa", "#00e5ff", "#ffee00"], behavior: behaviors.LIQUID, category: CAT, state: "liquid", density: 1100, viscosity: 3,
    tick: function (pixel) { pixel.color = pixelColorPick(pixel, hsl(pixelTicks * 6 + pixel.x * 4 + pixel.y * 4)); }
  };

  // 5. Прыгучее желе: иногда подпрыгивает
  elements.kub_jelly = {
    name: "прыгучее желе", color: ["#ff6fb5", "#ff9fd0"], behavior: behaviors.LIQUID, category: CAT, state: "liquid", density: 900, viscosity: 50,
    tick: function (pixel) {
      if (Math.random() < 0.03 && !isEmpty(pixel.x, pixel.y + 1, true)) {
        for (var i = 0; i < 6; i++) if (!tryMove(pixel, pixel.x + (Math.random() < 0.5 ? -1 : 1), pixel.y - 1)) break;
      }
    }
  };

  // 6. Множитель: копирует всё, что рядом
  elements.kub_multiplier = {
    name: "множитель", color: "#ffd600", behavior: behaviors.WALL, category: CAT, state: "solid", density: 3000, hardness: 1,
    tick: function (pixel) {
      if (Math.random() > 0.05) return;
      var src = null;
      near(pixel, adjacentCoords, function (p) { if (p.element !== "kub_multiplier") src = p.element; });
      if (!src) return;
      var c = rnd(squareCoords), x = pixel.x + c[0] * 2, y = pixel.y + c[1] * 2;
      if (isEmpty(x, y)) createPixel(src, x, y);
    }
  };

  // 7. Магнит: тянет к себе всё сыпучее и жидкое
  elements.kub_magnet = {
    name: "магнит", color: ["#d32f2f", "#9e9e9e"], behavior: behaviors.WALL, category: CAT, state: "solid", density: 7000, hardness: 1,
    tick: function (pixel) {
      for (var i = 0; i < 6; i++) {
        var x = pixel.x + Math.floor(Math.random() * 25) - 12, y = pixel.y + Math.floor(Math.random() * 25) - 12;
        if (isEmpty(x, y, true)) continue;
        var p = pixelMap[x][y];
        if (!movable(p)) continue;
        tryMove(p, p.x - Math.sign(p.x - pixel.x), p.y - Math.sign(p.y - pixel.y));
      }
    }
  };

  // 8. Пожиратель: ползает и ест всё вокруг
  elements.kub_eater = {
    name: "пожиратель", color: "#212121", category: CAT, state: "solid", density: 5000,
    properties: { dir: 1, eaten: 0 },
    tick: function (pixel) {
      near(pixel, squareCoords, function (p, x, y) {
        if (p.element !== "kub_eater" && elements[p.element].hardness !== 1 && Math.random() < 0.3) { deletePixel(x, y); pixel.eaten++; }
      });
      if (pixel.eaten > 40) { pixel.eaten = 0; var c = rnd(squareCoords); if (isEmpty(pixel.x + c[0], pixel.y + c[1])) createPixel("kub_eater", pixel.x + c[0], pixel.y + c[1]); }
      if (Math.random() < 0.05) pixel.dir = -pixel.dir;
      if (!tryMove(pixel, pixel.x, pixel.y + 1)) tryMove(pixel, pixel.x + pixel.dir, pixel.y);
      doDefaults(pixel);
    }
  };

  // 9. Радужная змейка: ползёт и оставляет радугу
  elements.kub_snake = {
    name: "радужная змейка", color: "#ffffff", category: CAT, state: "solid", density: 1000,
    properties: { dx: 1, dy: 0, len: 0 },
    tick: function (pixel) {
      if (Math.random() < 0.08) { var c = rnd(adjacentCoords); pixel.dx = c[0]; pixel.dy = c[1]; }
      var ox = pixel.x, oy = pixel.y;
      if (tryMove(pixel, pixel.x + pixel.dx, pixel.y + pixel.dy)) {
        createPixel("kub_rainbow", ox, oy);
        var t = pixelMap[ox][oy];
        if (t) t.color = pixelColorPick(t, hsl(pixel.len++ * 12));
      } else { pixel.dx = -pixel.dx; pixel.dy = -pixel.dy; }
    }
  };
  elements.kub_rainbow = {
    name: "радуга", color: "#ff0000", category: CAT, state: "solid", density: 100, hidden: true,
    tick: function (pixel) { if (Math.random() < 0.03) deletePixel(pixel.x, pixel.y); }
  };

  // 10. Конфетти-бомба: взрывается конфетти от любого касания
  elements.kub_confetti_bomb = {
    name: "конфетти-бомба", color: ["#ff4081", "#ffd740"], category: CAT, state: "solid", density: 1500,
    tick: function (pixel) {
      var hit = !tryMove(pixel, pixel.x, pixel.y + 1);
      if (hit && pixelTicks - pixel.start > 5) { explodeAt(pixel.x, pixel.y, 12, "confetti"); deletePixel(pixel.x, pixel.y); return; }
      doDefaults(pixel);
    }
  };

  // 11. Танцующий песок: подпрыгивает вбок
  elements.kub_dancing_sand = {
    name: "танцующий песок", color: ["#ffca28", "#ff7043"], category: CAT, state: "solid", density: 1600,
    tick: function (pixel) {
      if (Math.random() < 0.1 && !isEmpty(pixel.x, pixel.y + 1, true)) {
        var d = Math.sin(pixelTicks / 10) > 0 ? 1 : -1;
        if (tryMove(pixel, pixel.x, pixel.y - 1)) tryMove(pixel, pixel.x + d, pixel.y);
        return;
      }
      powder(pixel);
      doDefaults(pixel);
    }
  };

  // 12. Мыльный пузырь: летит вверх и лопается в воду
  elements.kub_bubble = {
    name: "мыльный пузырь", color: ["#e0f7fa", "#b2ebf2", "#f8bbd0"], category: CAT, state: "gas", density: 0.5,
    tick: function (pixel) {
      if (!tryMove(pixel, pixel.x + Math.floor(Math.random() * 3) - 1, pixel.y - 1) && Math.random() < 0.2) { changePixel(pixel, "water"); return; }
      if (Math.random() < 0.003) changePixel(pixel, "water");
    }
  };

  // 13. Рандомайзер: превращает всё, что тронет, в случайный элемент
  elements.kub_randomizer = {
    name: "рандомайзер", color: ["#ff1744", "#00e676", "#2979ff", "#ffea00"], behavior: behaviors.WALL, category: CAT, state: "solid", density: 3000, hardness: 1, excludeRandom: true,
    tick: function (pixel) {
      pixel.color = pixelColorPick(pixel, hsl(pixelTicks * 20));
      near(pixel, adjacentCoords, function (p) {
        if (p.element !== "kub_randomizer" && Math.random() < 0.02 && window.randomChoices) changePixel(p, rnd(randomChoices));
      });
    }
  };

  // 14. Призрак: летает сквозь всё
  elements.kub_ghost = {
    name: "призрак", color: ["#eceff1", "#cfd8dc"], category: CAT, state: "gas", density: 0.1, excludeRandom: true,
    tick: function (pixel) {
      if (Math.random() > 0.4) return;
      var c = rnd(squareCoords), x = pixel.x + c[0], y = pixel.y + c[1] - (Math.random() < 0.3 ? 1 : 0);
      if (outOfBounds(x, y)) return;
      if (isEmpty(x, y)) movePixel(pixel, x, y);
      else if (elements[pixelMap[x][y].element].hardness !== 1) swapPixels(pixel, pixelMap[x][y]);
    }
  };

  // 15. Кубик-прыгун: маленький кубик, который прыгает
  elements.kub_jumper = {
    name: "кубик-прыгун", color: "#3b7bff", category: CAT, state: "solid", density: 1000,
    properties: { vy: 0, dir: 1 },
    tick: function (pixel) {
      if (pixel.vy < 0) { if (!tryMove(pixel, pixel.x + pixel.dir, pixel.y - 1)) pixel.vy = 0; else pixel.vy++; }
      else if (!tryMove(pixel, pixel.x, pixel.y + 1)) {
        if (Math.random() < 0.05) { pixel.vy = -(4 + Math.floor(Math.random() * 6)); if (Math.random() < 0.3) pixel.dir = -pixel.dir; }
      }
      doDefaults(pixel);
    }
  };

  // 16. Облако сюрпризов: из него падает что попало
  elements.kub_surprise_cloud = {
    name: "облако сюрпризов", color: ["#ffffff", "#f3e5f5"], category: CAT, state: "gas", density: 0.6,
    tick: function (pixel) {
      if (Math.random() < 0.02 && isEmpty(pixel.x, pixel.y + 1)) createPixel(rnd(["water", "sand", "confetti", "frog", "kub_jelly", "kub_disco", "kub_bubble", "gold_coin" in elements ? "gold_coin" : "gold"]), pixel.x, pixel.y + 1);
      if (Math.random() < 0.05) tryMove(pixel, pixel.x + (Math.random() < 0.5 ? -1 : 1), pixel.y);
    }
  };

  // 17. Ледяная лава: выглядит как лава, а всё замораживает
  elements.kub_ice_lava = {
    name: "ледяная лава", color: ["#4fc3f7", "#0288d1", "#b3e5fc"], behavior: behaviors.LIQUID, category: CAT, state: "liquid", density: 2500, viscosity: 1000, temp: -200,
    reactions: { water: { elem2: "ice" }, salt_water: { elem2: "ice" }, fire: { elem2: "smoke" }, steam: { elem2: "ice" }, lava: { elem2: "rock" in elements ? "rock" : "ice" } }
  };

  // 18. Взрывная вода: от огня и жары взрывается
  elements.kub_boom_water = {
    name: "взрывная вода", color: ["#ff8a65", "#ffab91"], behavior: behaviors.LIQUID, category: CAT, state: "liquid", density: 1000,
    tick: function (pixel) { if (pixel.temp > 90 || pixel.burning) { explodeAt(pixel.x, pixel.y, 6); deletePixel(pixel.x, pixel.y); } },
    reactions: { fire: { func: function (p) { p.temp = 200; } } }
  };

  // 19. Мидас: всё, чего коснётся, превращается в золото
  elements.kub_midas = {
    name: "мидас", color: ["#ffd700", "#ffecb3"], category: CAT, state: "solid", density: 1900,
    tick: function (pixel) {
      near(pixel, adjacentCoords, function (p) { if (p.element !== "gold" && p.element !== "kub_midas" && elements[p.element].hardness !== 1 && Math.random() < 0.1) changePixel(p, "gold"); });
      powder(pixel);
      doDefaults(pixel);
    }
  };

  // 20. Батут: подбрасывает всё, что на него падает
  elements.kub_trampoline = {
    name: "батут", color: ["#00bfa5", "#1de9b6"], behavior: behaviors.WALL, category: CAT, state: "solid", density: 3000, hardness: 1,
    tick: function (pixel) {
      if (isEmpty(pixel.x, pixel.y - 1, true)) return;
      var p = pixelMap[pixel.x][pixel.y - 1];
      if (!movable(p) && !(p.element.indexOf("kub_") === 0 && p.element !== "kub_trampoline")) return;
      var d = Math.floor(Math.random() * 3) - 1;
      for (var i = 0; i < 15; i++) if (!tryMove(p, p.x + (i % 3 === 0 ? d : 0), p.y - 1)) break;
    }
  };
  // Названия: по-русски, если в игре выбран русский язык, иначе по-английски
  var EN = {
    kub_virus: "cube virus",
    kub_block: "cube block",
    kub_antisand: "antisand",
    kub_teleport_dust: "teleport dust",
    kub_disco: "disco goo",
    kub_jelly: "bouncy jelly",
    kub_multiplier: "multiplier",
    kub_magnet: "magnet",
    kub_eater: "eater",
    kub_snake: "rainbow snake",
    kub_rainbow: "rainbow",
    kub_confetti_bomb: "confetti bomb",
    kub_dancing_sand: "dancing sand",
    kub_bubble: "soap bubble",
    kub_randomizer: "randomizer",
    kub_ghost: "ghost",
    kub_jumper: "jumper cube",
    kub_surprise_cloud: "surprise cloud",
    kub_ice_lava: "ice lava",
    kub_boom_water: "boom water",
    kub_midas: "midas",
    kub_trampoline: "trampoline"
  };
  // Мод грузится раньше языка игры: если выбран английский, отменяем язык из Стима
  if (window.settings && settings.lang === "en" && window.initialData) delete initialData.langCode;
  var ru = (window.settings && settings.lang === "ru") || (window.settings && settings.lang !== "en" && window.initialData && initialData.langCode === "ru");
  for (var id in EN) {
    if (!elements[id]) continue;
    elements[id].ruName = elements[id].name;
    if (!ru) elements[id].name = EN[id];
  }

  // Кнопка «Русский» / «English» в верхней панели игры: переключает язык всей игры
  runAfterLoad(function () {
    if (ru) {
      lang[CAT] = "Странное";
      setTimeout(function () { var tab = document.getElementById("categoryButton-" + CAT); if (tab) tab.innerText = "Странное"; }, 0);
    }
    var bar = document.getElementById("toolControls");
    if (!bar || document.getElementById("kubLangButton")) return;
    var b = document.createElement("button");
    b.id = "kubLangButton";
    b.className = "controlButton";
    b.title = ru ? "Switch the game to English" : "Перевести игру на русский";
    b.innerText = ru ? "English" : "Русский";
    b.onclick = function () {
      setSetting("lang", ru ? "en" : "ru");
      window.location.reload();
    };
    bar.appendChild(b);
  });
})();
