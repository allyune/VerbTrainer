document.addEventListener('DOMContentLoaded', function () {
    var searchInput = document.getElementById('verb-search');
    searchInput.addEventListener('input', function () {
        var searchText = this.value.toLowerCase();
        var tableRows = document.getElementsByClassName('table-row');
        for (var row of tableRows) {
            var hebrewNameNikud = row.querySelector('.name').textContent.toLowerCase();
            var hebrewName = hebrewNameNikud.replace(/([\u05B0-\u05BD]|[\u05BF-\u05C7])/g, "");
            var englishMeaning = row.querySelector('.meaning').textContent.toLowerCase();
            if (
                searchText &&
                (!hebrewName.includes(searchText) && !englishMeaning.includes(searchText))
            ) {
                row.style.display = 'none';
            } else {
                row.style.display = '';
            }
        }
    });
});
