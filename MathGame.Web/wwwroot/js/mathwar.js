window.mathWarLoadSave = function () {
    try {
        const raw = localStorage.getItem('mathwar_save');
        return raw ? JSON.parse(raw) : null;
    } catch { return null; }
};

window.mathWarSaveSave = function (data) {
    try {
        localStorage.setItem('mathwar_save', JSON.stringify(data));
    } catch { }
};
