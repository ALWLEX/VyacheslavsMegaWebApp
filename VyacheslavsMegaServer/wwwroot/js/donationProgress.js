let current = document.getElementById("fundCurrent").value;
let max = document.getElementById("goalPrice").value;

const cellCount = 20;
const xpBar = document.getElementById("xpBar");

xpBar.innerHTML = ""; // очистить

// Сколько ячеек нужно заполнить
let filledCells = Math.round((parseFloat(current.replace(',', '.')) / parseFloat(max.replace(',', '.'))) * cellCount);

// Создаем 20 клеток
for (let i = 0; i < cellCount; i++) {
    let cell = document.createElement("div");
    cell.className = "mc-bar__cell";
    if (i < filledCells) cell.classList.add("mc-bar__cell--filled");
    xpBar.appendChild(cell);
}

// Обновление текста
document.getElementById("xpText").textContent = current.toLocaleString('ru-RU', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) + " / " + (max == 0 ? "Не определено" : max.toLocaleString('ru-RU', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) + " ₸");