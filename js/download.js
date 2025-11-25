function downloadFile(fileName, content) {
    var link = document.createElement('a');
    link.href = "data:text/json;charset=utf-8," + encodeURIComponent(content);
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}