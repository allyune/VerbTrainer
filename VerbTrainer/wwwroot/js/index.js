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

    var tableRows = document.querySelectorAll(".table-row");
    tableRows.forEach(row => {
        row.addEventListener('click', async function () {
            var verbId = this.getAttribute('verb-id');
            try {
                let response = await fetch(`/Home/GetVerbConjugations/${verbId}`);
                if (!response.ok) throw response.status;
                let verbConjugations = await response.json();
                var trainerContainer = document.getElementById('trainer-container');
                trainerContainer.innerHTML = '';
                for (var conjugation of verbConjugations) {
                    var conjDiv = document.createElement('div');
                    conjDiv.classList.add('conj-div')
                    //Meaning
                    var conjMeaning = document.createElement('p');
                    conjMeaning.innerHTML = conjugation.meaning
                    conjDiv.appendChild(conjMeaning);
                    //Text
                    var conjText = document.createElement('p');
                    conjText.innerHTML = conjugation.text
                    conjDiv.appendChild(conjText);
                    //Transcription
                    var conjTranscription = document.createElement('p');
                    conjTranscription.innerHTML = conjugation.transcription
                    conjDiv.appendChild(conjTranscription);

                    trainerContainer.appendChild(conjDiv);
                }
            } catch (e) {
                console.log(e);
            }
        })
    })
});



